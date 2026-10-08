using InterviewApp.Models;
using InterviewApp.Services;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Xunit;

namespace InterviewApp.Tests;

public sealed class GreetingServiceTests
{
    [Fact]
    public void EnglishGreetingWorks()
    {
        GreetingService service = CreateGreetingService("Welcome");
        TimeGreetingService timeGreetingService = CreateTimeGreetingService(
            new DateTimeOffset(2026, 10, 8, 9, 0, 0, TimeSpan.Zero));

        Assert.Equal("Good morning! Welcome", service.GetGreeting(timeGreetingService.GetGreeting("English")));
    }

    [Fact]
    public void AfrikaansGreetingWorks()
    {
        GreetingService service = CreateGreetingService("Welcome");
        TimeGreetingService timeGreetingService = CreateTimeGreetingService(
            new DateTimeOffset(2026, 10, 8, 9, 0, 0, TimeSpan.Zero));

        Assert.Equal("Goeiemôre! Welcome", service.GetGreeting(timeGreetingService.GetGreeting("Afrikaans")));
    }

    [Fact]
    public void ZuluGreetingWorks()
    {
        GreetingService service = CreateGreetingService("Welcome");
        TimeGreetingService timeGreetingService = CreateTimeGreetingService(
            new DateTimeOffset(2026, 10, 8, 9, 0, 0, TimeSpan.Zero));

        Assert.Equal("Sawubona ekuseni! Welcome", service.GetGreeting(timeGreetingService.GetGreeting("Zulu")));
    }

    [Fact]
    public void UnsupportedLanguageFallsBackToEnglish()
    {
        GreetingService service = CreateGreetingService("Welcome");
        TimeGreetingService timeGreetingService = CreateTimeGreetingService(
            new DateTimeOffset(2026, 10, 8, 9, 0, 0, TimeSpan.Zero));

        Assert.Equal("Good morning! Welcome", service.GetGreeting(timeGreetingService.GetGreeting("Klingon")));
    }

    [Theory]
    [InlineData(11, "Good morning")]
    [InlineData(12, "Good afternoon")]
    [InlineData(17, "Good afternoon")]
    [InlineData(18, "Good evening")]
    public void TimeGreetingUsesTimeOfDay(int hour, string expected)
    {
        TimeGreetingService service = CreateTimeGreetingService(
            new DateTimeOffset(2026, 10, 8, hour, 0, 0, TimeSpan.Zero));

        Assert.Equal(expected, service.GetGreeting("English"));
    }

    [Fact]
    public void MissingConfigurationIsRejected()
    {
        GreetingOptionsValidator validator = new();

        ValidateOptionsResult result = validator.Validate(
            Options.DefaultName,
            new GreetingOptions());

        Assert.True(result.Failed);
        Assert.Contains("Greeting:Message must not be null or empty.", result.Failures);
        Assert.Contains("Greeting:Language must not be null or empty.", result.Failures);
    }

    private static GreetingService CreateGreetingService(string message)
    {
        GreetingOptions options = new()
        {
            Message = message
        };

        return new GreetingService(
            Options.Create(options),
            NullLogger<GreetingService>.Instance);
    }

    private static TimeGreetingService CreateTimeGreetingService(DateTimeOffset localTime) =>
        new(new FixedTimeProvider(localTime), NullLogger<TimeGreetingService>.Instance);

    private sealed class FixedTimeProvider(DateTimeOffset localTime) : TimeProvider
    {
        public override DateTimeOffset GetUtcNow() => localTime.ToUniversalTime();

        public override TimeZoneInfo LocalTimeZone => TimeZoneInfo.Utc;
    }
}
