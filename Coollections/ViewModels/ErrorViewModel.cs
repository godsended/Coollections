namespace Coollections.ViewModels;

public class ErrorViewModel : ViewModel
{
    public string? RequestId { get; set; }

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}