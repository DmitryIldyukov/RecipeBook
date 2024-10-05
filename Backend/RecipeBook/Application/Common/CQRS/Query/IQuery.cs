using MediatR;

namespace Application.Common.CQRS.Query;

public interface IQuery<out TResponse> : IRequest<TResponse>
{ }
