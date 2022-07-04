using Coollections.Models.Database.Items;

namespace Coollections.ViewModels;

public class AdminViewModel
{
    public IEnumerable<User> Users { get; init; }
}