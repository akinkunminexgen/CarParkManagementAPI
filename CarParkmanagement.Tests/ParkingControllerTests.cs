using Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using CarParkManagement.Controllers;
using CarParkManagement.DataAccess.Data;
using CarParkManagement.DataAccess.Data.Models;
using CarParkManagement.DataAccess.Data.Dto;
using CarParkManagement.DataAccess.Data.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace CarParkmanagement.Tests
{
    public class ParkingControllerTests
    {
        private readonly MyDbConnection _db;
        private readonly ParkingController _controller;

        public ParkingControllerTests()
        {
            //Create in-memory database
            var options = new DbContextOptionsBuilder<MyDbConnection>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            
            _db = new MyDbConnection(options);
            SeedDatabase();
            var logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<ParkingController>();

            ITimeCalculation timeCalc = new TimeCalculation();
            _controller = new ParkingController(logger, _db, timeCalc);
        }

        private void SeedDatabase()
        {
            _db.ChargeRates.Add(new ChargeRate { ChargeRateId = 1, Size = "Small", RatePerMinute = 1, ExtraChargePer5Min = 2 });
            _db.ParkingSpaces.Add(new ParkingSpace { ParkingSpaceId = 1, SpaceNumber = 1, IsOccupied = false });
            _db.SaveChanges();
        }

        [Fact]
        public async Task OccupySpace_ShouldAllocateVehicle()
        {
            // Arrange
            var request = new OccupySpaceDto
            {
                VehicleReg = "TEST123",
                VehicleType = "Saloon"
            };

            // Act
            var result = await _controller.OccupySpace(request);

            // Assert
            Assert.NotNull(result);
            var allocation = Assert.Single(result);
            Assert.Equal("TEST123", allocation.VehicleReg);
        }

        [Fact]
        public async Task GetAvailableSpace_ShouldReturnAvailableCount()
        {
            // Act
            var result = await _controller.GetAvailableSpace();

            // Assert
            var space = Assert.Single(result);
            Assert.True(space.AvailableSpaces >= 0);
        }
    }
}
