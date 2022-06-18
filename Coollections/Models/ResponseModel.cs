namespace Coollections.Models;

[Serializable]
public class ResponseModel
{
    public bool IsSuccess { get; set; }
    public string? Message { get; set; }
    public int? Code { get; set; }
}