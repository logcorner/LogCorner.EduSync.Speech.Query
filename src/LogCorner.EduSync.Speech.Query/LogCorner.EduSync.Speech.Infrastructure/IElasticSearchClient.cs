using System;
using LogCorner.EduSync.Speech.ReadModel;
using LogCorner.EduSync.Speech.ReadModel.SpeechReadModel;

namespace LogCorner.EduSync.Speech.Infrastructure
{
    public interface IElasticSearchClient<T> : IRepository<T, Guid> where T : Entity<Guid>
    {
       
    }
}