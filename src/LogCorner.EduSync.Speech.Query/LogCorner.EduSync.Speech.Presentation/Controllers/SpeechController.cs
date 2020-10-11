using LogCorner.EduSync.Speech.Application.UseCases;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace LogCorner.EduSync.Speech.Presentation.Controllers
{
    [ApiController]
    [Route("api/speech")]
    public class SpeechController : ControllerBase
    {
        private readonly ISpeechUseCase _getSpeechUseCase;

        public SpeechController(ISpeechUseCase getSpeechUseCase)
        {
            _getSpeechUseCase = getSpeechUseCase;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _getSpeechUseCase.Handle();

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await _getSpeechUseCase.Handle(id);

            return Ok(result);
        }
    }
}