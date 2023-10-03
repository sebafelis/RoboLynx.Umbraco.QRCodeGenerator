
# Change Log
All notable changes to this project will be documented in this file.

## [Unreleased]

### Added

- Project containing references to default packages 

### Changed

- Project/library *RoboLynx.Umbraco.QRCodeGenerator* change name to *RoboLynx.Umbraco.QRCodeGenerator.Backoffice*
- Packages descriptions

## [11.0.0] - 2023-09-26

### Added

- Separate project to ImageSharp support

### Changed 

- Migration to .NET 7
- Support for Umbraco 11

## [10.0.0] - 2023-09-14

### Added

- Support for Umbraco Marketplace 
- Secure URLs support
- Custom classes for **QRCoder** library (`ImageSharpQRCode`, `ImageSharpSvgQRCode`) to support raster graphic generation through ImageSharp library
- Add AddLocalQRCodeCache() extension method for UmbracoBuilder
- Add support for CDN
- Add generic methods to QRCodeService

### Changed

- Migration to .NET 6
- Support for Umbraco 10
- Enable nullable reference types in projects
- Constants moved to dedicate classes
- Code from RoboLynx.Umbraco.QRCodeGenerator.Cache moved to RoboLynx.Umbraco.QRCodeGenerator.Core project 
- Change AddQRCodeCache() extension method for UmbracoBuilder

### Removed
- Removed RoboLynx.Umbraco.QRCodeGenerator.Cache project
- Removed RoboLynx.Umbraco.QRCodeGenerator.Cache.Local project
- Removed RoboLynx.Umbraco.QRCodeGenerator.Frontend.Cache.Local project
 
## [9.0.0] - 2022-04-11

### Added

- AddQRCodeCache extension method for IUmbracoBuilder

### Changed

- Migration to .NET 5
- Create IQRCodeResponseFactory to not duplicate code in controllers
- Constants moved to `RoboLynx.Umbraco.QRCodeGenerator.Constants` name space

### Removed

- ColorPickr property editor (now is using build-in EyeDropper)
- IQRCodeCacheFactory and IQRCodeCacheFileSystemFactory
- CreateResponse method from IQRCodeBuilder

## [8.1.0] - 2021-12-30

### Updating
  
I you doesn't extended this package you don't have to change anything to update to this version. All configuration and stored data are unchanged.

If you extend property editor with custom QR code Format, Source or Type you need to change your code because extensions architecture was change. 

### Added

- Service
- Hash ID Factory to generate unique names
- `IQRCodeFormatFactory` interface and `QRCodeFormatFactory` base class
- `IQRCodeSourceFactory` interface and `QRCodeSourceFactory` base class
- `IQRCodeTypeFactory` interface and `QRCodeTypeFactory` base class
- Controller to access QR code from frontend page
- Property converter for **QR Code** property editor
- Extension methods for `UrlHelper`
- Extension methods for `IQRCodeService`
- Cache for QR Code Generator
- Separate cache for umbraco backoffice
- Separate cache for frontend pages


### Changed
  
- `IQRCodeFormat` interface end depend classes
- `IQRCodeSource` interface end depend classes
- `IQRCodeType` interface end depend classes
- `IQRCodeBuilder` interface end depend classes
 
## [8.0.0] - 2021-04-29
 
### Added

- Property editor for Umbraco Backoffice
- Support output formats: BMP, JPEG, PNG, SVG
- Support code content types: SMS, Phone Number, Text, URL, Geolocation (GEO), Geolocation (Google Maps)
- Support code content source: Absolute URL, Simple Property



