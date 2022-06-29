using Coollections.Models.Database.Items;

namespace Coollections.ViewModels;

public class CollectionViewModel
{
    private readonly List<Data> data = null!;
    private readonly List<Field>? fields;
    private int checkboxId = 0;
    private int idCalls = 0;
    public Collection Collection { get; set; } = null!;

    public List<Data> Data
    {
        get => data;
        init
        {
            if (value.Count > 0)
            {
                data = value;
                data.Sort((a, b) => a.Item.CompareTo(b.Item));
                List<Data> group = new();
                int itemNum = data[0].Item;
                for (int i = 0; i < data.Count; i++)
                {
                    Data item = data[i];
                    if (group.Count == 0)
                        group.Add(item);
                    else
                    {
                        if (itemNum != item.Item)
                        {
                            group.Sort((a, b) => a.FieldId.CompareTo(b.FieldId));
                            DataGroups.Add(group);
                            itemNum = item.Item;
                            group = new List<Data>();
                        }

                        group.Add(item);
                    }
                }

                if (group.Count != 0)
                    DataGroups.Add(group);
            }
        }
    }

    public List<Field>? Fields
    {
        get => fields;
        init
        {
            fields = value;
            fields?.Sort((a, b) => a.Id.CompareTo(b.Id));
            NameFieldId = fields!.First(f => f.Name == "Name").Id;
            TagsFieldId = fields!.First(f => f.Name == "Tags").Id;
        }
    }

    public string AuthorName { get; set; } = null!;

    public List<List<Data>> DataGroups { get; } = new();

    public int NameFieldId { get; set; }
    public int TagsFieldId { get; set; }

    public int NextCheckboxId
    {
        get
        {
            idCalls++;
            int id = checkboxId;
            if (idCalls % 2 == 0)
                checkboxId++;
            return id;
        }
    }
}