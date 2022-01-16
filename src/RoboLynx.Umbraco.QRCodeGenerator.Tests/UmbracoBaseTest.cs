using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;
using System.IO;
using System.Text;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Dictionary;
using Umbraco.Cms.Core.Mapping;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Core.Security;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Templates;
using Umbraco.Cms.Web.Common;

namespace RoboLynx.Umbraco.QRCodeGenerator.Tests
{
    public abstract class UmbracoBaseTest
    {
        public ServiceContext ServiceContext;
        public IMemberManager MemberManager;
        public UmbracoHelper UmbracoHelper;
        public UmbracoMapper UmbracoMapper;

        public Mock<ICultureDictionary> CultureDictionary;
        public Mock<ICultureDictionaryFactory> CultureDictionaryFactory;
        public Mock<IPublishedContentQuery> PublishedContentQuery;

        public Mock<HttpContext> HttpContext;
        public Mock<IMemberService> memberService;
        public Mock<IPublishedMemberCache> memberCache;

        [SetUp]
        public virtual void SetUp()
        {
            this.SetupHttpContext();
            this.SetupCultureDictionaries();
            this.SetupPublishedContentQuerying();
            this.SetupMembership();

            this.ServiceContext = ServiceContext.CreatePartial(userService: Mock.Of<IUserService>());
            this.UmbracoHelper = new UmbracoHelper(CultureDictionaryFactory.Object, Mock.Of<IUmbracoComponentRenderer>(), PublishedContentQuery.Object);
        }

        public virtual void SetupHttpContext()
        {
            this.HttpContext = new Mock<HttpContext>();
        }

        public virtual void SetupCultureDictionaries()
        {
            this.CultureDictionary = new Mock<ICultureDictionary>();
            this.CultureDictionaryFactory = new Mock<ICultureDictionaryFactory>();
            this.CultureDictionaryFactory.Setup(x => x.CreateDictionary()).Returns(this.CultureDictionary.Object);
        }

        public virtual void SetupPublishedContentQuerying()
        {
            this.PublishedContentQuery = new Mock<IPublishedContentQuery>();
        }

        public virtual void SetupMembership()
        {
            this.memberService = new Mock<IMemberService>();
            this.memberCache = new Mock<IPublishedMemberCache>();
            this.MemberManager = Mock.Of<IMemberManager>();
        }

        public static void SetupPropertyValue(Mock<IPublishedContent> publishedContentMock, string alias, object value, string culture = null, string segment = null)
        {
            var property = new Mock<IPublishedProperty>();
            property.Setup(x => x.Alias).Returns(alias);
            property.Setup(x => x.GetValue(culture, segment)).Returns(value);
            property.Setup(x => x.HasValue(culture, segment)).Returns(value != null);
            publishedContentMock.Setup(x => x.GetProperty(alias)).Returns(property.Object);
        }

        public static void SetupPropertyValue(Mock<IPublishedContent> publishedContentMock, IPublishedPropertyType publishedPropertyType, string alias, object value, string culture = null, string segment = null)
        {
            var property = new Mock<IPublishedProperty>();
            property.Setup(x => x.Alias).Returns(alias);
            property.Setup(x => x.GetValue(culture, segment)).Returns(value);
            property.Setup(x => x.HasValue(culture, segment)).Returns(value != null);
            property.Setup(x => x.PropertyType).Returns(publishedPropertyType);

            publishedContentMock.Setup(x => x.GetProperty(alias)).Returns(property.Object);
        }

        protected static Stream CreateMockStream(string content = "test stream")
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(content));
        }

        protected static byte[] StreamToByteArray(Stream input)
        {
            using MemoryStream ms = new();
            input.Seek(0, SeekOrigin.Begin);
            input.CopyTo(ms);
            return ms.ToArray();
        }
    }
}
