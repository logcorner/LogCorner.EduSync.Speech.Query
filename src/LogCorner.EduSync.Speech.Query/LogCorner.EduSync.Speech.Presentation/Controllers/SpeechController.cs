using LogCorner.EduSync.Speech.Application.UseCases;
using LogCorner.EduSync.Speech.Presentation.Models;
using LogCorner.EduSync.Speech.ReadModel.SpeechReadModel;
using LogCorner.EduSync.Speech.Telemetry;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace LogCorner.EduSync.Speech.Presentation.Controllers
{
    [ApiController]
    [Route("api/speech")]
    public class SpeechController : ControllerBase
    {
        private readonly ISpeechUseCase _getSpeechUseCase;

        private static readonly ActivitySource Activity = new("query-api");
        private readonly ITraceService _traceService;

        public SpeechController(ISpeechUseCase getSpeechUseCase, ITraceService traceService)
        {
            _getSpeechUseCase = getSpeechUseCase;
            _traceService = traceService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            using var activity = Activity.StartActivity("Query data");
            var result = await _getSpeechUseCase.Handle();

            return Ok(result);
        }

        [HttpGet("paginated")]
        public async Task<IActionResult> Get([FromQuery] QueryModel model)
        {
            using var activity = Activity.StartActivity("Query data with pagination");
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
            var tags = new Dictionary<string, object>
            {
                { "model.Page", model.Page },
                { "model.Size", model.Size }
            };
            _traceService.SetActivityTags(activity, tags);
            return Ok(result);
        }

        [HttpGet("types")]
        public async Task<IActionResult> GetTypes()
        {
            var result = new List<SpeechType>
            {
                new(1,"SelfPacedLabs"),
                new(2,"TraingVideo"),
                new(3,"Conferences")
            };
            await Task.CompletedTask;
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            using var activity = Activity.StartActivity("Query data ById");
            var result = await _getSpeechUseCase.Handle(id);
            var tags = new Dictionary<string, object>
            {
                { "Id", id }
            };
            _traceService.SetActivityTags(activity, tags);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
    }
}