using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Moq;
using Xunit;
using LW_4_3_5_Daryev_PI231.DTOs;
using LW_4_3_5_Daryev_PI231.Enumerations;
using LW_4_3_5_Daryev_PI231.Models;
using LW_4_3_5_Daryev_PI231.Repositories;
using LW_4_3_5_Daryev_PI231.Services;

namespace LW_4_3_5_Daryev_PI231.Tests
{
    public class GamingAssetServiceTests
    {
        private readonly Mock<IGamingAssetRepository> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly GamingAssetService _service;

        public GamingAssetServiceTests()
        {
            _mockRepo = new Mock<IGamingAssetRepository>();
            _mockMapper = new Mock<IMapper>();
            _service = new GamingAssetService(_mockRepo.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task CreateAssetAsync_ShouldMapHourlyRateAndSetStatus()
        {
            var testTime = DateTime.Now; 

            var createDto = new CreateAssetDTO
            {
                Name = "PlayStation 5",
                Type = DeviceType.Console,
                HourlyRate = testTime
            };

            var assetEntity = new GamingAsset
            {
                Name = "PlayStation 5",
                Type = DeviceType.Console,
                HourlyRate = testTime
            };

            var expectedDto = new AssetDTO
            {
                Id = "new-id-1",
                Name = "PlayStation 5",
                Type = DeviceType.Console,
                HourlyRate = testTime,
                Status = Status.Available
            };

            _mockMapper.Setup(m => m.Map<GamingAsset>(createDto)).Returns(assetEntity);
            _mockMapper.Setup(m => m.Map<AssetDTO>(It.IsAny<GamingAsset>())).Returns(expectedDto);

            var result = await _service.CreateAssetAsync(createDto);

            Assert.NotNull(result);
            Assert.Equal(Status.Available, assetEntity.Status); 
            Assert.Equal(testTime, result.HourlyRate);         

            _mockRepo.Verify(r => r.CreateAsync(It.IsAny<GamingAsset>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAssetAsync_ShouldReturnFalse_WhenStatusIsRented()
        {
            string testId = "asset-123";
            var rentedAsset = new GamingAsset
            {
                Id = testId,
                Name = "VR Headset",
                HourlyRate = DateTime.Now,
                Status = Status.Rented
            };

            _mockRepo.Setup(r => r.GetByIdAsync(testId)).ReturnsAsync(rentedAsset);

            var result = await _service.DeleteAssetAsync(testId);

            Assert.False(result); 
            _mockRepo.Verify(r => r.DeleteAsync(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task UpdateAssetAsync_ShouldUpdateFieldsCorrectly()
        {
            string id = "test-id";
            var updateTime = DateTime.Now.AddHours(1);

            var updateDto = new UpdateAssetDTO
            {
                Name = "Updated PC",
                Type = DeviceType.PC,
                HourlyRate = updateTime,
                Status = Status.UnderMaintenance
            };

            var existingAsset = new GamingAsset { Id = id, Name = "Old PC" };
            var mappedAsset = new GamingAsset
            {
                Id = id,
                Name = "Updated PC",
                HourlyRate = updateTime
            };

            _mockRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(existingAsset);
            _mockMapper.Setup(m => m.Map<GamingAsset>(updateDto)).Returns(mappedAsset);
            _mockRepo.Setup(r => r.UpdateAsync(id, mappedAsset)).ReturnsAsync(true);

            var result = await _service.UpdateAssetAsync(id, updateDto);

            Assert.True(result);
            Assert.Equal(id, mappedAsset.Id); 
            _mockRepo.Verify(r => r.UpdateAsync(id, mappedAsset), Times.Once);
        }

        [Fact]
        public async Task GetAllAssetsAsync_ShouldReturnListOfDTOs()
        {
            var timeVal = DateTime.Parse("2023-10-10 12:00:00");
            var assets = new List<GamingAsset>
            {
                new GamingAsset { Id = "1", Name = "A1", HourlyRate = timeVal },
                new GamingAsset { Id = "2", Name = "A2", HourlyRate = timeVal }
            };

            var dtos = new List<AssetDTO>
            {
                new AssetDTO { Id = "1", Name = "A1", HourlyRate = timeVal },
                new AssetDTO { Id = "2", Name = "A2", HourlyRate = timeVal }
            };

            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(assets);
            _mockMapper.Setup(m => m.Map<IEnumerable<AssetDTO>>(assets)).Returns(dtos);

            var result = await _service.GetAllAssetsAsync();

            Assert.NotNull(result);
            Assert.Equal(2, (result as List<AssetDTO>).Count);
        }
    }
}