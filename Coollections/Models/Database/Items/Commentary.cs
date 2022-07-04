namespace Coollections.Models.Database.Items;

public class Commentary
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int ItemId { get; set; }
    public string Content { get; set; } = null!;
}