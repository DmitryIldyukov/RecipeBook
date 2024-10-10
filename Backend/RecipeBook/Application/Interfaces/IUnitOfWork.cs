namespace Application.Interfaces;

public interface IUnitOfWork
{
    Task Commit();
}
