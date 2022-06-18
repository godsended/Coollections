namespace Coollections.Models.Database.Items;

[Serializable]
public class Collection
{
    public int Id { get; set; }
    public int AuthorId { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public Subject Subject { get; set; } = null!;
    public string? Image { get; set; }
}