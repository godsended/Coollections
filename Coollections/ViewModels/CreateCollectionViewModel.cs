using Coollections.Models.Database.Items;

namespace Coollections.ViewModels;

public class CreateCollectionViewModel : ViewModel
{
    public IEnumerable<Subject> Subjects { get; set; } = null!;
}