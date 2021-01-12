using RoboLynx.Umbraco.QRCodeGenerator.QRCodeFormat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboLynx.Umbraco.QRCodeGenerator
{
    internal class Constants
    {
        public static Type DefaultFormat => typeof(SvgFormat);
        public static IQRCodeFormat GetDefaultFormat()
        {
            return (IQRCodeFormat)Activator.CreateInstance(DefaultFormat);
        }

        public const string PluginName = "QRCodeGenerator";
        public const string CodeSourceFieldName = "codeSource";
        public const string CodeSourceSettingsFieldName = "codeSourceSettings";
        public const string CodeTypeFieldName = "codeType";
        public const string DefaultSizeFieldName = "defaultSize";
        public const string DefaultFormatFieldName = "defaultFormat";
        public const string DefaultDarkColorFieldName = "defaultDarkColor";
        public const string DefaultLightColorFieldName = "defaultLightColor";
        public const string DefaultIconFieldName = "defaultIcon";
        public const string DefaultIconSizePercentFieldName = "defaultIconSizePercent";
        public const string DefaultIconBorderWidthFieldName = "defaultIconBorderWidth";
        public const string DefaultDrawQuietZoneFieldName = "defaultDrawQuiteZone";

        public const string SizeFieldName = "size";
        public const string FormatFieldName = "format";
        public const string DarkColorFieldName = "darkColor";
        public const string LightColorFieldName = "lightColor";
        public const string IconFieldName = "icon";
        public const string IconSizePercentFieldName = "iconSizePercent";
        public const string IconBorderWidthFieldName = "iconBorderWidth";
        public const string DrawQuietZoneFieldName = "drawQuiteZone";
    }
}
