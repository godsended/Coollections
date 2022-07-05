using Coollections.Models.Database.Items;

namespace Coollections.ViewModels;

public class AdminViewModel : ViewModel
{
    public IEnumerable<User> Users { get; init; }
}