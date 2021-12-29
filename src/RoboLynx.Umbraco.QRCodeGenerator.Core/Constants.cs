namespace RoboLynx.Umbraco.QRCodeGenerator
{
    internal class Constants
    {
        public const string PluginAlias = "QRCodeGenerator";
        public const string PluginName = "QR Code Generator";
        public const string PropertyEditorAlias = "qrCodeGenerator";
        public const string ContentAppAlias = "qrCodeGenerator";

        public class DefaultFieldsNames
        {
            public const string DefaultSizeFieldName = "defaultSize";
            public const string DefaultFormatFieldName = "defaultFormat";
            public const string DefaultDarkColorFieldName = "defaultDarkColor";
            public const string DefaultLightColorFieldName = "defaultLightColor";
            public const string DefaultIconFieldName = "defaultIcon";
            public const string DefaultIconSizePercentFieldName = "defaultIconSizePercent";
            public const string DefaultIconBorderWidthFieldName = "defaultIconBorderWidth";
            public const string DefaultDrawQuietZoneFieldName = "defaultDrawQuiteZone";
            public const string DefaultECCLevelFieldName = "defaultECCLevel";
        }

        public class DefaultFieldsValues
        {
            public const int DefaultSizeFieldValue = 40;
            public const string DefaultFormatFieldValue = "svg";
            public const string DefaultDarkColorFieldValue = "#000000";
            public const string DefaultLightColorFieldValue = "#ffffff";
            public const string DefaultIconFieldValue = null;
            public const int DefaultIconSizePercentFieldValue = 10;
            public const int DefaultIconBorderWidthFieldValue = 6;
            public const bool DefaultDrawQuietZoneFieldValue = true;
            public const ECCLevel DefaultECCLevelFieldValue = ECCLevel.M;
        }

        public class FieldsNames
        {
            public const string CodeSourceFieldName = "codeSource";
            public const string CodeSourceSettingsFieldName = "codeSourceSettings";
            public const string CodeTypeFieldName = "codeType";
            public const string SizeFieldName = "size";
            public const string FormatFieldName = "format";
            public const string DarkColorFieldName = "darkColor";
            public const string LightColorFieldName = "lightColor";
            public const string IconFieldName = "icon";
            public const string IconSizePercentFieldName = "iconSizePercent";
            public const string IconBorderWidthFieldName = "iconBorderWidth";
            public const string DrawQuietZoneFieldName = "drawQuiteZone";
            public const string ECCLevelFieldName = "eccLevel";
        }

        private const string BackofficeCacheNameValue = "Backoffice";
        public static string BackofficeCacheName { get; set; } = BackofficeCacheNameValue;
    }
}