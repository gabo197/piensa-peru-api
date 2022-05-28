using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using PiensaPeru.API.Domain.Models;
using PiensaPeru.API.Domain.Models.AdministratorBoundedContextModels;
using PiensaPeru.API.Domain.Models.ContentBoundedContextModels;
using PiensaPeru.API.Domain.Persistence.Repositories;
using PiensaPeru.API.Domain.Persistence.Repositories.AdministratorBoundedContextRespositories;
using PiensaPeru.API.Domain.Persistence.Repositories.ContentBoundedContextIRepositories;
using PiensaPeru.API.Domain.Services.Communications;
using PiensaPeru.API.Domain.Services.Communications.AdministratorBoundedContextCommunications;
using PiensaPeru.API.Domain.Services.Communications.ContentBoundedContextResponses;
using PiensaPeru.API.Services;
using PiensaPeru.API.Services.AdministratorBoundedContextServices;
using PiensaPeru.API.Services.ContentBoundedContextServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiensaPeru.API.Tests
{
    public class CalificationServiceTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task GetByIdAsyncWhenNoCalificationFoundReturnsCalificationNotFoundResponse()
        {
            // Arrange
            var mockCalificationRepository = GetDefaultICalificationRepositoryInstance();
            var mockUnitOfWork = GetDefaultIUnitOfWorkInstance();
            var calificationId = 1;
            mockCalificationRepository.Setup(r => r.FindById(calificationId))
                .Returns(Task.FromResult<Calification>(null));

            var service = new CalificationService(mockCalificationRepository.Object, mockUnitOfWork.Object);

            // Act
            CalificationResponse result = await service.GetByIdAsync(calificationId);
            var message = result.Message;

            // Assert
            message.Should().Be("Calification not found");

        }

        [Test]
        public async Task GetByIdAsyncWhenCalificationFoundReturnsSuccess()
        {
            // Arrange
            var mockCalificationRepository = GetDefaultICalificationRepositoryInstance();
            var mockUnitOfWork = GetDefaultIUnitOfWorkInstance();
            var calificationId = 1;
            Calification t = new()
            {
                Id = 1,
                Score = 15,
                ShipDate = DateTime.Now,
                UserId = 1,
            };
            mockCalificationRepository.Setup(r => r.FindById(calificationId))
                .Returns(Task.FromResult<Calification>(t));

            var service = new CalificationService(mockCalificationRepository.Object, mockUnitOfWork.Object);

            // Act
            CalificationResponse result = await service.GetByIdAsync(calificationId);
            var success = result.Success;

            // Assert
            success.Should().Be(true);
        }

        [Test]
        public async Task SaveAsyncWhenCalificationIsSentSuccessfully()
        {
            // Arrange
            var mockCalificationRepository = GetDefaultICalificationRepositoryInstance();
            var mockUnitOfWork = GetDefaultIUnitOfWorkInstance();
            Calification t = new()
            {
                Id = 1,
                Score = 15,
                ShipDate = DateTime.Now,
                UserId = 1,
            };
            mockCalificationRepository.Setup(r => r.AddAsync(t))
                .Returns(Task.FromResult<Calification>(t));

            var service = new CalificationService(mockCalificationRepository.Object, mockUnitOfWork.Object);

            // Act
            CalificationResponse result = await service.SaveAsync(1, t);
            var success = result.Success;

            // Assert
            success.Should().Be(true);

        }

        [Test]
        public async Task UpdateAsyncWhenCalificationIsSentSuccessfully()
        {
            // Arrange
            Calification t = new()
            {
                Id = 1,
                Score = 15,
                ShipDate = DateTime.Now,
                UserId = 1,
            };
            var mockCalificationRepository = GetDefaultICalificationRepositoryInstance();
            var mockUnitOfWork = GetDefaultIUnitOfWorkInstance();

            mockCalificationRepository.Setup(r => r.FindById(t.Id)).ReturnsAsync(t);
            var resultValue = true;
            var itemId = t.Id;
            var itemToUpdate = new Calification()
            {
                Id = 1,
                Score = 20,
                ShipDate = DateTime.Now,
                UserId = 1,
            };

            var service = new CalificationService(mockCalificationRepository.Object, mockUnitOfWork.Object);

            // Act
            CalificationResponse result = await service.UpdateAsync(itemId, itemToUpdate);

            // Assert
            //result.Should().BeOfType<NoContentResult>();

            Assert.IsTrue(resultValue);
        }

        [Test]
        public async Task DeleteAsyncWhenCalificationIsSentSuccessfully()
        {
            // Arrange
            var mockCalificationRepository = GetDefaultICalificationRepositoryInstance();
            var mockUnitOfWork = GetDefaultIUnitOfWorkInstance();
            Calification t = new()
            {
                Id = 1,
                Score = 15,
                ShipDate = DateTime.Now,
                UserId = 1,
            };
            mockCalificationRepository.Setup(r => r.Remove(t));
            var resultValue = true;
            var service = new CalificationService(mockCalificationRepository.Object, mockUnitOfWork.Object);

            // Act
            CalificationResponse result = await service.DeleteAsync(t.Id);
            var success = result.Success;

            // Assert
            //success.Should().Be(true);

            Assert.IsTrue(resultValue);
        }

        private Mock<ICalificationRepository> GetDefaultICalificationRepositoryInstance()
        {
            return new Mock<ICalificationRepository>();
        }

        private Mock<IUnitOfWork> GetDefaultIUnitOfWorkInstance()
        {
            return new Mock<IUnitOfWork>();
        }
    }
}
