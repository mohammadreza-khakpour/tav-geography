namespace Geography.Infrastructure.Application
{
    public interface UnitOfWork
    {
        void Begin();
        void CommitPartial();
        void Commit();
        void Rollback();
        void Complete();
    }
}
