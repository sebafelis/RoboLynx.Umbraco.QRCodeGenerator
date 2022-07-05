using RoboLynx.Umbraco.QRCodeGenerator.Models;
using System;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RoboLynx.Umbraco.QRCodeGenerator
{
    public class MD5HashIdFactory : IQRCodeHashIdFactory
    {
        [Serializable]
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

            var binaryObject = ObjectToByteArray(configContainer);

            var hashAlgoritm = System.Security.Cryptography.HashAlgorithm.Create(System.Security.Cryptography.HashAlgorithmName.MD5.Name ?? "MD5");
            if (hashAlgoritm is null)
            {
                throw new NotSupportedException("Hash algorithm was not found.");
            }
            var hashData = hashAlgoritm.ComputeHash(binaryObject);

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

        public static byte[] ObjectToByteArray(object objData)
        {
            if (objData == null)
                return Array.Empty<byte>();

            return Encoding.UTF8.GetBytes(JsonSerializer.Serialize(objData, GetJsonSerializerOptions()));
        }

        private static JsonSerializerOptions GetJsonSerializerOptions()
        {
            return new JsonSerializerOptions()
            {
                PropertyNamingPolicy = null,
                WriteIndented = true,
                AllowTrailingCommas = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            };
        }
    }
}