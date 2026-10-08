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