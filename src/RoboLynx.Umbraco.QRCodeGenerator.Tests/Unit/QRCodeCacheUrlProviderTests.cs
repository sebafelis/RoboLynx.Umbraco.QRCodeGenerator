using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;
using RoboLynx.Umbraco.QRCodeGenerator.Cache;
using System;
using Umbraco.Cms.Core.Models.PublishedContent;

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
            var requestUri = new Uri(requestUrl, UriKind.Absolute);
            var httpContext = Mock.Of<HttpContext>(c => c.Request == Mock.Of<HttpRequest>(r => r.Scheme == requestUri.Scheme
                                                                                          && r.Host == new HostString(requestUri.Host, requestUri.Port)
                                                                                          && r.Path == r.Path
                                                                                          && r.Query == r.Query));
            var httpContextAccessor = Mock.Of<IHttpContextAccessor>(c => c.HttpContext == httpContext);

            var urlProvider = new LocalCacheUrlProvider(httpContextAccessor, new Uri(baseUrl, UriKind.RelativeOrAbsolute), defaultUrlMode == UrlMode.Absolute);

            // Act
            var result = urlProvider.Url(path, urlMode);

            //Assert
            Assert.AreEqual(expectedResult, result);
        }
    }
}