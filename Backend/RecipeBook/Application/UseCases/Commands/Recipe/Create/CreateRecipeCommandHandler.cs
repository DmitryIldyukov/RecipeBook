using Application.Common.CQRS.Command;

namespace Application.UseCases.Commands.Recipe.Create;

public class CreateRecipeCommandHandler : ICommandHandler<CreateRecipeCommand, int>
{
    public Task<int> Handle( CreateRecipeCommand request, CancellationToken cancellationToken )
    {
        throw new NotImplementedException();
    }
}
