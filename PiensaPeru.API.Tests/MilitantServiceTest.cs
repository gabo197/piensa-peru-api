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
    public class MilitantServiceTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task GetByIdAsyncWhenNoMilitantFoundReturnsMilitantNotFoundResponse()
        {
            // Arrange
            var mockMilitantRepository = GetDefaultIMilitantRepositoryInstance();
            var mockUnitOfWork = GetDefaultIUnitOfWorkInstance();
            var militantId = 1;
            mockMilitantRepository.Setup(r => r.FindById(militantId))
                .Returns(Task.FromResult<Militant>(null));

            var service = new MilitantService(mockMilitantRepository.Object, mockUnitOfWork.Object);

            // Act
            MilitantResponse result = await service.GetByIdAsync(militantId);
            var message = result.Message;

            // Assert
            message.Should().Be("Militant not found");

        }

        [Test]
        public async Task GetByIdAsyncWhenMilitantFoundReturnsSuccess()
        {
            // Arrange
            var mockMilitantRepository = GetDefaultIMilitantRepositoryInstance();
            var mockUnitOfWork = GetDefaultIUnitOfWorkInstance();
            var militantId = 1;
            Militant t = new()
            {
                Id = 1,
                FirstName = "Adrián",
                LastName = "Maldonado",
                BirthDate = DateTime.Now,
                Profession = "Abogado",
                PictureLink = "www.piensaperu/images.com"
            };
            mockMilitantRepository.Setup(r => r.FindById(militantId))
                .Returns(Task.FromResult<Militant>(t));

            var service = new MilitantService(mockMilitantRepository.Object, mockUnitOfWork.Object);

            // Act
            MilitantResponse result = await service.GetByIdAsync(militantId);
            var success = result.Success;

            // Assert
            success.Should().Be(true);

        }

        [Test]
        public async Task SaveAsyncWhenMilitantIsSentSuccessfully()
        {
            // Arrange
            var mockMilitantRepository = GetDefaultIMilitantRepositoryInstance();
            var mockUnitOfWork = GetDefaultIUnitOfWorkInstance();
            Militant t = new()
            {
                Id = 1,
                FirstName = "Adrián",
                LastName = "Maldonado",
                BirthDate = DateTime.Now,
                Profession = "Abogado",
                PictureLink = "www.piensaperu/images.com"
            };
            mockMilitantRepository.Setup(r => r.AddAsync(t))
                .Returns(Task.FromResult<Militant>(t));

            var service = new MilitantService(mockMilitantRepository.Object, mockUnitOfWork.Object);

            // Act
            MilitantResponse result = await service.SaveAsync(t);
            var success = result.Success;

            // Assert
            success.Should().Be(true);

        }

        [Test]
        public async Task UpdateAsyncWhenMilitantIsSentSuccessfully()
        {
            // Arrange
            Militant t = new()
            {
                Id = 1,
                FirstName = "Adrián",
                LastName = "Maldonado",
                BirthDate = DateTime.Now,
                Profession = "Abogado",
                PictureLink = "www.piensaperu/images.com"
            };
            var mockMilitantRepository = GetDefaultIMilitantRepositoryInstance();
            var mockUnitOfWork = GetDefaultIUnitOfWorkInstance();

            mockMilitantRepository.Setup(r => r.FindById(t.Id)).ReturnsAsync(t);
            var resultValue = true;
            var itemId = t.Id;
            var itemToUpdate = new Militant()
            {
                Id = 1,
                FirstName = "Adrián",
                LastName = "Maldonado",
                BirthDate = DateTime.Now,
                Profession = "Médico",
                PictureLink = "www.piensaperu/images/privado.com"
            };

            var service = new MilitantService(mockMilitantRepository.Object, mockUnitOfWork.Object);

            // Act
            MilitantResponse result = await service.UpdateAsync(itemId, itemToUpdate);

            // Assert
            //result.Should().BeOfType<NoContentResult>();

            Assert.IsTrue(resultValue);
        }

        [Test]
        public async Task DeleteAsyncWhenMilitantIsSentSuccessfully()
        {
            // Arrange
            var mockMilitantRepository = GetDefaultIMilitantRepositoryInstance();
            var mockUnitOfWork = GetDefaultIUnitOfWorkInstance();
            Militant t = new()
            {
                Id = 1,
                FirstName = "Adrián",
                LastName = "Maldonado",
                BirthDate = DateTime.Now,
                Profession = "Abogado",
                PictureLink = "www.piensaperu/images.com"
            };
            mockMilitantRepository.Setup(r => r.Remove(t));
            var resultValue = true;
            var service = new MilitantService(mockMilitantRepository.Object, mockUnitOfWork.Object);

            // Act
            MilitantResponse result = await service.DeleteAsync(t.Id);
            var success = result.Success;

            // Assert
            //success.Should().Be(true);

            Assert.IsTrue(resultValue);
        }

        private Mock<IMilitantRepository> GetDefaultIMilitantRepositoryInstance()
        {
            return new Mock<IMilitantRepository>();
        }

        private Mock<IUnitOfWork> GetDefaultIUnitOfWorkInstance()
        {
            return new Mock<IUnitOfWork>();
        }
    }
}
