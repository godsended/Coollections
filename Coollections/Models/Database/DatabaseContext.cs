using Coollections.Models.Database.Items;
using Microsoft.EntityFrameworkCore;

namespace Coollections.Models.Database;

public class DatabaseContext : DbContext
{
    #region PROPERTIES

    public DbSet<User> Users { get; set; }
    public DbSet<Field> Fields { get; set; }
    public DbSet<Subject> Subjects { get; set; }
    public DbSet<Data> Data { get; set; }
    public DbSet<Collection> Collections { get; set; }
    public DbSet<Item> Items { get; set; }
    public DbSet<Like> Likes { get; set; }
    public DbSet<Commentary> Commentaries { get; set; }

    #endregion

    #region CONFIGURING

    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(
            @"Host=localhost;Port=5432;Database=coollectionsdb;Username=postgres;Password=174285396020");
    }

    #endregion

    #region USERS

    public async Task<User?> GetUserById(int id)
    {
        return await Users.FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<bool> IsAdmin(int userId)
    {
        User? user = await Users.FirstOrDefaultAsync(u => u.Id == userId);
        return user != null && user.IsAdmin;
    }

    public async Task<int> AddUser(User user)
    {
        await Users.AddAsync(user);
        await SaveChangesAsync();
        return user.Id;
    }

    public async Task<bool> ContainsUserWithEmail(string email)
    {
        return await Users.FirstOrDefaultAsync(u => u.Email == email) != null;
    }

    public async Task<bool> ContainsUserWithId(int id)
    {
        return await Users.FirstOrDefaultAsync(u => u.Id == id) != null;
    }

    public async Task<bool> IsLoginDataCorrect(string email, string password)
    {
        return await Users.FirstOrDefaultAsync(u => u.Email == email && u.Password == password) != null;
    }

    public async Task<int> GetUserIdByEmail(string email)
    {
        if (await ContainsUserWithEmail(email))
            return (await Users.FirstOrDefaultAsync(u => u.Email == email))!.Id;
        return -1;
    }

    public async Task SetBlock(bool isBlocked, int userId)
    {
        User? user = await Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null) return;
        user.IsBlocked = isBlocked;
        Users.Update(user);
        await SaveChangesAsync();
    }

    public async Task SetAdminByUserId(bool isAdmin, int userId)
    {
        User? user = await Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null) return;
        user.IsAdmin = isAdmin;
        Users.Update(user);
        await SaveChangesAsync();
    }

    public async Task DeleteUser(int userId)
    {
        User? user = await Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null) return;
        Users.Remove(user);
        await SaveChangesAsync();
    }

    #endregion

    #region SUBJECTS

    public async Task<int> SubjectIdByName(string name)
    {
        return (await Subjects.FirstOrDefaultAsync(s => s.Name == name))!.Id;
    }

    public IEnumerable<Subject> GetSubjects()
    {
        return Subjects;
    }

    #endregion

    #region COLLECTIONS

    public async Task<int> AddCollection(Collection collection)
    {
        await Collections.AddAsync(collection);
        await SaveChangesAsync();
        return collection.Id;
    }

    public async Task<Collection?> GetCollectionByFieldId(int id)
    {
        Field? field = await GetFieldById(id);
        if (field == null)
            return null;
        return await GetCollectionById(field.CollectionId);
    }

    public IEnumerable<Collection> GetCollectionsByAuthorId(int id)
    {
        return Collections.Where(c => c.AuthorId == id);
    }

    public async Task<Collection?> GetCollectionById(int id)
    {
        return await Collections.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task RemoveItemFromCollection(int itemId, int collectionId)
    {
        Collection? collection = await GetCollectionById(collectionId);
        if (collection == null)
            return;
        Item? item = await GetItemById(itemId);
        if (item == null)
            return;
        List<Data> data = await Data.Where(d => d.ItemId == itemId).ToListAsync();
        Data.RemoveRange(data);
        Items.Remove(item);
        collection.ItemsCount--;
        Collections.Update(collection);
        await SaveChangesAsync();
    }

    public async Task DeleteCollection(int id)
    {
        Collection? collection = await GetCollectionById(id);
        if (collection != null)
            Collections.Remove(collection);
        await SaveChangesAsync();
    }

    #endregion

    #region FIELDS

    public async Task<Field?> GetFieldById(int id)
    {
        return await Fields.FirstOrDefaultAsync(f => id == f.Id);
    }

    public IEnumerable<Field> GetFieldsByCollectionId(int id)
    {
        return Fields.Where(f => f.CollectionId == id);
    }

    public async Task AddField(Field field)
    {
        await Fields.AddAsync(field);
        await SaveChangesAsync();
    }

    #endregion

    #region DATA

    public IEnumerable<Data> GetDataByFieldId(int id)
    {
        return Data.Where(d => d.FieldId == id);
    }

    public async Task<bool> AddDataRange(Dictionary<int, string> range, Item item)
    {
        if (range.Count <= 0)
            return false;

        Collection? collection = await GetCollectionByFieldId(range.Keys.ElementAt(0));
        if (collection != null)
        {
            foreach (var fieldId in range.Keys)
            {
                Data data = new Data {Content = range[fieldId], ItemId = item.Id, FieldId = fieldId};
                Data.Add(data);
            }

            collection.ItemsCount++;
            Collections.Update(collection);
            await SaveChangesAsync();
            return true;
        }

        return false;
    }

    public async Task<bool> UpdateDataRange(Dictionary<int, string> range, int itemId, int collectionId)
    {
        if (range.Count <= 0)
            return false;

        Collection? collection = await GetCollectionById(collectionId);
        if (collection != null)
        {
            foreach (var fieldId in range.Keys)
            {
                Data? data = Data.FirstOrDefault(d => d.FieldId == fieldId && d.ItemId == itemId);
                if (data != null)
                {
                    data.Content = range[fieldId];
                    Data.Update(data);
                }
            }

            await SaveChangesAsync();
            return true;
        }

        return false;
    }

    public async Task<List<Data>> GetDataByItemId(int id)
    {
        return await Data.Where(d => d.ItemId == id).ToListAsync();
    }

    #endregion

    #region ITEMS

    public async Task<Item?> GetItemById(int id)
    {
        return await Items.FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task<bool> AddItem(Dictionary<int, string> range)
    {
        if (range.Count <= 0)
            return false;

        Collection? collection = await GetCollectionByFieldId(range.Keys.ElementAt(0));
        if (collection != null)
        {
            Item item = new Item {CollectionId = collection.Id};
            await Items.AddAsync(item);
            await SaveChangesAsync();
            if (await AddDataRange(range, item))
            {
                return true;
            }
            else
            {
                Items.Remove(item);
                await SaveChangesAsync();
            }
        }

        return false;
    }

    public IEnumerable<Item> GetItemsByCollectionId(int id)
    {
        return Items.Where(i => i.CollectionId == id);
    }

    #endregion

    #region LIKES

    public async Task AddLike(Like like)
    {
        await Likes.AddAsync(like);
        await SaveChangesAsync();
    }

    public async Task RemoveLike(int userId, int itemId)
    {
        Like? like = await Likes.FirstOrDefaultAsync(l => l.ItemId == itemId && l.UserId == userId);
        if (like != null)
        {
            Likes.Remove(like);
            await SaveChangesAsync();
        }
    }

    public async Task<bool> IsLiked(int userId, int itemId)
    {
        return await Likes.FirstOrDefaultAsync(l => l.ItemId == itemId && l.UserId == userId) != null;
    }

    public int GetLikesCount(int itemId)
    {
        return Likes.Count(l => l.ItemId == itemId);
    }

    #endregion
}