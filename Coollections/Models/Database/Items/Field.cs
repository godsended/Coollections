namespace Coollections.Models.Database.Items;

public class Field
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int CollectionId { get; set; }
    public FieldType Type { get; set; }
    public enum FieldType
    {
        Text,
        TextArea,
        CheckBox,
        Date
    }
}