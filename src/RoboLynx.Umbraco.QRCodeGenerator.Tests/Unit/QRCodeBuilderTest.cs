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
using Umbraco.Core.Models.PublishedContent;

namespace RoboLynx.Umbraco.QRCodeGenerator.Tests.Unit
{
    [TestFixture]
    public class QRCodeBuilderTest : QRCodeGeneratorBaseTest
    {
        const string editorAlias = "qrCodeGenerator";

        private readonly IEnumerable<KeyValuePair<string, object>> defaultConfiguration = new List<KeyValuePair<string, object>>()
        {
            new KeyValuePair<string, object>("codeSource", "AbsoluteUrl"),
            new KeyValuePair<string, object>("codeSourceSettings", "somePropertyAlias"),
            new KeyValuePair<string, object>("codeType", "URL"),
            new KeyValuePair<string, object>("defaultSize", "40"),
            new KeyValuePair<string, object>("defaultFormat", "svg"),
            new KeyValuePair<string, object>("defaultDarkColor", "#000000"),
            new KeyValuePair<string, object>("defaultLightColor", "#ffffff"),
            new KeyValuePair<string, object>("defaultIcon", null),
            new KeyValuePair<string, object>("defaultIconSizePercent", "10"),
            new KeyValuePair<string, object>("defaultIconBorderWidth", "1"),
            new KeyValuePair<string, object>("defaultDrawQuiteZone", "1"),
            new KeyValuePair<string, object>("defaultECCLevel", "M")
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
                    new List<KeyValuePair<string, object>>()
                    {
                        new KeyValuePair<string, object>("codeSource", "Source1"),
                        new KeyValuePair<string, object>("codeSourceSettings", "somePropertyAlias"),
                        new KeyValuePair<string, object>("codeType", "Type1"),
                        new KeyValuePair<string, object>("defaultSize", "40"),
                        new KeyValuePair<string, object>("defaultFormat", "Format1"),
                        new KeyValuePair<string, object>("defaultDarkColor", "#000000"),
                        new KeyValuePair<string, object>("defaultLightColor", "#ffffff"),
                        new KeyValuePair<string, object>("defaultIcon", null),
                        new KeyValuePair<string, object>("defaultIconSizePercent", "10"),
                        new KeyValuePair<string, object>("defaultIconBorderWidth", "1"),
                        new KeyValuePair<string, object>("defaultDrawQuiteZone", "1"),
                        new KeyValuePair<string, object>("defaultECCLevel", "M")
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
                    new List<KeyValuePair<string, object>>()
                    {
                        new KeyValuePair<string, object>("codeSource", "Source1"),
                        new KeyValuePair<string, object>("codeSourceSettings", "somePropertyAlias"),
                        new KeyValuePair<string, object>("codeType", "Type1"),
                        new KeyValuePair<string, object>("defaultSize", "40"),
                        new KeyValuePair<string, object>("defaultFormat", "Format1"),
                        new KeyValuePair<string, object>("defaultDarkColor", "#000000"),
                        new KeyValuePair<string, object>("defaultLightColor", "#ffffff"),
                        new KeyValuePair<string, object>("defaultIcon", null),
                        new KeyValuePair<string, object>("defaultIconSizePercent", "10"),
                        new KeyValuePair<string, object>("defaultIconBorderWidth", "1"),
                        new KeyValuePair<string, object>("defaultDrawQuiteZone", "1"),
                        new KeyValuePair<string, object>("defaultECCLevel", "M")
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
                    new List<KeyValuePair<string, object>>()
                    {
                        new KeyValuePair<string, object>("codeSource", "Source1"),
                        new KeyValuePair<string, object>("codeSourceSettings", "somePropertyAlias"),
                        new KeyValuePair<string, object>("codeType", "Type1"),
                        new KeyValuePair<string, object>("defaultSize", "40"),
                        new KeyValuePair<string, object>("defaultFormat", "Format1"),
                        new KeyValuePair<string, object>("defaultDarkColor", "#000000"),
                        new KeyValuePair<string, object>("defaultLightColor", "#ffffff"),
                        new KeyValuePair<string, object>("defaultIcon", "/path2/img.png"),
                        new KeyValuePair<string, object>("defaultIconSizePercent", "10"),
                        new KeyValuePair<string, object>("defaultIconBorderWidth", "1"),
                        new KeyValuePair<string, object>("defaultDrawQuiteZone", "1"),
                        new KeyValuePair<string, object>("defaultECCLevel", "M")
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
                    new List<KeyValuePair<string, object>>()
                    {
                        new KeyValuePair<string, object>("codeSource", "Source1"),
                        new KeyValuePair<string, object>("codeSourceSettings", "somePropertyAlias"),
                        new KeyValuePair<string, object>("codeType", "Type1"),
                        new KeyValuePair<string, object>("defaultSize", "40"),
                        new KeyValuePair<string, object>("defaultFormat", "Format1"),
                        new KeyValuePair<string, object>("defaultDarkColor", "#000000"),
                        new KeyValuePair<string, object>("defaultLightColor", "#ffffff"),
                        new KeyValuePair<string, object>("defaultIcon", "/path1/img.png"),
                        new KeyValuePair<string, object>("defaultIconSizePercent", "10"),
                        new KeyValuePair<string, object>("defaultIconBorderWidth", "1"),
                        new KeyValuePair<string, object>("defaultDrawQuiteZone", "1"),
                        new KeyValuePair<string, object>("defaultECCLevel", "M")
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
                   new List<KeyValuePair<string, object>>()
                   {
                        new KeyValuePair<string, object>("codeSource", "Source1"),
                        new KeyValuePair<string, object>("codeSourceSettings", "somePropertyAlias"),
                        new KeyValuePair<string, object>("codeType", "Type1"),
                        new KeyValuePair<string, object>("defaultSize", "40"),
                        new KeyValuePair<string, object>("defaultFormat", "Format1"),
                        new KeyValuePair<string, object>("defaultDarkColor", "#000000"),
                        new KeyValuePair<string, object>("defaultLightColor", "#ffffff"),
                        new KeyValuePair<string, object>("defaultIcon", "/path2/img.png"),
                        new KeyValuePair<string, object>("defaultIconSizePercent", "10"),
                        new KeyValuePair<string, object>("defaultIconBorderWidth", "1"),
                        new KeyValuePair<string, object>("defaultDrawQuiteZone", "1"),
                        new KeyValuePair<string, object>("defaultECCLevel", "M")
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

        public override void SetUp()
        {
            base.SetUp();


        }

        [Test]
        public void GetDefaultSettings_WhenPropertyWithSpecifyAliasExist_ShouldReturnCorrectSetting()
        {
            //Assign
            var contentId = 123;
            var propertyAlias = "qrCodePropertyAlias";

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

            var publishedDataType = new PublishedDataType(1234, editorAlias, new Lazy<object>(() => defaultConfiguration));

            var publishedPropertyType = new Mock<IPublishedPropertyType>();
            publishedPropertyType.Setup(s => s.DataType).Returns(publishedDataType);

            var publishedContentMock = new Mock<IPublishedContent>();
            SetupPropertyValue(publishedContentMock, publishedPropertyType.Object, propertyAlias, null);
            PublishedContentQuery.Setup(c => c.Content(contentId)).Returns(publishedContentMock.Object);

            var builder = new QRCodeBuilder(
                   new QRCodeFormat.QRCodeFormatsCollection(new IQRCodeFormat[] { Mock.Of<IQRCodeFormat>() }),
                   new QRCodeGenerator.QRCodeSources.QRCodeSourcesCollection(new IQRCodeSource[] { Mock.Of<IQRCodeSource>() }),
                   new QRCodeTypes.QRCodeTypesCollection(new IQRCodeType[] { Mock.Of<IQRCodeType>() })
               );

            //Act
            var returnedSettings = builder.GetDefaultSettings(publishedContentMock.Object, propertyAlias);

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

            var publishedContentMock = new Mock<IPublishedContent>();

            PublishedContentQuery.Setup(c => c.Content(contentId)).Returns(publishedContentMock.Object);

            var builder = new QRCodeBuilder(
                   new QRCodeFormat.QRCodeFormatsCollection(new IQRCodeFormat[] { Mock.Of<IQRCodeFormat>() }),
                   new QRCodeGenerator.QRCodeSources.QRCodeSourcesCollection(new IQRCodeSource[] { Mock.Of<IQRCodeSource>() }),
                   new QRCodeTypes.QRCodeTypesCollection(new IQRCodeType[] { Mock.Of<IQRCodeType>() })
               );

            //Act
            var returnedSettings = builder.GetDefaultSettings(publishedContentMock.Object, propertyAlias);

            //Assert
            Assert.IsNull(returnedSettings);
        }

        [Test]
        public void GetDefaultSettings_WhenPropertyWithSpecifyAliasExist_WhenPropertyIsNotQRCodeGeneratorType_ShouldReturnNull()
        {
            //Assign
            var contentId = 123;
            var propertyAlias = "qrCodePropertyAlias";

            var publishedDataType = new PublishedDataType(1234, "anyEditorAlias", new Lazy<object>(() => defaultConfiguration));

            var publishedPropertyType = new Mock<IPublishedPropertyType>();
            publishedPropertyType.Setup(s => s.DataType).Returns(publishedDataType);

            var publishedContentMock = new Mock<IPublishedContent>();
            SetupPropertyValue(publishedContentMock, publishedPropertyType.Object, propertyAlias, null);
            PublishedContentQuery.Setup(c => c.Content(contentId)).Returns(publishedContentMock.Object);

            var builder = new QRCodeBuilder(
                   new QRCodeFormat.QRCodeFormatsCollection(new IQRCodeFormat[] { Mock.Of<IQRCodeFormat>() }),
                   new QRCodeGenerator.QRCodeSources.QRCodeSourcesCollection(new IQRCodeSource[] { Mock.Of<IQRCodeSource>() }),
                   new QRCodeTypes.QRCodeTypesCollection(new IQRCodeType[] { Mock.Of<IQRCodeType>() })
               );

            //Act
            var returnedSettings = builder.GetDefaultSettings(publishedContentMock.Object, propertyAlias);

            //Assert
            Assert.IsNull(returnedSettings);
        }

        [TestCaseSource("CreateQRCodeAsResponse_CorrentConfiguration_Data")]
        public void CreateConfiguration_WhenPropertyExistAndSettingsArePassedByUser_ShouldReturnCorrectConfiguration(
            QRCodeSettings userSettings,
            IEnumerable<KeyValuePair<string, object>> defaultPrevalues,
            QRCodeConfig expectedConfig)
        {
            //Assign
            var contentId = 123;
            var propertyAlias = "qrCodePropertyAlias";


            var publishedDataType = new PublishedDataType(1234, editorAlias, new Lazy<object>(() => defaultPrevalues));

            var publishedPropertyType = new Mock<IPublishedPropertyType>();
            publishedPropertyType.Setup(s => s.DataType).Returns(publishedDataType);

            var publishedContentMock = new Mock<IPublishedContent>();
            SetupPropertyValue(publishedContentMock, publishedPropertyType.Object, propertyAlias, null);
            PublishedContentQuery.Setup(c => c.Content(contentId)).Returns(publishedContentMock.Object);

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
                   })
               );

            //Act
            var configuration = builder.CreateConfiguration(publishedContentMock.Object, propertyAlias, userSettings);


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
            string culture = null;

            var publishedDataType = new PublishedDataType(1234, editorAlias, new Lazy<object>(() => defaultConfiguration));

            var publishedPropertyType = new Mock<IPublishedPropertyType>();
            publishedPropertyType.Setup(s => s.DataType).Returns(publishedDataType);

            var publishedContentMock = new Mock<IPublishedContent>();
            SetupPropertyValue(publishedContentMock, publishedPropertyType.Object, propertyAlias, null);

            var publishedContent = publishedContentMock.Object;

            PublishedContentQuery.Setup(c => c.Content(contentId)).Returns(publishedContent);

            var httpContent = Mock.Of<HttpContent>();
            var svgFormat = Mock.Of<IQRCodeFormat>(f => f.Id == "svg" && f.ResponseContent(It.IsAny<string>(), It.IsAny<QRCodeSettings>()) == httpContent);
            var absoluteUrlSource = Mock.Of<IQRCodeSource>(s => s.Id == "AbsoluteUrl");
            var urlType = Mock.Of<IQRCodeType>(t => t.Id == "URL");


            var builder = new QRCodeBuilder(
                   new QRCodeFormat.QRCodeFormatsCollection(new IQRCodeFormat[] { svgFormat }),
                   new QRCodeGenerator.QRCodeSources.QRCodeSourcesCollection(new IQRCodeSource[] { absoluteUrlSource }),
                   new QRCodeTypes.QRCodeTypesCollection(new IQRCodeType[] { urlType })
               );

            //Act
            var httpRespones = builder.CreateQRCodeAsResponse(publishedContent, propertyAlias, culture, null);

            //Assert
            Assert.NotNull(httpRespones);
            Assert.AreEqual(httpContent, httpRespones);
        }
    }
}
