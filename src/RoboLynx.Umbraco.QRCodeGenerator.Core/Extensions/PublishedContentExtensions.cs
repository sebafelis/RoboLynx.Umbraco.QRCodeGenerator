using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace RoboLynx.Umbraco.QRCodeGenerator
{
    public static class PublishedContentExtensions
    {
        public static Udi GetUdi(this IPublishedContent publishedContent)
        {
            var itemType = publishedContent.ItemType;
            string entityType;

            switch (itemType)
            {
                case PublishedItemType.Content:
                    entityType = UmbracoObjectTypes.Document.GetUdiType();
                    break;

                case PublishedItemType.Media:
                    entityType = UmbracoObjectTypes.Media.GetUdiType();
                    break;

                case PublishedItemType.Member:
                    entityType = UmbracoObjectTypes.Member.GetUdiType();
                    break;

                default:
                    entityType = UmbracoObjectTypes.Unknown.GetUdiType();
                    break;
            }

            var udi = Udi.Create(entityType, publishedContent.Key);

            return udi;
        }
    }
}