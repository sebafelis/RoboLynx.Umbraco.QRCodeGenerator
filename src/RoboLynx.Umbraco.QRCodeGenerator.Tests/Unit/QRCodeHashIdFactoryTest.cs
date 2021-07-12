using NUnit.Framework;
using RoboLynx.Umbraco.QRCodeGenerator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboLynx.Umbraco.QRCodeGenerator.Tests.Unit
{
    [TestFixture]
    public class QRCodeHashIdFactoryTest : QRCodeGeneratorBaseTest
    {
        [Test]
        public void MD5HashIdFactory_ComputeHashMethod_ReturnHash()
        {
            var codeContent = "testcontent";
            var settings = new QRCodeSettings()
            {
                Size = 40,
                Format = "format1",
                DarkColor = "#000000",
                LightColor = "#ffffff",
                Icon = null,
                IconSizePercent = 10,
                IconBorderWidth = 1,
                DrawQuiteZone = true,
                ECCLevel = ECCLevel.M
            };

            var hashFactory = new MD5HashIdFactory();

            var hash = hashFactory.ComputeHash(codeContent, settings);

            Assert.IsNotEmpty(hash);
        }

        [Test]
        public void MD5HashIdFactory_ComputeHashMethod_CallManyTimes_WithSameParameters_ReturnSameHash()
        {
            var iterations = 5;
            var hashes = new List<string>();
            for (var i = 0; i <= iterations; i++)
            {
                var codeContent = "testcontent";
                var settings = new QRCodeSettings()
                {
                    Size = 40,
                    Format = "format1",
                    DarkColor = "#000000",
                    LightColor = "#ffffff",
                    Icon = null,
                    IconSizePercent = 10,
                    IconBorderWidth = 1,
                    DrawQuiteZone = true,
                    ECCLevel = ECCLevel.M
                };

                var hashFactory = new MD5HashIdFactory();

                hashes.Add(hashFactory.ComputeHash(codeContent, settings));            
            }
            Assert.That(hashes.Distinct().Count(), Is.EqualTo(1), "Not all computed hashes are the same.");
        }

        [Test]
        public void MD5HashIdFactory_ComputeHashMethod_CallManyTimes_WithDifferentParameters_ReturnDifferentHash()
        {
            //Assign
            var items = new List<(string codeContent, QRCodeSettings settings)>
            {
                ("testcontentA",
                new QRCodeSettings()
                {
                    Size = 40,
                    Format = "format1",
                    DarkColor = "#000000",
                    LightColor = "#ffffff",
                    Icon = null,
                    IconSizePercent = 10,
                    IconBorderWidth = 1,
                    DrawQuiteZone = true,
                    ECCLevel = ECCLevel.M
                }),
                ("testcontentB",
                new QRCodeSettings()
                {
                    Size = 40,
                    Format = "format1",
                    DarkColor = "#000000",
                    LightColor = "#ffffff",
                    Icon = null,
                    IconSizePercent = 10,
                    IconBorderWidth = 1,
                    DrawQuiteZone = true,
                    ECCLevel = ECCLevel.M
                }),
                ("testcontentA",
                new QRCodeSettings()
                {
                    Size = 40,
                    Format = "format4",
                    DarkColor = "#000000",
                    LightColor = "#ffffff",
                    Icon = null,
                    IconSizePercent = 10,
                    IconBorderWidth = 1,
                    DrawQuiteZone = true,
                    ECCLevel = ECCLevel.M
                }),
                ("testcontentC",
                new QRCodeSettings()
                {
                    Size = 40,
                    Format = "format4",
                    DarkColor = "#000000",
                    LightColor = "#ffffff",
                    Icon = null,
                    IconSizePercent = 10,
                    IconBorderWidth = 1,
                    DrawQuiteZone = true,
                    ECCLevel = ECCLevel.M
                }),
                ("testcontentA",
                new QRCodeSettings()
                {
                    Size = 40,
                    Format = "format4",
                    DarkColor = "#000000",
                    LightColor = "#ffffff",
                    Icon = null,
                    IconSizePercent = 10,
                    IconBorderWidth = 1,
                    DrawQuiteZone = true,
                    ECCLevel = ECCLevel.L
                })
            };

            var hashFactory = new MD5HashIdFactory();
            var hashes = new List<string>();

            //Act
            foreach (var item in items)
            {     
                hashes.Add(hashFactory.ComputeHash(item.codeContent, item.settings));
            }

            //Assert
            Assert.That(hashes.Distinct().Count(), Is.EqualTo(items.Count()), "Some of the computed hashes are the same.");
        }
    }
}
