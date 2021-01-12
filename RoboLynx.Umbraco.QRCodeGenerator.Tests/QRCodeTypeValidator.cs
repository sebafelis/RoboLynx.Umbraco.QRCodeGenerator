// NUnit 3 tests
// See documentation : https://github.com/nunit/docs/wiki/NUnit-Documentation
using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace RoboLynx.Umbraco.QRCodeGenerator.Test
{
    [TestFixture]
    public class TestClass
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

        [TestCaseSource("UrlValidator_ShouldBeValid_TestCases")]
        public void UrlValidator_ShouldBeValid(object value)
        {
            RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes.Validators.IQRCodeTypeValidator validator = new RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes.Validators.UrlValidator();

            var result = validator.Validate(value, out string message);

            Assert.IsTrue(result, "Passed value should be valid URL.");
        }

        [TestCaseSource("UrlValidator_ShouldNotBeValid_TestCases")]
        public void UrlValidator_ShouldBeNotValid(object value)
        {
            RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes.Validators.IQRCodeTypeValidator validator = new RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes.Validators.UrlValidator();

            var result = validator.Validate(value, out string message);

            Assert.IsFalse(result, "Passed value should not be valid URL.");
        }
    }
}
