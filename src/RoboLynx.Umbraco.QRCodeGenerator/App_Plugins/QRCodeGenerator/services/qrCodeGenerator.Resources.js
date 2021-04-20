angular.module("umbraco")
    .factory("RoboLynx.Umbraco.QRCodeGeneratorResources", function ($http, $log, $q, $injector) {
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

        function _readAsBase64(response) {
            if (response.size == 0) {
                return $q.when(null);
            }

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

        function _readAsJson(response) {
            if (response.size == 0) {
                return $q.when({ message: '' });
            }

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
                            return $q.reject(reason.data.message);
                        }
                    );
            }

            return userPromises[id];
        }

        return {
            getQRCode: function (nodeId, propertyAlias, settings) {
                if (!nodeId) {
                    throw new Error("Content ID cannot be empty.");
                }
                if (!propertyAlias) {
                    throw new Error("Property Alias cannot be empty.");
                }
                return $http.get(qrCodeApiUrl + 'Image', {
                    responseType: "blob",
                    params: {
                        nodeId: nodeId, propertyAlias: propertyAlias, size: settings.size, format: settings.format,
                        darkColor: settings.darkColor, lightColor: settings.lightColor, icon: settings.icon,
                        iconSizePercent: settings.iconSizePercent, iconBorderWidth: settings.iconBorderWidth,
                        drawQuiteZone: _convertToBool(settings.drawQuiteZone), eccLevel: settings.eccLevel
                    }
                });
            },
            getQRCodeAsBase64: function (nodeId, propertyAlias, settings) {
                return this.getQRCode(nodeId, propertyAlias, settings)
                    .then(
                        function (response) {
                            var deferred = $q.defer();
                            _readAsBase64(response.data).then(function (base64Data) {
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
                            _readAsJson(error.data).then(function (errorObj) {
                                deferred.reject(errorObj.message);
                            }, function (readerError) {
                                deferred.reject(readerError);
                            });
                            return deferred.promise;
                        }
                    );
            },
            getQRCodeSettings: function (nodeId, propertyAlias) {
                return $http.get(qrCodeApiUrl + 'DefaultSettings?nodeId=' + nodeId + '&propertyAlias=' + propertyAlias).then(
                    function (response) {
                        return response.data;
                    }, function (error) {
                        return $q.reject(error.data.message);
                    });
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