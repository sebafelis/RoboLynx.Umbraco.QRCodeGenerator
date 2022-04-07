using NUnit.Framework;
using RoboLynx.Umbraco.QRCodeGenerator.Models;

namespace RoboLynx.Umbraco.QRCodeGenerator.Tests
{
    public abstract class QRCodeGeneratorBaseTest : UmbracoBaseTest
    {
        public QRCodeSettings DefaultQRCodeSettings { get; set; }

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            SetDefaultVariable();
        }

        //private UmbracoBackOfficeIdentity CreateIdentity(int userId)
        //{
        //    var sessionId = Guid.NewGuid().ToString();

        //    var allowedApplications = new[] { "content", "media" };
        //    var culture = "en-us";
        //    var realName = "hello world";
        //    var roles = new[] { "admin" };
        //    var startContentNodes = new[] { -1 };
        //    var startMediaNodes = new[] { 654 };
        //    var username = "testing";

        //    var identity = new UmbracoBackOfficeIdentity(userId, username, realName, startContentNodes, startMediaNodes, culture, sessionId, securityStamp, allowedApplications, roles);
        //    return identity;
        //}

        private void SetDefaultVariable()
        {
            DefaultQRCodeSettings = new QRCodeSettings()
            {
                DarkColor = "#000000",
                LightColor = "#ffffff",
                DrawQuiteZone = true,
                ECCLevel = ECCLevel.L,
                Format = "svg",
                Icon = null,
                IconBorderWidth = 1,
                IconSizePercent = 20,
                Size = 20
            };
        }
    }
}