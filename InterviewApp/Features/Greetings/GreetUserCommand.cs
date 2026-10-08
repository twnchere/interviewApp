using MediatR;

namespace InterviewApp.Features.Greetings;

public sealed record GreetUserCommand : IRequest<string>;
