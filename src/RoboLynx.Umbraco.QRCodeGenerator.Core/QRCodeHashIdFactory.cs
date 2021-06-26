using RoboLynx.Umbraco.QRCodeGenerator.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace RoboLynx.Umbraco.QRCodeGenerator
{
    public class QRCodeHashIdFactory : IQRCodeHashIdFactory
    {
        protected struct QRCodeConfigContainer
        {
            public string CodeContent { get; }
            public QRCodeSettings Settings { get; }

            public QRCodeConfigContainer(string codeContent, QRCodeSettings settings) : this()
            {
                CodeContent = codeContent;
                Settings = settings;
            }
        }

        public string ComputeHash(string codeContent, QRCodeSettings settings)
        {
            var configContainer = new QRCodeConfigContainer(codeContent, settings);

            BinaryFormatter binaryFormatter = new();
            using MemoryStream memoryStream = new();
            binaryFormatter.Serialize(memoryStream, configContainer);

            var hashAlgoritm = System.Security.Cryptography.HashAlgorithm.Create(System.Security.Cryptography.HashAlgorithmName.MD5.Name);
            var hashData = hashAlgoritm.ComputeHash(memoryStream);

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            var sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data
            // and format each one as a hexadecimal string.
            for (int i = 0; i < hashData.Length; i++)
            {
                sBuilder.Append(hashData[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }
    }
}
