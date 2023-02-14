namespace RoboLynx.Umbraco.QRCodeGenerator
{
#pragma warning disable IDE1006 // Add readonly modifier

    internal class Core
    {
        internal const string PluginAlias = "QRCodeGenerator";
        internal const string PluginName = "QR Code Generator";
        internal const string OptionsSectionName = "QRCodeGenerator";
    }

    public class Backoffice
    {
        public const string PropertyEditorAlias = "qrCodeGenerator";
        internal const string ContentAppAlias = "qrCodeGenerator";
        private const string BackofficeCacheNameValue = "Backoffice";
        public static string BackofficeCacheName { get; set; } = BackofficeCacheNameValue;
    }

    internal class DefaultFieldsNames
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

    internal class DefaultFieldsValues
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

    internal class FieldsNames
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

    public class SettingProperties
    {
        public const string Size = FieldsNames.SizeFieldName;
        public const string DarkColor = FieldsNames.DarkColorFieldName;
        public const string LightColor = FieldsNames.LightColorFieldName;
        public const string Icon = FieldsNames.IconFieldName;
        public const string IconSizePercent = FieldsNames.IconSizePercentFieldName;
        public const string IconBorderWidth = FieldsNames.IconBorderWidthFieldName;
        public const string DrawQuietZone = FieldsNames.DrawQuietZoneFieldName;
        public const string ECCLevel = FieldsNames.ECCLevelFieldName;
    }

#pragma warning restore IDE1006 // Add readonly modifier
}