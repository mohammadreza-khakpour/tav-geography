using Geography.Infrastructure.Application;

namespace Geography.Persistence.EF
{
    public class EFUnitOfWork : UnitOfWork
    {
        private readonly EFDataContext _dataContext;

        public EFUnitOfWork(EFDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public void Begin()
        {
            _dataContext.Database.BeginTransaction();
        }

        public void CommitPartial()
        {
            _dataContext.SaveChanges();
        }

        public void Commit()
        {
            _dataContext.SaveChanges();
            _dataContext.Database.CommitTransaction();
        }

        public void Rollback()
        {
            _dataContext.Database.RollbackTransaction();
        }

        public void Complete()
        {
            _dataContext.SaveChanges();
        }
    }
}
