using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using RoboLynx.Umbraco.QRCodeGenerator.Frontend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboLynx.Umbraco.QRCodeGenerator.Tests.Unit
{
    [TestFixture]
    public class QueryCipherTest
    {
        [Test]
        public void OutputEncriptedAndDecriptedString_ShouldBeEqualInputString()
        {
            //Asset
            var inputString = "testValue";
            var queryCipher = new QueryCipher(DataProtectionProvider.Create("UnitTest"), Mock.Of<IOptions<QRCodeFrontendOptions>>());

            //Act
            var outputString = queryCipher.DecryptQuery(queryCipher.EncryptQuery(inputString));

            //Arrange
            Assert.AreEqual(inputString, outputString);
        }

        [Test]
        public void EncriptedString_ShouldNotBeEqualInputString()
        {
            //Asset
            var inputString = "testValue";
            var queryCipher = new QueryCipher(DataProtectionProvider.Create("UnitTest"), Mock.Of<IOptions<QRCodeFrontendOptions>>());

            //Act
            var outputString = queryCipher.EncryptQuery(inputString);

            //Arrange
            Assert.AreNotEqual(inputString, outputString);
        }
    }
}
