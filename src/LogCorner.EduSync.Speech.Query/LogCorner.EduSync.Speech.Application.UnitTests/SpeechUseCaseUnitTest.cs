using System;
using LogCorner.EduSync.Speech.Application.UseCases;
using LogCorner.EduSync.Speech.Infrastructure;
using LogCorner.EduSync.Speech.ReadModel.SpeechReadModel;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace LogCorner.EduSync.Speech.Application.UnitTests
{
    public class SpeechUseCaseUnitTest
    {
        [Fact]
        public async Task ShouldGetSpeechs()
        {
            //Arrange
            var mockElasticSearchClient = new Mock<IElasticSearchClient<SpeechView>>();
            IList<SpeechView> speeches = new List<SpeechView>
            {
                new SpeechView(Guid.NewGuid(), It.IsAny<string>(),It.IsAny<string>(),It.IsAny<string>(),It.IsAny<string>())
            };
            mockElasticSearchClient.Setup(m => m.Get()).Returns(Task.FromResult(speeches));
            ISpeechUseCase speechUseCase = new SpeechUseCase(mockElasticSearchClient.Object);
            
            //Act
            var result = await speechUseCase.Handle();

            //Assert
            Assert.Equal(speeches, result);
        }

        [Fact]
        public async Task ShouldGetSpeech()
        {
            //Arrange
            Guid id = Guid.NewGuid();
            var mockElasticSearchClient = new Mock<IElasticSearchClient<SpeechView>>();
            var speech =
                new SpeechView(id, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<string>());
           
            mockElasticSearchClient.Setup(m => m.Get(id)).Returns(Task.FromResult(speech));
            ISpeechUseCase speechUseCase = new SpeechUseCase(mockElasticSearchClient.Object);

            //Act
            var result = await speechUseCase.Handle(id);

            //Assert
            Assert.Equal(speech, result);
            Assert.Equal(speech.Id, result.Id);
            Assert.Equal(speech.Title, result.Title);
            Assert.Equal(speech.Description, result.Description);
            Assert.Equal(speech.Type, result.Type);
            Assert.Equal(speech.Url, result.Url);
        }
    }
}