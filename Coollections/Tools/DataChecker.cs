namespace Coollections.Tools;

public static class DataChecker
{
    public static bool StringsNotEmpty(params string[] strings)
    {
        foreach (var s in strings)
        {
            if (string.IsNullOrEmpty(s) || string.IsNullOrWhiteSpace(s))
                return false;
        }
        return true;
    }
}