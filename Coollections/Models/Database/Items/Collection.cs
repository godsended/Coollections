namespace Coollections.Models.Database.Items;

[Serializable]
public class Collection
{
    public int Id { get; set; }
    public int AuthorId { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public int SubjectId { get; set; }
    public string? Image { get; set; }
    public int ItemsCount { get; set; }
}