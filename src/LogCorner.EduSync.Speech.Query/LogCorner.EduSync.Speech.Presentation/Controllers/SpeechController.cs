using LogCorner.EduSync.Speech.Application.UseCases;
using LogCorner.EduSync.Speech.Presentation.Models;
using LogCorner.EduSync.Speech.ReadModel.SpeechReadModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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

        [HttpGet("paginated")]
        public async Task<IActionResult> Get([FromQuery] QueryModel model)
        {
            if (model == null)
            {
                return BadRequest(" The querystring is not valid : provide (page number and page size");
            }
            if (model.Page == 0)
            {
                return BadRequest($" The querystring is not valid : (page number = {model.Page})");
            }
            if (model.Size == 0)
            {
                return BadRequest($" The querystring is not valid : (page size ={model.Size})");
            }
            var result = await _getSpeechUseCase.Handle(model.Page, model.Size);

            return Ok(result);
        }

        //TODO : implement this
        [HttpGet("types")]
        public async Task<IActionResult> GetTypes()
        {
            var result = new List<SpeechType>
            {
                new SpeechType(1,"SelfPacedLabs"),
                new SpeechType(2,"TraingVideo"),
                new SpeechType(3,"Conferences")
            };
            await Task.CompletedTask;
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