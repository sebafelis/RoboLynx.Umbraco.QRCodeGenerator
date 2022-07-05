using Moq;
using NUnit.Framework;
using RoboLynx.Umbraco.QRCodeGenerator.Cache;
using RoboLynx.Umbraco.QRCodeGenerator.Models;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeFormat;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace RoboLynx.Umbraco.QRCodeGenerator.Tests.Unit
{
    [TestFixture]
    public class QRCodeServiceTest : QRCodeGeneratorBaseTest
    {
        public override void SetUp()
        {
            base.SetUp();
        }

        [Test]
        public void GetStream_Variant1_ShouldUseBuilder()
        {
            //Assign
            var propertyAlias = "qrCodePropertyAlias";

            var publishedContent = Mock.Of<IPublishedContent>();
            var culture = "cu";
            var userSettings = Mock.Of<QRCodeSettings>();
            var configuraton = new QRCodeConfig(Mock.Of<IQRCodeType>(), Mock.Of<IQRCodeFormat>(), Mock.Of<QRCodeSettings>());
            var cacheName = "testCache";
            var stream = CreateMockStream();

            var builderMock = new Mock<IQRCodeBuilder>();
            builderMock.Setup(b => b.CreateConfiguration(publishedContent, propertyAlias, culture, userSettings)).Returns(configuraton);
            builderMock.Setup(b => b.CreateStream(configuraton, cacheName)).Returns(stream);
            var service = new QRCodeService(builderMock.Object);

            //Act
            var returnedStream = service.GetStream(publishedContent, propertyAlias, culture, userSettings, cacheName);

            //Assert
            Assert.AreEqual(stream, returnedStream);
            builderMock.Verify(e => e.CreateConfiguration(It.IsAny<IPublishedContent>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<QRCodeSettings>()), Times.Once, "CreateConfiguration method was not invoke.");
            builderMock.Verify(e => e.CreateStream(It.IsAny<QRCodeConfig>(), It.IsAny<string>()), Times.Once, "CreateStream method was not invoke.");

            stream.Dispose();
        }

        [Test]
        public void GetStream_Variant2_ShouldUseBuilder()
        {
            //Assign
            var userSettings = Mock.Of<QRCodeSettings>();
            var configuraton = new QRCodeConfig(Mock.Of<IQRCodeType>(), Mock.Of<IQRCodeFormat>(), Mock.Of<QRCodeSettings>());
            var cacheName = "testCache";
            var stream = CreateMockStream();

            var codeType = Mock.Of<IQRCodeType>();
            var builderMock = new Mock<IQRCodeBuilder>();
            builderMock.Setup(b => b.CreateConfiguration(codeType, userSettings)).Returns(configuraton);
            builderMock.Setup(b => b.CreateStream(configuraton, cacheName)).Returns(stream);
            var service = new QRCodeService(builderMock.Object);

            //Act
            var returnedStream = service.GetStream(codeType, userSettings, cacheName);

            //Assert
            Assert.AreEqual(stream, returnedStream);
            builderMock.Verify(e => e.CreateConfiguration(It.IsAny<IQRCodeType>(), It.IsAny<QRCodeSettings>()), Times.Once, "CreateConfiguration method was not invoke.");
            builderMock.Verify(e => e.CreateStream(It.IsAny<QRCodeConfig>(), It.IsAny<string>()), Times.Once, "CreateStream method was not invoke.");

            stream.Dispose();
        }

        [Test]
        public void GetDefaultSettings_ShouldUseBuilder()
        {
            //Assign
            var propertyAlias = "qrCodePropertyAlias";

            var settings = Mock.Of<QRCodeSettings>();

            var publishedContent = Mock.Of<IPublishedContent>();

            var builderMock = new Mock<IQRCodeBuilder>();
            builderMock.Setup(b => b.GetDefaultSettings(publishedContent, propertyAlias)).Returns(settings);
            var service = new QRCodeService(builderMock.Object);

            //Act
            var returnedSettings = service.GetDefaultSettings(publishedContent, propertyAlias);

            //Assert
            Assert.AreEqual(settings, returnedSettings);
            builderMock.Verify(e => e.GetDefaultSettings(It.IsAny<IPublishedContent>(), It.IsAny<string>()), Times.Once, "GetDefaultSettings method was not invoke.");
        }

        [Test]
        public void ClearCache_Variant1_ShouldUseCacheManager()
        {
            //Assign
            var userSettings = Mock.Of<QRCodeSettings>();
            var configuraton = new QRCodeConfig(Mock.Of<IQRCodeType>(), Mock.Of<IQRCodeFormat>(), userSettings);
            var cacheName = "testCache";

            var cacheManagerMock = new Mock<IQRCodeCacheManager>();
            var codeType = Mock.Of<IQRCodeType>();

            var builderMock = new Mock<IQRCodeBuilder>();
            builderMock.Setup(b => b.CreateConfiguration(codeType, userSettings)).Returns(configuraton);
            builderMock.Setup(b => b.CacheManager).Returns(cacheManagerMock.Object);

            var service = new QRCodeService(builderMock.Object);

            //Act
            service.ClearCache(codeType, userSettings, cacheName);

            //Assert
            cacheManagerMock.Verify(e => e.Clear(It.IsAny<string>(), cacheName), Times.Once, "Clear method was not invoke.");
        }

        [Test]
        public void ClearCache_Variant2_ShouldUseCacheManager()
        {
            //Assign
            var cacheName = "testCache";

            var cacheManagerMock = new Mock<IQRCodeCacheManager>();

            var builderMock = new Mock<IQRCodeBuilder>();
            builderMock.Setup(b => b.CacheManager).Returns(cacheManagerMock.Object);

            var service = new QRCodeService(builderMock.Object);

            //Act
            service.ClearCache(cacheName);

            //Assert
            cacheManagerMock.Verify(e => e.ClearAll(cacheName), Times.Once, "Clear method was not invoke.");
        }

        [Test]
        public void ClearCache_Variant3_ShouldUseCacheManager()
        {
            //Assign
            var userSettings = Mock.Of<QRCodeSettings>();
            var configuraton = new QRCodeConfig(Mock.Of<IQRCodeType>(), Mock.Of<IQRCodeFormat>(), Mock.Of<QRCodeSettings>());
            var cacheName = "testCache";

            var cacheManagerMock = new Mock<IQRCodeCacheManager>();
            var publishedContent = Mock.Of<IPublishedContent>();
            var propertyAlias = "qrCodePropertyAlias";
            var culture = "co";

            var builderMock = new Mock<IQRCodeBuilder>();
            builderMock.Setup(b => b.CreateConfiguration(publishedContent, propertyAlias, culture, userSettings)).Returns(configuraton);
            builderMock.Setup(b => b.CacheManager).Returns(cacheManagerMock.Object);

            var service = new QRCodeService(builderMock.Object);

            //Act
            service.ClearCache(publishedContent, propertyAlias, culture, userSettings, cacheName);

            //Assert
            cacheManagerMock.Verify(e => e.Clear(It.IsAny<string>(), cacheName), Times.Once, "Clear method was not invoke.");
        }
    }
}