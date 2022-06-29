namespace Coollections.Models;

[Serializable]
public class CreateCollectionRequestModel
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Subject { get; set; } = null!;
    public string? ImageUrl { get; set; }
    public string[]? FieldsTypes { get; set; } 
    public string[]? FieldsNames { get; set; }
}