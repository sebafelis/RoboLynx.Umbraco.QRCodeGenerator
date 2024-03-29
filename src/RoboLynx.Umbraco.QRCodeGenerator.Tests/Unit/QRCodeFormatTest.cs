﻿using DotNetColorParser;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using RoboLynx.Umbraco.QRCodeGenerator.Models;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeFormat;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Web.Common;

namespace RoboLynx.Umbraco.QRCodeGenerator.Tests.Unit
{
    [TestFixture]
    public class QRCodeFormatTest : QRCodeGeneratorBaseTest
    {
        [Test]
        public void BmpFormat_WhenValueAndSettingsAreCorrent_ReturnResponseContent()
        {
            //Arrange
            var hashId = "hash";
            var qrCodeFormat = new BmpFormat(
               Mock.Of<IMediaService>(),
                Mock.Of<IUmbracoHelperAccessor>(a => a.TryGetUmbracoHelper(out this.UmbracoHelper)),
                Mock.Of<IQRCodeHashIdFactory>(h => h.ComputeHash(It.IsAny<string>(), It.IsAny<QRCodeSettings>()) == hashId),
                Mock.Of<IColorParser>(),
                Mock.Of<ILogger<BmpFormat>>(),
                Mock.Of<IQRCodeType>(s => s.GetCodeContent() == "testCode"),
                DefaultQRCodeSettings
                );

            //Act
            var responseStream = qrCodeFormat.Stream();

            //Assert
            Assert.IsNotNull(responseStream);
            Assert.NotZero(responseStream.Length);

            responseStream.Dispose();
        }

        [Test]
        public void SvgFormat_WhenValueAndSettingsAreCorrent_ReturnResponseContent()
        {
            //Arrange
            var hashId = "hash";
            var qrCodeFormat = new SvgFormat(
                Mock.Of<IUmbracoHelperAccessor>(a => a.TryGetUmbracoHelper(out this.UmbracoHelper)),
                Mock.Of<IQRCodeHashIdFactory>(h => h.ComputeHash(It.IsAny<string>(), It.IsAny<QRCodeSettings>()) == hashId),
                Mock.Of<IColorParser>(),
                Mock.Of<ILogger<SvgFormat>>(),
                Mock.Of<IQRCodeType>(s => s.GetCodeContent() == "testCode"),
                DefaultQRCodeSettings
                );

            //Act
            var responseStream = qrCodeFormat.Stream();

            //Assert
            Assert.IsNotNull(responseStream);
            Assert.NotZero(responseStream.Length);

            responseStream.Dispose();
        }

        [Test]
        public void JpegFormat_WhenValueAndSettingsAreCorrent_ReturnResponseContent()
        {
            //Arrange
            var hashId = "hash";
            var qrCodeFormat = new JpegFormat(
              Mock.Of<IMediaService>(),
                Mock.Of<IUmbracoHelperAccessor>(a => a.TryGetUmbracoHelper(out this.UmbracoHelper)),
                Mock.Of<IQRCodeHashIdFactory>(h => h.ComputeHash(It.IsAny<string>(), It.IsAny<QRCodeSettings>()) == hashId),
                Mock.Of<IColorParser>(),
                Mock.Of<ILogger<JpegFormat>>(),
                Mock.Of<IQRCodeType>(s => s.GetCodeContent() == "testCode"),
                DefaultQRCodeSettings
                );

            //Act
            var responseStream = qrCodeFormat.Stream();

            //Assert
            Assert.IsNotNull(responseStream);
            Assert.NotZero(responseStream.Length);

            responseStream.Dispose();
        }

        [Test]
        public void PngFormat_WhenValueAndSettingsAreCorrent_ReturnResponseContent()
        {
            //Arrange
            var hashId = "hash";
            var qrCodeFormat = new PngFormat(
                Mock.Of<IMediaService>(),
                Mock.Of<IUmbracoHelperAccessor>(a => a.TryGetUmbracoHelper(out this.UmbracoHelper)),
                Mock.Of<IQRCodeHashIdFactory>(h => h.ComputeHash(It.IsAny<string>(), It.IsAny<QRCodeSettings>()) == hashId),
                Mock.Of<ILogger<PngFormat>>(),
                Mock.Of<IColorParser>(),
                Mock.Of<IQRCodeType>(s => s.GetCodeContent() == "testCode"),
                DefaultQRCodeSettings
                );

            //Act
            var responseStream = qrCodeFormat.Stream();

            //Assert
            Assert.IsNotNull(responseStream);
            Assert.NotZero(responseStream.Length);

            responseStream.Dispose();
        }
    }
}