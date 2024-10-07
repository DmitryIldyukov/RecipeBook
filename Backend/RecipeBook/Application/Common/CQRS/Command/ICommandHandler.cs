namespace Application.Common.CQRS.Command;

public interface ICommandHandler<in TCommand, TResponse>
    where TCommand : class
{
    Task<TResponse> Handle( TCommand command );
}

public interface ICommandHandler<in TCommand>
    where TCommand : class
{
    Task Handle( TCommand command );
}