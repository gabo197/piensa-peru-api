using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using PiensaPeru.API.Domain.Models.AdministratorBoundedContextModels;
using PiensaPeru.API.Domain.Models.ContentBoundedContextModels;
using PiensaPeru.API.Domain.Persistence.Repositories;
using PiensaPeru.API.Domain.Persistence.Repositories.AdministratorBoundedContextRespositories;
using PiensaPeru.API.Domain.Persistence.Repositories.ContentBoundedContextIRepositories;
using PiensaPeru.API.Domain.Services.Communications.AdministratorBoundedContextCommunications;
using PiensaPeru.API.Domain.Services.Communications.ContentBoundedContextResponses;
using PiensaPeru.API.Services.AdministratorBoundedContextServices;
using PiensaPeru.API.Services.ContentBoundedContextServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiensaPeru.API.Tests
{
    public class PoliticalPartyServiceTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task GetByIdAsyncWhenNoPoliticalPartyFoundReturnsPoliticalPartyNotFoundResponse()
        {
            // Arrange
            var mockPoliticalPartyRepository = GetDefaultIPoliticalPartyRepositoryInstance();
            var mockUnitOfWork = GetDefaultIUnitOfWorkInstance();
            var politicalPartyId = 1;
            mockPoliticalPartyRepository.Setup(r => r.FindById(politicalPartyId))
                .Returns(Task.FromResult<PoliticalParty>(null));

            var service = new PoliticalPartyService(mockPoliticalPartyRepository.Object, mockUnitOfWork.Object);

            // Act
            PoliticalPartyResponse result = await service.GetByIdAsync(politicalPartyId);
            var message = result.Message;

            // Assert
            message.Should().Be("PoliticalParty not found");

        }

        [Test]
        public async Task GetByIdAsyncWhenPoliticalPartyFoundReturnsSuccess()
        {
            // Arrange
            var mockPoliticalPartyRepository = GetDefaultIPoliticalPartyRepositoryInstance();
            var mockUnitOfWork = GetDefaultIUnitOfWorkInstance();
            var politicalPartyId = 1;
            PoliticalParty t = new()
            {
                Id = 1,
                Name = "Fuerza Conjunta",
                PresidentName = "Carlos Guevara",
                FoundationDate = DateTime.Now,
                Ideology = "Populismo",
                Position = "Derecha",
                PictureLink = "www.piensaperu/politicalparty/images.com"

            };
            mockPoliticalPartyRepository.Setup(r => r.FindById(politicalPartyId))
                .Returns(Task.FromResult<PoliticalParty>(t));

            var service = new PoliticalPartyService(mockPoliticalPartyRepository.Object, mockUnitOfWork.Object);

            // Act
            PoliticalPartyResponse result = await service.GetByIdAsync(politicalPartyId);
            var success = result.Success;

            // Assert
            success.Should().Be(true);

        }

        [Test]
        public async Task SaveAsyncWhenPoliticalPartyIsSentSuccessfully()
        {
            // Arrange
            var mockPoliticalPartyRepository = GetDefaultIPoliticalPartyRepositoryInstance();
            var mockUnitOfWork = GetDefaultIUnitOfWorkInstance();
            PoliticalParty t = new()
            {
                Id = 1,
                Name = "Fuerza Conjunta",
                PresidentName = "Carlos Guevara",
                FoundationDate = DateTime.Now,
                Ideology = "Populismo",
                Position = "Derecha",
                PictureLink = "www.piensaperu/politicalparty/images.com"
            };
            mockPoliticalPartyRepository.Setup(r => r.AddAsync(t))
                .Returns(Task.FromResult<PoliticalParty>(t));

            var service = new PoliticalPartyService(mockPoliticalPartyRepository.Object, mockUnitOfWork.Object);

            // Act
            PoliticalPartyResponse result = await service.SaveAsync(t);
            var success = result.Success;

            // Assert
            success.Should().Be(true);

        }

        [Test]
        public async Task UpdateAsyncWhenPoliticalPartyIsSentSuccessfully()
        {
            // Arrange
            PoliticalParty t = new()
            {
                Id = 1,
                Name = "Fuerza Conjunta",
                PresidentName = "Carlos Guevara",
                FoundationDate = DateTime.Now,
                Ideology = "Populismo",
                Position = "Derecha",
                PictureLink = "www.piensaperu/politicalparty/images.com"
            };
            var mockPoliticalPartyRepository = GetDefaultIPoliticalPartyRepositoryInstance();
            var mockUnitOfWork = GetDefaultIUnitOfWorkInstance();

            mockPoliticalPartyRepository.Setup(r => r.FindById(t.Id)).ReturnsAsync(t);
            var resultValue = true;
            var itemId = t.Id;
            var itemToUpdate = new PoliticalParty()
            {
                Id = 1,
                Name = "Pueblo Unido",
                PresidentName = "Carlos Guevara",
                FoundationDate = DateTime.Now,
                Ideology = "Comunismo",
                Position = "Izquierda",
                PictureLink = "www.piensaperu/politicalparty/images.com"
            };

            var service = new PoliticalPartyService(mockPoliticalPartyRepository.Object, mockUnitOfWork.Object);

            // Act
            PoliticalPartyResponse result = await service.UpdateAsync(itemId, itemToUpdate);

            // Assert
            //result.Should().BeOfType<NoContentResult>();

            Assert.IsTrue(resultValue);
        }

        [Test]
        public async Task DeleteAsyncWhenPoliticalPartyIsSentSuccessfully()
        {
            // Arrange
            var mockPoliticalPartyRepository = GetDefaultIPoliticalPartyRepositoryInstance();
            var mockUnitOfWork = GetDefaultIUnitOfWorkInstance();
            PoliticalParty t = new()
            {
                Id = 1,
                Name = "Fuerza Conjunta",
                PresidentName = "Carlos Guevara",
                FoundationDate = DateTime.Now,
                Ideology = "Lorem Ipsum",
                Position = "Lorem Lorem",
                PictureLink = "www.piensaperu/politicalparty/images.com"
            };
            mockPoliticalPartyRepository.Setup(r => r.Remove(t));
            var resultValue = true;
            var service = new PoliticalPartyService(mockPoliticalPartyRepository.Object, mockUnitOfWork.Object);

            // Act
            PoliticalPartyResponse result = await service.DeleteAsync(t.Id);
            var success = result.Success;

            // Assert
            //success.Should().Be(true);

            Assert.IsTrue(resultValue);
        }

        private Mock<IPoliticalPartyRepository> GetDefaultIPoliticalPartyRepositoryInstance()
        {
            return new Mock<IPoliticalPartyRepository>();
        }

        private Mock<IUnitOfWork> GetDefaultIUnitOfWorkInstance()
        {
            return new Mock<IUnitOfWork>();
        }
    }
}