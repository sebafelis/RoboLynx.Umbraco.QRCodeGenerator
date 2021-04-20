using DotNetColorParser;
using Moq;
using NUnit.Framework;
using RoboLynx.Umbraco.QRCodeGenerator.Models;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeFormat;
using System.Collections.Generic;
using System.Web;
using Umbraco.Core;
using Umbraco.Core.IO;
using Umbraco.Core.Logging;
using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.SqlSyntax;
using Umbraco.Core.Profiling;
using Umbraco.Core.Services;
using Umbraco.Tests.TestHelpers;
using Umbraco.Web;
using Umbraco.Web.Routing;
using Umbraco.Web.Security;

namespace RoboLynx.Umbraco.QRCodeGenerator.Tests.Unit
{
    [TestFixture]
    public class QRCodeFormatTest
    {
        readonly QRCodeSettings defaultQRCodeSettings = new QRCodeSettings()
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
        }

        [Test]
        public void BmpFormat_WhenValueAndSettingsAreCorrent_ReturnResponseContent()
        {

            //Arrange
            var qrCodeFormat = new BmpFormat(
                Mock.Of<ILocalizedTextService>(),
                Mock.Of<IMediaService>(),
                umbracoHelper,
                Mock.Of<IColorParser>()
                );

            //Act
            var response = qrCodeFormat.ResponseContent("testCode", defaultQRCodeSettings);

            //Assert
            Assert.AreEqual("image/bmp", response.Headers.ContentType.MediaType);
            Assert.NotZero(response.Headers.ContentLength.Value);
        }

        [Test]
        public void SvgFormat_WhenValueAndSettingsAreCorrent_ReturnResponseContent()
        {

            //Arrange
            var qrCodeFormat = new SvgFormat(
                Mock.Of<ILocalizedTextService>(),
                umbracoHelper,
                Mock.Of<IColorParser>()
                );

            //Act
            var response = qrCodeFormat.ResponseContent("testCode", defaultQRCodeSettings);

            //Assert
            Assert.AreEqual("image/svg+xml", response.Headers.ContentType.MediaType);
            Assert.AreEqual("utf-8", response.Headers.ContentType.CharSet);
            Assert.NotZero(response.Headers.ContentLength.Value);
        }

        [Test]
        public void JpegFormat_WhenValueAndSettingsAreCorrent_ReturnResponseContent()
        {

            //Arrange
            var qrCodeFormat = new JpegFormat(
                Mock.Of<ILocalizedTextService>(),
                Mock.Of<IMediaService>(),
                umbracoHelper,
                Mock.Of<IColorParser>()
                );

            //Act
            var response = qrCodeFormat.ResponseContent("testCode", defaultQRCodeSettings);

            //Assert
            Assert.AreEqual("image/jpeg", response.Headers.ContentType.MediaType);
            Assert.NotZero(response.Headers.ContentLength.Value);
        }

        [Test]
        public void PngFormat_WhenValueAndSettingsAreCorrent_ReturnResponseContent()
        {

            //Arrange
            var qrCodeFormat = new PngFormat(
                Mock.Of<ILocalizedTextService>(),
                Mock.Of<IMediaService>(),
                umbracoHelper,
                Mock.Of<IColorParser>()
                );

            //Act
            var response = qrCodeFormat.ResponseContent("testCode", defaultQRCodeSettings);

            //Assert
            Assert.AreEqual("image/png", response.Headers.ContentType.MediaType);
            Assert.NotZero(response.Headers.ContentLength.Value);
        }
    }
}
