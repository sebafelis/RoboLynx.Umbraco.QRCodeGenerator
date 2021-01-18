﻿angular.module("umbraco")
    .factory("RoboLynx.Umbraco.QRCodeGeneratorResources", function ($http, $log, $q) {
        var qrCodeApiUrl = "/Umbraco/backoffice/QRCodeGenerator/QRCode/",
            qrCodeTypePickerUrl = "/Umbraco/backoffice/QRCodeGenerator/QRCodeTypePicker/",
            qrCodeSourcePickerUrl = "/Umbraco/backoffice/QRCodeGenerator/QRCodeSourcePicker/",
            qrCodeFormatPickerUrl = "/Umbraco/backoffice/QRCodeGenerator/QRCodeFormatPicker/",
            qrCodeLevelPickerUrl = "/Umbraco/backoffice/QRCodeGenerator/QRCodeLevelPicker/";

        var userPromises = {};

        function _convertToBool(value) {
            if (typeof value == "boolean") {
                return value;
            }
            switch (value) {
                case "1":
                case "yes":
                case "true":
                    return true;
                case "0":
                case "no":
                case "false":
                    return false;
            }
            return undefined;
        }

        function _raadAsBase64(response) {
            var deferred = $q.defer();

            var reader = new FileReader();
            reader.onloadend = function () {
                deferred.resolve(reader.result);
            }
            reader.onerror = function () {
                deferred.reject(reader.error);
            }
            reader.readAsDataURL(response);

            return deferred.promise;
        }

        function _raadAsJson(response) {
            var deferred = $q.defer();

            var reader = new FileReader();
            reader.onloadend = function () {
                deferred.resolve(JSON.parse(reader.result));
            }
            reader.onerror = function () {
                deferred.reject(reader.error);
            }
            reader.readAsText(response);

            return deferred.promise;
        }

        function _getFileName(response) {
            var contentDisposition = response.headers("content-disposition");
            if (contentDisposition) {
                var filenameRegexp = /filename=['"]?(.*)['"]?/;
                var match = filenameRegexp.exec(contentDisposition);
                return match[1];
            }
        }

        function _callOneTimeHttpPromise(id, promise) {
            if (!userPromises[id]) {
                userPromises[id] = promise
                    .then(
                        function (response) {
                            return response.data;
                        },
                        function (reason) {
                            userPromises[id] = null;
                            throw new Error(reason.message);
                            return reason;
                        }
                    );
            }

            return userPromises[id];
        }

        return {
            getQRCode: function (contentId, propertyAlias, settings) {
                if (!contentId) {
                    throw new Error("Content ID cannot be empty.");
                }
                if (!propertyAlias) {
                    throw new Error("Property Alias cannot be empty.");
                }
                return $http.get(qrCodeApiUrl + 'Image', {
                    responseType: "blob",
                    params: {
                        contentId: contentId, propertyAlias: propertyAlias, size: settings.size, format: settings.format,
                        darkColor: settings.darkColor, lightColor: settings.lightColor, icon: settings.icon,
                        iconSizePercent: settings.iconSizePercent, iconBorderWidth: settings.iconBorderWidth,
                        drawQuiteZone: _convertToBool(settings.drawQuiteZone), eCCLevel: settings.eCCLevel
                    }
                }).then(function (response) { return response; });
            },
            getQRCodeAsBase64: function (contentId, propertyAlias, settings) {
                return this.getQRCode(contentId, propertyAlias, settings)
                    .then(
                        function (response) {
                            var deferred = $q.defer();
                            _raadAsBase64(response.data).then(function (base64Data) {
                                deferred.resolve({
                                    data: base64Data,
                                    fileName: _getFileName(response)
                                });
                            }, function (base64Error) {
                                deferred.reject(base64Error);
                            });
                            return deferred.promise;
                        },
                        function (error) {
                            var deferred = $q.defer();
                            _raadAsJson(error.data).then(function (errorObj) {
                                deferred.resolve(errorObj.message);
                            }, function (readerError) {
                                deferred.reject(readerError);
                            });
                            return deferred.promise;
                        }
                    );
            },
            getQRCodeSettings: function (contentId, propertyAlias) {
                return $http.get(qrCodeApiUrl + 'DefaultSettings?contentId=' + contentId + '&propertyAlias=' + propertyAlias).then(function (response) { return response.data; });
            },
            getRequiredSettingsForFormats: function () {
                return _callOneTimeHttpPromise(arguments.callee.name, $http.get(qrCodeApiUrl + 'RequiredSettingsForFormats'));
            },
            getQRCodeTypes: function () {
                return _callOneTimeHttpPromise(arguments.callee.name, $http.get(qrCodeTypePickerUrl + 'Get'));
            },
            getQRCodeSources: function () {
                return _callOneTimeHttpPromise(arguments.callee.name, $http.get(qrCodeSourcePickerUrl + 'Get'));
            },
            getQRCodeFormats: function () {
                return _callOneTimeHttpPromise(arguments.callee.name, $http.get(qrCodeFormatPickerUrl + 'Get'));
            },
            getQRCodeLevels: function () {
                return _callOneTimeHttpPromise(arguments.callee.name, $http.get(qrCodeLevelPickerUrl + 'Get'));
            }
        };
    });