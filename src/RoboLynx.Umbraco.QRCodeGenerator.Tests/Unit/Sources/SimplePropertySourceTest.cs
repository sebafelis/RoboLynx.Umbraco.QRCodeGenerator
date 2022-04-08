using Moq;
using NUnit.Framework;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeSources;
using RoboLynx.Umbraco.QRCodeGenerator.Tests.TestExtensions;
using System;
using System.Collections.Generic;
using Umbraco.Cms.Core.Models.Membership;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Security;

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

        // Different property type and output type
        [TestCaseGeneric("property1", "propertyValue1", "property1", 0, "propertyValue1", TypeArguments = new Type[] { typeof(string) }, TestName = "GetValueByIndex_WhenPropertyValueIsString_ThenReturnTypeIsString")]
        [TestCaseGeneric("property1", 34, "property1", 0, 34, TypeArguments = new Type[] { typeof(int) }, TestName = "GetValueByIndex_WhenPropertyValueIsInteger_ThenReturnTypeIsInteger")]
        [TestCaseGeneric("property1", 44, "property1", 0, "44", TypeArguments = new Type[] { typeof(string) }, TestName = "GetValueByIndex_WhenPropertyValueIsInteger_ThenReturnTypeIsString")]
        [TestCaseGeneric("property1", 12.23344, "property1", 0, 12.23344, TypeArguments = new Type[] { typeof(double) }, TestName = "GetValueByIndex_WhenPropertyValueIsDouble_ThenReturnTypeIsDouble")]
        [TestCaseGeneric("property1", 12.23344, "property1", 0, 12, TypeArguments = new Type[] { typeof(int) }, TestName = "GetValueByIndex_WhenPropertyValueIsDouble_ThenReturnTypeIsInt")]
        [TestCaseGeneric("property1", 12.23344, "property1", 0, "12.23344", TypeArguments = new Type[] { typeof(string) }, TestName = "GetValueByIndex_WhenPropertyValueIsDouble_ThenReturnTypeIsString")]
        [TestCaseGeneric("property1", "12.23344", "property1", 0, 12.23344, TypeArguments = new Type[] { typeof(double) }, TestName = "GetValueByIndex_WhenPropertyValueIsString_ThenReturnTypeIsDouble")]

        // Different property type and output type and settings containing Regex
        [TestCaseGeneric("property1", "0.5233,0.52323,6", @"property1{{(?<=,)\d*(\.\d*)?(?=,)}}", 0, 0.52323, TypeArguments = new Type[] { typeof(decimal) }, TestName = "GetValueByIndex_WhenIndexIs1_AndPropertyValueIsString_AndSettingsDefineOnePropertyWithRegex_ThenReturnDoubleValue")]

        //Many delimited properties in source settings
        [TestCaseGeneric("property2", "propertyValue2", "property1, property2", 1, "propertyValue2", TypeArguments = new Type[] { typeof(string) }, TestName = "GetValueByIndex_WhenIndexIs2_WhenPropertyValueIsString_AndSettingsDefineTwoProperties_ThenReturnStringValue")]
        [TestCaseGeneric("property3", "propertyValue3", "property1, property2, property3", 2, "propertyValue3", TypeArguments = new Type[] { typeof(string) }, TestName = "GetValueByIndex_WithIndexIs3_WhenPropertyValueIsString_AndSettingsDefineThreeProperties_ThenReturnStringValue")]

        //Many delimited properties with regular expression in source settings
        [TestCaseGeneric("property2", "0.5233,0.52323,6", @"property1{{^\d*(\.\d*)?}}, property2{{(?<=,)\d*(\.\d*)?(?=,)}}", 1, "0.52323", TypeArguments = new Type[] { typeof(string) }, TestName = "GetValueByIndex_WhenIndexIs2_WhenPropertyValueIsString_AndSettingsDefineTwoPropertiesWithRegex_ThenReturnStringValue")]
        [TestCaseGeneric("property3", "propertyValue3", "property1, property2, property3", 2, "propertyValue3", TypeArguments = new Type[] { typeof(string) }, TestName = "GetValueByIndex_WithIndexIs3_WhenPropertyValueIsString_AndSettingsDefineThreeProperties_ThenReturnStringValue")]
        public void GetValue_WhenIndexIsPassAndPropertyExist_ShouldReturnValueByIndex<TReturn>(string propertyName, object propertyValue, string sourceSettings, int index, object expectedResult) where TReturn : System.IConvertible
        {
            //Arrange
            string culture = null;

            var mockedPublishedContent = new Mock<IPublishedContent>();
            SetupPropertyValue(mockedPublishedContent, Mock.Of<IPublishedPropertyType>(pt=>pt.IsUserProperty == true && pt.ContentType == Mock.Of<IPublishedContentType>(ct => ct.ItemType == PublishedItemType.Member)), propertyName, propertyValue);
            
            var backOfficeSecurityAccessor =  Mock.Of<IBackOfficeSecurityAccessor>(sa => sa.BackOfficeSecurity == Mock.Of<IBackOfficeSecurity>(s => s.IsAuthenticated() == false && s.CurrentUser == Mock.Of<IUser>(u => u.AllowedSections == new List<string>() { "member" })));
            var source = new SimplePropertySource(backOfficeSecurityAccessor, Mock.Of<IPublishedValueFallback>(), mockedPublishedContent.Object, sourceSettings, culture);

            //Act
            var value = source.GetValue<TReturn>(index, null);

            //Assert
            Assert.IsInstanceOf<TReturn>(value);
            Assert.AreEqual(value, expectedResult);
        }

        [TestCaseGeneric("property1", "propertyValue1", "{ Properties: { 'key1': 'property1' } }", "key1", "propertyValue1", TypeArguments = new Type[] { typeof(string) }, TestName = "GetValueByKey_WhenPropertyValueIsString_AndReturnTypeIsString")]
        [TestCaseGeneric("property1", 34, "{ Properties: { 'key1': 'property1' } }", "key1", 34, TypeArguments = new Type[] { typeof(int) }, TestName = "GetValueByKey_WhenPropertyValueIsInteger_AndReturnTypeIsInteger")]
        [TestCaseGeneric("property1", 44, "{ Properties: { 'key1': 'property1' } }", "key1", "44", TypeArguments = new Type[] { typeof(string) }, TestName = "GetValueByKey_WhenPropertyValueIsInteger_AndReturnTypeIsString")]
        [TestCaseGeneric("property1", 12.23344, "{ Properties: { 'key1': 'property1' } }", "key1", 12.23344, TypeArguments = new Type[] { typeof(double) }, TestName = "GetValueByKey_WhenPropertyValueIsDouble_AndReturnTypeIsDouble")]
        [TestCaseGeneric("property1", 12.23344, "{ Properties: { 'key1': 'property1' } }", "key1", 12, TypeArguments = new Type[] { typeof(int) }, TestName = "GetValueByKey_WhenPropertyValueIsDouble_AndReturnTypeIsInt")]
        [TestCaseGeneric("property1", 12.23344, "{ Properties: { 'key1': 'property1' } }", "key1", "12.23344", TypeArguments = new Type[] { typeof(string) }, TestName = "GetValueByKey_WhenPropertyValueIsDouble_AndReturnTypeIsString")]
        [TestCaseGeneric("property1", "12.23344", "{ Properties: { 'key1': 'property1' } }", "key1", 12.23344, TypeArguments = new Type[] { typeof(double) }, TestName = "GetValueByKey_WhenPropertyValueIsString_AndReturnTypeIsDouble")]

        [TestCaseGeneric("property1", "0.5233,0.52323,6", @"{ Properties: { 'key1': { name: 'property1', regex: '(?<=,)\\d*(\\.\\d*)?(?=,)' } } }", "key1", 0.52323, TypeArguments = new Type[] { typeof(double) }, TestName = "GetValueByKey_AndSettingsDefineAsJson_ThenReturnIsDoubleValue")]
        public void GetValue_WhenKeyIsPassAndPropertyExist_ShouldReturnValueByKey<TReturn>(string propertyName, object propertyValue, string sourceSettings, string key, object expectedResult) where TReturn : System.IConvertible
        {
            //Arrange
            string culture = null;

            var mockedPublishedContent = new Mock<IPublishedContent>();
            SetupPropertyValue(mockedPublishedContent, Mock.Of<IPublishedPropertyType>(pt => pt.IsUserProperty == true && pt.ContentType == Mock.Of<IPublishedContentType>(ct => ct.ItemType == PublishedItemType.Member)), propertyName, propertyValue);

            var backOfficeSecurityAccessor = Mock.Of<IBackOfficeSecurityAccessor>(sa => sa.BackOfficeSecurity == Mock.Of<IBackOfficeSecurity>(s => s.IsAuthenticated() == false && s.CurrentUser == Mock.Of<IUser>(u => u.AllowedSections == new List<string>() { "member" })));
            var source = new SimplePropertySource(backOfficeSecurityAccessor, Mock.Of<IPublishedValueFallback>(), mockedPublishedContent.Object, sourceSettings, culture);

            //Act
            var value = source.GetValue<TReturn>(-1, key);

            //Assert
            Assert.IsInstanceOf<TReturn>(value);
            Assert.AreEqual(value, expectedResult);
        }

        //public void GetValue_WhenIndexIsPassAndPropertyExist_ShouldReturnValueByIndex()
        //{
        //}

        //public void GetValue_WhenIndexAndKeyIsPassAndPropertyExist_ShouldReturnValueByKey()
        //{
        //}
    }
}