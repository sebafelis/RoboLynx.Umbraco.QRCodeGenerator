using System;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeTypes.Validators
{
    public class UrlValidator : IQRCodeTypeValidator
    {
        bool IQRCodeTypeValidator.Validate(object? url, out string? message)
        {
            if (url is string @string)
            {
                var isValidUrl = Uri.TryCreate(@string, UriKind.Absolute, out _);

                message = !isValidUrl ? "Passed value is not a correct absolute URL." : null;

                return isValidUrl;
            }

            if (url is Uri uri)
            {
                message = !uri.IsAbsoluteUri ? "Passed URL is not absolute." : null;

                return uri.IsAbsoluteUri;
            }

            message = "Unsupported input.";

            return false;
        }
    }
}