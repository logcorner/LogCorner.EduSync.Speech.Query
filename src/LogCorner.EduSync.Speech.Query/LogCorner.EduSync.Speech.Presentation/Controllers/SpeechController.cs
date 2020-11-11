﻿using LogCorner.EduSync.Speech.Application.UseCases;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LogCorner.EduSync.Speech.ReadModel.SpeechReadModel;

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