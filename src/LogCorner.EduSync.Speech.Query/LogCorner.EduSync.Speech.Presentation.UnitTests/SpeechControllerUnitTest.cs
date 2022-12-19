using LogCorner.EduSync.Speech.Application.UseCases;
using LogCorner.EduSync.Speech.Infrastructure.Model;
using LogCorner.EduSync.Speech.Presentation.Controllers;
using LogCorner.EduSync.Speech.Presentation.Models;
using LogCorner.EduSync.Speech.ReadModel.SpeechReadModel;
using LogCorner.EduSync.Speech.Telemetry;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace LogCorner.EduSync.Speech.Presentation.UnitTests
{
    public class SpeechControllerUnitTest
    {
        [Fact]
        public async Task ShouldGetSpeechs()
        {
            //Arrange
            var mockSpeechUseCase = new Mock<ISpeechUseCase>();
            IEnumerable<SpeechView> speeches = new List<SpeechView>
            {
                new SpeechView(Guid.NewGuid(), It.IsAny<string>(),It.IsAny<string>(),It.IsAny<string>(),It.IsAny<SpeechType>(),It.IsAny<int>())
            };
            mockSpeechUseCase.Setup(m => m.Handle()).Returns(Task.FromResult(speeches));
            var mockTraceService = new Mock<ITraceService>();
            mockTraceService.SetupAllProperties();
            var speechController = new SpeechController(mockSpeechUseCase.Object, mockTraceService.Object);

            //Act
            var result = await speechController.Get();

            //Assert
            var actionResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<OkObjectResult>(actionResult);
            Assert.Equal(speeches, model.Value);
        }

        [Fact]
        public async Task ShouldGetSpeechsWithPaginationWhenModelIsNullTaskShoudReturnBadRequest()
        {
            //Arrange

            var mockSpeechUseCase = new Mock<ISpeechUseCase>();
            var mockTraceService = new Mock<ITraceService>();
            mockTraceService.SetupAllProperties();
            var speechController = new SpeechController(mockSpeechUseCase.Object, mockTraceService.Object);

            //Act
            var result = await speechController.Get(null);

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(0, 1)]
        [InlineData(1, 0)]
        public async Task ShouldGetSpeechsWithPaginationWhenModelIsNotValidShoudReturnBadRequest(int page, int size)
        {
            //Arrange
            var queryModel = new QueryModel
            {
                Page = page,
                Size = size
            };

            var mockSpeechUseCase = new Mock<ISpeechUseCase>();
            var mockTraceService = new Mock<ITraceService>();
            mockTraceService.SetupAllProperties();

            var speechController = new SpeechController(mockSpeechUseCase.Object, mockTraceService.Object);

            //Act
            var result = await speechController.Get(queryModel);

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task ShouldGetSpeechsWithPagination()
        {
            //Arrange
            var queryModel = new QueryModel
            {
                Page = 1,
                Size = 2
            };
            IEnumerable<SpeechView> speeches = new List<SpeechView>
            {
                new SpeechView(Guid.NewGuid(), It.IsAny<string>(),It.IsAny<string>(),It.IsAny<string>(),It.IsAny<SpeechType>(),It.IsAny<int>())
            };

            var searchResult = new SearchResult<SpeechView>()
            {
                Results = speeches
            };
            var mockSpeechUseCase = new Mock<ISpeechUseCase>();

            mockSpeechUseCase.Setup(m => m.Handle(It.IsAny<int>(), It.IsAny<int>())).Returns(Task.FromResult(searchResult));
            var mockTraceService = new Mock<ITraceService>();
            mockTraceService.SetupAllProperties();
            var speechController = new SpeechController(mockSpeechUseCase.Object, mockTraceService.Object);

            //Act

            var result = await speechController.Get(queryModel);

            //Assert
            var actionResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<OkObjectResult>(actionResult);
            Assert.Equal(searchResult, model.Value);
        }

        [Fact]
        public async Task ShouldGetSpeech()
        {
            //Arrange
            Guid id = Guid.NewGuid();
            var speech =
                new SpeechView(id, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<SpeechType>(), It.IsAny<int>());
            var mockSpeechUseCase = new Mock<ISpeechUseCase>();

            mockSpeechUseCase.Setup(m => m.Handle(id)).Returns(Task.FromResult(speech));
            var mockTraceService = new Mock<ITraceService>();
            mockTraceService.SetupAllProperties();
            var speechController = new SpeechController(mockSpeechUseCase.Object, mockTraceService.Object);

            //Act
            var result = await speechController.Get(id);

            //Assert
            var actionResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<OkObjectResult>(actionResult);
            Assert.Equal(speech, model.Value);
        }
    }
}