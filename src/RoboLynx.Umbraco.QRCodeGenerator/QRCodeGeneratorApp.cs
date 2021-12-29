using System.Collections.Generic;
using System.Linq;
using Umbraco.Core.Models;
using Umbraco.Core.Models.ContentEditing;
using Umbraco.Core.Models.Membership;
using Umbraco.Core.Services;

namespace RoboLynx.Umbraco.QRCodeGenerator
{
    public class QRCodeGeneratorApp : IContentAppFactory
    {
        private readonly ILocalizedTextService textService;

        public QRCodeGeneratorApp(ILocalizedTextService textService)
        {
            this.textService = textService;
        }

        public ContentApp GetContentAppFor(object source, IEnumerable<IReadOnlyUserGroup> userGroups)
        {
            if (source is IContentBase entity)
            {
                if (entity.Properties.Where(p => p.PropertyType.PropertyEditorAlias == Constants.PropertyEditorAlias).Any())
                {
                    var qrCodeGeneratorApp = new ContentApp
                    {
                        Alias = Constants.ContentAppAlias,
                        Name = textService.Localize("qrCode"),
                        Icon = "icon-qr-code",
                        View = "/App_Plugins/QRCodeGenerator/contentApps/qrCodeGeneratorApp/qrCodeGeneratorApp.html",
                        Weight = 0
                    };
                    return qrCodeGeneratorApp;
                }
            }
            return null;
        }
    }
}
