using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.ContentEditing;
using Umbraco.Cms.Core.Models.Membership;
using Umbraco.Cms.Core.Services;
using Umbraco.Extensions;

namespace RoboLynx.Umbraco.QRCodeGenerator
{
    public class QRCodeGeneratorApp : IContentAppFactory
    {
        private readonly ILocalizedTextService _textService;

        public QRCodeGeneratorApp(ILocalizedTextService textService)
        {
            this._textService = textService;
        }

        public ContentApp? GetContentAppFor(object source, IEnumerable<IReadOnlyUserGroup> userGroups)
        {
            if (source is IContentBase entity)
            {
                if (entity.Properties.Where(p => p.PropertyType.PropertyEditorAlias == Constants.Backoffice.PropertyEditorAlias).Any())
                {
                    var qrCodeGeneratorApp = new ContentApp
                    {
                        Alias = Constants.Backoffice.ContentAppAlias,
                        Name = _textService.Localize("qrCode", "qrCode"),
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