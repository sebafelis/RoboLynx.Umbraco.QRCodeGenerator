using Moq;
using NUnit.Framework;
using RoboLynx.Umbraco.QRCodeGenerator.Exceptions;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeSources;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes;

namespace RoboLynx.Umbraco.QRCodeGenerator.Tests.Unit.Types
{
    [TestFixture]
    public class QRCodeTypeTest
    {
        [TestCase("0.5", "1.5", "geo:0.5,1.5")]
        [TestCase("0", "0", "geo:0,0")]
        [TestCase("89", "179", "geo:89,179")]
        [TestCase("-89", "-179", "geo:-89,-179")]
        [TestCase("90", "180", "geo:90,180")]
        [TestCase("-90", "-180", "geo:-90,-180")]
        public void GeolocationGEOType_GetCodeContentMethod_ShouldReturnCorrectString(string lat, string lng, string expectedResult)
        {
            //Assign
            IQRCodeType type = new GeolocationGEOType(Mock.Of<IQRCodeSource>(
                                    s => s.GetValue<string>(0, "latitude") == lat
                                    && s.GetValue<string>(1, "longitude") == lng)
                                );

            //Act
            var codeContent = type.GetCodeContent();

            //Assert
            Assert.AreEqual(expectedResult, codeContent);
        }

        [TestCase("", "0")]
        [TestCase(null, "0")]
        [TestCase("0", "")]
        [TestCase("0", null)]
        [TestCase("90.5", "0")]
        [TestCase("91", "0")]
        [TestCase("-91", "0")]
        [TestCase("0", "-181")]
        [TestCase("0", "180.1")]
        public void GeolocationGEOType_GetCodeContentMethod_WhenValuesAreWrong_ShouldThrowValidationException(string lat, string lng)
        {
            //Assign
            IQRCodeType type = new GeolocationGEOType(Mock.Of<IQRCodeSource>(
                                    s => s.GetValue<string>(0, "latitude") == lat
                                    && s.GetValue<string>(1, "longitude") == lng)
                                );

            //Act and Assert
            Assert.Throws<ValidationQRCodeGeneratorException>(() => type.GetCodeContent());
        }
    }
}