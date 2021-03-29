# QR Code Generator

Property editor allowing to generate QR codes straight from Umbraco Backoffice. 
User can customizing generate code by color, size, output format, error correction level, adding quite zone and also by adding icon (not for all formats).
The developer can set the source provider on the basis of which the code will be generated and type of passed data. 
Now data source it can be document property (now simple only), document URL or you can create a custom QR code data source.

> **Attention!**
> 
> It's can be us only for Document content. It can't be used for Media, Member or User at now.
> 

## Configuration

1. Create a new data type under the Settings tab and select QR Code
1. Select data source
1. Set **Source provider settings** field ([see here](#source-providers))

1. Select **QR code type**
1. Set default settings (all settings with prefix Default can be customize by backoffice user in when generating code but this value will be set up when user select this code type).
1. Save data type
1. Edit or create document type where you wont you **QR Code Generator**
1. Add new property and select created before data type. Remember! Document type has to have all property names defined in **Source provider settings**. 
1. Save document type.
1. Now you can open or create document of type where you add QR Code Generator property.
1. If document is published you will see active QR Code icon between content icon and info icon. Click them.
1. HearUmbraco backoffice users can create QR Codes and download them.

## Source providers

Source provider gets data from specify source and pass it to attributes containing by QR Code type. Now we have available only two build-in source providers. But you can write your on data source.

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
            name: 'propertyName1',
            regex: '\d*'
        },
        atributeName2: 'propertyName2'
}
```
When you use this syntax property value is pass to **QR Code type** as attributes by attribute name. Check attribute names [hear](#qr-code-types). You can find them also in information bellow selected **QR Code type** filed in backoffice.

### 2. Absolute URL

Always pass absolute document URL for each attribute of **QR Code type**. That's way this source provider should be use only with [**URL**](#qr-code-types) **QR Code type**.

### 3. Custom

You can write your own QR Code type writing a class extending `QRCodeSource` class.

## QR Code Types

The setting defines what type of data will be passed on to the end user and how it will be displayed to him. 

Available build-in types:

* **Text** - After user will scan the code it will saw specify text in code scanner. *Argument: `text`*
* **Phone number** - After user will scan the code it will saw specify phone number ready to call. *Argument: `number`*
* **URL** - After user will scan the code it will open specify resource in default application for specify protocol. i.e. specify website in default browser if protocol will bee _HTTP_ or _HTTPS_. *Argument: `url`*
* **SMS** - After user will scan the code it will saw new SMS message with specific content ready to send on specific number. *Arguments: `number`, `subject`*
* **Geo-location (Google Map)** - After user will scan the code it will open Google Maps application or website with pinned specify position. *Arguments: `latitude`, `longitude`*
* **Geo-location (GEO)** - After user will scan the code it will open default map application with pinned specify position. *Arguments: `latitude`, `longitude`*
* **Custom** - You can also write your own QR Code type writing a class extending `QRCodeType` class.

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

* When user scan the code then will see position what you pin in document on map. **Geo-location (GEO)** and **Geo-location (Google Map)** **QR Code type** needs two arguments, the latitude and the longitude attributes. To achieve the aim you can use the property editor like [**Our.Umbraco.OsmMaps**](https://our.umbraco.com/packages/backoffice-extensions/openstreetmap-property-editor/) and create property with alias _location_. This editor save latitude and longitude in one string like `53.35055131839989,18.74267578125,7`. To get correct data we need select **Content Property** set correct configuration witch extract correct value by regular expression. In this case, I should look like this:
`location{{^\d*(\.\d*)?}}, location{{(?<=,)\d*(\.\d*)?(?=,)}}`. 
