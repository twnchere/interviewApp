using MediatR;

namespace InterviewApp.Features.Greetings;

public sealed record GetTimeGreetingQuery(string Language) : IRequest<string>;
