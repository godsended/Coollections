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

    public async Task<User> GetUserById(int id)
    {
        return (await Users.FirstOrDefaultAsync(u => u.Id == id))!;
    }

    public async Task<int> AddUser(User user)
    {
        await Users.AddAsync(user);
        await SaveChangesAsync();

        return (await Users.FirstOrDefaultAsync(u => u.Email == user.Email))!.Id;
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

    #endregion

    #region SUBJECTS

    public async Task<int> SubjectIdByName(string name)
    {
        return (await Subjects.FirstOrDefaultAsync(s => s.Name == name))!.Id;
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

    public async Task RemoveItemFromCollection(int item, int collectionId)
    {
        Collection? collection = await GetCollectionById(collectionId);
        if (collection == null)
            return;
        List<Data> data = await Data.Where(
                d => d.Item == item
                     && Fields.FirstOrDefault(
                         f => f.CollectionId == collectionId && d.FieldId == f.Id) != null)
            .ToListAsync();
        Data.RemoveRange(data);
        collection.ItemsCount--;
        Collections.Update(collection);
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

    public async Task<bool> AddDataRange(Dictionary<int, string> range)
    {
        if (range.Count <= 0)
            return false;

        Collection? collection = await GetCollectionByFieldId(range.Keys.ElementAt(0));
        if (collection != null)
        {
            foreach (var fieldId in range.Keys)
            {
                Data data = new Data {Content = range[fieldId], Item = collection.ItemsCount, FieldId = fieldId};
                Data.Add(data);
            }

            collection.ItemsCount++;
            Collections.Update(collection);
            await SaveChangesAsync();
            return true;
        }

        return false;
    }

    public async Task<bool> UpdateDataRange(Dictionary<int, string> range, int item, int collectionId)
    {
        if (range.Count <= 0)
            return false;

        Collection? collection = await GetCollectionById(collectionId);
        if (collection != null)
        {
            foreach (var fieldId in range.Keys)
            {
                Data? data = Data.FirstOrDefault(d => d.FieldId == fieldId && d.Item == item);
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

    #endregion
}