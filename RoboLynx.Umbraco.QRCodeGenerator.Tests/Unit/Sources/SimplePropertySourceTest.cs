using Moq;
using Moq.Language;
using NUnit.Framework;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeSources;
using RoboLynx.Umbraco.QRCodeGenerator.Tests.TestExtensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Composing;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.Services;

namespace RoboLynx.Umbraco.QRCodeGenerator.Tests.Unit.Sources
{
    [TestFixture]
    [SetCulture("en-US")]
    public class SimplePropertySourceTest : QRCodeGeneratorBaseTest
    {

        public override void SetUp()
        {
            base.SetUp();
        }

        [TestCaseGeneric("property1", "propertyValue1", "property1", "propertyValue1", TypeArguments = new Type[] { typeof(string) }, TestName = "GetValueByIndex_WhenPropertyValueIsString_AndReturnTypeIsString")]
        [TestCaseGeneric("property1", 34, "property1", 34, TypeArguments = new Type[] { typeof(int) }, TestName = "GetValueByIndex_WhenPropertyValueIsInteger_AndReturnTypeIsInteger")]
        [TestCaseGeneric("property1", 44, "property1", "44", TypeArguments = new Type[] { typeof(string) }, TestName = "GetValueByIndex_WhenPropertyValueIsInteger_AndReturnTypeIsString")]
        [TestCaseGeneric("property1", 12.23344, "property1", 12.23344, TypeArguments = new Type[] { typeof(double) }, TestName = "GetValueByIndex_WhenPropertyValueIsDouble_AndReturnTypeIsDouble")]
        [TestCaseGeneric("property1", 12.23344, "property1", 12, TypeArguments = new Type[] { typeof(int) }, TestName = "GetValueByIndex_WhenPropertyValueIsDouble_AndReturnTypeIsInt")]
        [TestCaseGeneric("property1", 12.23344, "property1", "12.23344", TypeArguments = new Type[] { typeof(string) }, TestName = "GetValueByIndex_WhenPropertyValueIsDouble_AndReturnTypeIsString")]
        [TestCaseGeneric("property1", "12.23344", "property1", 12.23344, TypeArguments = new Type[] { typeof(double) }, TestName = "GetValueByIndex_WhenPropertyValueIsString_AndReturnTypeIsDouble")]
        public void GetValue_WhenIndexIsPassAndPropertyExist_ShouldReturnValue<TReturn>(string propertyName, object propertyValue, string sourceSettings, object expectedResult) where TReturn : System.IConvertible
        {
            //Arrange
            string culture = null;

            var source = new SimplePropertySource(Mock.Of<ILocalizedTextService>());

            var mockedPublishedContent = new Mock<IPublishedContent>();

            SetupPropertyValue(mockedPublishedContent, propertyName, propertyValue);

            //Act
            var value = source.GetValue<TReturn>(0, null, mockedPublishedContent.Object, sourceSettings, culture);

            //Assert
            Assert.IsInstanceOf<TReturn>(value);
            Assert.AreEqual(value, expectedResult);
        }

        [TestCaseGeneric("property1", "propertyValue1", "{ Preperties: { 'key1': 'property1' } }", "key1", "propertyValue1", TypeArguments = new Type[] { typeof(string) }, TestName = "GetValueByKey_WhenPropertyValueIsString_AndReturnTypeIsString")]
        [TestCaseGeneric("property1", 34, "{ Preperties: { 'key1': 'property1' } }", "key1", 34, TypeArguments = new Type[] { typeof(int) }, TestName = "GetValueByKey_WhenPropertyValueIsInteger_AndReturnTypeIsInteger")]
        [TestCaseGeneric("property1", 44, "{ Preperties: { 'key1': 'property1' } }", "key1", "44", TypeArguments = new Type[] { typeof(string) }, TestName = "GetValueByKey_WhenPropertyValueIsInteger_AndReturnTypeIsString")]
        [TestCaseGeneric("property1", 12.23344, "{ Preperties: { 'key1': 'property1' } }", "key1", 12.23344, TypeArguments = new Type[] { typeof(double) }, TestName = "GetValueByKey_WhenPropertyValueIsDouble_AndReturnTypeIsDouble")]
        [TestCaseGeneric("property1", 12.23344, "{ preperties: { 'key1': 'property1' } }", "key1", 12, TypeArguments = new Type[] { typeof(int) }, TestName = "GetValueByKey_WhenPropertyValueIsDouble_AndReturnTypeIsInt")]
        [TestCaseGeneric("property1", 12.23344, "{ preperties: { 'key1': 'property1' } }", "key1", "12.23344", TypeArguments = new Type[] { typeof(string) }, TestName = "GetValueByKey_WhenPropertyValueIsDouble_AndReturnTypeIsString")]
        [TestCaseGeneric("property1", "12.23344", "{ preperties: { 'key1': 'property1' } }", "key1", 12.23344, TypeArguments = new Type[] { typeof(double) }, TestName = "GetValueByKey_WhenPropertyValueIsString_AndReturnTypeIsDouble")]
        public void GetValue_WhenKeyIsPassAndPropertyExist_ShouldReturnValue<TReturn>(string propertyName, object propertyValue, string sourceSettings, string key, object expectedResult) where TReturn : System.IConvertible
        {
            //Arrange
            string culture = null;

            var source = new SimplePropertySource(Mock.Of<ILocalizedTextService>());

            var mockedPublishedContent = new Mock<IPublishedContent>();

            SetupPropertyValue(mockedPublishedContent, propertyName, propertyValue);

            //Act
            var value = source.GetValue<TReturn>(-1, key, mockedPublishedContent.Object, sourceSettings, culture);

            //Assert
            Assert.IsInstanceOf<TReturn>(value);
            Assert.AreEqual(value, expectedResult);
        }

        public void GetValue_WhenIndexIsPassAndPropertyExist_ShouldReturnValue()
        {

        }

        public void GetValue_WhenIndexAndPropertyIsPassAndPropertyExist_ShouldReturnValue()
        {

        }


    }
}
