using LogCorner.EduSync.Speech.Infrastructure.Model;
using LogCorner.EduSync.Speech.ReadModel;
using LogCorner.EduSync.Speech.ReadModel.SpeechReadModel;
using System;
using System.Threading.Tasks;

namespace LogCorner.EduSync.Speech.Infrastructure
{
    public interface IElasticSearchClient<T> : IRepository<T, Guid> where T : Entity<Guid>
    {
        Task<SearchResult<T>> Get(int page, int pageSize);
    }
}