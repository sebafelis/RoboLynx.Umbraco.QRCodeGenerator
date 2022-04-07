using Moq;
using NUnit.Framework;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeSources;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes;
using Umbraco.Cms.Core.Services;

namespace RoboLynx.Umbraco.QRCodeGenerator.Tests.Unit.Types
{
    [TestFixture]
    public class QRCodeTypeFactoryTest
    {
        [Test]
        public void GeolocationGEOTypeFactory_CreateMethod_ReturnCorrectObjectType()
        {
            //Assign
            IQRCodeTypeFactory factory = new GeolocationGEOTypeFactory(Mock.Of<ILocalizedTextService>());

            Create_ReturnCorrectObjectType<GeolocationGEOType>(factory);
        }

        [Test]
        public void GeolocationGooleMapTypeFactory_CreateMethod_ReturnCorrectObjectType()
        {
            //Assign
            IQRCodeTypeFactory factory = new GeolocationGooleMapTypeFactory(Mock.Of<ILocalizedTextService>());

            Create_ReturnCorrectObjectType<GeolocationGooleMapType>(factory);
        }

        [Test]
        public void PhoneNumberTypeFactory_CreateMethod_ReturnCorrectObjectType()
        {
            //Assign
            IQRCodeTypeFactory factory = new PhoneNumberTypeFactory(Mock.Of<ILocalizedTextService>());

            Create_ReturnCorrectObjectType<PhoneNumberType>(factory);
        }

        [Test]
        public void SmsTypeFactory_CreateMethod_ReturnCorrectObjectType()
        {
            //Assign
            IQRCodeTypeFactory factory = new SmsTypeFactory(Mock.Of<ILocalizedTextService>());

            Create_ReturnCorrectObjectType<SmsType>(factory);
        }

        [Test]
        public void TextTypeFactory_CreateMethod_ReturnCorrectObjectType()
        {
            //Assign
            IQRCodeTypeFactory factory = new TextTypeFactory(Mock.Of<ILocalizedTextService>());

            Create_ReturnCorrectObjectType<TextType>(factory);
        }

        [Test]
        public void UrlTypeFactory_CreateMethod_ReturnCorrectObjectType()
        {
            //Assign
            IQRCodeTypeFactory factory = new UrlTypeFactory(Mock.Of<ILocalizedTextService>());

            Create_ReturnCorrectObjectType<UrlType>(factory);
        }

        private void Create_ReturnCorrectObjectType<T>(IQRCodeTypeFactory factory)
        {
            //Act
            var type = factory.Create(Mock.Of<IQRCodeSource>());

            //Assert
            Assert.IsInstanceOf<T>(type);
        }
    }
}