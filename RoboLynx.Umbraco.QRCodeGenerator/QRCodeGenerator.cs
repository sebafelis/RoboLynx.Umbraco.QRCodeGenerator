using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.PropertyEditors;

namespace RoboLynx.Umbraco.QRCodeGenerator
{
    [PropertyEditor(Constants.PluginAlias, Constants.PluginName, PropertyEditorValueTypes.Json, "~/App_Plugins/QRCodeGenerator/propertyEditors/qrCodeGenerator/qrCodeGenerator.html", Icon = "icon-qr-code", HideLabel = true, IsParameterEditor = false)]
    public class QRCodeGenerator : PropertyEditor
    {
        protected override PreValueEditor CreatePreValueEditor()
        {
            return base.CreatePreValueEditor();
        }

        /// <summary>
        /// A custom pre-value editor class to deal with the legacy way that the pre-value data is stored.
        /// </summary>
        internal class DecimalPreValueEditor : PreValueEditor
        {
            public DecimalPreValueEditor()
            {
                //create the fields
                Fields.Add(new PreValueField()
                {
                    Description = "Return the data to generate QR Code.",
                    Key = "codeSource",
                    View = "~/App_Plugins/QRCodeGenerator/parameterEditors/qrCodeSourcePicker/qrCodeSourcePicker.html",
                    Name = "Source provider",
                    Validators = new List<IPropertyValidator>() { new RequiredValidator }

                });
                Fields.Add(new PreValueField()
                {
                    Description = "Settings passed to source provider. See source provider documentation to get details.",
                    Key = "codeSourceSettings",
                    View = "textarea",
                    Name = "Source provider settings"
                });
                Fields.Add(new PreValueField(new DecimalValidator())
                {
                    Description = "Enter the maximum amount of number to be entered",
                    Key = "max",
                    View = "decimal",
                    Name = "Maximum"
                });

                {
                    "label": "Source provider",
            "description": "Return the data to generate QR Code.",
            "key": "codeSource",
            "view": "~/App_Plugins/QRCodeGenerator/parameterEditors/qrCodeSourcePicker/qrCodeSourcePicker.html",
            "validation": [
              {
                        "type": "Required"
              }
            ]
          },
          {
                    "label": "Source provider settings",
            "description": "Settings passed to source provider. See source provider documentation to get details.",
            "key": "codeSourceSettings",
            "view": "textarea"
          },
          {
                    "label": "QR Code type",
            "description": "Property name which value will be use to generate QR Code.",
            "key": "codeType",
            "view": "~/App_Plugins/QRCodeGenerator/parameterEditors/qrCodeTypePicker/qrCodeTypePicker.html",
            "validation": [
              {
                        "type": "Required"
              }
            ]
          },
          {
                    "label": "Default format",
            "description": "",
            "key": "defaultFormat",
            "view": "~/App_Plugins/QRCodeGenerator/parameterEditors/qrCodeFormatPicker/qrCodeFormatPicker.html"
          },
          {
                    "label": "Default pixel per module",
            "description": "",
            "key": "defaultSize",
            "view": "number"
          },
          {
                    "label": "Error correction level",
            "description": "",
            "key": "defaultECCLevel",
            "view": "~/App_Plugins/QRCodeGenerator/parameterEditors/qrCodeLevelPicker/qrCodeLevelPicker.html"
          },
          {
                    "label": "Default dark color",
            "description": "",
            "key": "defaultDarkColor",
            "view": "~/App_Plugins/SpectrumColorPicker/SpectrumColorPicker.html"
          },
          {
                    "label": "Default light color",
            "description": "",
            "key": "defaultLightColor",
            "view": "~/App_Plugins/SpectrumColorPicker/SpectrumColorPicker.html"
          },
          {
                    "label": "Default icon",
            "description": "",
            "key": "defaultIcon",
            "view": "mediapicker"
          },
          {
                    "label": "Default icon size (percent)",
            "description": "",
            "key": "defaultIconSizePercent",
            "view": "number"
            //"validation": [
            //  {
            //    "type": "Range",
            //    "config": {
            //      "min": 0,
            //      "max": 0
            //    }
            //  }
            //]
                },
          {
                    "label": "Default icon border width",
            "description": "",
            "key": "defaultIconBorderWidth",
            "view": "number"
          },
          {
                    "label": "Default draw quite zone",
            "description": "",
            "key": "defaultDrawQuiteZone",
            "view": "boolean"
          }
            }
        }
    }
}
