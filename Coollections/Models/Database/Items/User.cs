using Coollections.Tools;
using Microsoft.AspNetCore.Identity;

namespace Coollections.Models.Database.Items;

[Serializable]
public class User
{
    public int Id { get; set; }
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Name { get; set; } = null!;

    public bool IsValidForLogin()
    {
        return DataChecker.StringsNotEmpty(Password, Email);
    }
    public bool IsValid()
    {
        return DataChecker.StringsNotEmpty(Name, Password, Email);
    }
}