 # InterviewApp

This is a starter .NET Core console application designed for interview purposes. The goal is to evaluate your understanding of:

- .NET Core Console Application structure
- Dependency Injection (DI)
- Configuration via `appsettings.json`
- MediatR for decoupled request handling
- Debugging
- Clean code and extensibility principles

---

## 🧠 Tasks

Please complete the following tasks. You may use any libraries or patterns you are comfortable with, but aim for clarity, maintainability, and testability.

### 1. Extend the Greeting Service

- Modify `GreetingService` to support multiple languages (e.g., English, Afrikaans, Zulu).
- Use the `Language` property from `appsettings.json` to determine which greeting to display.
- Add a fallback if the language is not supported.

### 2. Add Logging

- Inject `ILogger<GreetingService>` and log:
    - When the service starts
    - What message is displayed
    - Any errors or unsupported languages
  
### 3. Validate Configuration

- Ensure that the `Message` and `Language` values in `appsettings.json` are not null or empty.
- If validation fails, log an error and exit gracefully.

### 4. Add a Time-Based Greeting Option

- Add a new service `ITimeGreetingService` that returns a greeting based on the current time (e.g., "Good morning", "Good afternoon").
- Inject this service into `GreetingService` and combine it with the configured message.

### 5. Unit Tests (Optional)

- Write unit tests for `GreetingService` using a testing framework of your choice (e.g., xUnit, NUnit).
- Mock dependencies where appropriate.

---

## 🧭 Advanced Tasks: MediatR Integration

These tasks are designed to assess your familiarity with the MediatR library and how to use it to decouple application logic.

### 6. Integrate MediatR

- Add the MediatR NuGet package to the project.
- Register MediatR in the DI container.

### 7. Create a Greeting Request

- Define a `GreetUserCommand` (or `Query`) that encapsulates the greeting logic.
- Implement a `GreetUserHandler` that handles the command and returns the greeting message.
- Replace the direct call to `GreetingService.Run()` with a MediatR `Send()` call.

### 8. Add a Time-Based Greeting Request
- Create a separate `GetTimeGreetingQuery` and handler.
- Combine the result with the configured greeting message using MediatR.

---

## 🧰 Suggested NuGet Packages

```bash
dotnet add package MediatR
dotnet add package MediatR.Extensions.Microsoft.DependencyInjection
```

---

## 🛠️ Setup Instructions

1. Ensure you have .NET 8 SDK or later installed
2. Run the application:

Note: You may use any IDE of the following: Visual Studio, Visual Studio Code, or Jetbrains Rider

```bash
dotnet run
```

Run the xUnit tests from the application directory with:

```bash
dotnet test ..\InterviewApp.Tests\InterviewApp.Tests.csproj
```

## Implemented behavior

- `Greeting:Language` accepts English, Afrikaans, or Zulu (case-insensitive); unsupported values fall back to English.
- The time greeting uses the machine's local time: morning before 12:00, afternoon before 18:00, and evening afterward.
- On interactive runs, the console waits for a keypress after displaying the greeting so it remains visible; redirected runs exit without waiting.
- `Greeting:Message` and `Greeting:Language` are validated at startup. Invalid values are logged and the application exits with a nonzero code.
- The console sends a `GreetUserCommand` through MediatR. Its handler sends a `GetTimeGreetingQuery` and combines the result with the configured message.

## Architecture

```text
Program
  -> MediatR sends GreetUserCommand
  -> GreetUserHandler
  -> MediatR sends GetTimeGreetingQuery
  -> GetTimeGreetingHandler
  -> TimeGreetingService selects a greeting by language and local time
  -> GreetingService combines it with Greeting:Message
  -> Program prints the final greeting
```

The Options pattern binds and validates greeting configuration at startup. `TimeProvider`
supplies local time to `TimeGreetingService`, allowing tests to use a fixed time instead
of depending on the clock. MediatR keeps request dispatch separate from the greeting
services, while each service has one clear responsibility: selecting the time/language
greeting or composing the final message.