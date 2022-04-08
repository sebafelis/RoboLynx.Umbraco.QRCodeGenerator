using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using RoboLynx.Umbraco.QRCodeGenerator.Controllers;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeFormat;
using System;
using System.Collections.Generic;
using Umbraco.Cms.Core.Services;

namespace RoboLynx.Umbraco.QRCodeGenerator.Tests.Unit.Controllers
{
    [TestFixture]
    public class QRCodeControllerTest : QRCodeGeneratorBaseTest
    {
        //const string _propertyAlias = "qrCodePropertyAlias";

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
        }

        //[Test]
        //public async Task DefaultSettings_ByUdi_WhenSpecifyContentWithPropertyExist_ShouldReturnOKWithSettings()
        //{
        //    //Arrange
        //    var expectedResponse = DefaultQRCodeSettings;
        //    var contentKey = Guid.Parse("6101138E-0F3D-4E55-B018-DC8CEEE054B9");
        //    var contentUdi = Udi.Create(UmbracoObjectTypes.Document.GetUdiType(), contentKey);

        //    var publishedContent = Mock.Of<IPublishedContent>();

        //    PublishedContentQuery.Setup(c => c.Content(contentUdi)).Returns(publishedContent);

        //    var formats = new QRCodeFormatFactoryCollection(() => new IQRCodeFormatFactory[] { Mock.Of<IQRCodeFormatFactory>() });

        //    IQRCodeBuilder qrCodeBuilderMock = Mock.Of<IQRCodeBuilder>(b => b.GetDefaultSettings(publishedContent, _propertyAlias) == DefaultQRCodeSettings);
        //    IEntityService entityService = Mock.Of<IEntityService>();

        //    IQRCodeResponesFactory responesFactory = Mock.Of<IQRCodeResponesFactory>();

        //    var controller = new QRCodeController(responesFactory, formats, UmbracoHelper, MemberManager, Mock.Of<IEntityService>());

        //    //Act
        //    var actionResult = await controller.DefaultSettings(contentUdi, _propertyAlias);
        //    var contentResult = actionResult as OkObjectResult;

        //    //Assert
        //    Assert.IsNotNull(contentResult);
        //    Assert.IsNotNull(contentResult.Value);
        //    Assert.IsInstanceOf<QRCodeSettings>(contentResult.Value);
        //    Assert.AreEqual(expectedResponse, contentResult.Value);
        //}

        //[Test]
        //public async Task DefaultSettings_ByUdi_WhenSpecifyContentNotExist_ShouldReturnNotFound()
        //{
        //    //Arrange
        //    var contentKey = Guid.Parse("6101138E-0F3D-4E55-B018-DC8CEEE054B9");
        //    var contentUdi = Udi.Create(UmbracoObjectTypes.Document.GetUdiType(), contentKey);

        //    var formats = new QRCodeFormatFactoryCollection(() => new IQRCodeFormatFactory[] { Mock.Of<IQRCodeFormatFactory>() });

        //    IEntityService entityService = Mock.Of<IEntityService>();

        //    var controller = new QRCodeController(Mock.Of<IQRCodeBuilder>(), formats, UmbracoHelper, MemberManager, entityService);

        //    //Act
        //    var actionResult = await controller.DefaultSettings(contentUdi, _propertyAlias);

        //    //Assert
        //    Assert.IsInstanceOf(typeof(NotFoundResult), actionResult);
        //}

        //[Test]
        //public async Task DefaultSettings_ByUdi_WhenSpecifyContentExistAndPropertyNotExist_ShouldReturnBadRequest()
        //{
        //    //Arrange
        //    var expectedResponse = DefaultQRCodeSettings;
        //    var contentKey = Guid.Parse("6101138E-0F3D-4E55-B018-DC8CEEE054B9");
        //    var contentUdi = Udi.Create(UmbracoObjectTypes.Document.GetUdiType(), contentKey);

        //    var publishedContent = Mock.Of<IPublishedContent>();
        //    PublishedContentQuery.Setup(c => c.Content(contentUdi)).Returns(publishedContent);

        //    var formats = new QRCodeFormatFactoryCollection(() => new IQRCodeFormatFactory[] { Mock.Of<IQRCodeFormatFactory>() });

        //    IQRCodeBuilder qrCodeBuilderMock = Mock.Of<IQRCodeBuilder>(b => b.GetDefaultSettings(publishedContent, _propertyAlias) == null);

        //    IEntityService entityService = Mock.Of<IEntityService>();

        //    var controller = new QRCodeController(qrCodeBuilderMock, formats, UmbracoHelper, MemberManager, entityService);

        //    //Act
        //    var actionResult = await controller.DefaultSettings(contentUdi, _propertyAlias);

        //    //Assert
        //    Assert.IsInstanceOf(typeof(BadRequestResult), actionResult);
        //}

        [Test]
        public void RequiredSettingsForFormats_WhenAnyQRCodeFormatExist_ShouldReturnOKWithValue()
        {
            //Arrange
            var expectedResponse = new Dictionary<string, IEnumerable<string>>()
            {
                { "id1", new List<string> { "setting1", "setting2" } },
                { "id2", new List<string> { "setting3", "setting4" } },
                { "id3", new List<string> { "setting1", "setting3" } }
            };

            var formats = new QRCodeFormatFactoryCollection(() => new IQRCodeFormatFactory[] {
                Mock.Of<IQRCodeFormatFactory>(f=>f.Id == "id1" && f.RequiredSettings == new List<string> { "setting1", "setting2" }),
                Mock.Of<IQRCodeFormatFactory>(f=>f.Id == "id2" && f.RequiredSettings == new List<string> { "setting3", "setting4" }),
                Mock.Of<IQRCodeFormatFactory>(f=>f.Id == "id3" && f.RequiredSettings == new List<string> { "setting1", "setting3" }),
            });

            var controller = new QRCodeController(Mock.Of<IQRCodeResponesFactory>(), formats, UmbracoHelper, MemberManager, Mock.Of<IIdKeyMap>(), Mock.Of<ILogger<QRCodeController>>());

            //Act
            var actionResult = controller.RequiredSettingsForFormats();
            var contentResult = actionResult as OkObjectResult;

            //Assert
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Value);
            Assert.AreEqual(expectedResponse, contentResult.Value);
        }

        [Test]
        public void RequiredSettingsForFormats_WhenAnyQRCodeFormatNotExist_ShouldReturnOKWithEmptyValue()
        {
            //Arrange
            var expectedResponse = new Dictionary<string, IEnumerable<string>>()
            {
            };

            var formats = new QRCodeFormatFactoryCollection(() => Array.Empty<IQRCodeFormatFactory>());

            IQRCodeResponesFactory responesFactory = Mock.Of<IQRCodeResponesFactory>();
            IEntityService entityService = Mock.Of<IEntityService>();

            var controller = new QRCodeController(responesFactory, formats, UmbracoHelper, MemberManager, Mock.Of<IIdKeyMap>(), Mock.Of<ILogger<QRCodeController>>());

            //Act
            var actionResult = controller.RequiredSettingsForFormats();
            var contentResult = actionResult as OkObjectResult;

            //Assert
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Value);
            Assert.AreEqual(expectedResponse, contentResult.Value);
        }

        //[Test]
        //public async Task Image_WhenSpecifyContentWithPropertyExist_ShouldReturnOKWithContent()
        //{
        //    //Arrange
        //    var contentKey = Guid.Parse("6101138E-0F3D-4E55-B018-DC8CEEE054B9");
        //    var contentUdi = Udi.Create(UmbracoObjectTypes.Document.GetUdiType(), contentKey);

        //    string culture = null;

        //    var publishedContent = Mock.Of<IPublishedContent>();

        //    PublishedContentQuery.Setup(c => c.Content(contentUdi)).Returns(publishedContent);

        //    var formats = new QRCodeFormatFactoryCollection(() => Array.Empty<IQRCodeFormatFactory>());

        //    var stream = CreateMockStream();
        //    var response = Mock.Of<HttpResponseMessage>();

        //    //IQRCodeBuilder qrCodeBuilderMock = Mock.Of<IQRCodeBuilder>(b => b.CreateConfiguration(publishedContent, _propertyAlias, culture, It.IsAny<QRCodeSettings>()) == Mock.Of<QRCodeConfig>()
        //    //    && b.CreateResponse(It.IsAny<QRCodeConfig>(), It.IsAny<bool>(), Constants.Core.BackofficeCacheName) == response);

        //    IEntityService entityService = Mock.Of<IEntityService>();

        //    var controller = new QRCodeController(qrCodeBuilderMock, formats, UmbracoHelper, MemberManager, entityService);

        //    //Act
        //    var actionResult = await controller.Image(contentUdi, _propertyAlias, DefaultQRCodeSettings, culture);
        //    var contentResult =  actionResult as OkObjectResult;

        //    //Assert
        //    Assert.IsNotNull(contentResult);
        //    Assert.AreEqual(response, contentResult.Value);

        //    stream.Dispose();
        //}

        //[Test]
        //public async Task Image_WhenSpecifyContentNotExist_ShouldReturnNotFound()
        //{
        //    //Arrange
        //    var expectedResponse = DefaultQRCodeSettings;
        //    var contentKey = Guid.Parse("6101138E-0F3D-4E55-B018-DC8CEEE054B9");
        //    var contentUdi = Udi.Create(UmbracoObjectTypes.Document.GetUdiType(), contentKey);
        //    string culture = null;
        //    var stream = CreateMockStream();

        //    var formats = new QRCodeFormatFactoryCollection(() => Array.Empty<IQRCodeFormatFactory>());

        //    var source = Mock.Of<IQRCodeSource>();
        //    var type = Mock.Of<IQRCodeType>();
        //    var format = Mock.Of<IQRCodeFormat>(f => f.Stream() == stream);
        //    var qrCodeConfig = new QRCodeConfig() { Format = format, Source = source, Type = type, Settings = new QRCodeSettings() };

        //    var httpRequest = new HttpRequestMessage();

        //    IQRCodeBuilder qrCodeBuilderMock = Mock.Of<IQRCodeBuilder>();

        //    IEntityService entityService = Mock.Of<IEntityService>();

        //    var controller = new QRCodeController(qrCodeBuilderMock, formats, UmbracoHelper, MemberManager, entityService);

        //    //Act
        //    var actionResult = await controller.Image(contentUdi, _propertyAlias, null, culture);

        //    //Assert
        //    Assert.IsInstanceOf(typeof(NotFoundResult), actionResult);

        //    stream.Dispose();
        //}

        //[Test]
        //public async Task Image_WhenSpecifyContentExistAndPropertyNotExist_ShouldReturnBadRequest()
        //{
        //    //Arrange
        //    var expectedResponse = DefaultQRCodeSettings;
        //    var contentKey = Guid.Parse("6101138E-0F3D-4E55-B018-DC8CEEE054B9");
        //    var contentUdi = Udi.Create(UmbracoObjectTypes.Document.GetUdiType(), contentKey);
        //    string culture = null;

        //    var publishedContent = Mock.Of<IPublishedContent>();

        //    PublishedContentQuery.Setup(c => c.Content(contentUdi)).Returns(publishedContent);

        //    var formats = new QRCodeFormatFactoryCollection(() => new IQRCodeFormatFactory[] { Mock.Of<IQRCodeFormatFactory>() });

        //    IQRCodeBuilder qrCodeBuilder = Mock.Of<IQRCodeBuilder>(b => b.CreateConfiguration(publishedContent, _propertyAlias, It.IsAny<string>(), It.IsAny<QRCodeSettings>()) == null);

        //    IEntityService entityService = Mock.Of<IEntityService>();

        //    var controller = new QRCodeController(qrCodeBuilder, formats, UmbracoHelper, MemberManager, entityService);

        //    //Act
        //    var actionResult = await controller.Image(contentUdi, _propertyAlias, null, culture);

        //    //Assert
        //    Assert.IsInstanceOf(typeof(BadRequestResult), actionResult);
        //}
    }
}