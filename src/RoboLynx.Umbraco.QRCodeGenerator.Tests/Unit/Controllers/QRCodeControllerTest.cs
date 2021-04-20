using Microsoft.AspNet.Identity;
using Moq;
using NUnit.Framework;
using RoboLynx.Umbraco.QRCodeGenerator.Controllers;
using RoboLynx.Umbraco.QRCodeGenerator.Extensions;
using RoboLynx.Umbraco.QRCodeGenerator.Models;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeFormat;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Core.Models;
using Umbraco.Core.Models.Identity;
using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.SqlSyntax;
using Umbraco.Core.Profiling;
using Umbraco.Core.Security;
using Umbraco.Core.Services;
using Umbraco.Tests.TestHelpers;
using Umbraco.Web;
using Umbraco.Web.Routing;
using Umbraco.Web.Security;

namespace RoboLynx.Umbraco.QRCodeGenerator.Tests.Unit.Controllers
{
    [TestFixture]
    public class QRCodeControllerTest
    {
        const string propertyAlias = "qrCodePropertyAlias";

        QRCodeSettings defaultQRCodeSettings = new QRCodeSettings()
        {
            DarkColor = "#000000",
            LightColor = "#ffffff",
            DrawQuiteZone = true,
            ECCLevel = ECCLevel.L,
            Format = "svg",
            Icon = null,
            IconBorderWidth = 1,
            IconSizePercent = 20,
            Size = 20
        };

        public ApplicationContext applicationContext;
        public UmbracoContext umbracoContext;
        public UmbracoHelper umbracoHelper;
        public BackOfficeUserManager<BackOfficeIdentityUser> backOfficeUserManager;

        [SetUp]
        public void Setup()
        {
            var settings = SettingsForTests.GenerateMockSettings();
            SettingsForTests.ConfigureSettings(settings);

            var logger = Mock.Of<ILogger>();

            var applicationContext = ApplicationContext.EnsureContext(
                new DatabaseContext(
                    Mock.Of<IDatabaseFactory2>(),
                    logger,
                    new SqlSyntaxProviders(new ISqlSyntaxProvider[0])
                    ),
                new ServiceContext(
                    contentService: Mock.Of<IContentService>() //, ...
                    ),
                CacheHelper.CreateDisabledCacheHelper(),
                new ProfilingLogger(logger, Mock.Of<IProfiler>()),
                true
            );

            var httpContext = Mock.Of<HttpContextBase>();

            umbracoContext = UmbracoContext.EnsureContext(
                httpContext,
                applicationContext,
                new WebSecurity(httpContext, applicationContext),
                settings,
                new List<IUrlProvider>(),
                true,
                false
            );

            umbracoHelper = new UmbracoHelper(umbracoContext);

            backOfficeUserManager = new BackOfficeUserManager<BackOfficeIdentityUser>(Mock.Of<IUserStore<BackOfficeIdentityUser, int>>());
        }

        [Test]
        public void DefaultSettings_WhenSpecifyContentWithPropertyExist_ShouldReturnOKWithSettings()
        {
            //Arrange
            var expectedResponse = defaultQRCodeSettings;
            var contentId = 123;
            var publishedContent = Mock.Of<IPublishedContent>();

            QRCodeFormatsCollection formats = new QRCodeFormatsCollection(new IQRCodeFormat[] { Mock.Of<IQRCodeFormat>() });

            IQRCodeBuilder qrCodeBuilderMock = Mock.Of<IQRCodeBuilder>(b => b.GetDefaultSettings(publishedContent, propertyAlias) == defaultQRCodeSettings);

            QRCodeController controller = new QRCodeController(qrCodeBuilderMock, formats, umbracoContext, umbracoHelper, backOfficeUserManager);

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
            var expectedResponse = defaultQRCodeSettings;
            var contentId = 123;
            var publishedContent = Mock.Of<IPublishedContent>();   

            QRCodeFormatsCollection formats = new QRCodeFormatsCollection(new IQRCodeFormat[] { Mock.Of<IQRCodeFormat>() });

            IQRCodeBuilder qrCodeBuilderMock = Mock.Of<IQRCodeBuilder>(b => b.GetDefaultSettings(publishedContent, propertyAlias) == defaultQRCodeSettings);

            QRCodeController controller = new QRCodeController(qrCodeBuilderMock, formats, umbracoContext, umbracoHelper, backOfficeUserManager);

            //Act
            var actionResult = controller.DefaultSettings(contentId, propertyAlias);

            //Assert
            Assert.IsInstanceOf(typeof(BadRequestErrorMessageResult), actionResult);
        }

        [Test]
        public void DefaultSettings_WhenSpecifyContentExistAndPropertyNotExist_ShouldReturnNotFound()
        {
            //Arrange
            var expectedResponse = defaultQRCodeSettings;
            var contentId = 123;
            var publishedContent = Mock.Of<IPublishedContent>();

            QRCodeFormatsCollection formats = new QRCodeFormatsCollection(new IQRCodeFormat[] { Mock.Of<IQRCodeFormat>() });

            IQRCodeBuilder qrCodeBuilderMock = Mock.Of<IQRCodeBuilder>(b => b.GetDefaultSettings(publishedContent, propertyAlias) == null);

            QRCodeController controller = new QRCodeController(qrCodeBuilderMock, formats, umbracoContext, umbracoHelper, backOfficeUserManager);

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

            QRCodeController controller = new QRCodeController(qrCodeBuilderMock, formats, umbracoContext, umbracoHelper, backOfficeUserManager);

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

            QRCodeController controller = new QRCodeController(qrCodeBuilderMock, formats, umbracoContext, umbracoHelper, backOfficeUserManager);

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

            QRCodeFormatsCollection formats = new QRCodeFormatsCollection(new IQRCodeFormat[0]);

            IQRCodeBuilder qrCodeBuilderMock = Mock.Of<IQRCodeBuilder>(b => b.CreateQRCodeAsResponse(publishedContent, propertyAlias, It.IsAny<string>(), It.IsAny<QRCodeSettings>()) == httpContent);

            QRCodeController controller = new QRCodeController(qrCodeBuilderMock, formats, umbracoContext, umbracoHelper, backOfficeUserManager)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            //Act
            var actionResult = controller.Image(contentId, propertyAlias, defaultQRCodeSettings, culture);
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
            var expectedResponse = defaultQRCodeSettings;
            var contentId = 123;
            string culture = null;

            QRCodeFormatsCollection formats = new QRCodeFormatsCollection(new IQRCodeFormat[] { Mock.Of<IQRCodeFormat>() });

            IQRCodeBuilder qrCodeBuilderMock = Mock.Of<IQRCodeBuilder>();
            Mock.Get(qrCodeBuilderMock).Setup(b => b.CreateQRCodeAsResponse(null, propertyAlias, It.IsAny<string>(), It.IsAny<QRCodeSettings>())).Throws(new ArgumentNullException());

            QRCodeController controller = new QRCodeController(qrCodeBuilderMock, formats, umbracoContext, umbracoHelper, backOfficeUserManager);

            //Act
            var actionResult = controller.Image(contentId, propertyAlias, null, culture);

            //Assert
            Assert.IsInstanceOf(typeof(BadRequestErrorMessageResult), actionResult);
        }

        [Test]
        public void Image_WhenSpecifyContentExistAndPropertyNotExist_ShouldReturnBadRequest()
        {
            //Arrange
            var expectedResponse = defaultQRCodeSettings;
            var contentId = 123;
            string culture = null;
            var publishedContent = Mock.Of<IPublishedContent>();

            QRCodeFormatsCollection formats = new QRCodeFormatsCollection(new IQRCodeFormat[] { Mock.Of<IQRCodeFormat>() });

            IQRCodeBuilder qrCodeBuilderMock = Mock.Of<IQRCodeBuilder>(b => b.CreateQRCodeAsResponse(publishedContent, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<QRCodeSettings>()) == null);

            QRCodeController controller = new QRCodeController(qrCodeBuilderMock, formats, umbracoContext, umbracoHelper, backOfficeUserManager);

            //Act
            var actionResult = controller.Image(contentId, propertyAlias, null, culture);

            //Assert
            Assert.IsInstanceOf(typeof(BadRequestErrorMessageResult), actionResult);
        }
    }
}
