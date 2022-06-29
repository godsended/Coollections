using Coollections.Models.Database.Items;

namespace Coollections.ViewModels;

public class CreateCollectionViewModel
{
    public IEnumerable<Subject> Subjects { get; set; } = null!;
}