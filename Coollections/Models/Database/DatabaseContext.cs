using System.Diagnostics;
using Coollections.Models.Database.Items;
using Microsoft.EntityFrameworkCore;

namespace Coollections.Models.Database;

public class DatabaseContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Field> Fields { get; set; }
    public DbSet<Subject> Subjects { get; set; }
    public DbSet<Data> Data { get; set; }
    public DbSet<Collection> Collections { get; set; }

    public DatabaseContext()
    {
    }

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

    public async Task AddUser(User user)
    {
        await Users.AddAsync(user).ConfigureAwait(false);
        await SaveChangesAsync().ConfigureAwait(false);
    }

    public async Task<bool> ContainsUserWithEmail(string email)
    {
        return await Users.FirstOrDefaultAsync(u => u.Email == email) != null;
    }
    
    public async Task<bool> IsLoginDataCorrect(string email, string password)
    {
        return await Users.FirstOrDefaultAsync(u => u.Email == email && u.Password == password) != null;
    }
}