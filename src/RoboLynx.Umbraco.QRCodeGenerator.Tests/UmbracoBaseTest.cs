﻿using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Security;
using Umbraco.Core.Cache;
using Umbraco.Core.Dictionary;
using Umbraco.Core.Logging;
using Umbraco.Core.Mapping;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.Services;
using Umbraco.Web;
using Umbraco.Web.PublishedCache;
using Umbraco.Web.Security;
using Umbraco.Web.Security.Providers;

namespace RoboLynx.Umbraco.QRCodeGenerator.Tests
{
    public abstract class UmbracoBaseTest
    {
        public ServiceContext ServiceContext;
        public MembershipHelper MembershipHelper;
        public UmbracoHelper UmbracoHelper;
        public UmbracoMapper UmbracoMapper;

        public Mock<ICultureDictionary> CultureDictionary;
        public Mock<ICultureDictionaryFactory> CultureDictionaryFactory;
        public Mock<IPublishedContentQuery> PublishedContentQuery;

        public Mock<HttpContextBase> HttpContext;
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
            this.UmbracoHelper = new UmbracoHelper(Mock.Of<IPublishedContent>(), Mock.Of<ITagQuery>(), CultureDictionaryFactory.Object, Mock.Of<IUmbracoComponentRenderer>(), PublishedContentQuery.Object, MembershipHelper);
            this.UmbracoMapper = new UmbracoMapper(new MapDefinitionCollection(new List<IMapDefinition>()));
        }

        public virtual void SetupHttpContext()
        {
            this.HttpContext = new Mock<HttpContextBase>();
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
            var memberTypeService = Mock.Of<IMemberTypeService>();
            var membershipProvider = new MembersMembershipProvider(memberService.Object, memberTypeService);

            this.memberCache = new Mock<IPublishedMemberCache>();
            this.MembershipHelper = new MembershipHelper(this.HttpContext.Object, this.memberCache.Object, membershipProvider, Mock.Of<RoleProvider>(), memberService.Object, memberTypeService, Mock.Of<IUserService>(), Mock.Of<IPublicAccessService>(), AppCaches.NoCache, Mock.Of<ILogger>());
        }

        public void SetupPropertyValue(Mock<IPublishedContent> publishedContentMock, string alias, object value, string culture = null, string segment = null)
        {
            var property = new Mock<IPublishedProperty>();
            property.Setup(x => x.Alias).Returns(alias);
            property.Setup(x => x.GetValue(culture, segment)).Returns(value);
            property.Setup(x => x.HasValue(culture, segment)).Returns(value != null);
            publishedContentMock.Setup(x => x.GetProperty(alias)).Returns(property.Object);
        }

        public void SetupPropertyValue(Mock<IPublishedContent> publishedContentMock, IPublishedPropertyType publishedPropertyType, string alias, object value, string culture = null, string segment = null)
        {
            var property = new Mock<IPublishedProperty>();
            property.Setup(x => x.Alias).Returns(alias);
            property.Setup(x => x.GetValue(culture, segment)).Returns(value);
            property.Setup(x => x.HasValue(culture, segment)).Returns(value != null);
            property.Setup(x => x.PropertyType).Returns(publishedPropertyType);

            publishedContentMock.Setup(x => x.GetProperty(alias)).Returns(property.Object);
        }

        protected Stream CreateMockStream(string content = "test stream")
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(content));
        }

        protected byte[] StreamToByteArray(Stream input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }
    }
}
