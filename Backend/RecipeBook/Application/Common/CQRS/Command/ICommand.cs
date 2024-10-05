using MediatR;

namespace Application.Common.CQRS.Command;

public interface ICommand<out TResponse> : IRequest<TResponse>
{ }
