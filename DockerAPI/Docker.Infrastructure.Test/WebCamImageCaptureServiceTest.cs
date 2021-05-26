using Autofac.Extras.Moq;
using AutoMapper;
using Docker.Infrastructure.DataModel;
using Docker.Infrastructure.DTO;
using Docker.Infrastructure.Entities;
using Docker.Infrastructure.Repositories;
using Docker.Infrastructure.Services;
using Docker.Infrastructure.UnitOfWorks;
using Moq;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Docker.Infrastructure.Test
{
    [TestFixture]
    public class WebCamImageCaptureServiceTest
    {
        private AutoMock _mock;
        private Mock<IApiUnitOfWork> _apiUnitOfWorkMock;
        private Mock<IWebCamImageRepository> _webCamImageRepositoryMock;
		private Mock<IMapper> _mapperMock;
        private IWebCamImageCaptureService _webCamImageCaptureServiceMock;
		private Guid _userId;
		private Guid _objectId;

		[OneTimeSetUp]
		public void ClassSetup()
		{
			_mock = AutoMock.GetLoose();
		}

		[SetUp]
		public void SetUp()
		{
			_apiUnitOfWorkMock = _mock.Mock<IApiUnitOfWork>();
			_webCamImageRepositoryMock = _mock.Mock<IWebCamImageRepository>();
			_mapperMock = _mock.Mock<IMapper>();
			_webCamImageCaptureServiceMock = _mock.Create<WebCamImageCaptureService>();
			_userId = Guid.NewGuid();
			_objectId = Guid.NewGuid();
		}

		[TearDown]
		public void Clean()
		{
			_apiUnitOfWorkMock?.Reset();
			_webCamImageRepositoryMock?.Reset();
			_mapperMock?.Reset();
			_userId = Guid.Empty;
			_objectId = Guid.Empty;
		}

		[OneTimeTearDown]
		public void ClassClean()
		{
			_mock?.Dispose();
		}

		[Test, Category("Unit Test")]
		public void GetWebCamImages_RecordsExistInDB_ReturnsListOfWebCamImages()
		{
			// Arrange
			var capturedImages = GetCapturedImageList();
			var capturedDTOImages = GetCapturedImageDTOList();

			_webCamImageRepositoryMock
				.Setup(x => x.GetAll())
				.Returns(capturedImages)
				.Verifiable();

			_apiUnitOfWorkMock
				.Setup(x => x.WebCamImageRepository)
				.Returns(_webCamImageRepositoryMock.Object)
				.Verifiable();

			_mapperMock.Setup(m => m.Map<IList<WebCamImage>, IList<WebCamImageQueryDTO>>(capturedImages))
				.Returns(capturedDTOImages);

			// Act
			var webCamImageQueryDTO = _webCamImageCaptureServiceMock.GetWebCamImages();

			// Assert
			this.ShouldSatisfyAllConditions(
				() => _apiUnitOfWorkMock.Verify(),
				() => _webCamImageRepositoryMock.Verify(),
				() => Assert.AreEqual("Image02", webCamImageQueryDTO.Last().ImageName),
				() => Assert.AreEqual("Image01", webCamImageQueryDTO.FirstOrDefault().ImageName)
			);
		}

		[Test, Category("Unit Test")]
		public void GetWebCamImages_RecordsDoNotExistInDB_ThrowInvalidOperationException()
		{
			// Arrange
			_webCamImageRepositoryMock
				.Setup(x => x.GetAll())
				.Returns((IList<WebCamImage>)null)
				.Verifiable();

			_apiUnitOfWorkMock
				.Setup(x => x.WebCamImageRepository)
				.Returns(_webCamImageRepositoryMock.Object)
				.Verifiable();

			// Assert
			Should.Throw<InvalidOperationException>(() => _webCamImageCaptureServiceMock.GetWebCamImages()).Message
				.ShouldBe("No data found in Webcam Image Table.");

			this.ShouldSatisfyAllConditions(
				() => _apiUnitOfWorkMock.Verify(),
				() => _webCamImageRepositoryMock.Verify()
			);
		}

		[Test, Category("Unit Test")]
		public void GetWebCamImage_RecordExistsInDB_ReturnsWebCamImageQueryDTO()
		{
			// Arrange
			var capturedImage = GetCapturedImage(_objectId);
			var capturedImageDTO = GetCapturedImageDTO(_objectId);

			_webCamImageRepositoryMock
				.Setup(x => x.GetById(capturedImage.Id))
				.Returns(capturedImage)
				.Verifiable();

			_apiUnitOfWorkMock
				.Setup(x => x.WebCamImageRepository)
				.Returns(_webCamImageRepositoryMock.Object)
				.Verifiable();

			_mapperMock.Setup(m => m.Map<WebCamImage, WebCamImageQueryDTO>(capturedImage))
				.Returns(capturedImageDTO);

			// Act
			var webCamImageQueryDTO = _webCamImageCaptureServiceMock.GetWebCamImage(capturedImage.Id);

			// Assert
			this.ShouldSatisfyAllConditions(
				() => _apiUnitOfWorkMock.Verify(),
				() => _webCamImageRepositoryMock.Verify(),
				() => webCamImageQueryDTO.ShouldBeOfType<WebCamImageQueryDTO>(),
				() => Assert.AreEqual(webCamImageQueryDTO.Id, capturedImage.Id),
				() => Assert.AreEqual(webCamImageQueryDTO.ImageName, capturedImage.ImageName),
				() => Assert.AreEqual(webCamImageQueryDTO.CaptureTime, capturedImage.CaptureTime)
			);
		}

		[Test, Category("Unit Test")]
		public void GetWebCamImage_RecordDoesNotExistInDB_ThrowInvalidOperationException()
		{
			// Arrange
			var id = Guid.NewGuid();

			_webCamImageRepositoryMock
				.Setup(x => x.GetById(It.IsAny<Guid>()))
				.Returns((WebCamImage)null)
				.Verifiable();

			_apiUnitOfWorkMock
				.Setup(x => x.WebCamImageRepository)
				.Returns(_webCamImageRepositoryMock.Object)
				.Verifiable();

			// Assert
			Should.Throw<InvalidOperationException>(() => _webCamImageCaptureServiceMock.GetWebCamImage(id)).Message
				.ShouldContain($"No data found with id");

			this.ShouldSatisfyAllConditions(
				() => _apiUnitOfWorkMock.Verify(),
				() => _webCamImageRepositoryMock.Verify()
			);
		}

		[Test, Category("Unit Test")]
		public async Task SyncLocalWebCamImageData_ListNotEmpty_UpdateDB()
		{
			//Arrange
			var webCamImageInsertDTO = GetWebCamImageInsertDTO();

			_webCamImageRepositoryMock
				.Setup(x => x.SyncLocalWebCamImageData(webCamImageInsertDTO))
				.Verifiable();

			_apiUnitOfWorkMock
				.Setup(x => x.WebCamImageRepository)
				.Returns(_webCamImageRepositoryMock.Object)
				.Verifiable();

			_apiUnitOfWorkMock
				.Setup(x => x.SaveChangesAsync())
				.Returns(() => Task.Run(() => { return 1; }))
				.Verifiable();

			// Act
			var data = await _webCamImageCaptureServiceMock.SyncLocalWebCamImageData(webCamImageInsertDTO);

			// Assert
			this.ShouldSatisfyAllConditions(
				() => _apiUnitOfWorkMock.Verify(),
				() => _webCamImageRepositoryMock.Verify(),
				() => data.ShouldBe(true)
			);
		}

		[Test, Category("Unit Test")]
		public void SyncLocalWebCamImageData_ListIsEmpty_ThrowInvalidOperationException()
		{
			// Arrange
			WebCamImageInsertDTO webCamImageInsertDTO = null;

			// Assert
			Should.Throw<InvalidOperationException>(() => _webCamImageCaptureServiceMock.SyncLocalWebCamImageData(webCamImageInsertDTO)).Message
				.ShouldContain($"Provided image data is null");
		}

		public WebCamImage GetCapturedImage(Guid id)
		{
			return new WebCamImage
			{
				Id = id,
				UserId = _userId,
				ImageName = "Image01",
				CaptureTime = new DateTime(2021, 10, 12, 05, 15, 04)
			};
		}

		public WebCamImageQueryDTO GetCapturedImageDTO(Guid id)
		{
			return new WebCamImageQueryDTO
			{
				Id = id,
				UserId = _userId,
				ImageName = "Image01",
				CaptureTime = new DateTime(2021, 10, 12, 05, 15, 04)
			};
		}

		public List<WebCamImage> GetCapturedImageList()
		{
			List<WebCamImage> capturedImages = new List<WebCamImage>();

			capturedImages.Add(new WebCamImage
			{
				Id = Guid.NewGuid(),
				UserId = _userId,
				ImageName = "Image01",
				CaptureTime = new DateTime(2021, 10, 12, 05, 15, 04)
			});

			capturedImages.Add(new WebCamImage
			{
				Id = Guid.NewGuid(),
				UserId = _userId,
				ImageName = "Image02",
				CaptureTime = new DateTime(2020, 9, 2, 7, 25, 23)
			});

			return capturedImages;
		}

		public List<WebCamImageQueryDTO> GetCapturedImageDTOList()
        {
            List<WebCamImageQueryDTO> capturedImages = new List<WebCamImageQueryDTO>();

            capturedImages.Add(new WebCamImageQueryDTO
            {
                Id = Guid.NewGuid(),
                UserId = _userId,
                ImageName = "Image01",
                CaptureTime = new DateTime(2021, 10, 12, 05, 15, 04)
            });

            capturedImages.Add(new WebCamImageQueryDTO
            {
                Id = Guid.NewGuid(),
                UserId = _userId,
                ImageName = "Image02",
                CaptureTime = new DateTime(2020, 9, 2, 7, 25, 23)
            });

            return capturedImages;
		}

		public WebCamImageInsertDTO GetWebCamImageInsertDTO()
		{
			return new WebCamImageInsertDTO
			{
				ImageName = "Bird Image",
				UserId = _userId,
				CaptureTime = new DateTime(2021, 10, 12, 05, 15, 04)
			};
		}
	}
}
