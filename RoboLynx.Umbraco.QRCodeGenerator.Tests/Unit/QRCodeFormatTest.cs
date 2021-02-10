using Moq;
using NUnit.Framework;
using RoboLynx.Umbraco.QRCodeGenerator.Controllers;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeFormat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core;
using Umbraco.Core.IO;
using Umbraco.Core.Services;

namespace RoboLynx.Umbraco.QRCodeGenerator.Tests.Unit
{
    [TestFixture]
    public class QRCodeFormatTest : QRCodeGeneratorBaseTest
    {

        [Test]
        public void BmpFormat_WhenValueAndSettingsAreCorrent_ReturnResponseContent()
        {

            //Arrange
            var qrCodeFormat = new BmpFormat(
                Mock.Of<ILocalizedTextService>(),
                Mock.Of<IMediaFileSystem>(),
                this.UmbracoHelper
                );

            //Act
            var response = qrCodeFormat.ResponseContent("testCode", DefaultQRCodeSettings);

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
                this.UmbracoHelper
                );

            //Act
            var response = qrCodeFormat.ResponseContent("testCode", DefaultQRCodeSettings);

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
                Mock.Of<IMediaFileSystem>(),
                this.UmbracoHelper
                );

            //Act
            var response = qrCodeFormat.ResponseContent("testCode", DefaultQRCodeSettings);

            //Assert
            Assert.AreEqual("image/jpeg", response.Headers.ContentType.MediaType);
            Assert.NotZero(response.Headers.ContentLength.Value);
        }

        [Test]
        public void PngFormat_WhenValueAndSettingsAreCorrent_ReturnResponseContent()
        {

            //Arrange
            var qrCodeFormat = new  PngFormat(
                Mock.Of<ILocalizedTextService>(),
                Mock.Of<IMediaFileSystem>(),
                this.UmbracoHelper
                );

            //Act
            var response = qrCodeFormat.ResponseContent("testCode", DefaultQRCodeSettings);

            //Assert
            Assert.AreEqual("image/png", response.Headers.ContentType.MediaType);
            Assert.NotZero(response.Headers.ContentLength.Value);
        }
    }
}
