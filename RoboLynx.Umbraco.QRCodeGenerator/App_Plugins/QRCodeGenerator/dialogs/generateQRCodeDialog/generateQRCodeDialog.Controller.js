(function () {
    'use strict';

    angular
        .module('umbraco')
        .controller('RoboLynx.Umbraco.GenerateQRCodeDialog', generateQRCode);

    generateQRCode.$inject = ['$scope', 'RoboLynx.Umbraco.QRCodeGeneratorResources', '$q', 'dialogService', 'notificationsService'];

    function generateQRCode($scope, qrCodeGeneratorResources, $q, dialogService, notificationsService) {

        var dialogData = $scope.dialogData;
        var defaultSettings = {};
        var requierdSettingsForFormats = {};

        $scope.qrCodeLoaded = false;
        $scope.qrCodeSettingsLoaded = false;
        $scope.requiredSettingsForFormatsLoaded = false;
        $scope.qrCodeError = null;
        $scope.qrCode = null;
        $scope.qrCodeWidth = null;
        $scope.qrCodeHeight = null;
        $scope.qrCodeFileName = null;

        $scope.settingsModel = [
            {
                label: "@qrCode_darkColor",
                show: true,
                view: "/App_Plugins/SpectrumColorPicker/SpectrumColorPicker.html",
                alias: "darkColor",                
                config: {
                    enableTransparency: "0",
                    preferredFormat: "hex"
                },
                value: null,
                order: 4
            },
            {
                label: "@qrCode_lightColor",
                show: true,
                view: "/App_Plugins/SpectrumColorPicker/SpectrumColorPicker.html",
                alias: "lightColor",
                config: {
                    enableTransparency: "0",
                    preferredFormat: "hex"
                },
                value: null,
                order: 5
            },
            {
                label: "@qrCode_icon",
                show: true,
                view: "mediapicker",
                alias: "icon",
                config: {
                    multiPicker: false,
                    onlyImages: true,
                    disableFolderSelect: true
                },
                value: null,
                order: 7
            },
            {
                label: "@qrCode_iconSizePercent",
                show: true,
                view: "integer",
                alias: "iconSizePercent",
                config: {
                    min: 1,
                    max: 100,
                    step: 1
                },
                value: null,
                order: 8
            },
            {
                label: "@qrCode_iconBorderWidth",
                show: true,
                view: "integer",
                alias: "iconBorderWidth",
                config: {
                    min: 1,
                    max: 100,
                    step: 1
                },
                value: null,
                order: 9
            },
            {
                label: "@qrCode_drawQuiteZone",
                show: true,
                view: "boolean",
                alias: "drawQuiteZone",
                value: null,
                order: 6
            },
            {
                label: "@qrCode_size",
                descrition: "Pixels per module",
                show: true,
                view: "integer",
                alias: "size",
                config: {
                    min: 1,
                    max: 1000,
                    step: 1
                },
                value: null,
                order: 3
            },
            {
                label: "@qrCode_format",
                show: true,
                view: "/App_Plugins/QRCodeGenerator/parameterEditors/qrCodeFormatPicker/qrCodeFormatPicker.html",
                alias: "format",
                change: formatChange,
                value: null,
                order: 1
            },
            {
                label: "@qrCode_eccLevel",
                show: true,
                view: "/App_Plugins/QRCodeGenerator/parameterEditors/qrCodeLevelPicker/qrCodeLevelPicker.html",
                alias: "eccLevel",
                value: null,
                order: 2
            }
        ];

        $scope.confirm = function () {
            download($scope.qrCode, $scope.qrCodeFileName);
        };

        function formatChange(formatModel) {
            _.each($scope.settingsModel, function (property) {
                property.show = formatModel.value && _.contains(requierdSettingsForFormats[formatModel.value], property.alias);
            });
        }

        function mapSettings(settingsModel) {
            var result = {};
            _.each(settingsModel, function (item) {
                result[item.alias] = item.value;
            });
            return result;
        }

        function setDefaultSettings() {
            for (var property in defaultSettings) {
                var propertyModel = _.find($scope.settingsModel, { alias: property });
                if (propertyModel) {
                    propertyModel.value = defaultSettings[propertyModel.alias];
                }
            }
        }

        function watchSettings() {
            $scope.$watch("settingsModel", function (newForm, oldForm) {
                getQRCode(dialogData.contentId, dialogData.propertyAlias, mapSettings($scope.settingsModel));
            }, true);
        }

        function getRequiredSettingsForFormats() {
            $scope.requiredSettingsForFormatsLoaded = false;
            return qrCodeGeneratorResources.getRequiredSettingsForFormats()
                .then(
                    function (data) {
                        requierdSettingsForFormats = data;
                        $scope.requiredSettingsForFormatsLoaded = true;
                        return data;
                    },
                    function (error) {
                        notificationsService.error("Error", "Could not get required settings for formats.", null, error);
                        $scope.qrCodeError = error;
                        return error;
                    });
        }

        function getQRCodeSettings(contentId, propertyAlias) {
            $scope.qrCodeSettingsLoaded = false;
            return qrCodeGeneratorResources.getQRCodeSettings(contentId, propertyAlias)
                .then(
                    function (data) {
                        defaultSettings = data;
                        $scope.qrCodeSettingsLoaded = true;
                        return data;
                    },
                    function (error) {
                        notificationsService.error("Error", "Could not get default settings.", null, error);
                        $scope.qrCodeError = error;
                        return error;
                    });
        }

        function getQRCode(contentId, propertyAlias, settings) {
            $scope.qrCodeLoaded = false;
            return qrCodeGeneratorResources.getQRCodeAsBase64(contentId, propertyAlias, settings)
                .then(
                    function (data) {
                        const img = new Image();
                        img.onload = function () {
                            $scope.qrCodeWidth = img.width;
                            $scope.qrCodeHeight = img.height;
                        };
                        img.src = data.data;

                        $scope.qrCode = data.data;
                        $scope.qrCodeFileName = data.fileName;
                        $scope.qrCodeLoaded = true;
                        $scope.qrCodeError = null;
                        return data;
                    },
                    function (error) {
                        notificationsService.error("Error", "Could not get QR Code URL.", null, error);
                        $scope.qrCodeError = error;
                        return error;
                    });
        }


        function init() {
            $q.all(getRequiredSettingsForFormats(), getQRCodeSettings(dialogData.contentId, dialogData.propertyAlias)
                .then(function () {
                    setDefaultSettings();
                }))
                .then(function () {
                    watchSettings()
                });
        }

        init();
    }

})();
