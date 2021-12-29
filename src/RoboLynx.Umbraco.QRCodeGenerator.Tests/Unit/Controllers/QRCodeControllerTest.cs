using Moq;
using NUnit.Framework;
using RoboLynx.Umbraco.QRCodeGenerator.Controllers;
using RoboLynx.Umbraco.QRCodeGenerator.Models;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeFormat;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeSources;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Results;
using Umbraco.Core;
using Umbraco.Core.Cache;
using Umbraco.Core.Composing;
using Umbraco.Core.Configuration;
using Umbraco.Core.Logging;
using Umbraco.Core.Mapping;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.Persistence;
using Umbraco.Core.Services;
using Umbraco.Web;

namespace RoboLynx.Umbraco.QRCodeGenerator.Tests.Unit.Controllers
{
    [TestFixture]
    public class QRCodeControllerTest : QRCodeGeneratorBaseTest
    {
        const string propertyAlias = "qrCodePropertyAlias";

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            Current.Factory = Mock.Of<IFactory>(f => f.GetInstance(typeof(UmbracoMapper)) == UmbracoMapper);
        }

        [TearDown]
        public void TearDown()
        {
            Current.Reset();
        }

        [Test]
        public void DefaultSettings_ByUdi_WhenSpecifyContentWithPropertyExist_ShouldReturnOKWithSettings()
        {
            //Arrange
            var expectedResponse = DefaultQRCodeSettings;
            var contentKey = Guid.Parse("6101138E-0F3D-4E55-B018-DC8CEEE054B9");
            var contentUdi = Udi.Create(UmbracoObjectTypes.Document.GetUdiType(), contentKey);

            var publishedContent = Mock.Of<IPublishedContent>();

            PublishedContentQuery.Setup(c => c.Content(contentKey)).Returns(publishedContent);

            QRCodeFormatFactoryCollection formats = new QRCodeFormatFactoryCollection(new IQRCodeFormatFactory[] { Mock.Of<IQRCodeFormatFactory>() });

            IQRCodeBuilder qrCodeBuilderMock = Mock.Of<IQRCodeBuilder>(b => b.GetDefaultSettings(publishedContent, propertyAlias) == DefaultQRCodeSettings);

           
            QRCodeController controller = new QRCodeController(qrCodeBuilderMock, formats, Mock.Of<IGlobalSettings>(), Mock.Of<IUmbracoContextAccessor>(), Mock.Of<ISqlContext>(), ServiceContext, AppCaches.NoCache, Mock.Of<IProfilingLogger>(), Mock.Of<IRuntimeState>(), UmbracoHelper);

            //Act
            var actionResult = controller.DefaultSettings(contentUdi, propertyAlias);
            var contentResult = actionResult as OkNegotiatedContentResult<QRCodeSettings>;

            //Assert
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(expectedResponse, contentResult.Content);
        }

        [Test]
        public void DefaultSettings_ByUdi_WhenSpecifyContentNotExist_ShouldReturnNotFound()
        {
            //Arrange
            var contentKey = Guid.Parse("6101138E-0F3D-4E55-B018-DC8CEEE054B9");
            var contentUdi = Udi.Create(UmbracoObjectTypes.Document.GetUdiType(), contentKey);
            
            QRCodeFormatFactoryCollection formats = new QRCodeFormatFactoryCollection(new IQRCodeFormatFactory[] { Mock.Of<IQRCodeFormatFactory>() });

            QRCodeController controller = new QRCodeController(Mock.Of<IQRCodeBuilder>(), formats, Mock.Of<IGlobalSettings>(), Mock.Of<IUmbracoContextAccessor>(), Mock.Of<ISqlContext>(), ServiceContext, AppCaches.NoCache, Mock.Of<IProfilingLogger>(), Mock.Of<IRuntimeState>(), UmbracoHelper);

            //Act
            var actionResult = controller.DefaultSettings(contentUdi, propertyAlias);

            //Assert
            Assert.IsInstanceOf(typeof(NotFoundResult), actionResult);
        }

        [Test]
        public void DefaultSettings_ByUdi_WhenSpecifyContentExistAndPropertyNotExist_ShouldReturnBadRequest()
        {
            //Arrange
            var expectedResponse = DefaultQRCodeSettings;
            var contentKey = Guid.Parse("6101138E-0F3D-4E55-B018-DC8CEEE054B9");
            var contentUdi = Udi.Create(UmbracoObjectTypes.Document.GetUdiType(), contentKey);

            var publishedContent = Mock.Of<IPublishedContent>();
            PublishedContentQuery.Setup(c => c.Content(contentKey)).Returns(publishedContent);

            QRCodeFormatFactoryCollection formats = new QRCodeFormatFactoryCollection(new IQRCodeFormatFactory[] { Mock.Of<IQRCodeFormatFactory>() });

            IQRCodeBuilder qrCodeBuilderMock = Mock.Of<IQRCodeBuilder>(b => b.GetDefaultSettings(publishedContent, propertyAlias) == null);

            //var entityServiceMock = new Mock<IEntityService>();
            //entityServiceMock.Setup(s => s.Get(contentUdi.AsGuid(), UmbracoObjectTypes.Document)).Returns(Attempt<Guid>.Succeed(contentKey));
            //entityServiceMock.Setup(s=>s.GetObjectType(contentId)).Returns(UmbracoObjectTypes.Document);

            //var serviceContext = ServiceContext.CreatePartial(entityService: entityServiceMock.Object);

            QRCodeController controller = new QRCodeController(qrCodeBuilderMock, formats, Mock.Of<IGlobalSettings>(), 
                Mock.Of<IUmbracoContextAccessor>(), Mock.Of<ISqlContext>(), ServiceContext, AppCaches.NoCache, 
                Mock.Of<IProfilingLogger>(), Mock.Of<IRuntimeState>(), UmbracoHelper);

            //Act 
            var actionResult = controller.DefaultSettings(contentUdi, propertyAlias);

            //Assert
            Assert.IsInstanceOf(typeof(BadRequestErrorMessageResult), actionResult);
        }

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

            QRCodeFormatFactoryCollection formats = new QRCodeFormatFactoryCollection(new IQRCodeFormatFactory[] {
                Mock.Of<IQRCodeFormatFactory>(f=>f.Id == "id1" && f.RequiredSettings == new List<string> { "setting1", "setting2" }),
                Mock.Of<IQRCodeFormatFactory>(f=>f.Id == "id2" && f.RequiredSettings == new List<string> { "setting3", "setting4" }),
                Mock.Of<IQRCodeFormatFactory>(f=>f.Id == "id3" && f.RequiredSettings == new List<string> { "setting1", "setting3" }),
            });

            IQRCodeBuilder qrCodeBuilderMock = Mock.Of<IQRCodeBuilder>();

            QRCodeController controller = new QRCodeController(qrCodeBuilderMock, formats, Mock.Of<IGlobalSettings>(), Mock.Of<IUmbracoContextAccessor>(), Mock.Of<ISqlContext>(), ServiceContext, AppCaches.NoCache, Mock.Of<IProfilingLogger>(), Mock.Of<IRuntimeState>(), UmbracoHelper);

            //Act
            var actionResult = controller.RequiredSettingsForFormats();
            var contentResult = actionResult as OkNegotiatedContentResult<Dictionary<string, IEnumerable<string>>>;

            //Assert
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(expectedResponse, contentResult.Content);
        }

        [Test]
        public void RequiredSettingsForFormats_WhenAnyQRCodeFormatNotExist_ShouldReturnOKWithEmptyValue()
        {
            //Arrange
            var expectedResponse = new Dictionary<string, IEnumerable<string>>()
            {
            };

            QRCodeFormatFactoryCollection formats = new QRCodeFormatFactoryCollection(new IQRCodeFormatFactory[0]);

            IQRCodeBuilder qrCodeBuilderMock = Mock.Of<IQRCodeBuilder>();

            QRCodeController controller = new QRCodeController(qrCodeBuilderMock, formats, Mock.Of<IGlobalSettings>(), Mock.Of<IUmbracoContextAccessor>(), Mock.Of<ISqlContext>(), ServiceContext, AppCaches.NoCache, Mock.Of<IProfilingLogger>(), Mock.Of<IRuntimeState>(), UmbracoHelper);

            //Act
            var actionResult = controller.RequiredSettingsForFormats();
            var contentResult = actionResult as OkNegotiatedContentResult<Dictionary<string, IEnumerable<string>>>;

            //Assert
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(expectedResponse, contentResult.Content);
        }

        [Test]
        public void Image_WhenSpecifyContentWithPropertyExist_ShouldReturnOKWithContent()
        {
            //Arrange
            var contentKey = Guid.Parse("6101138E-0F3D-4E55-B018-DC8CEEE054B9");
            var contentUdi = Udi.Create(UmbracoObjectTypes.Document.GetUdiType(), contentKey);
           
            string culture = null;

            var publishedContent = Mock.Of<IPublishedContent>();

            PublishedContentQuery.Setup(c => c.Content(contentKey)).Returns(publishedContent);

            QRCodeFormatFactoryCollection formats = new QRCodeFormatFactoryCollection(new IQRCodeFormatFactory[0]);

            var stream = CreateMockStream();
            var response = Mock.Of<HttpResponseMessage>();

            IQRCodeBuilder qrCodeBuilderMock = Mock.Of<IQRCodeBuilder>(b => b.CreateConfiguration(publishedContent, propertyAlias, culture, It.IsAny<QRCodeSettings>()) == Mock.Of<QRCodeConfig>() 
                && b.CreateResponse(It.IsAny<HttpRequestMessage>(), It.IsAny<QRCodeConfig>(), It.IsAny<bool>(), Constants.BackofficeCacheName) == response);

            QRCodeController controller = new QRCodeController(qrCodeBuilderMock, formats, Mock.Of<IGlobalSettings>(), Mock.Of<IUmbracoContextAccessor>(), Mock.Of<ISqlContext>(), ServiceContext, AppCaches.NoCache, Mock.Of<IProfilingLogger>(), Mock.Of<IRuntimeState>(), UmbracoHelper)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            //Act
            var actionResult = controller.Image(contentUdi, propertyAlias, DefaultQRCodeSettings, culture);
            var contentResult = actionResult as System.Web.Http.Results.ResponseMessageResult;

            //Assert
            Assert.IsNotNull(contentResult);
            Assert.AreEqual(response, contentResult.Response);

            stream.Dispose();
        }

        [Test]
        public void Image_WhenSpecifyContentNotExist_ShouldReturnNotFound()
        {
            //Arrange
            var expectedResponse = DefaultQRCodeSettings;
            var contentKey = Guid.Parse("6101138E-0F3D-4E55-B018-DC8CEEE054B9");
            var contentUdi = Udi.Create(UmbracoObjectTypes.Document.GetUdiType(), contentKey);
            string culture = null;
            var stream = CreateMockStream();

            QRCodeFormatFactoryCollection formats = new QRCodeFormatFactoryCollection(new IQRCodeFormatFactory[] { });

            var source = Mock.Of<IQRCodeSource>();
            var type = Mock.Of<IQRCodeType>();
            var format = Mock.Of<IQRCodeFormat>(f => f.Stream() == stream);
            var qrCodeConfig = new QRCodeConfig() { Format = format, Source = source, Type = type, Settings = new QRCodeSettings() };

            var httpRequest = new HttpRequestMessage();

            IQRCodeBuilder qrCodeBuilderMock = Mock.Of<IQRCodeBuilder>();

            QRCodeController controller = new QRCodeController(qrCodeBuilderMock, formats, Mock.Of<IGlobalSettings>(), Mock.Of<IUmbracoContextAccessor>(), Mock.Of<ISqlContext>(), ServiceContext, AppCaches.NoCache, Mock.Of<IProfilingLogger>(), Mock.Of<IRuntimeState>(), UmbracoHelper);

            //Act
            var actionResult = controller.Image(contentUdi, propertyAlias, null, culture);

            //Assert
            Assert.IsInstanceOf(typeof(NotFoundResult), actionResult);

            stream.Dispose();
        }

        [Test]
        public void Image_WhenSpecifyContentExistAndPropertyNotExist_ShouldReturnBadRequest()
        {
            //Arrange
            var expectedResponse = DefaultQRCodeSettings;
            var contentKey = Guid.Parse("6101138E-0F3D-4E55-B018-DC8CEEE054B9");
            var contentUdi = Udi.Create(UmbracoObjectTypes.Document.GetUdiType(), contentKey);
            string culture = null;

            var publishedContent = Mock.Of<IPublishedContent>();

            PublishedContentQuery.Setup(c => c.Content(contentKey)).Returns(publishedContent);

            QRCodeFormatFactoryCollection formats = new QRCodeFormatFactoryCollection(new IQRCodeFormatFactory[] { Mock.Of<IQRCodeFormatFactory>() });

            IQRCodeBuilder qrCodeBuilder = Mock.Of<IQRCodeBuilder>(b => b.CreateConfiguration(publishedContent, propertyAlias, It.IsAny<string>(), It.IsAny<QRCodeSettings>()) == null);

            QRCodeController controller = new QRCodeController(qrCodeBuilder, formats, Mock.Of<IGlobalSettings>(), Mock.Of<IUmbracoContextAccessor>(), Mock.Of<ISqlContext>(), ServiceContext, AppCaches.NoCache, Mock.Of<IProfilingLogger>(), Mock.Of<IRuntimeState>(), UmbracoHelper);

            //Act
            var actionResult = controller.Image(contentUdi, propertyAlias, null, culture);

            //Assert
            Assert.IsInstanceOf(typeof(BadRequestResult), actionResult);
        }
    }
}
