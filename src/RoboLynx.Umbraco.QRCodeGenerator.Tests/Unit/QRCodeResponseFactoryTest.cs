using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using RoboLynx.Umbraco.QRCodeGenerator.Cache;
using RoboLynx.Umbraco.QRCodeGenerator.Controllers;
using RoboLynx.Umbraco.QRCodeGenerator.Models;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeFormat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Services;

namespace RoboLynx.Umbraco.QRCodeGenerator.Tests.Unit
{
    [TestFixture]
    public class QRCodeResponseFactoryTest : QRCodeGeneratorBaseTest
    {
        [Test]
        public void CreateResponseWithDefaultSettings_WhenSpecifyContentAndPropertyExist_ShouldReturnOKWithSettings()
        {
            //Arrange
            var expectedResponse = DefaultQRCodeSettings;

            var publishedContent = Mock.Of<IPublishedContent>();
            string propertyAlias = "propertyEditorAlias";

            IQRCodeBuilder qrCodeBuilderMock = Mock.Of<IQRCodeBuilder>(b => b.GetDefaultSettings(publishedContent, propertyAlias) == DefaultQRCodeSettings);

            var responesFactory = new QRCodeResponesFactory(qrCodeBuilderMock);

            //Act
            var actionResult = responesFactory.CreateResponseWithDefaultSettings(publishedContent, propertyAlias);
            var contentResult = actionResult as OkObjectResult;

            //Assert
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Value);
            Assert.IsInstanceOf<QRCodeSettings>(contentResult.Value);
            Assert.AreEqual(expectedResponse, contentResult.Value);
        }

        [Test]
        public void CreateResponesWithQRCode_WhenSpecifyContentAndPropertyExist_AndDirectUrlsAreSupport_AndFileIsCached_ShouldReturnRedirect()
        {
            //Arrange
            var publishedContent = Mock.Of<IPublishedContent>();
            var propertyAlias = "propertyEditorAlias";
            var culture = "pl-PL";
            var cacheName = "cache";
            var settings = DefaultQRCodeSettings;
            var hashId = "hash";
            var redirectUrl = "http://test.url";

            IQRCodeBuilder qrCodeBuilderMock = Mock.Of<IQRCodeBuilder>(b =>
                b.CreateConfiguration(publishedContent, propertyAlias, culture, settings) == Mock.Of<QRCodeConfig>(c => c.Format == Mock.Of<IQRCodeFormat>(f => f.HashId == hashId))
                && b.CacheManager == Mock.Of<IQRCodeCacheManager>(c => c.UrlSupport(cacheName) == true && c.IsCached(hashId, cacheName) == true && c.GetUrl(hashId, It.IsAny<UrlMode>(), cacheName) == redirectUrl)
            );

            var responesFactory = new QRCodeResponesFactory(qrCodeBuilderMock);

            //Act
            var actionResult = responesFactory.CreateResponesWithQRCode(publishedContent, propertyAlias, culture, settings, cacheName);
            var contentResult = actionResult as RedirectResult;

            //Assert
            Assert.IsNotNull(contentResult);
            Assert.AreEqual(redirectUrl, contentResult.Url);
            Assert.IsFalse(contentResult.Permanent);
        }

        [Test]
        public void CreateResponesWithQRCode_WhenSpecifyContentAndPropertyExist_AndDirectUrlsAreSupport_AndFileIsNotCached_ShouldReturnRedirect()
        {
            //Arrange
            var publishedContent = Mock.Of<IPublishedContent>();
            var propertyAlias = "propertyEditorAlias";
            var culture = "pl-PL";
            var cacheName = "cache";
            var settings = DefaultQRCodeSettings;
            var hashId = "hash";
            var redirectUrl = "http://test.url";
            var stream = CreateMockStream("test");

            IQRCodeBuilder qrCodeBuilderMock = Mock.Of<IQRCodeBuilder>(b =>
                b.CreateConfiguration(publishedContent, propertyAlias, culture, settings) == Mock.Of<QRCodeConfig>(c => c.Format == Mock.Of<IQRCodeFormat>(f => f.HashId == hashId))
                && b.CacheManager == Mock.Of<IQRCodeCacheManager>(c => c.UrlSupport(cacheName) == true && c.IsCached(hashId, cacheName) == true && c.GetUrl(hashId, It.IsAny<UrlMode>(), cacheName) == redirectUrl)
            );

            var responesFactory = new QRCodeResponesFactory(qrCodeBuilderMock);

            //Act
            var actionResult = responesFactory.CreateResponesWithQRCode(publishedContent, propertyAlias, culture, settings, cacheName);
            var contentResult = actionResult as RedirectResult;

            //Assert
            Assert.IsNotNull(contentResult);
            Assert.AreEqual(redirectUrl, contentResult.Url);
            Assert.IsFalse(contentResult.Permanent);
        }

        [Test]
        public void CreateResponesWithQRCode_WhenSpecifyContentAndPropertyExist_AndDirectUrlsAreNotSupport_ShouldReturnOKWithFileStrem()
        {
            //Arrange
            var fixture = new Fixture();

            var publishedContent = Mock.Of<IPublishedContent>();
            var propertyAlias = fixture.Create<string>();
            var culture = fixture.Create<string>();
            var cacheName = fixture.Create<string>();
            var settings = DefaultQRCodeSettings;
            var stream = CreateMockStream("test");
            var hashId = fixture.Create<string>();
            var lastModified = fixture.Create<DateTimeOffset>();

            IQRCodeBuilder qrCodeBuilderMock = Mock.Of<IQRCodeBuilder>(b =>
                b.CreateConfiguration(publishedContent, propertyAlias, culture, settings) == Mock.Of<QRCodeConfig>(c => c.Format == Mock.Of<IQRCodeFormat>(f => f.HashId == hashId && f.Mime == "test/mine"))
                && b.CacheManager == Mock.Of<IQRCodeCacheManager>(c => c.UrlSupport(cacheName) == false && c.LastModified(hashId, cacheName) == lastModified)
                && b.CreateStream(It.IsAny<QRCodeConfig>(), cacheName) == stream);

            var responesFactory = new QRCodeResponesFactory(qrCodeBuilderMock);

            //Act
            var actionResult = responesFactory.CreateResponesWithQRCode(publishedContent, propertyAlias, culture, settings, cacheName);
            var contentResult = actionResult as FileStreamResult;

            //Assert
            Assert.IsNotNull(contentResult);
            Assert.AreEqual(stream, contentResult.FileStream);
            Assert.AreEqual(lastModified, contentResult.LastModified);
            Assert.IsNotNull(contentResult.EntityTag?.Tag);

            stream.Dispose();
        }

        [Test]
        public void CreateResponesWithQRCode_WhenSpecifyContentNotExist_ShouldReturnNotFound()
        {
            //Arrange
            var propertyAlias = "propertyEditorAlias";
            var culture = "pl-PL";
            var cacheName = "cache";
            var settings = DefaultQRCodeSettings;

            IQRCodeBuilder qrCodeBuilderMock = Mock.Of<IQRCodeBuilder>();

            var responesFactory = new QRCodeResponesFactory(qrCodeBuilderMock);

            //Act
            var actionResult = responesFactory.CreateResponesWithQRCode(null, propertyAlias, culture, settings, cacheName);
            var result = actionResult as NotFoundResult;

            //Assert
            Assert.IsInstanceOf(typeof(NotFoundResult), result);
        }

        [Test]
        public void CreateResponesWithQRCode_WhenSpecifyContentExist_ButPropertyNotExist_ShouldReturnBadRequest()
        {
            //Arrange
            var publishedContent = Mock.Of<IPublishedContent>();
            var propertyAlias = "propertyEditorAlias";
            var culture = "pl-PL";
            var cacheName = "cache";
            var settings = DefaultQRCodeSettings;

            IQRCodeBuilder qrCodeBuilderMock = Mock.Of<IQRCodeBuilder>();

            var responesFactory = new QRCodeResponesFactory(qrCodeBuilderMock);

            //Act
            var actionResult = responesFactory.CreateResponesWithQRCode(publishedContent, propertyAlias, culture, settings, cacheName);
            var result = actionResult as BadRequestResult;

            //Assert
            Assert.IsInstanceOf(typeof(BadRequestResult), result);
        }
    }
}
