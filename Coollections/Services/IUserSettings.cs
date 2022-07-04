namespace Coollections.Services;

public interface IUserSettings
{
    Theme UserTheme { get; set; }
    Language UserLanguage { get; set; }
    
    public enum Language
    {
        English,
        Russian
    }
    public enum Theme
    {
        Light,
        Dark
    }
}