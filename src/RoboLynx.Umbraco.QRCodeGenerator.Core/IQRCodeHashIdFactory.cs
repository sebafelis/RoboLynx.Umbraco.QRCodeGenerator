using RoboLynx.Umbraco.QRCodeGenerator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboLynx.Umbraco.QRCodeGenerator
{
    public interface IQRCodeHashIdFactory
    {
        /// <summary>
        /// Compute hash ID by code content and settings. Hash is use to identify the identical QR codes. Is use to build file name also.
        /// </summary>
        /// <param name="codeContent">Content of the code.</param>
        /// <param name="settings">Settings of the code.</param>
        /// <returns>MD5 hash</returns>
        string ComputeHash(string codeContent, QRCodeSettings settings);
    }
}
