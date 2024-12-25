using Accounts.Core.DbContext;
using Accounts.Core.Models;
using BaseClassLibrary.Interface;
using System.Linq.Expressions;

namespace Accounts.Core.Repositories
{
    public interface ISeriesMasterRepository
    {
        Task<List<SeriesMaster>> GetAllSeriesMasters();
        Task<SeriesMaster> AddSeriesMasterAsync(SeriesMaster seriesMaster);
        Task<SeriesMaster> UpdateSeriesMasterAsync(SeriesMaster seriesMaster);
        Task<bool> DeleteSeriesMasterAsync(long seriesMasterId);
        Task<List<SeriesMaster>> GetQuery(int pageIndex, int pageSize);
        Task<SeriesMaster> GetQuery(long seriesMasterId, int pageIndex, int pageSize);
    }
}

namespace Accounts.Core.Repositories
{
    public class SeriesMasterRepository : ISeriesMasterRepository
    {
        private readonly IBaseRepository<SeriesMaster, AppDbContext> _seriesMasterRepo;

        public SeriesMasterRepository(IBaseRepository<SeriesMaster, AppDbContext> seriesMasterRepo)
        {
            _seriesMasterRepo = seriesMasterRepo;
        }

        public async Task<SeriesMaster> AddSeriesMasterAsync(SeriesMaster seriesMaster)
        {
            try
            {
                await _seriesMasterRepo.BeginTransactionAsync();

                var result = await _seriesMasterRepo.AddAsync(seriesMaster);

                await _seriesMasterRepo.CommitTransactionAsync();

                return result;
            }
            catch (Exception)
            {
                await _seriesMasterRepo.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<bool> DeleteSeriesMasterAsync(long seriesMasterId)
        {
            await _seriesMasterRepo.DeleteAsync(seriesMasterId);
            return true;
        }

        public async Task<List<SeriesMaster>> GetAllSeriesMasters()
        {
            Expression<Func<SeriesMaster, bool>> predicate = c => c.Id > 0;

            return await _seriesMasterRepo.GetAllAsync(predicate);
        }

        public async Task<List<SeriesMaster>> GetQuery(int pageIndex, int pageSize)
        {
            return await _seriesMasterRepo.QueryAsync(
                query => query.Id > 0,
                orderBy: c => c.CreatedDate ?? DateTime.Now,
                pageIndex, pageSize);
        }

        public async Task<SeriesMaster> GetQuery(long seriesMasterId, int pageIndex, int pageSize)
        {
            var result = await _seriesMasterRepo.QueryAsync(
               query => query.Id == seriesMasterId,
               orderBy: c => c.CreatedDate,
               pageIndex, pageSize);

            return result?.FirstOrDefault();
        }

        public async Task<SeriesMaster> UpdateSeriesMasterAsync(SeriesMaster seriesMaster)
        {
            await _seriesMasterRepo.UpdateAsync(seriesMaster);
            return seriesMaster;
        }
    }
}
