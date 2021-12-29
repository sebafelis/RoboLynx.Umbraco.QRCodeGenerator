using Moq;
using NUnit.Framework;
using RoboLynx.Umbraco.QRCodeGenerator.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;

namespace RoboLynx.Umbraco.QRCodeGenerator.Tests.Unit
{
    [TestFixture()]
    public class QRCodeCacheUrlProviderTests : QRCodeGeneratorBaseTest
    {
        [TestCase("/", UrlMode.Absolute, "http://domain.pl/subpage/", "image.ext", UrlMode.Default, "http://domain.pl/image.ext")]
        [TestCase("/", UrlMode.Absolute, "http://domain.pl/subpage/", "image.ext", UrlMode.Absolute, "http://domain.pl/image.ext")]
        [TestCase("/", UrlMode.Absolute, "http://domain.pl/subpage/", "image.ext", UrlMode.Auto, "http://domain.pl/image.ext")]
        [TestCase("/", UrlMode.Absolute, "http://domain.pl/subpage/", "image.ext", UrlMode.Relative, "/image.ext")]
        [TestCase("http://domain1.pl", UrlMode.Absolute, "http://domain2.pl/subpage/", "image.ext", UrlMode.Default, "http://domain1.pl/image.ext")]
        [TestCase("/directory", UrlMode.Absolute, "http://domain.pl/subpage/", "image.ext", UrlMode.Default, "http://domain.pl/directory/image.ext")]
        [TestCase("/directory/", UrlMode.Absolute, "http://domain.pl/subpage/", "image.ext", UrlMode.Default, "http://domain.pl/directory/image.ext")]
        [TestCase("/directory", UrlMode.Relative, "http://domain.pl/subpage/", "image.ext", UrlMode.Default, "/directory/image.ext")]
        [TestCase("/directory/", UrlMode.Relative, "http://domain.pl/subpage/", "image.ext", UrlMode.Default, "/directory/image.ext")]
        [TestCase("/directory", UrlMode.Relative, "http://domain.pl/subpage/", "image.ext", UrlMode.Absolute, "http://domain.pl/directory/image.ext")]
        [TestCase("http://domain.pl/directory", UrlMode.Relative, "http://domain.pl/subpage/", "image.ext", UrlMode.Absolute, "http://domain.pl/directory/image.ext")]
        [TestCase("http://domain.pl/directory", UrlMode.Relative, "http://domain.pl/subpage/", "image.ext", UrlMode.Default, "/directory/image.ext")]
        [TestCase("http://domain.pl/directory", UrlMode.Absolute, "http://domain.pl/subpage/", "image.ext", UrlMode.Absolute, "http://domain.pl/directory/image.ext")]
        [TestCase("http://domain.pl/directory", UrlMode.Absolute, "http://domain.pl/subpage/", "image.ext", UrlMode.Default, "http://domain.pl/directory/image.ext")]
        public void UrlMethod_WhenParametersAreCorrect_ThenReturnUrl(string baseUrl, UrlMode defaultUrlMode, string requestUrl, string path, UrlMode urlMode, string expectedResult)
        {
            // Assign
            var httpContext = new HttpContext(new HttpRequest(null, requestUrl, null), new HttpResponse(null));
            var httpContextAccessor = Mock.Of<IHttpContextAccessor>(c=>c.HttpContext == httpContext);

            var urlProvider = new LocalCacheUrlProvider(httpContextAccessor, new Uri(baseUrl, UriKind.RelativeOrAbsolute), defaultUrlMode == UrlMode.Absolute);
            var requestUri = !string.IsNullOrEmpty(requestUrl) ? new Uri(requestUrl, UriKind.RelativeOrAbsolute) : null;
            
            // Act
            var result = urlProvider.Url(path, urlMode);
            
            //Assert
            Assert.AreEqual(expectedResult, result);
        }
    }
}