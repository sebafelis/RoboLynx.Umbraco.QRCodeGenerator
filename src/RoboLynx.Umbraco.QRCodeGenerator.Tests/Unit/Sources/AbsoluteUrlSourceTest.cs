using Moq;
using NUnit.Framework;
using RoboLynx.Umbraco.QRCodeGenerator.QRCodeSources;
using System;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Web.Common.UmbracoContext;

namespace RoboLynx.Umbraco.QRCodeGenerator.Tests.Unit.Sources
{
    [TestFixture]
    public class AbsoluteUrlSourceTest : QRCodeGeneratorBaseTest
    {
        [Test]
        public void GetValue_WhenNullKeyAndNullIndexIsPassAndContentExist_ShouldReturnStringValue()
        {
            //Arrange
            var publishedContent = Mock.Of<IPublishedContent>(c=> c.ContentType == Mock.Of<IPublishedContentType>(t=>t.ItemType == PublishedItemType.Content));
            var url = @"https:\\testurl.cc\content\"; 
            var culture = "en";
            var publishedUrlProvider = Mock.Of<IPublishedUrlProvider>(p => p.GetUrl(publishedContent, UrlMode.Absolute, culture, It.IsAny<Uri>()) == url);
            var source = new AbsoluteUrlSource(publishedUrlProvider, publishedContent, culture);
            
            //Act
            var value = source.GetValue<string>(0, null);

            //Assert
            Assert.NotNull(value);
            Assert.IsInstanceOf<string>(value);
            Assert.AreEqual(value, url);
        }
    }
}
