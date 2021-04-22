using Moq;
using NUnit.Framework;
using RoboLynx.Umbraco.QRCodeGenerator.Extensions;
using RoboLynx.Umbraco.QRCodeGenerator.Models;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeFormat;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeSources;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes;
using System;
using System.Collections.Generic;
using System.Net.Http;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.Services;

namespace RoboLynx.Umbraco.QRCodeGenerator.Tests.Unit
{
    [TestFixture]
    public class QRCodeBuilderTest
    {
        private readonly Dictionary<string, PreValue> defaultPrevalues = new Dictionary<string, PreValue>()
        {
            { "codeSource", new PreValue("AbsoluteUrl") },
            {"codeSourceSettings", new PreValue("somePropertyAlias") },
            {"codeType", new PreValue("URL") },
            {"defaultSize", new PreValue("40") },
            {"defaultFormat", new PreValue("svg") },
            {"defaultDarkColor", new PreValue("#000000") },
            {"defaultLightColor", new PreValue("#ffffff") },
            {"defaultIcon", new PreValue(null) },
            {"defaultIconSizePercent", new PreValue("10") },
            {"defaultIconBorderWidth", new PreValue("1") },
            {"defaultDrawQuiteZone", new PreValue("1") },
            {"defaultECCLevel", new PreValue("M") }
        };

        static IEnumerable<TestCaseData> CreateQRCodeAsResponse_CorrentConfiguration_Data
        {
            get
            {
                yield return new TestCaseData(
                    new QRCodeSettings()
                    {
                        Size = 40,
                        Format = "Format1",
                        DarkColor = "#000000",
                        LightColor = "#ffffff",
                        Icon = null,
                        IconSizePercent = 10,
                        IconBorderWidth = 1,
                        DrawQuiteZone = true,
                        ECCLevel = ECCLevel.M
                    },
                    new Dictionary<string, PreValue>()
                    {
                        { "codeSource", new PreValue("Source1") },
                        { "codeSourceSettings", new PreValue("somePropertyAlias") },
                        { "codeType", new PreValue("Type1") },
                        { "defaultSize", new PreValue("40") },
                        { "defaultFormat", new PreValue("Format1") },
                        { "defaultDarkColor", new PreValue("#000000") },
                        { "defaultLightColor", new PreValue("#ffffff") },
                        { "defaultIcon", new PreValue(null) },
                        { "defaultIconSizePercent", new PreValue("10") },
                        { "defaultIconBorderWidth", new PreValue("1") },
                        { "defaultDrawQuiteZone", new PreValue("1") },
                        { "defaultECCLevel", new PreValue("M") }
                    },
                    new QRCodeConfig()
                    {
                        Format = Mock.Of<IQRCodeFormat>(f => f.Id == "Format1"),
                        Source = Mock.Of<IQRCodeSource>(s => s.Id == "Source1"),
                        Type = Mock.Of<IQRCodeType>(t => t.Id == "Type1"),
                        SourceSettings = "somePropertyAlias",
                        Settings = new QRCodeSettings()
                        {
                            Size = 40,
                            Format = "Format1",
                            DarkColor = "#000000",
                            LightColor = "#ffffff",
                            Icon = null,
                            IconSizePercent = 10,
                            IconBorderWidth = 1,
                            DrawQuiteZone = true,
                            ECCLevel = ECCLevel.M
                        }
                    });

                yield return new TestCaseData(
                    new QRCodeSettings()
                    {
                        Size = 40,
                        Format = "Format2",
                        DarkColor = "#111111",
                        LightColor = "#eeeeee",
                        Icon = "/path/img.jpg",
                        IconSizePercent = 20,
                        IconBorderWidth = 4,
                        DrawQuiteZone = false,
                        ECCLevel = ECCLevel.H
                    },
                    new Dictionary<string, PreValue>()
                    {
                        { "codeSource", new PreValue("Source1") },
                        { "codeSourceSettings", new PreValue("somePropertyAlias") },
                        { "codeType", new PreValue("Type1") },
                        { "defaultSize", new PreValue("40") },
                        { "defaultFormat", new PreValue("Format1") },
                        { "defaultDarkColor", new PreValue("#000000") },
                        { "defaultLightColor", new PreValue("#ffffff") },
                        { "defaultIcon", new PreValue(null) },
                        { "defaultIconSizePercent", new PreValue("10") },
                        { "defaultIconBorderWidth", new PreValue("1") },
                        { "defaultDrawQuiteZone", new PreValue("1") },
                        { "defaultECCLevel", new PreValue("M") }
                    },
                    new QRCodeConfig()
                    {
                        Format = Mock.Of<IQRCodeFormat>(f => f.Id == "Format2"),
                        Source = Mock.Of<IQRCodeSource>(s => s.Id == "Source1"),
                        Type = Mock.Of<IQRCodeType>(t => t.Id == "Type1"),
                        SourceSettings = "somePropertyAlias",
                        Settings = new QRCodeSettings()
                        {
                            Size = 40,
                            Format = "Format2",
                            DarkColor = "#111111",
                            LightColor = "#eeeeee",
                            Icon = "/path/img.jpg",
                            IconSizePercent = 20,
                            IconBorderWidth = 4,
                            DrawQuiteZone = false,
                            ECCLevel = ECCLevel.H
                        }
                    });

                yield return new TestCaseData(
                    new QRCodeSettings()
                    {
                        Icon = ""
                    },
                    new Dictionary<string, PreValue>()
                    {
                        { "codeSource", new PreValue("Source1") },
                        { "codeSourceSettings", new PreValue("somePropertyAlias") },
                        { "codeType", new PreValue("Type1") },
                        { "defaultSize", new PreValue("40") },
                        { "defaultFormat", new PreValue("Format1") },
                        { "defaultDarkColor", new PreValue("#000000") },
                        { "defaultLightColor", new PreValue("#ffffff") },
                        { "defaultIcon", new PreValue("/path2/img.png") },
                        { "defaultIconSizePercent", new PreValue("10") },
                        { "defaultIconBorderWidth", new PreValue("1") },
                        { "defaultDrawQuiteZone", new PreValue("1") },
                        { "defaultECCLevel", new PreValue("M") }
                    },
                    new QRCodeConfig()
                    {
                        Format = Mock.Of<IQRCodeFormat>(f => f.Id == "Format1"),
                        Source = Mock.Of<IQRCodeSource>(s => s.Id == "Source1"),
                        Type = Mock.Of<IQRCodeType>(t => t.Id == "Type1"),
                        SourceSettings = "somePropertyAlias",
                        Settings = new QRCodeSettings()
                        {
                            Size = 40,
                            Format = "Format1",
                            DarkColor = "#000000",
                            LightColor = "#ffffff",
                            Icon = string.Empty,
                            IconSizePercent = 10,
                            IconBorderWidth = 1,
                            DrawQuiteZone = true,
                            ECCLevel = ECCLevel.M
                        }
                    });

                yield return new TestCaseData(
                    new QRCodeSettings()
                    {
                        Icon = "/path2/img.jpg"
                    },
                    new Dictionary<string, PreValue>()
                    {
                        { "codeSource", new PreValue("Source1") },
                        { "codeSourceSettings", new PreValue("somePropertyAlias") },
                        { "codeType", new PreValue("Type1") },
                        { "defaultSize", new PreValue("40") },
                        { "defaultFormat", new PreValue("Format1") },
                        { "defaultDarkColor", new PreValue("#000000") },
                        { "defaultLightColor", new PreValue("#ffffff") },
                        { "defaultIcon", new PreValue("/path1/img.png") },
                        { "defaultIconSizePercent", new PreValue("10") },
                        { "defaultIconBorderWidth", new PreValue("1") },
                        { "defaultDrawQuiteZone", new PreValue("1") },
                        { "defaultECCLevel", new PreValue("M") }
                    },
                    new QRCodeConfig()
                    {
                        Format = Mock.Of<IQRCodeFormat>(f => f.Id == "Format1"),
                        Source = Mock.Of<IQRCodeSource>(s => s.Id == "Source1"),
                        Type = Mock.Of<IQRCodeType>(t => t.Id == "Type1"),
                        SourceSettings = "somePropertyAlias",
                        Settings = new QRCodeSettings()
                        {
                            Size = 40,
                            Format = "Format1",
                            DarkColor = "#000000",
                            LightColor = "#ffffff",
                            Icon = "/path2/img.jpg",
                            IconSizePercent = 10,
                            IconBorderWidth = 1,
                            DrawQuiteZone = true,
                            ECCLevel = ECCLevel.M
                        }
                    });

                yield return new TestCaseData(
                   new QRCodeSettings()
                   {
                   },
                   new Dictionary<string, PreValue>()
                   {
                        { "codeSource", new PreValue("Source1") },
                        { "codeSourceSettings", new PreValue("somePropertyAlias") },
                        { "codeType", new PreValue("Type1") },
                        { "defaultSize", new PreValue("40") },
                        { "defaultFormat", new PreValue("Format1") },
                        { "defaultDarkColor", new PreValue("#000000") },
                        { "defaultLightColor", new PreValue("#ffffff") },
                        { "defaultIcon", new PreValue("/path2/img.png") },
                        { "defaultIconSizePercent", new PreValue("10") },
                        { "defaultIconBorderWidth", new PreValue("1") },
                        { "defaultDrawQuiteZone", new PreValue("1") },
                        { "defaultECCLevel", new PreValue("M") }
                   },
                   new QRCodeConfig()
                   {
                       Format = Mock.Of<IQRCodeFormat>(f => f.Id == "Format1"),
                       Source = Mock.Of<IQRCodeSource>(s => s.Id == "Source1"),
                       Type = Mock.Of<IQRCodeType>(t => t.Id == "Type1"),
                       SourceSettings = "somePropertyAlias",
                       Settings = new QRCodeSettings()
                       {
                           Size = 40,
                           Format = "Format1",
                           DarkColor = "#000000",
                           LightColor = "#ffffff",
                           Icon = "/path2/img.png",
                           IconSizePercent = 10,
                           IconBorderWidth = 1,
                           DrawQuiteZone = true,
                           ECCLevel = ECCLevel.M
                       }
                   });
            }
        }

        public virtual void SetupPropertyValue<T>(IPublishedContent publishedContentMock, string alias, T value, string propertyTypeAlias)
        {
            var property = new Mock<IPublishedProperty>();
            property.Setup(x => x.PropertyTypeAlias).Returns(propertyTypeAlias);
            property.Setup(x => x.Value).Returns(value);

            Mock.Get(publishedContentMock).Setup(x => x.GetProperty(alias)).Returns(property.Object);
        }

        [Test]
        public void GetDefaultSettings_WhenPropertyWithSpecifyAliasExist_ShouldReturnCorrectSetting()
        {
            //Assign
            var contentId = 123;
            var propertyAlias = "qrCodePropertyAlias";
            var propertyTypeAlias = "propertyTypeAlias";
            var dataTypeDefinitionId = 1234;
            IPublishedContent publishedContent = Mock.Of<IPublishedContent>(c => c.Id == contentId);

            var expectedResult = new QRCodeSettings()
            {
                Size = 40,
                Format = "svg",
                DarkColor = "#000000",
                LightColor = "#ffffff",
                Icon = null,
                IconSizePercent = 10,
                IconBorderWidth = 1,
                DrawQuiteZone = true,
                ECCLevel = ECCLevel.M
            };

            var builder = new QRCodeBuilder(
                   new QRCodeFormat.QRCodeFormatsCollection(new IQRCodeFormat[] { Mock.Of<IQRCodeFormat>() }),
                   new QRCodeGenerator.QRCodeSources.QRCodeSourcesCollection(new IQRCodeSource[] { Mock.Of<IQRCodeSource>() }),
                   new QRCodeTypes.QRCodeTypesCollection(new IQRCodeType[] { Mock.Of<IQRCodeType>() }),
                   Mock.Of<IContentService>(s => s.GetById(contentId) == Mock.Of<IContent>(c => c.Properties == new PropertyCollection(new List<Property>() {
                       new Property(new PropertyType(Mock.Of<IDataTypeDefinition>(d => d.Id == dataTypeDefinitionId && d.HasIdentity == true), propertyAlias)) }
                   ))),
                   Mock.Of<IDataTypeService>(s => s.GetPreValuesCollectionByDataTypeId(dataTypeDefinitionId) == new PreValueCollection(defaultPrevalues))
               );

            //Act
            var returnedSettings = builder.GetDefaultSettings(publishedContent, propertyAlias);

            //Assert
            Assert.AreEqual(expectedResult.DarkColor, returnedSettings.DarkColor);
            Assert.AreEqual(expectedResult.LightColor, returnedSettings.LightColor);
            Assert.AreEqual(expectedResult.DrawQuiteZone, returnedSettings.DrawQuiteZone);
            Assert.AreEqual(expectedResult.ECCLevel, returnedSettings.ECCLevel);
            Assert.AreEqual(expectedResult.Format, returnedSettings.Format);
            Assert.AreEqual(expectedResult.Icon, returnedSettings.Icon);
            Assert.AreEqual(expectedResult.IconBorderWidth, returnedSettings.IconBorderWidth);
            Assert.AreEqual(expectedResult.IconSizePercent, returnedSettings.IconSizePercent);
            Assert.AreEqual(expectedResult.Size, returnedSettings.Size);
        }

        [Test]
        public void GetDefaultSettings_WhenPropertyWithSpecifyAliasNotExist_ShouldReturnNull()
        {
            //Assign
            var contentId = 123;
            var propertyAlias = "qrCodePropertyAlias";
            var dataTypeDefinitionId = 1234;
            IPublishedContent publishedContent = Mock.Of<IPublishedContent>(c => c.Id == contentId);

            var builder = new QRCodeBuilder(
                   new QRCodeFormat.QRCodeFormatsCollection(new IQRCodeFormat[] { Mock.Of<IQRCodeFormat>() }),
                   new QRCodeGenerator.QRCodeSources.QRCodeSourcesCollection(new IQRCodeSource[] { Mock.Of<IQRCodeSource>() }),
                   new QRCodeTypes.QRCodeTypesCollection(new IQRCodeType[] { Mock.Of<IQRCodeType>() }),
                   Mock.Of<IContentService>(s => s.GetById(contentId) == Mock.Of<IContent>(c => c.Properties == new PropertyCollection(new List<Property>() {
                       new Property(new PropertyType(Mock.Of<IDataTypeDefinition>(d => d.Id == dataTypeDefinitionId), propertyAlias)) }
                   ))),
                   Mock.Of<IDataTypeService>(s => s.GetPreValuesCollectionByDataTypeId(dataTypeDefinitionId) == new PreValueCollection(defaultPrevalues))
               );

            //Act
            var returnedSettings = builder.GetDefaultSettings(publishedContent, propertyAlias);

            //Assert
            Assert.IsNull(returnedSettings);
        }

        [Test]
        public void GetDefaultSettings_WhenPropertyWithSpecifyAliasExist_WhenPropertyIsNotQRCodeGeneratorType_ShouldReturnNull()
        {
            //Assign
            var contentId = 123;
            var propertyAlias = "qrCodePropertyAlias";
            var propertyTypeAlias = "propertyTypeAlias";
            var dataTypeDefinitionId = 1234;
            IPublishedContent publishedContent = Mock.Of<IPublishedContent>(s => s.Id == contentId);

            SetupPropertyValue(publishedContent, propertyAlias, string.Empty, propertyTypeAlias);

            var builder = new QRCodeBuilder(
                   new QRCodeFormat.QRCodeFormatsCollection(new IQRCodeFormat[] { Mock.Of<IQRCodeFormat>() }),
                   new QRCodeGenerator.QRCodeSources.QRCodeSourcesCollection(new IQRCodeSource[] { Mock.Of<IQRCodeSource>() }),
                   new QRCodeTypes.QRCodeTypesCollection(new IQRCodeType[] { Mock.Of<IQRCodeType>() }),
                   Mock.Of<IContentService>(s => s.GetById(contentId) == Mock.Of<IContent>(c => c.Properties == new PropertyCollection(new List<Property>() {
                       new Property(new PropertyType(Mock.Of<IDataTypeDefinition>(d => d.Id == dataTypeDefinitionId), propertyAlias)) }
                   ))),
                   Mock.Of<IDataTypeService>(s => s.GetPreValuesCollectionByDataTypeId(dataTypeDefinitionId) == new PreValueCollection(defaultPrevalues))
               );

            //Act
            var returnedSettings = builder.GetDefaultSettings(publishedContent, propertyAlias);

            //Assert
            Assert.IsNull(returnedSettings);
        }

        [TestCaseSource("CreateQRCodeAsResponse_CorrentConfiguration_Data")]
        public void CreateConfiguration_WhenPropertyExistAndSettingsArePassedByUser_ShouldReturnCorrectConfiguration(
            QRCodeSettings userSettings,
            Dictionary<string, PreValue> defaultPrevalues,
            QRCodeConfig expectedConfig)
        {
            //Assign
            var contentId = 123;
            var propertyAlias = "qrCodePropertyAlias";
            var propertyTypeAlias = "propertyTypeAlias";
            var dataTypeDefinitionId = 1234;
            IPublishedContent publishedContent = Mock.Of<IPublishedContent>(p => p.Id == contentId);

            SetupPropertyValue(publishedContent, propertyAlias, string.Empty, propertyTypeAlias);

            var builder = new QRCodeBuilder(
                   new QRCodeFormat.QRCodeFormatsCollection(new IQRCodeFormat[] {
                       Mock.Of<IQRCodeFormat>(f => f.Id == "Format1"),
                       Mock.Of<IQRCodeFormat>(f => f.Id == "Format2"),
                       Mock.Of<IQRCodeFormat>(f => f.Id == "Format3")
                   }),
                   new QRCodeGenerator.QRCodeSources.QRCodeSourcesCollection(new IQRCodeSource[] {
                       Mock.Of<IQRCodeSource>(s => s.Id == "Source1"),
                       Mock.Of<IQRCodeSource>(s => s.Id == "Source2"),
                       Mock.Of<IQRCodeSource>(s => s.Id == "Source3")
                   }),
                   new QRCodeTypes.QRCodeTypesCollection(new IQRCodeType[] {
                       Mock.Of<IQRCodeType>(t => t.Id == "Type1"),
                       Mock.Of<IQRCodeType>(t => t.Id == "Type2"),
                       Mock.Of<IQRCodeType>(t => t.Id == "Type3")
                   }),
                   Mock.Of<IContentService>(s => s.GetById(contentId) == Mock.Of<IContent>(c => c.Properties == new PropertyCollection(new List<Property>() {
                       new Property(new PropertyType(Mock.Of<IDataTypeDefinition>(d => d.Id == dataTypeDefinitionId && d.HasIdentity == true), propertyAlias)) }
                   ))),
                   Mock.Of<IDataTypeService>(s => s.GetPreValuesCollectionByDataTypeId(dataTypeDefinitionId) == new PreValueCollection(defaultPrevalues))
               );

            //Act
            var configuration = builder.CreateConfiguration(publishedContent, propertyAlias, userSettings);


            //Assert
            Assert.AreEqual(expectedConfig.Format.Id, configuration.Format.Id);
            Assert.AreEqual(expectedConfig.Source.Id, configuration.Source.Id);
            Assert.AreEqual(expectedConfig.Type.Id, configuration.Type.Id);
            Assert.AreEqual(expectedConfig.SourceSettings, configuration.SourceSettings);

            Assert.AreEqual(expectedConfig.Settings.DarkColor, configuration.Settings.DarkColor);
            Assert.AreEqual(expectedConfig.Settings.LightColor, configuration.Settings.LightColor);
            Assert.AreEqual(expectedConfig.Settings.DrawQuiteZone, configuration.Settings.DrawQuiteZone);
            Assert.AreEqual(expectedConfig.Settings.ECCLevel, configuration.Settings.ECCLevel);
            Assert.AreEqual(expectedConfig.Settings.Format, configuration.Settings.Format);
            Assert.AreEqual(expectedConfig.Settings.Icon, configuration.Settings.Icon);
            Assert.AreEqual(expectedConfig.Settings.IconBorderWidth, configuration.Settings.IconBorderWidth);
            Assert.AreEqual(expectedConfig.Settings.IconSizePercent, configuration.Settings.IconSizePercent);
            Assert.AreEqual(expectedConfig.Settings.Size, configuration.Settings.Size);
        }

        [Test]
        public void CreateQRCodeAsResponse_WhenPropertyExist_WithCorrectConfiguration_ShouldReturnHttpContent()
        {
            //Assign
            var contentId = 123;
            var propertyAlias = "qrCodePropertyAlias";
            var propertyTypeAlias = "propertyTypeAlias";
            string culture = null;
            var dataTypeDefinitionId = 1234;

            IPublishedContent publishedContent = Mock.Of<IPublishedContent>(s => s.Id == contentId);

            SetupPropertyValue(publishedContent, propertyAlias, string.Empty, propertyTypeAlias);

            var httpContent = Mock.Of<HttpContent>();
            var svgFormat = Mock.Of<IQRCodeFormat>(f => f.Id == "svg" && f.ResponseContent(It.IsAny<string>(), It.IsAny<QRCodeSettings>()) == httpContent);
            var absoluteUrlSource = Mock.Of<IQRCodeSource>(s => s.Id == "AbsoluteUrl");
            var urlType = Mock.Of<IQRCodeType>(t => t.Id == "URL");


            var builder = new QRCodeBuilder(
                   new QRCodeFormat.QRCodeFormatsCollection(new IQRCodeFormat[] { svgFormat }),
                   new QRCodeGenerator.QRCodeSources.QRCodeSourcesCollection(new IQRCodeSource[] { absoluteUrlSource }),
                   new QRCodeTypes.QRCodeTypesCollection(new IQRCodeType[] { urlType }),
                   Mock.Of<IContentService>(s => s.GetById(contentId) == Mock.Of<IContent>(c => c.Properties == new PropertyCollection(new List<Property>() {
                       new Property(new PropertyType(Mock.Of<IDataTypeDefinition>(d => d.Id == dataTypeDefinitionId && d.HasIdentity), propertyAlias)) }
                   ))),
                   Mock.Of<IDataTypeService>(s => s.GetPreValuesCollectionByDataTypeId(dataTypeDefinitionId) == new PreValueCollection(defaultPrevalues))
               );

            //Act
            var httpRespones = builder.CreateQRCodeAsResponse(publishedContent, propertyAlias, culture, null);

            //Assert
            Assert.NotNull(httpRespones);
            Assert.AreEqual(httpContent, httpRespones);
        }
    }
}
