![Logo](https://github.com/sebafelis/RoboLynx.Umbraco.CodeGenerator/raw/main-u7/assets/RoboLynx.Umbraco.QRCodeGenerator_128.png)
# QR Code Generator

[![RoboLynx.Umbraco.QRCodeGenerator NuGet Package](https://img.shields.io/nuget/v/RoboLynx.Umbraco.QRCodeGenerator)](https://www.nuget.org/packages/RoboLynx.Umbraco.QRCodeGenerator/)
[![RoboLynx.Umbraco.QRCodeGenerator.Core NuGet Package](https://img.shields.io/nuget/v/RoboLynx.Umbraco.QRCodeGenerator.Core)](https://www.nuget.org/packages/RoboLynx.Umbraco.QRCodeGenerator.Core/)
[![Our Umbraco project page](https://img.shields.io/badge/our-umbraco-orange.svg)](https://our.umbraco.com/packages/backoffice-extensions/qr-code-generator/) 
[![Build Status](https://dev.azure.com/robolynx/RoboLynx.Umbraco.QRCodeGenerator/apis/build/status/sebafelis.RoboLynx.Umbraco.QRCodeGenerator?branchName=main-u7)](https://dev.azure.com/robolynx/RoboLynx.Umbraco.QRCodeGenerator/build/latest?definitionId=7&branchName=main-u7)
![Licence](https://img.shields.io/github/license/sebafelis/RoboLynx.Umbraco.QRCodeGenerator)

Property editor for Umbraco 7 allowing to generate QR codes straight from Umbraco Backoffice. 
User can customize generating code by color, size, output format, error correction level, adding quiet zone and also by adding icon (not for all formats). Code is available to generated under specify document type, base on specify data source like current document property or URL. Source provider, code type and document type where code is available to generate is specify by developer. At this moment data source it can be document property (or part of selected by regular expression), document URL or custom ([see here](#source-providers)).

> **Attention!**
> 
> It's can be us only for Document content. It can't be used for Media, Member or User at now.

>Version for Umbraco 8 is [here](https://github.com/sebafelis/RoboLynx.Umbraco.QRCodeGenerator/tree/main-u8)

## Installation

Use NuGet to install RoboLynx.Umbraco.QRCodeGenerator:

```Install-Package RoboLynx.Umbraco.QRCodeGenerator -Version 7.0.0```

or

Download the Umbraco Package from: https://our.umbraco.com/packages/backoffice-extensions/qr-code-generator/

>* Versions starts with 7.* are intended for Umbraco 7.
>* Versions starts with 8.* are intended for Umbraco 8.
 
## Configuration

1. Create a new Data Type in Developer section and select QR Code
1. Select data source
1. Set **Source provider settings** field ([see here](#source-providers))
1. Select **Code type**
1. Set default settings (all settings with prefix Default can be customize by backoffice user in when generating code but this value will be set up when user select this code type).
1. Save data type
1. Edit or create document type where you wont you **QR Code Generator**
1. Add new property and select created before data type. Remember! Document type has to have all property names defined in **Source provider settings**. 
1. Save document type. 

   ![Configuration](https://github.com/sebafelis/RoboLynx.Umbraco.QRCodeGenerator/raw/main-u7/assets/screenshots/screen3.png)

1. Now you can open or create document of type where you add QR Code Generator property.
1. When you open the document you will see a green button **Generate QR Code** next to property added at the moment before. Click on it.
1. In dialog window on the right side users will see generated QR code, here settings can be also change and code download.
   
    ![QR Code tab](https://github.com/sebafelis/RoboLynx.Umbraco.QRCodeGenerator/raw/main-u7/assets/screenshots/screen4.png)

## Source providers

Source provider gets data from specify source and pass it to attributes containing by **Code type**. Now we have available only two build-in source providers. But you can write your own.

### 1. Content Property

It's get data from content property by property name. Configuration is required. 
Configuration can by write on two way:
1. Property names separated by commas. i.e. `propertyName1, propertyName2, propertyName3`.
When you use this syntax you must remember then property names order is important. Values will be pass to QR Code type as attributes by index.
This syntax may also contain regular expression (just after property name between double curly brackets `{{\d*}}`) witch can extract only part of property value. Regular expression is optional. i.e.
`location{{^\d*(\.\d*)?}}, location{{(?<=,)\d*(\.\d*)?(?=,)}}`
1. JSON syntax. i.e. 
```json
{ 
    properties: {
        atributeName1: {
            name: "propertyName1",
            regex: "\d*"
        },
        atributeName2: "propertyName2"
}
```
When you use this syntax property value is pass to **Code type** as attributes by attribute name. Check attribute names [hear](#code-types). You can find them also in information bellow selected **Code type** filed in backoffice.

### 2. Absolute URL

Always pass absolute document URL for each attribute of **Code type**. That's way this source provider should be use only with [**URL**](#code-types) **Code type**.

### 3. Custom

You can write your own **Code type** writing a class extending `QRCodeSource` class. Class will be fond automatically. e.g. Write source provider getting data from external web service.

## Code Types

The setting defines what type of data will be passed on to the end user and how it will be displayed to him. 

Available build-in types:

* **Text** - After user will scan the code it will saw specify text in code scanner. *Argument: `text`*
* **Phone number** - After user will scan the code it will saw specify phone number ready to call. *Argument: `number`*
* **URL** - After user will scan the code it will open specify resource in default application for specify protocol. i.e. specify website in default browser if protocol will bee _HTTP_ or _HTTPS_. *Argument: `url`*
* **SMS** - After user will scan the code it will saw new SMS message with specific content ready to send on specific number. *Arguments: `number`, `subject`*
* **Geo-location (Google Map)** - After user will scan the code it will open Google Maps application or website with pinned specify position. *Arguments: `latitude`, `longitude`*
* **Geo-location (GEO)** - After user will scan the code it will open default map application with pinned specify position. *Arguments: `latitude`, `longitude`*
* **Custom** - You can also write your own **Code type** writing a class extending `QRCodeType` class.

## Formats

Output format of generated file.

Supported formats:
* **svg**
* **jpg** (with icon support)
* **png** (with icon support)
* **bmp** (with icon support)
* **Custom** - You can make your own output format writing a new class extending `QRCodeFormat` class. 

> **Attention!**
> Custom format must be readable by \<img \/> element

## Example

* When user scan the code then will see position what you pin in document on map. **Geo-location (GEO)** and **Geo-location (Google Map)** **Code type** needs two arguments, the latitude and the longitude attributes. To achieve the aim you can use the property editor like [**Our.Umbraco.OsmMaps**](https://our.umbraco.com/packages/backoffice-extensions/openstreetmap-property-editor/) and create property with alias _location_. This editor save latitude and longitude in one string like `53.35055131839989,18.74267578125,7`. To get correct data we need select **Content Property** set correct configuration witch extract correct value by regular expression. In this case, I should look like this:
`location{{^\d*(\.\d*)?}}, location{{(?<=,)\d*(\.\d*)?(?=,)}}`. 
