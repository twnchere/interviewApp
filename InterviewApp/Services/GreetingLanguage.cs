namespace InterviewApp.Services;

internal static class GreetingLanguage
{
    public const string English = "English";
    public const string Afrikaans = "Afrikaans";
    public const string Zulu = "Zulu";

    public static bool TryResolve(string language, out string resolvedLanguage)
    {
        resolvedLanguage = (language ?? string.Empty).Trim().ToLowerInvariant() switch
        {
            "english" => English,
            "afrikaans" => Afrikaans,
            "zulu" => Zulu,
            _ => string.Empty
        };

        return resolvedLanguage.Length > 0;
    }
}
