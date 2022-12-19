using LogCorner.EduSync.Speech.Infrastructure;
using LogCorner.EduSync.Speech.Infrastructure.Model;
using LogCorner.EduSync.Speech.ReadModel.SpeechReadModel;
using LogCorner.EduSync.Speech.Resiliency;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LogCorner.EduSync.Speech.Application.UseCases
{
    public class SpeechUseCase : ISpeechUseCase
    {
        private readonly IElasticSearchClient<SpeechView> _repo;
        private readonly IResiliencyService _resiliencyService;

        public SpeechUseCase(IElasticSearchClient<SpeechView> repo, IResiliencyService resiliencyService)
        {
            _repo = repo;
            _resiliencyService = resiliencyService;
        }

        public async Task<IEnumerable<SpeechView>> Handle()
        {
            return await _resiliencyService.ExponentialExceptionRetry.ExecuteAsync(async () => await _repo.Get());
        }

        public async Task<SearchResult<SpeechView>> Handle(int page, int size)
        {
            return await _repo.Get(page, size);
        }

        public async Task<SpeechView> Handle(Guid id)
        {
            return await _repo.Get(id);
        }
    }
}