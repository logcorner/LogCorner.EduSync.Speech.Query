using LogCorner.EduSync.Speech.ReadModel.SpeechReadModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LogCorner.EduSync.Speech.ReadModel
{
    public interface IRepository<T, TIdentifier> where T : Entity<TIdentifier>
    {
        Task<IList<T>> Get();

        Task<T> Get(Guid id);
    }
}