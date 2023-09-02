using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SolarWatch.Controllers;
using SolarWatch.Models;
using System.Net;

namespace SolarWatch.Tests
{
    [TestFixture]
    public class SolarControllerTests
    {
        private SolarController _controller;
        private Mock<ILogger<SolarController>> _loggerMock;
        private Mock<HttpClient> _httpClientMock;

        [SetUp]
        public void Setup()
        {
            _loggerMock = new Mock<ILogger<SolarController>>();
            _httpClientMock = new Mock<HttpClient>();
            _controller = new SolarController(_loggerMock.Object)
            {
                ControllerContext = new ControllerContext()
            };
            _controller.ControllerContext.HttpContext = new DefaultHttpContext();
            _controller.ControllerContext.HttpContext.Request.Headers["User-Agent"] = "Test";
            _controller.ControllerContext.HttpContext.Connection.RemoteIpAddress = IPAddress.Loopback;
        }


        [Test]
        public async Task GetSolarInfo_InvalidCity_ReturnsBadRequest()
        {
            // Arrange
            var cityName = "InvalidCity";
            var date = DateTime.Now;

            _httpClientMock.Setup(c => c.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.NotFound));

            // Act
            var result = await _controller.GetSolarInfo(cityName, date);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task GetSolarInfo_ErrorInSunriseSunsetApi_ReturnsBadRequest()
        {
            // Arrange
            var cityName = "TestCity";
            var date = DateTime.Now;

            var geoApiResponse = new GeocodingApiResponse
            {
                Coord = new Coordinates { Lat = 0.0, Lon = 0.0 }
            };

            _httpClientMock.SetupSequence(c => c.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("responseBody") })
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.InternalServerError));

            // Act
            var result = await _controller.GetSolarInfo(cityName, date);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task GetSolarInfo_ExceptionThrown_ReturnsBadRequest()
        {
            // Arrange
            var cityName = "TestCity";
            var date = DateTime.Now;

            _httpClientMock.Setup(c => c.GetAsync(It.IsAny<string>()))
                .ThrowsAsync(new Exception());

            // Act
            var result = await _controller.GetSolarInfo(cityName, date);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }
    }
}
