using CSharpTest.Net.Interfaces;
using Moq;
using NUnit.Framework;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeSources;
using System;
using System.Web;
using Umbraco.Core.Composing;
using Umbraco.Core.Configuration;
using Umbraco.Core.Configuration.UmbracoSettings;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.Services;
using Umbraco.Web;
using Umbraco.Web.PublishedCache;
using Umbraco.Web.Routing;
using CoreComposing = Umbraco.Core.Composing;
using WebComposing = Umbraco.Web.Composing;

namespace RoboLynx.Umbraco.QRCodeGenerator.Tests.Unit.Sources
{
    [TestFixture]
    public class AbsoluteUrlSourceTest : QRCodeGeneratorBaseTest
    {
        [Test]
        public void GetValue_WhenNullKeyAndNullIndexIsPassAndContentExist_ShouldReturnStringValue()
        {
            //Arrange
            var publishedContent = Mock.Of<IPublishedContent>(c=> c.ItemType == PublishedItemType.Content && c.ContentType == Mock.Of<IPublishedContentType>(t=>t.ItemType == PublishedItemType.Content));
            var culture = "en";
            var source = new AbsoluteUrlSource(publishedContent, culture);
            var url = @"https:\\testurl.cc\content\";

            

            var _umbracoContextFactory = new UmbracoContextFactory(
                     Mock.Of<IUmbracoContextAccessor>(),
                     Mock.Of<IPublishedSnapshotService>(),
                     Mock.Of<IVariationContextAccessor>(),
                     Mock.Of<IDefaultCultureAccessor>(),
                     new UmbracoSettingsSection(),
                     Mock.Of<IGlobalSettings>(),
                     new UrlProviderCollection(new IUrlProvider[] { Mock.Of<IUrlProvider>(p => p.GetUrl(It.IsAny<UmbracoContext>(), publishedContent, UrlMode.Absolute, culture, It.IsAny<Uri>()) == new UrlInfo(url, true, culture)) }),
                     new MediaUrlProviderCollection(new IMediaUrlProvider[] { Mock.Of<IMediaUrlProvider>() }),
                     Mock.Of<IUserService>()
                 );

            var umbracoContext = _umbracoContextFactory.EnsureUmbracoContext(Mock.Of<HttpContextBase>());

            var umbracoContextAnccestor = Mock.Of<IUmbracoContextAccessor>(a => a.UmbracoContext == umbracoContext.UmbracoContext);
            
            var factoryMock = new Mock<IFactory>();
            factoryMock.Setup(s=> s.GetInstance(typeof(IUmbracoContextAccessor))).Returns(umbracoContextAnccestor);

            CoreComposing.Current.Factory = factoryMock.Object;
            
            //Act
            var value = source.GetValue<string>(0, null);

            //Assert
            Assert.NotNull(value);
            Assert.IsInstanceOf<string>(value);
            Assert.AreEqual(value, url);
        }
    }
}
