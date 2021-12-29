# Create a custom output file format

If you need generate QR code in other file format then default supporting, you can create an extension using two interfaces _IQRCodeFormat_ and _IQRCodeFormatFactory_.

_IQRCodeFormatFactory_ is intended to create and instance of _IQRCodeFormat_. It's also return informations like ID, display name and required settings properties to generate code.

Setting properties are limited to properties contained by QRCodeSettings class.

_IQRCodeFormat_ is intended to generate QR code in format what you want to support and return it as a stream.

It's return also informations like file name (FileName), file extension (FileExtension), MIME specific for returned file format (Mime), and unique code ID (HashId).

To implement _IQRCodeFormatFactory_ and _IQRCodeFormat_ you can inherit from ready abstract classes _QRCodeFormat_ and _QRCodeFormatFactory_ and only override required properties and methods.

QR Code Generator using IoC container defined by Umbraco, so you can inject in constructor any objects what you need to and register them in custom [Composer](https://our.umbraco.com/documentation/reference/using-ioc/).

Create by you classes implementing _IQRCodeFormatFactory_ and _IQRCodeFormat_ don't need to be registered. All objects on during application startup implementing _IQRCodeFormatFactory_ interface are automatically register in IoC containser.