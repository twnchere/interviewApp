using System;
using Microsoft.Extensions.Logging;

namespace InterviewApp.Services;

public sealed class TimeGreetingService(
    TimeProvider timeProvider,
    ILogger<TimeGreetingService> logger) : ITimeGreetingService
{
    public string GetGreeting(string language)
    {
        string resolvedLanguage;
        if (GreetingLanguage.TryResolve(language, out string supportedLanguage))
        {
            resolvedLanguage = supportedLanguage;
        }
        else
        {
            logger.LogWarning(
                "Language '{Language}' is not supported. Falling back to English.",
                language);
            resolvedLanguage = GreetingLanguage.English;
        }

        int hour = timeProvider.GetLocalNow().Hour;
        return resolvedLanguage switch
        {
            GreetingLanguage.Afrikaans => hour < 12 ? "Goeiemôre" : hour < 18 ? "Goeiemiddag" : "Goeienaand",
            GreetingLanguage.Zulu => hour < 12 ? "Sawubona ekuseni" : hour < 18 ? "Sawubona emini" : "Sawubona kusihlwa",
            _ => hour < 12 ? "Good morning" : hour < 18 ? "Good afternoon" : "Good evening"
        };
    }
}
