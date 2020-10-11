using LogCorner.EduSync.Speech.Infrastructure;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LogCorner.EduSync.Speech.ReadModel.SpeechReadModel;

namespace LogCorner.EduSync.Speech.Application.UseCases
{
    public class SpeechUseCase : ISpeechUseCase
    {
        private readonly IElasticSearchClient<SpeechView> _repo;

        public SpeechUseCase(IElasticSearchClient<SpeechView> repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<SpeechView>> Handle()
        {
            return await _repo.Get();
        }

        public async Task<SpeechView> Handle(Guid id)
        {
            return await _repo.Get(id);
        }
    }
}