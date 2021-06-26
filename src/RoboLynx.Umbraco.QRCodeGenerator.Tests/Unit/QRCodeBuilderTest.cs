using Moq;
using NUnit.Framework;
using RoboLynx.Umbraco.QRCodeGenerator.Cache;
using RoboLynx.Umbraco.QRCodeGenerator.Models;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeFormat;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeSources;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Models.PublishedContent;

namespace RoboLynx.Umbraco.QRCodeGenerator.Tests.Unit
{
    [TestFixture]
    public class QRCodeBuilderTest : QRCodeGeneratorBaseTest
    {
        const string editorAlias = "qrCodeGenerator";

        // Format1
        private const string Format1Id = "Format1";
        private static IQRCodeFormat Format1 => Mock.Of<IQRCodeFormat>();
        private static IQRCodeFormatFactory Format1Factory => Mock.Of<IQRCodeFormatFactory>(f => f.Id == Format1Id && f.Create(It.IsAny<IQRCodeType>(), It.IsAny<QRCodeSettings>()) == Format1);
        // Format2
        private const string Format2Id = "Format2";
        private static IQRCodeFormat Format2 => Mock.Of<IQRCodeFormat>();
        private static IQRCodeFormatFactory Format2Factory => Mock.Of<IQRCodeFormatFactory>(f => f.Id == Format2Id && f.Create(It.IsAny<IQRCodeType>(), It.IsAny<QRCodeSettings>()) == Format2);
        // Format3
        private const string Format3Id = "Format3";
        private static IQRCodeFormat Format3 => Mock.Of<IQRCodeFormat>();
        private static IQRCodeFormatFactory Format3Factory => Mock.Of<IQRCodeFormatFactory>(f => f.Id == Format3Id && f.Create(It.IsAny<IQRCodeType>(), It.IsAny<QRCodeSettings>()) == Format3);
        // Source1
        private const string Source1Id = "Source1";
        private static IQRCodeSource Source1 => Mock.Of<IQRCodeSource>();
        private static IQRCodeSourceFactory Source1Factory => Mock.Of<IQRCodeSourceFactory>(f => f.Id == Source1Id && f.Create(It.IsAny<IPublishedContent>(), It.IsAny<string>(), It.IsAny<string>()) == Source1);
        // Source2
        private const string Source2Id = "Source2";
        private static IQRCodeSource Source2 => Mock.Of<IQRCodeSource>();
        private static IQRCodeSourceFactory Source2Factory => Mock.Of<IQRCodeSourceFactory>(f => f.Id == Source2Id && f.Create(It.IsAny<IPublishedContent>(), It.IsAny<string>(), It.IsAny<string>()) == Source2);
        // Source3
        private const string Source3Id = "Source3";
        private static IQRCodeSource Source3 => Mock.Of<IQRCodeSource>();
        private static IQRCodeSourceFactory Source3Factory => Mock.Of<IQRCodeSourceFactory>(f => f.Id == Source3Id && f.Create(It.IsAny<IPublishedContent>(), It.IsAny<string>(), It.IsAny<string>()) == Source3);
        // Type1
        private const string Type1Id = "Type1";
        private static IQRCodeType Type1 => Mock.Of<IQRCodeType>();
        private static IQRCodeTypeFactory Type1Factory => Mock.Of<IQRCodeTypeFactory>(f => f.Id == Type1Id && f.Create(It.IsAny<IQRCodeSource>()) == Type1);
        // Type2
        private const string Type2Id = "Type2";
        private static IQRCodeType Type2 => Mock.Of<IQRCodeType>();
        private static IQRCodeTypeFactory Type2Factory => Mock.Of<IQRCodeTypeFactory>(f => f.Id == Type2Id && f.Create(It.IsAny<IQRCodeSource>()) == Type2);
        // Type3
        private const string Type3Id = "Type3";
        private static IQRCodeType Type3 => Mock.Of<IQRCodeType>();
        private static IQRCodeTypeFactory Type3Factory => Mock.Of<IQRCodeTypeFactory>(f => f.Id == Type3Id && f.Create(It.IsAny<IQRCodeSource>()) == Type3);


        private readonly IEnumerable<KeyValuePair<string, object>> _defaultConfiguration = new List<KeyValuePair<string, object>>()
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
                        Format = Format1Id,
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
                        new KeyValuePair<string, object>("codeSource", Source1Id),
                        new KeyValuePair<string, object>("codeSourceSettings", "somePropertyAlias"),
                        new KeyValuePair<string, object>("codeType", Type1Id),
                        new KeyValuePair<string, object>("defaultSize", "40"),
                        new KeyValuePair<string, object>("defaultFormat", Format1Id),
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
                        Format = Format1,
                        Source = Source1,
                        Type = Type1,
                        Settings = new QRCodeSettings()
                        {
                            Size = 40,
                            Format = Format1Id,
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
                        Format = Format2Id,
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
                        new KeyValuePair<string, object>("codeSource", Source1Id),
                        new KeyValuePair<string, object>("codeSourceSettings", "somePropertyAlias"),
                        new KeyValuePair<string, object>("codeType", Type1Id),
                        new KeyValuePair<string, object>("defaultSize", "40"),
                        new KeyValuePair<string, object>("defaultFormat", Format1Id),
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
                        Format = Format2,
                        Source = Source1,
                        Type = Type1,
                        Settings = new QRCodeSettings()
                        {
                            Size = 40,
                            Format = Format2Id,
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
                        new KeyValuePair<string, object>("codeSource", Source1Id),
                        new KeyValuePair<string, object>("codeSourceSettings", "somePropertyAlias"),
                        new KeyValuePair<string, object>("codeType", Type1Id),
                        new KeyValuePair<string, object>("defaultSize", "40"),
                        new KeyValuePair<string, object>("defaultFormat", Format1Id),
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
                        Format = Format1,
                        Source = Source1,
                        Type = Type1,
                        Settings = new QRCodeSettings()
                        {
                            Size = 40,
                            Format = Format1Id,
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
                        new KeyValuePair<string, object>("codeSource", Source1Id),
                        new KeyValuePair<string, object>("codeSourceSettings", "somePropertyAlias"),
                        new KeyValuePair<string, object>("codeType", Type1Id),
                        new KeyValuePair<string, object>("defaultSize", "40"),
                        new KeyValuePair<string, object>("defaultFormat", Format1Id),
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
                        Format = Format1,
                        Source = Source1,
                        Type = Type1,
                        Settings = new QRCodeSettings()
                        {
                            Size = 40,
                            Format = Format1Id,
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
                        new KeyValuePair<string, object>("codeSource", Source1Id),
                        new KeyValuePair<string, object>("codeSourceSettings", "somePropertyAlias"),
                        new KeyValuePair<string, object>("codeType", Type1Id),
                        new KeyValuePair<string, object>("defaultSize", "40"),
                        new KeyValuePair<string, object>("defaultFormat", Format1Id),
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
                       Format = Format1,
                       Source = Source1,
                       Type = Type1,
                       Settings = new QRCodeSettings()
                       {
                           Size = 40,
                           Format = Format1Id,
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

            var publishedDataType = new PublishedDataType(1234, editorAlias, new Lazy<object>(() => _defaultConfiguration));

            var publishedPropertyType = new Mock<IPublishedPropertyType>();
            publishedPropertyType.Setup(s => s.DataType).Returns(publishedDataType);

            var publishedContentMock = new Mock<IPublishedContent>();
            SetupPropertyValue(publishedContentMock, publishedPropertyType.Object, propertyAlias, null);
            PublishedContentQuery.Setup(c => c.Content(contentId)).Returns(publishedContentMock.Object);

            var builder = new QRCodeBuilder(
                   new QRCodeFormat.QRCodeFormatFactoryCollection(new IQRCodeFormatFactory[] { Mock.Of<IQRCodeFormatFactory>() }),
                   new QRCodeGenerator.QRCodeSources.QRCodeSourceFactoryCollection(new IQRCodeSourceFactory[] { Mock.Of<IQRCodeSourceFactory>() }),
                   new QRCodeTypes.QRCodeTypeFactoryCollection(new IQRCodeTypeFactory[] { Mock.Of<IQRCodeTypeFactory>() }),
                    Mock.Of<IQRCodeCacheManager>()
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
                   new QRCodeFormat.QRCodeFormatFactoryCollection(new IQRCodeFormatFactory[] { Mock.Of<IQRCodeFormatFactory>() }),
                   new QRCodeGenerator.QRCodeSources.QRCodeSourceFactoryCollection(new IQRCodeSourceFactory[] { Mock.Of<IQRCodeSourceFactory>() }),
                   new QRCodeTypes.QRCodeTypeFactoryCollection(new IQRCodeTypeFactory[] { Mock.Of<IQRCodeTypeFactory>() }),
                    Mock.Of<IQRCodeCacheManager>()
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

            var publishedDataType = new PublishedDataType(1234, "anyEditorAlias", new Lazy<object>(() => _defaultConfiguration));

            var publishedPropertyType = new Mock<IPublishedPropertyType>();
            publishedPropertyType.Setup(s => s.DataType).Returns(publishedDataType);

            var publishedContentMock = new Mock<IPublishedContent>();
            SetupPropertyValue(publishedContentMock, publishedPropertyType.Object, propertyAlias, null);
            PublishedContentQuery.Setup(c => c.Content(contentId)).Returns(publishedContentMock.Object);

            var builder = new QRCodeBuilder(
                   new QRCodeFormat.QRCodeFormatFactoryCollection(new IQRCodeFormatFactory[] { Mock.Of<IQRCodeFormatFactory>() }),
                   new QRCodeGenerator.QRCodeSources.QRCodeSourceFactoryCollection(new IQRCodeSourceFactory[] { Mock.Of<IQRCodeSourceFactory>() }),
                   new QRCodeTypes.QRCodeTypeFactoryCollection(new IQRCodeTypeFactory[] { Mock.Of<IQRCodeTypeFactory>() }),
                    Mock.Of<IQRCodeCacheManager>()
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
            var culture = "en-GB";

            var publishedDataType = new PublishedDataType(1234, editorAlias, new Lazy<object>(() => defaultPrevalues));

            var publishedPropertyType = new Mock<IPublishedPropertyType>();
            publishedPropertyType.Setup(s => s.DataType).Returns(publishedDataType);

            var publishedContentMock = new Mock<IPublishedContent>();
            SetupPropertyValue(publishedContentMock, publishedPropertyType.Object, propertyAlias, culture);
            PublishedContentQuery.Setup(c => c.Content(contentId)).Returns(publishedContentMock.Object);

            var builder = new QRCodeBuilder(
                   new QRCodeFormat.QRCodeFormatFactoryCollection(new IQRCodeFormatFactory[] {
                       Format1Factory,
                       Format2Factory,
                       Format3Factory
                   }),
                   new QRCodeGenerator.QRCodeSources.QRCodeSourceFactoryCollection(new IQRCodeSourceFactory[] {
                       Source1Factory,
                       Source2Factory,
                       Source3Factory
                   }),
                   new QRCodeTypes.QRCodeTypeFactoryCollection(new IQRCodeTypeFactory[] {
                       Type1Factory,
                       Type2Factory,
                       Type3Factory
                   }),
                   Mock.Of<IQRCodeCacheManager>()
               );

            //Act
            var configuration = builder.CreateConfiguration(publishedContentMock.Object, propertyAlias, culture, userSettings);


            //Assert
            Assert.AreEqual(expectedConfig.Format, configuration.Format);
            Assert.AreEqual(expectedConfig.Source, configuration.Source);
            Assert.AreEqual(expectedConfig.Type, configuration.Type);

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
        public async Task CreateQRCodeAsResponse_WhenPropertyExist_WithCorrectConfiguration_ShouldReturnHttpContent()
        {
            //Assign
            var contentId = 123;
            var propertyAlias = "qrCodePropertyAlias";

            var publishedDataType = new PublishedDataType(1234, editorAlias, new Lazy<object>(() => _defaultConfiguration));

            var publishedPropertyType = new Mock<IPublishedPropertyType>();
            publishedPropertyType.Setup(s => s.DataType).Returns(publishedDataType);

            var publishedContentMock = new Mock<IPublishedContent>();
            SetupPropertyValue(publishedContentMock, publishedPropertyType.Object, propertyAlias, null);

            var publishedContent = publishedContentMock.Object;

            PublishedContentQuery.Setup(c => c.Content(contentId)).Returns(publishedContent);

            var source = Mock.Of<IQRCodeSource>();
            var type = Mock.Of<IQRCodeType>();
            var format = Mock.Of<IQRCodeFormat>(f => f.Stream() == new MemoryStream(Encoding.UTF8.GetBytes("test stream")) && f.Mime == "format/mime");
            var qrCodeConfig = new QRCodeConfig() { Format = format, Source = source, Type = type, Settings = new QRCodeSettings() };

            var builder = new QRCodeBuilder(
                   new QRCodeFormatFactoryCollection(new IQRCodeFormatFactory[] { }),
                   new QRCodeSourceFactoryCollection(new IQRCodeSourceFactory[] { }),
                   new QRCodeTypeFactoryCollection(new IQRCodeTypeFactory[] { }),
                   Mock.Of<IQRCodeCacheManager>(m=>m.UrlSupport(It.IsAny<string>()) == false)
               );
            var httpRequest = new HttpRequestMessage();

            //Act
            var httpRespones = builder.CreateResponse(httpRequest, qrCodeConfig);

            var byteContent = await httpRespones.Content.ReadAsByteArrayAsync();

            //Assert
            Assert.NotNull(httpRespones);
            Assert.IsInstanceOf<StreamContent>(httpRespones.Content);
            Assert.AreEqual(StreamToByteArray(format.Stream()), byteContent);
            Assert.AreEqual(format.Mime, httpRespones.Content.Headers.ContentType.MediaType);
        }

        private byte[] StreamToByteArray(Stream input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }
    }
}
