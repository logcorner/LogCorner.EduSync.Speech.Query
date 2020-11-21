using LogCorner.EduSync.Speech.Infrastructure.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LogCorner.EduSync.Speech.Application.Interfaces
{
    public interface IQueryHandler<T> : IQuery
    {
        Task<IEnumerable<T>> Handle();

        Task<SearchResult<T>> Handle(int page, int size);

        Task<T> Handle(Guid id);
    }
}