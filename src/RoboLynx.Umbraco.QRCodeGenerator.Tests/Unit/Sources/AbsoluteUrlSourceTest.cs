using NUnit.Framework;

namespace RoboLynx.Umbraco.QRCodeGenerator.Tests.Unit.Sources
{
    [TestFixture]
    public class AbsoluteUrlSourceTest : QRCodeGeneratorBaseTest
    {
        //[Test]
        //public void GetValue_WhenNUllKeyAndNullIndexIsPassAndContentExist_ShouldReturnStringValue()
        //{
        //    //Arrange
        //    var source = new AbsoluteUrlSource(Mock.Of<ILocalizedTextService>());
        //    var publishedContent = Mock.Of<IPublishedContent>();
        //    var url = @"https:\\testurl.cc\content\";

        //    Current.Factory = Mock.Of<IFactory>();

        //    var _umbracoContextFactory = new UmbracoContextFactory(
        //             Mock.Of<IUmbracoContextAccessor>(),
        //             Mock.Of<IPublishedSnapshotService>(),
        //             Mock.Of<IVariationContextAccessor>(),
        //             Mock.Of<IDefaultCultureAccessor>(),
        //             new UmbracoSettingsSection(),
        //             Mock.Of<IGlobalSettings>(),
        //             new UrlProviderCollection(new IUrlProvider[] { Mock.Of<IUrlProvider>(p => p.GetUrl(It.IsAny<UmbracoContext>(), publishedContent, UrlMode.Absolute, It.IsAny<string>(), It.IsAny<Uri>()) == new UrlInfo(url, true, null)) }),
        //             new MediaUrlProviderCollection(new IMediaUrlProvider[] { Mock.Of<IMediaUrlProvider>() }),
        //             Mock.Of<IUserService>()
        //         );

        //    var umbracoContext = _umbracoContextFactory.EnsureUmbracoContext(Mock.Of<HttpContextBase>());


        //    var umbracoContextAnccestor = Mock.Of<IUmbracoContextAccessor>(a => a.UmbracoContext == umbracoContext.UmbracoContext);


        //    Mock.Get(Current.Factory).Setup(s => s.GetInstance(typeof(IUmbracoContextAccessor))).Returns(umbracoContextAnccestor);

        //    //Act
        //    var value = source.GetValue<string>(0, null, publishedContent, null);

        //    //Assert
        //    Assert.NotNull(value);
        //    Assert.IsInstanceOf<string>(value);
        //    Assert.AreEqual(value, url);
        //}
    }
}
