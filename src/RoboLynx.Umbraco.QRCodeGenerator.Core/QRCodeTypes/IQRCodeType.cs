using RoboLynx.Umbraco.QRCodeGenerator.QRCodeSources;
using Umbraco.Core.Composing;
using Umbraco.Core.Models.PublishedContent;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes
{
    public interface IQRCodeType : IDiscoverable
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


        IQRCodeTypeValueFactory CreateValueFactory(IFactory factory);
    }
}
