using Coollections.Models.Database.Items;

namespace Coollections.ViewModels;

public class UserViewModel : ViewModel
{
    public User User { get; set; } = null!;
    public IEnumerable<Collection>? Collections { get; set; }
    
    public bool IsEditable { get; set; }
}