using Moq;
using NUnit.Framework;
using RoboLynx.Umbraco.QRCodeGenerator.Controllers;
using RoboLynx.Umbraco.QRCodeGenerator.Extensions;
using RoboLynx.Umbraco.QRCodeGenerator.Models;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeFormat;
using System;
using System.Collections.Generic;
using System.Net.Http;
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
        public void DefaultSettings_WhenSpecifyContentWithPropertyExist_ShouldReturnOKWithSettings()
        {
            //Arrange
            var expectedResponse = DefaultQRCodeSettings;
            var contentId = 123;

            var publishedContent = Mock.Of<IPublishedContent>();

            PublishedContentQuery.Setup(c => c.Content(contentId)).Returns(publishedContent);

            QRCodeFormatsCollection formats = new QRCodeFormatsCollection(new IQRCodeFormat[] { Mock.Of<IQRCodeFormat>() });

            IQRCodeBuilder qrCodeBuilderMock = Mock.Of<IQRCodeBuilder>(b => b.GetDefaultSettings(publishedContent, propertyAlias) == DefaultQRCodeSettings);

            var serviceContext = ServiceContext.CreatePartial(entityService: Mock.Of<IEntityService>(b => b.GetObjectType(contentId) == UmbracoObjectTypes.Document));
            QRCodeController controller = new QRCodeController(qrCodeBuilderMock, formats, Mock.Of<IGlobalSettings>(), Mock.Of<IUmbracoContextAccessor>(), Mock.Of<ISqlContext>(), serviceContext, AppCaches.NoCache, Mock.Of<IProfilingLogger>(), Mock.Of<IRuntimeState>(), UmbracoHelper);

            //Act
            var actionResult = controller.DefaultSettings(contentId, propertyAlias);
            var contentResult = actionResult as OkNegotiatedContentResult<QRCodeSettings>;

            //Assert
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(expectedResponse, contentResult.Content);
        }

        [Test]
        public void DefaultSettings_WhenSpecifyContentNotExist_ShouldReturnBadRequest()
        {
            //Arrange
            var expectedResponse = DefaultQRCodeSettings;
            var contentId = 123;

            var publishedContent = Mock.Of<IPublishedContent>();

            QRCodeFormatsCollection formats = new QRCodeFormatsCollection(new IQRCodeFormat[] { Mock.Of<IQRCodeFormat>() });

            IQRCodeBuilder qrCodeBuilderMock = Mock.Of<IQRCodeBuilder>(b => b.GetDefaultSettings(publishedContent, propertyAlias) == DefaultQRCodeSettings);

            var serviceContext = ServiceContext.CreatePartial(entityService: Mock.Of<IEntityService>(b => b.GetObjectType(contentId) == UmbracoObjectTypes.Document));
            QRCodeController controller = new QRCodeController(qrCodeBuilderMock, formats, Mock.Of<IGlobalSettings>(), Mock.Of<IUmbracoContextAccessor>(), Mock.Of<ISqlContext>(), serviceContext, AppCaches.NoCache, Mock.Of<IProfilingLogger>(), Mock.Of<IRuntimeState>(), UmbracoHelper);

            //Act
            var actionResult = controller.DefaultSettings(contentId, propertyAlias);

            //Assert
            Assert.IsInstanceOf(typeof(BadRequestErrorMessageResult), actionResult);
        }

        [Test]
        public void DefaultSettings_WhenSpecifyContentExistAndPropertyNotExist_ShouldReturnNotFound()
        {
            //Arrange
            var expectedResponse = DefaultQRCodeSettings;
            var contentId = 123;

            var publishedContent = Mock.Of<IPublishedContent>();

            QRCodeFormatsCollection formats = new QRCodeFormatsCollection(new IQRCodeFormat[] { Mock.Of<IQRCodeFormat>() });

            IQRCodeBuilder qrCodeBuilderMock = Mock.Of<IQRCodeBuilder>(b => b.GetDefaultSettings(publishedContent, propertyAlias) == null);

            var serviceContext = ServiceContext.CreatePartial(entityService: Mock.Of<IEntityService>(b => b.GetObjectType(contentId) == UmbracoObjectTypes.Document));
            QRCodeController controller = new QRCodeController(qrCodeBuilderMock, formats, Mock.Of<IGlobalSettings>(), Mock.Of<IUmbracoContextAccessor>(), Mock.Of<ISqlContext>(), serviceContext, AppCaches.NoCache, Mock.Of<IProfilingLogger>(), Mock.Of<IRuntimeState>(), UmbracoHelper);

            //Act
            var actionResult = controller.DefaultSettings(contentId, propertyAlias);

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

            QRCodeFormatsCollection formats = new QRCodeFormatsCollection(new IQRCodeFormat[] {
                Mock.Of<IQRCodeFormat>(f=>f.Id == "id1" && f.RequiredSettings == new List<string> { "setting1", "setting2" }),
                Mock.Of<IQRCodeFormat>(f=>f.Id == "id2" && f.RequiredSettings == new List<string> { "setting3", "setting4" }),
                Mock.Of<IQRCodeFormat>(f=>f.Id == "id3" && f.RequiredSettings == new List<string> { "setting1", "setting3" }),
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

            QRCodeFormatsCollection formats = new QRCodeFormatsCollection(new IQRCodeFormat[0]);

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
            var contentId = 123;
            string culture = null;

            var publishedContent = Mock.Of<IPublishedContent>();
            var httpContent = Mock.Of<HttpContent>();

            PublishedContentQuery.Setup(c => c.Content(contentId)).Returns(publishedContent);

            QRCodeFormatsCollection formats = new QRCodeFormatsCollection(new IQRCodeFormat[0]);

            IQRCodeBuilder qrCodeBuilderMock = Mock.Of<IQRCodeBuilder>(b => b.CreateQRCodeAsResponse(publishedContent, propertyAlias, It.IsAny<string>(), It.IsAny<QRCodeSettings>()) == httpContent);

            var serviceContext = ServiceContext.CreatePartial(entityService: Mock.Of<IEntityService>(b => b.GetObjectType(contentId) == UmbracoObjectTypes.Document));
            QRCodeController controller = new QRCodeController(qrCodeBuilderMock, formats, Mock.Of<IGlobalSettings>(), Mock.Of<IUmbracoContextAccessor>(), Mock.Of<ISqlContext>(), serviceContext, AppCaches.NoCache, Mock.Of<IProfilingLogger>(), Mock.Of<IRuntimeState>(), UmbracoHelper);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            //Act
            var actionResult = controller.Image(contentId, propertyAlias, DefaultQRCodeSettings, culture);
            var contentResult = actionResult as System.Web.Http.Results.ResponseMessageResult;

            //Assert
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Response.Content);
            Assert.AreEqual(httpContent, contentResult.Response.Content);
        }

        [Test]
        public void Image_WhenSpecifyContentNotExist_ShouldReturnBadRequest()
        {
            //Arrange
            var expectedResponse = DefaultQRCodeSettings;
            var contentId = 123;
            string culture = null;

            QRCodeFormatsCollection formats = new QRCodeFormatsCollection(new IQRCodeFormat[] { Mock.Of<IQRCodeFormat>() });

            IQRCodeBuilder qrCodeBuilderMock = Mock.Of<IQRCodeBuilder>();
            Mock.Get(qrCodeBuilderMock).Setup(b => b.CreateQRCodeAsResponse(null, propertyAlias, It.IsAny<string>(), It.IsAny<QRCodeSettings>())).Throws(new ArgumentNullException());

            var serviceContext = ServiceContext.CreatePartial(entityService: Mock.Of<IEntityService>(b => b.GetObjectType(contentId) == UmbracoObjectTypes.Document));
            QRCodeController controller = new QRCodeController(qrCodeBuilderMock, formats, Mock.Of<IGlobalSettings>(), Mock.Of<IUmbracoContextAccessor>(), Mock.Of<ISqlContext>(), serviceContext, AppCaches.NoCache, Mock.Of<IProfilingLogger>(), Mock.Of<IRuntimeState>(), UmbracoHelper);

            //Act
            var actionResult = controller.Image(contentId, propertyAlias, null, culture);

            //Assert
            Assert.IsInstanceOf(typeof(BadRequestErrorMessageResult), actionResult);
        }

        [Test]
        public void Image_WhenSpecifyContentExistAndPropertyNotExist_ShouldReturnBadRequest()
        {
            //Arrange
            var expectedResponse = DefaultQRCodeSettings;
            var contentId = 123;
            string culture = null;

            var publishedContent = Mock.Of<IPublishedContent>();

            QRCodeFormatsCollection formats = new QRCodeFormatsCollection(new IQRCodeFormat[] { Mock.Of<IQRCodeFormat>() });

            IQRCodeBuilder qrCodeBuilder = Mock.Of<IQRCodeBuilder>(b => b.CreateQRCodeAsResponse(publishedContent, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<QRCodeSettings>()) == null);

            var serviceContext = ServiceContext.CreatePartial(entityService: Mock.Of<IEntityService>(b => b.GetObjectType(contentId) == UmbracoObjectTypes.Document));
            QRCodeController controller = new QRCodeController(qrCodeBuilder, formats, Mock.Of<IGlobalSettings>(), Mock.Of<IUmbracoContextAccessor>(), Mock.Of<ISqlContext>(), serviceContext, AppCaches.NoCache, Mock.Of<IProfilingLogger>(), Mock.Of<IRuntimeState>(), UmbracoHelper);

            //Act
            var actionResult = controller.Image(contentId, propertyAlias, null, culture);

            //Assert
            Assert.IsInstanceOf(typeof(BadRequestErrorMessageResult), actionResult);
        }
    }
}
