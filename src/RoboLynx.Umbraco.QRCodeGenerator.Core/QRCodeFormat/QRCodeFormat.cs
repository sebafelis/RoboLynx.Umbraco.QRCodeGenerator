using RoboLynx.Umbraco.QRCodeGenerator.Exceptions;
using RoboLynx.Umbraco.QRCodeGenerator.Helpers;
using RoboLynx.Umbraco.QRCodeGenerator.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Umbraco.Core;
using Umbraco.Core.Services;
using Umbraco.Web;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeFormat
{
    public abstract class QRCodeFormat : IQRCodeFormat
    {
        protected readonly UmbracoHelper _umbracoHelper;
        private readonly string _codeContent;
        private readonly QRCodeSettings _settings;

        protected struct QRCodeConfigContainer
        {
            string CodeContent { get; }
            QRCodeSettings Settings { get; }

            public QRCodeConfigContainer(string codeContent, QRCodeSettings settings) : this()
            {
                CodeContent = codeContent;
                Settings = settings;
            }
        }

        public QRCodeFormat(UmbracoHelper umbracoHelper, string codeContent, QRCodeSettings settings)
        {
            _umbracoHelper = umbracoHelper;
            _codeContent = codeContent;
            _settings = settings;
        }

        public abstract string Mime { get; }

        public virtual string FileName => $"{ComputeHash(_codeContent, _settings)}.{FileExtension}";

        public abstract string FileExtension { get; }

        public abstract Stream Stream();


        /// <summary>
        /// Compute hash by code content and settings. Hash is use to identify the identical QR codes. Is use to build file name also.
        /// </summary>
        /// <param name="codeContent">Content of the code.</param>
        /// <param name="settings">Settings of the code.</param>
        /// <returns>MD5 hash</returns>
        protected string ComputeHash(string value, QRCodeSettings settings)
        {
            return HashHelper.ComputeHash(value, settings);
        }

        /// <summary>
        /// Resolve icon source
        /// </summary>
        /// <param name="icon">Image path, Media ID or Media UDI</param>
        /// <returns>Image path</returns>
        protected string ResolveIconUrl(string icon)
        {
            if (!string.IsNullOrEmpty(icon))
            {
                if (int.TryParse(icon, out int mediaId))
                {
                    return _umbracoHelper.Media(mediaId)?.Url();
                }
                if (Udi.TryParse(icon, out Udi mediaUdi))
                {
                    return _umbracoHelper.Media(mediaUdi)?.Url();
                }
                return icon;
            }
            return null;
        }
    }
}
