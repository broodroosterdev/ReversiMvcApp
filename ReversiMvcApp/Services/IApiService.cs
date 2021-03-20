using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using ReversiMvcApp.Helpers;
using ReversiMvcApp.Schemas;

namespace ReversiMvcApp.Services
{
    public interface IApiService
    {
        public Task<Result<List<PendingGame>>> GetPendingGames();
        public Task<Result<string>> CreateNew(string playerToken, string description);
        public Task<Result<Game>> GetGame(string gameToken);
    }
}