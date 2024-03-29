﻿using RoboLynx.Umbraco.QRCodeGenerator.QRCodeSources;
using Umbraco.Cms.Core.Composing;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes
{
    public interface IQRCodeTypeFactory : IDiscoverable
    {
        /// <summary>
        /// Identifier
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Name
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Description
        /// </summary>
        string Description { get; }

        IQRCodeType Create(IQRCodeSource qrCodeSource);
    }
}