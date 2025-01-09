using Accounts.Core.Models;
using Accounts.Core.Repositories;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;

namespace Accounts.Core.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SeriesMasterController : ControllerBase
    {
        private readonly ILogger<SeriesMasterController> _logger;
        private readonly ISeriesMasterRepository _seriesMasterRepository;

        public SeriesMasterController(ILogger<SeriesMasterController> logger,
            ISeriesMasterRepository seriesMasterRepository)
        {
            _logger = logger;
            _seriesMasterRepository = seriesMasterRepository;
        }

        /// <summary>
        /// Read all SeriesMaster from table.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<SeriesMaster>> Get()
        {
            return await _seriesMasterRepository.GetAllSeriesMasters();
        }

        /// <summary>
        /// Read SeriesMaster from table.
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetSeriesMaster/{seriesMasterId}")]
        public async Task<SeriesMaster> GetRow(long seriesMasterId, int pageIndex, int pageSize)
        {
            return await _seriesMasterRepository.GetQuery(seriesMasterId, 0, 1);
        }

        /// <summary>
        /// Read list of SeriesMaster from table.
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetSeriesMasterWithPagging")]
        public async Task<List<SeriesMaster>> GetseriesMasterWithPagging(int pageIndex, int pageSize)
        {
            var result = await _seriesMasterRepository.GetQuery(pageIndex, pageSize);
            return result;
        }

        /// <summary>
        /// Create SeriesMaster.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<SeriesMaster> Post(SeriesMaster seriesMaster)
        {
            return await _seriesMasterRepository.AddSeriesMasterAsync(seriesMaster);
        }

        /// <summary>
        /// Update SeriesMaster.
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        public async Task<SeriesMaster> Put(SeriesMaster seriesMaster)
        {
            return await _seriesMasterRepository.UpdateSeriesMasterAsync(seriesMaster);
        }

        [HttpPut("ActiveSeries")]
        public async Task<bool> ActiveSeriesAsync(long seriesMasterId, long userId, bool status)
        {
            return await _seriesMasterRepository.ActiveInActiveSeries(seriesMasterId, userId, status);
        }

        /// <summary>
        /// Delete SeriesMaster.
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{seriesMasterId}")]
        public async Task<bool> Delete(long seriesMasterId)
        {
            return await _seriesMasterRepository.DeleteSeriesMasterAsync(seriesMasterId);
        }
    }
}
