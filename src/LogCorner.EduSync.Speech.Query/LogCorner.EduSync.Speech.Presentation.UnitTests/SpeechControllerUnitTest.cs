using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LogCorner.EduSync.Speech.Application.UseCases;
using LogCorner.EduSync.Speech.Presentation.Controllers;
using LogCorner.EduSync.Speech.ReadModel.SpeechReadModel;
using Microsoft.AspNetCore.Mvc;
using Moq;
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
                new SpeechView(Guid.NewGuid(), It.IsAny<string>(),It.IsAny<string>(),It.IsAny<string>(),It.IsAny<string>())
            };
            mockSpeechUseCase.Setup(m => m.Handle()).Returns(Task.FromResult(speeches));
            var speechController = new SpeechController(mockSpeechUseCase.Object);

            //Act
            var result = await speechController.Get();

            //Assert
            var actionResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<OkObjectResult>(actionResult);
            Assert.Equal(speeches, model.Value);
        }

        [Fact]
        public async Task ShouldGetSpeech()
        {
            //Arrange
            Guid id = Guid.NewGuid();
            var speech =
                new SpeechView(id, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<string>());
            var mockSpeechUseCase = new Mock<ISpeechUseCase>();
           
            mockSpeechUseCase.Setup(m => m.Handle(id)).Returns(Task.FromResult(speech));
            var speechController = new SpeechController(mockSpeechUseCase.Object);

            //Act
            var result = await speechController.Get(id);

            //Assert
            var actionResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<OkObjectResult>(actionResult);
            Assert.Equal(speech, model.Value);
        }
    }
}