(function () {
    'use strict';

    angular
        .module('umbraco')
        .controller('RoboLynx.Umbraco.GenerateQRCodeDialog', generateQRCode);

    generateQRCode.$inject = ['$scope', 'RoboLynx.Umbraco.QRCodeGeneratorResources', '$q', 'dialogService', 'notificationsService'];

    function generateQRCode($scope, qrCodeGeneratorResources, $q, dialogService, notificationsService) {

        var dialogData = $scope.dialogData,
            defaultSettings = {},
            requierdSettingsForFormats = {},
            settingsModelWatches = [];

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
                description: "@qrCode_darkColorDescription",
                show: true,
                view: "/App_Plugins/SpectrumColorPicker/SpectrumColorPicker.html",
                alias: "darkColor",
                config: {
                    enableTransparency: "0",
                    preferredFormat: "hex"
                },
                value: null,
                validation: {
                    mandatory: true
                },
                order: 4
            },
            {
                label: "@qrCode_lightColor",
                description: "@qrCode_lightColorDescription",
                show: true,
                view: "/App_Plugins/SpectrumColorPicker/SpectrumColorPicker.html",
                alias: "lightColor",
                config: {
                    enableTransparency: "0",
                    preferredFormat: "hex"
                },
                value: null,
                validation: {
                    mandatory: true
                },
                order: 5
            },
            {
                label: "@qrCode_icon",
                description: "@qrCode_iconDescription",
                show: true,
                view: "mediapicker",
                alias: "icon",
                config: {
                    multiPicker: false,
                    onlyImages: true,
                    disableFolderSelect: true
                },
                validation: {
                    mandatory: false
                },
                value: null,
                order: 7
            },
            {
                label: "@qrCode_iconSizePercent",
                description: "@qrCode_iconSizePercentDescription",
                show: true,
                view: "integer",
                alias: "iconSizePercent",
                config: {
                    min: 1,
                    max: 100,
                    step: 1
                },
                value: null,
                validation: {
                    mandatory: false
                },
                order: 8
            },
            {
                label: "@qrCode_iconBorderWidth",
                description: "@qrCode_iconBorderWidthDescription",
                show: true,
                view: "integer",
                alias: "iconBorderWidth",
                config: {
                    min: 1,
                    max: 100,
                    step: 1
                },
                value: null,
                validation: {
                    mandatory: false
                },
                order: 9
            },
            {
                label: "@qrCode_drawQuiteZone",
                description: "@qrCode_drawQuiteZoneDescription",
                show: true,
                view: "boolean",
                alias: "drawQuiteZone",
                value: null,
                validation: {
                    mandatory: true
                },
                order: 6
            },
            {
                label: "@qrCode_size",
                description: "@qrCode_sizeDescription",
                show: true,
                view: "integer",
                alias: "size",
                config: {
                    min: 1,
                    max: 1000,
                    step: 1
                },
                value: null,
                validation: {
                    mandatory: true
                },
                order: 3
            },
            {
                label: "@qrCode_format",
                description: "@qrCode_formatDescription",
                show: true,
                view: "/App_Plugins/QRCodeGenerator/parameterEditors/qrCodeFormatPicker/qrCodeFormatPicker.html",
                alias: "format",
                change: formatChange,
                value: null,
                validation: {
                    mandatory: true
                },
                order: 1
            },
            {
                label: "@qrCode_eccLevel",
                description: "@qrCode_eccLevelDescription",
                show: true,
                view: "/App_Plugins/QRCodeGenerator/parameterEditors/qrCodeLevelPicker/qrCodeLevelPicker.html",
                alias: "eccLevel",
                value: null,
                validation: {
                    mandatory: true
                },
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
            unwatchSettings();

            var watchExpressions = generateExpressionsToWatchSettingsValue();
            _.each(watchExpressions, function (expression) {
                settingsModelWatches.push($scope.$watch(expression, function (newForm, oldForm) {
                    getQRCode(dialogData.contentId, dialogData.propertyAlias, mapSettings($scope.settingsModel));
                }));
            });
        }

        function unwatchSettings() {
            _.each(settingsModelWatches, function (watched) {
                watched();
            });
        }

        function generateExpressionsToWatchSettingsValue() {
            return _.map($scope.settingsModel, function (item, index) {
                return "settingsModel[" + index + "].value";
            });
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
                        return $q.reject(error);
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
                        return $q.reject(error);
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
                        return $q.reject(error);
                    });
        }

        $scope.$on('$destroy', function () {
            unwatchSettings();
        });

        function init() {
            $q.all(
                [
                    getRequiredSettingsForFormats(),
                    getQRCodeSettings(dialogData.contentId, dialogData.propertyAlias)
                        .then(function () {
                            setDefaultSettings();
                        })
                ])
                .then(function () {
                    watchSettings()
                });
        }

        init();
    }

})();
