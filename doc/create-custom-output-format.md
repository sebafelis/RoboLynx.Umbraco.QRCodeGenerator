# Create a custom output file format

If you need generate QR code in other file format then default supporting, you can create an extension using two interfaces `IQRCodeFormat` and `IQRCodeFormatFactory`.

`IQRCodeFormatFactory` is intended to create an instance of `IQRCodeFormat`. It's also return informations like ID, display name and what settings properties are required to generate code.

Setting properties are limited to properties contained by `QRCodeSettings` class.

The default implementation of display name (`Name` property) is inherited from `QRCodeFormatFactory` class and needs to adding key words to some [Localization File](https://our.umbraco.com/Documentation/Extending/Language-Files/). It's should looks like:

```xml
<area alias="qrCodeFormats">
    <key alias="customFormatName">Administrar hostnames</key>        
</area>
```

> Key alias is created from class name inherit from `QRCodeFormatFactory`. **Factory** word is removed and **Name** word is added at the end.

> You can overwrite **Name** property and pass custom name directly.

`IQRCodeFormat` is intended to generate QR code in format what you want to support and return it as a stream.

It's return also informations like file name (FileName), file extension (FileExtension), MIME specific for returned file format (Mime), and unique code ID (HashId).

To implement `IQRCodeFormatFactory` and `IQRCodeFormat` you can inherit from ready abstract classes `QRCodeFormat` and `QRCodeFormatFactory` and only override required properties and methods.

QR Code Generator using Umbraco IoC container, so you can register in container any needed object by custom [Composer](https://our.umbraco.com/documentation/reference/using-ioc/) and inject this object in constructor to use them in your `IQRCodeFormat` implementation.

Create by you classes implementing `IQRCodeFormatFactory` and `IQRCodeFormat` don't need to be registered. All objects on during application startup implementing `IQRCodeFormatFactory` interface are automatically register in IoC container.

## Sample:

```c#
    public class CustomFormatFactory : QRCodeFormatFactory
    {
        private readonly ISomeDependency _someDependency;
        private readonly IUmbracoHelperAccessor _umbracoHelperAccessor;
        private readonly IQRCodeHashIdFactory _hashIdFactory;
        private readonly ILogger _logger;

        public CustomFormatFactory(ISomeDependency someDependency, IUmbracoHelperAccessor umbracoHelperAccessor,
            IQRCodeHashIdFactory hashIdFactory, ILogger<CustomFormat> logger, 
            ILocalizedTextService localizedTextService) : base(localizedTextService)
        {
            _someDependency = someDependency;
            _umbracoHelperAccessor = umbracoHelperAccessor;
            _hashIdFactory = hashIdFactory;
            _logger = logger;
        }


        public override string Id => "customId"; // Unique identifier for your format

        public override IEnumerable<string> RequiredSettings => new List<string> {
            Constants.SettingProperties.Size //,
            //Constants.SettingProperties.DarkColor,
            //Constants.SettingProperties.LightColor,
            //Constants.SettingProperties.DrawQuietZone,
            //Constants.SettingProperties.IconBorderWidth,
            //Constants.SettingProperties.Icon,
            //Constants.SettingProperties.IconSizePercent,
            //Constants.SettingProperties.ECCLevel
        }; // Only Size property will be display in settings in this case

        public override IQRCodeFormat Create(IQRCodeType codeType, QRCodeSettings settings)
        {
            return new CustomFormat(_someDependency, _umbracoHelperAccessor, _hashIdFactory, codeType, settings);
        }
    }
```

```C#
    public class CustomFormat : QRCodeFormat
    {
        private readonly ISomeDependency _someDependency; //Some dependency you need to create custom QR code.

        public CustomFormat(ISomeDependency someDependency, IUmbracoHelperAccessor umbracoHelperAccessor, IQRCodeHashIdFactory hashIdFactory, ILogger logger, 
            IQRCodeType codeType, QRCodeSettings settings) : base(umbracoHelperAccessor, hashIdFactory, logger, codeType, settings)
        {
            _someDependency = someDependency;
        }

        public override string Mime => "image/custom"; //Media type for the generating file

        public override string FileExtension => "custom"; //File extension without dot. 

        public override Stream Stream()
        {
            // Here you can create QR code in any format that you need. Code content is deliver by `CodeType` object. Settings passed by user are accessible in `Settings` object
            return _someDependency.GanerateCode(CodeType.GetCodeContent(), Settings.Size);
        }
    }
```

In main umbraco project you need to found language files for all cultures what you wont to support (This file is localized in _/config/lang/\{language\}.user.xml_) and add the following lines:

```xml
<area alias="qrCodeFormats">
    <key alias="customFormatName">Administrar hostnames</key>        
</area>
```