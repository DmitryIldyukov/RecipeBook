using Application.Common.CQRS.Command;

namespace Application.UseCases.Commands.Recipe.Create;

public class CreateRecipeCommandHandler : ICommandHandler<CreateRecipeCommand>
{
    public Task Handle( CreateRecipeCommand command )
    {
        throw new NotImplementedException();
    }
}
