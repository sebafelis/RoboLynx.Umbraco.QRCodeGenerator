// NUnit 3 Test
// See documentation : https://github.com/nunit/docs/wiki/NUnit-Documentation
using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace RoboLynx.Umbraco.QRCodeGenerator.Tests.Unit
{
    [TestFixture]
    public class QRCodeTypeValidators
    {

        public static IEnumerable<TestCaseData> UrlValidator_ShouldBeValid_TestCases
        {
            get
            {
                yield return new TestCaseData("https://our.umbraco.com/");
                yield return new TestCaseData("https://our.umbraco.com/documentation/extending/property-editors/package-manifest");
                yield return new TestCaseData(new Uri("https://our.umbraco.com/"));
            }
        }

        public static IEnumerable<TestCaseData> UrlValidator_ShouldNotBeValid_TestCases
        {
            get
            {
                yield return new TestCaseData("our.umbraco.com/");
                yield return new TestCaseData("our.umbraco.com/documentation/extending/property-editors/package-manifest");
                yield return new TestCaseData(new Uri("/", UriKind.Relative));
            }
        }

        public static IEnumerable<object> LatitudeValidator_Valid_TestCases = new object[]
        {
           "90",
           "-90",
           "89.3443434",
           "-89.34434",
           89,
           90,
           90.0d,
           90.0f,
           90.0m,
           -90.0d,
           -90.0f,
           -90.0m,
        };

        public static IEnumerable<object> LatitudeValidator_Unvalid_TestCases = new object[]
        {
           "90.5550",
           "-90.34434",
           91,
           90.23323d,
           90.23323d,
           90.23323f,
           90.23323m,
           -90.23323d,
           -90.23323f,
           -90.23323m,
        };

        public static IEnumerable<object> LongitudeValidator_Valid_TestCases = new object[]
       {
           "180",
           "-180",
           "179.3443434",
           "-179.34434",
           180,
           -180,
           179.34434d,
           179.34434f,
           179.344343434m,
           -179.34434d,
           -179.34434f,
           -179.344343434m
       };

        public static IEnumerable<object> LongitudeValidator_Unvalid_TestCases = new object[]
        {
           "180.5550",
           "-180.34434",
           181,
           -181,
           180.34434d,
           180.34434f,
           180.34434m,
           -180.34434d,
           -180.34434f,
           -180.34434m,
        };

        public static IEnumerable<object> NotEmptyValidator_Valid_TestCases = new object[]
        {
            "test",
            23,
            23.434,
            43.2323m,
            new object(),
            true
        };

        public static IEnumerable<object> NotEmptyValidator_Unvalid_TestCases = new object[]
        {
           "",
           null
        };

        public static IEnumerable<object> PhoneNumberValidator_Valid_TestCases = new object[]
        {
            "0106434774000",
            "00116434774000",
            "8106434774000",
            "+6434774000",
            "034774000",
            "016434774000",
            "+61291011948",
            "0283354600",
            "+61282294333",
            "+61386419083"
        };

        public static IEnumerable<object> PhoneNumberValidator_Unvalid_TestCases = new object[]
        {
           "  ",
           "sdassd",
           "+132-424-424",
           "s2323131323",
           "+61386419o83"
        };

        [TestCaseSource("UrlValidator_ShouldBeValid_TestCases")]
        public void UrlValidator_ShouldBeValid(object value)
        {
            //Assign
            RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes.Validators.IQRCodeTypeValidator validator = new RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes.Validators.UrlValidator();

            //Act
            var result = validator.Validate(value, out string message);

            //Assert
            Assert.IsTrue(result, "Passed value should be valid URL.");
            Assert.IsNull(message);
        }

        [TestCaseSource("UrlValidator_ShouldNotBeValid_TestCases")]
        public void UrlValidator_ShouldBeNotValid(object value)
        {
            //Assign
            RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes.Validators.IQRCodeTypeValidator validator = new RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes.Validators.UrlValidator();

            //Act
            var result = validator.Validate(value, out string message);

            //Assert
            Assert.IsFalse(result, "Passed value should not be valid URL.");
            Assert.IsNotEmpty(message);
        }


        [TestCaseSource("LatitudeValidator_Valid_TestCases")]
        public void LatitudeValidator_ShouldBeValid(object value)
        {
            //Assign
            RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes.Validators.IQRCodeTypeValidator validator = new RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes.Validators.LatitudeValidator();

            //Act
            var result = validator.Validate(value, out string message);

            //Assert
            Assert.IsTrue(result, "Passed value should not be valid Latitude.");
            Assert.IsNull(message);
        }

        [TestCaseSource("LatitudeValidator_Unvalid_TestCases")]
        public void LatitudeValidator_ShouldBeNotValid(object value)
        {
            //Assign
            RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes.Validators.IQRCodeTypeValidator validator = new RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes.Validators.LatitudeValidator();
            //Act

            var result = validator.Validate(value, out string message);

            //Assert
            Assert.IsFalse(result, "Passed value should not be valid Latitude.");
            Assert.IsNotEmpty(message);
        }

        [TestCaseSource("LongitudeValidator_Valid_TestCases")]
        public void LongitudeValidator_ShouldBeValid(object value)
        {
            //Assign
            RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes.Validators.IQRCodeTypeValidator validator = new RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes.Validators.LongitudeValidator();

            //Act
            var result = validator.Validate(value, out string message);

            //Assert
            Assert.IsTrue(result, "Passed value should not be valid Longitude.");
            Assert.IsNull(message);
        }

        [TestCaseSource("LongitudeValidator_Unvalid_TestCases")]
        public void LongitudeValidator_ShouldBeNotValid(object value)
        {
            //Assign
            RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes.Validators.IQRCodeTypeValidator validator = new RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes.Validators.LongitudeValidator();
            //Act

            var result = validator.Validate(value, out string message);

            //Assert
            Assert.IsFalse(result, "Passed value should not be valid Longitude.");
            Assert.IsNotEmpty(message);
        }

        [TestCaseSource("NotEmptyValidator_Valid_TestCases")]
        public void NotEmptyValidator_ShouldBeValid(object value)
        {
            //Assign
            RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes.Validators.IQRCodeTypeValidator validator = new RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes.Validators.NotEmptyValidator();

            //Act
            var result = validator.Validate(value, out string message);

            //Assert
            Assert.IsTrue(result, "Passed value should not be valid not empty value.");
            Assert.IsNull(message);
        }

        [TestCaseSource("NotEmptyValidator_Unvalid_TestCases")]
        public void NotEmptyValidator_ShouldBeNotValid(object value)
        {
            //Assign
            RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes.Validators.IQRCodeTypeValidator validator = new RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes.Validators.NotEmptyValidator();

            //Act
            var result = validator.Validate(value, out string message);

            //Assert
            Assert.IsFalse(result, "Passed value should not be valid not empty value.");
            Assert.IsNotEmpty(message);
        }

        [TestCaseSource("PhoneNumberValidator_Valid_TestCases")]
        public void PhoneNumberValidator_ShouldBeValid(object value)
        {
            //Assign
            RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes.Validators.IQRCodeTypeValidator validator = new RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes.Validators.PhoneNumberValidator();

            //Act
            var result = validator.Validate(value, out string message);

            //Assert
            Assert.IsTrue(result, "Passed value should not be valid phone number.");
            Assert.IsNull(message);
        }

        [TestCaseSource("PhoneNumberValidator_Unvalid_TestCases")]
        public void PhoneNumberValidator_ShouldBeNotValid(object value)
        {
            //Assign
            RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes.Validators.IQRCodeTypeValidator validator = new RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes.Validators.PhoneNumberValidator();

            //Act
            var result = validator.Validate(value, out string message);

            //Assert
            Assert.IsFalse(result, "Passed value should not be valid phone number.");
            Assert.IsNotEmpty(message);
        }
    }
}
