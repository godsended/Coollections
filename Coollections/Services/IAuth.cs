namespace Coollections.Services;

public interface IAuth
{
    public Task Authenticate(string email, bool isAdmin);
    public Task<bool> IsAuthorized();
    public int? GetUserId();
    public Task<bool> HasAccessToCollection(int collectionId);
}