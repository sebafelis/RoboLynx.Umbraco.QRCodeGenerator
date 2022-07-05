using NUnit.Framework;
using System.IO;
using SixLabors.ImageSharp;
using RoboLynx.Umbraco.QRCodeGenerator.ImageSharp;
using System.Security.Cryptography;
using System;
using System.Text;

namespace RoboLynx.Umbraco.QRCodeGenerator.Tests.Unit.ImageShape
{
    [TestFixture]
    public class ImageSharpSvgQRCoder
    {

        private string GetAssemblyPath()
        {
            return AppDomain.CurrentDomain.BaseDirectory;
        }

        private string ByteArrayToHash(byte[] data)
        {
#pragma warning disable SCS0006 // Weak hashing function.
            var md5 = MD5.Create();
#pragma warning restore SCS0006 // Weak hashing function.
            var hash = md5.ComputeHash(data);

            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }

        private string StringToHash(string data)
        {
            return ByteArrayToHash(Encoding.UTF8.GetBytes(data));
        }

        [Test]
        public void GetGraphic_WithModulSize()
        {
            //Create QR code
            var gen = new QRCoder.QRCodeGenerator();
            var data = gen.CreateQrCode("This is a quick test! 123#?", QRCoder.QRCodeGenerator.ECCLevel.L);
            var svg = new ImageSharpSvgQRCode(data).GetGraphic(5);

            var result = StringToHash(svg);
            Assert.AreEqual("105d6e61589dd7f8b74287c984fcbea4", result);
        }

        [Test]
        public void GetGraphic_WithModulSizeAndColors()
        {        
            //Create QR code
            var gen = new QRCoder.QRCodeGenerator();
            var data = gen.CreateQrCode("This is a quick test! 123#?",  QRCoder.QRCodeGenerator.ECCLevel.H);
            var svg = new ImageSharpSvgQRCode(data).GetGraphic(10, Color.Red, Color.White);

            var result = StringToHash(svg);
            Assert.AreEqual("539c9aa144cb93476b980469a867a619", result);
        }

        [Test]
        public void GetGraphic_WithViewBox()
        {
            //Create QR code
            var gen = new  QRCoder.QRCodeGenerator();
            var data = gen.CreateQrCode("This is a quick test! 123#?",  QRCoder.QRCodeGenerator.ECCLevel.H);
            var svg = new ImageSharpSvgQRCode(data).GetGraphic(new Size(128,128));

            var result = StringToHash(svg);
            Assert.AreEqual("a78a88bc31f78922abb660f61b79be46", result);
        }

        [Test]
        public void GetGraphic_WithViewBoxAndViewBoxAttrMode()
        {
            //Create QR code
            var gen = new  QRCoder.QRCodeGenerator();
            var data = gen.CreateQrCode("This is a quick test! 123#?",  QRCoder.QRCodeGenerator.ECCLevel.H);
            var svg = new ImageSharpSvgQRCode(data).GetGraphic(new Size(128, 128), sizingMode: ImageSharpSvgQRCode.SizingMode.ViewBoxAttribute);

            var result = StringToHash(svg);
            Assert.AreEqual("8ac53c2682954ba0af5b072ef881771a", result);
        }

        [Test]
        public void GetGraphic_WithDisableQuietZone()
        {
            //Create QR code
            var gen = new  QRCoder.QRCodeGenerator();
            var data = gen.CreateQrCode("This is a quick test! 123#?",  QRCoder.QRCodeGenerator.ECCLevel.H);
            var svg = new ImageSharpSvgQRCode(data).GetGraphic(10, Color.Red, Color.White, false);

            var result = StringToHash(svg);
            Assert.AreEqual("379b780e77ebd2a8e9407779015f0320", result);
        }

        [Test]
        public void GetGraphic_WithDisableQuietZoneAndHexColors()
        {
            //Create QR code
            var gen = new  QRCoder.QRCodeGenerator();
            var data = gen.CreateQrCode("This is a quick test! 123#?", QRCoder.QRCodeGenerator.ECCLevel.H);
            var svg = new ImageSharpSvgQRCode(data).GetGraphic(10, "#000000", "#ffffff", false);

            var result = StringToHash(svg);
            Assert.AreEqual("4ab0417cc6127e347ca1b2322c49ed7d", result);
        }

        [Test]
        public void GetGraphic_WithRasterLogoParameter()
        {
            //Create QR code
            var gen = new QRCoder.QRCodeGenerator();
            var data = gen.CreateQrCode("This is a quick test! 123#?", QRCoder.QRCodeGenerator.ECCLevel.H);

            //Used logo is licensed under public domain.
            var logoBitmap = Image.Load(GetAssemblyPath() + "\\assets\\testlogo.png");
            var logoObj = new ImageSharpSvgQRCode.SvgLogo(iconRasterized: logoBitmap, 15);
            Assert.AreEqual(ImageSharpSvgQRCode.SvgLogo.MediaType.PNG, logoObj.GetMediaType());

            var svg = new ImageSharpSvgQRCode(data).GetGraphic(10, Color.DarkGray, Color.White, logo: logoObj);
            
            var result = StringToHash(svg);
            Assert.AreEqual("e323c15890e31c85ada6c06c8abbf8fc", result);            
        }

        [Test]
        public void GetGraphic_WithEmbeddedSvgLogo_WithoutViewBoxArg()
        {
            //Create QR code
            var gen = new QRCoder.QRCodeGenerator();
            var data = gen.CreateQrCode("This is a quick test! 123#?",  QRCoder.QRCodeGenerator.ECCLevel.H);

            //Used logo is licensed under public domain.
            var logoSvg = File.ReadAllText(GetAssemblyPath() + "\\assets\\testlogo.svg");            
            var logoObj = new ImageSharpSvgQRCode.SvgLogo(logoSvg, 20);
            Assert.AreEqual(ImageSharpSvgQRCode.SvgLogo.MediaType.SVG, logoObj.GetMediaType());

            var svg = new ImageSharpSvgQRCode(data).GetGraphic(10, Color.DarkGray, Color.White, logo: logoObj);

            var result = StringToHash(svg);
            Assert.AreEqual("4777798d266a7a884ca4b0c935888e68", result);            
        }

        [Test]
        public void GetGraphic_WithEmbeddedSvgLogo_WithViewBoxArg()
        {
            //Create QR code
            var gen = new QRCoder.QRCodeGenerator();
            var data = gen.CreateQrCode("This is a quick test! 123#?", QRCoder.QRCodeGenerator.ECCLevel.H);

            //Used logo is licensed under public domain.
            var logoSvg = File.ReadAllText(GetAssemblyPath() + "\\assets\\testlogo2.svg");
            var logoObj = new ImageSharpSvgQRCode.SvgLogo(logoSvg, 20);
            Assert.AreEqual(ImageSharpSvgQRCode.SvgLogo.MediaType.SVG, logoObj.GetMediaType());

            var svg = new ImageSharpSvgQRCode(data).GetGraphic(10, Color.DarkGray, Color.White, logo: logoObj);

            var result = StringToHash(svg);
            Assert.AreEqual("4ad220f0beccd518e2f9c415d5b4c3d3", result);
        }

        [Test]
        public void GetGraphic_WithSvgLogo()
        {
            //Create QR code
            var gen = new QRCoder.QRCodeGenerator();
            var data = gen.CreateQrCode("This is a quick test! 123#?",  QRCoder.QRCodeGenerator.ECCLevel.H);

            //Used logo is licensed under public domain. R
            var logoSvg = File.ReadAllText(GetAssemblyPath() + "\\assets\\testlogo2.svg");
            var logoObj = new ImageSharpSvgQRCode.SvgLogo(logoSvg, 20, iconEmbedded: false);

            var svg = new ImageSharpSvgQRCode(data).GetGraphic(10, Color.DarkGray, Color.White, logo: logoObj);

            var result = StringToHash(svg);
            Assert.AreEqual("9074582545ade51c598b62b1a02928e5", result);
        }

        [Test]
        public void CreateInstance_WithoutParemeters()
        {
            var svgCode = new ImageSharpSvgQRCode();
            Assert.NotNull(svgCode);
            Assert.IsInstanceOf<ImageSharpSvgQRCode>(svgCode);
        }

        [Test]
        public void GetQRCode_FromHelper()
        {
            //Create QR code                   
            var svg = ImageSharpSvgQRCodeHelper.GetQRCode("A", 2, "#000000", "#ffffff",  QRCoder.QRCodeGenerator.ECCLevel.Q);

            var result = StringToHash(svg);
            Assert.AreEqual("f5ec37aa9fb207e3701cc0d86c4a357d", result);
        }
    }
}
