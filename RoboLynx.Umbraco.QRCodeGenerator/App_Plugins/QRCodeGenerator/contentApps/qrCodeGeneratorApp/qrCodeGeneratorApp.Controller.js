(function () {
    'use strict';

    angular.module("umbraco")
        .controller("RoboLynx.Umbraco.QRCodeContentAppController", qrCodeContentApp);

    qrCodeContentApp.$inject = ['$scope', 'editorState', 'contentResource', 'RoboLynx.Umbraco.QRCodeGeneratorResources', '$q', 'notificationsService'];

    function qrCodeContentApp($scope, editorState, contentResource, qrCodeGeneratorResources, $q, notificationsService) {

        var vm = this;

        var defaultSettings = {};
        var requierdSettingsForFormats = {};
        var watchSettingsModel = null;

        vm.appActive = false;
        vm.currentNodeId = editorState.current.id;
        vm.currentNodeAlias = editorState.current.contentTypeAlias;
        vm.qrCodeProperties = null;
        vm.selectedQRCodePropertyAlias = null;
        vm.qrCodeLoaded = false;
        vm.qrCodeSettingsLoaded = false;
        vm.requiredSettingsForFormatsLoaded = false;
        vm.qrCodeError = null;
        vm.qrCode = null;
        vm.qrCodeWidth = null;
        vm.qrCodeHeight = null;
        vm.qrCodeFileName = null;

        vm.settingsModel = [
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

        vm.download = function () {
            download(vm.qrCode, vm.qrCodeFileName);
        };

        vm.qrCodePropertyChange = function () {
            unwatchSettings();

            getQRCodeSettings(vm.currentNodeId, vm.selectedQRCodePropertyAlias)
                .then(function () {
                    setDefaultSettings();
                    watchSettings();
                })
        };

        function formatChange(formatModel) {
            _.each(vm.settingsModel, function (property) {
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
                var propertyModel = _.find(vm.settingsModel, { alias: property });
                if (propertyModel) {
                    propertyModel.value = defaultSettings[propertyModel.alias];
                }
            }
        }

        function unwatchSettings() {
            if (watchSettingsModel) {
                watchSettingsModel = null;
            }
        }

        function watchSettings() {
            if (!watchSettingsModel) {
                $scope.$watch("vm.settingsModel", function (newForm, oldForm) {
                    if (vm.selectedQRCodePropertyAlias) {
                        getQRCode(vm.currentNodeId, vm.selectedQRCodePropertyAlias, mapSettings(vm.settingsModel));
                    }
                }, true);
            }
        }

        function getRequiredSettingsForFormats() {
            vm.requiredSettingsForFormatsLoaded = false;
            return qrCodeGeneratorResources.getRequiredSettingsForFormats()
                .then(
                    function (data) {
                        requierdSettingsForFormats = data;
                        vm.requiredSettingsForFormatsLoaded = true;
                        return data;
                    },
                    function (error) {
                        notificationsService.error("Error", "Could not get required settings for formats.", null, error);
                        vm.qrCodeError = error;
                        return error;
                    });
        }

        function getQRCodeSettings(contentId, propertyAlias) {
            vm.qrCodeSettingsLoaded = false;
            return qrCodeGeneratorResources.getQRCodeSettings(contentId, propertyAlias)
                .then(
                    function (data) {
                        defaultSettings = data;
                        vm.qrCodeSettingsLoaded = true;
                        return data;
                    },
                    function (error) {
                        notificationsService.error("Error", "Could not get default settings.", null, error);
                        vm.qrCodeError = error;
                        return error;
                    });
        }

        function getQRCode(contentId, propertyAlias, settings) {
            vm.qrCodeLoaded = false;
            return qrCodeGeneratorResources.getQRCodeAsBase64(contentId, propertyAlias, settings)
                .then(
                    function (data) {
                        const img = new Image();
                        img.onload = function () {
                            vm.qrCodeWidth = img.width;
                            vm.qrCodeHeight = img.height;
                        };
                        img.src = data.data;

                        vm.qrCode = data.data;
                        vm.qrCodeFileName = data.fileName;
                        vm.qrCodeLoaded = true;
                        vm.qrCodeError = null;
                        return data;
                    },
                    function (error) {
                        notificationsService.error("Error", "Could not get QR Code URL.", null, error);
                        vm.qrCodeError = error;
                        return error;
                    });
        }


        function findProperties() {
            return contentResource.getById(vm.currentNodeId).then(function (node) {
                vm.qrCodeProperties = _.filter(node.variants[0].tabs[0].properties,
                    function (prop) {
                        return prop.view.endsWith('qrCodeGenerator.html');
                    });

                if (!vm.qrCodeProperties || vm.qrCodeProperties.length == 0) {
                    return $q.reject();
                }

                vm.selectedQRCodePropertyAlias = vm.qrCodeProperties[0].alias;

                return vm.selectedQRCodePropertyAlias;
            });
        }

        function waitForActive() {
            var deferred = $q.defer();

            $scope.$on("editors.apps.appChanged", function ($event, $args) {
                vm.appActive = $args.app.alias == "qrCodeGenerator";

                if (vm.appActive)
                    deferred.resolve('Hello, ' + name + '!');
            });

            return deferred.promise;
        }

        function init() {
            $scope.model.disabled = true;
            $q.all([
                waitForActive(),
                $q.all([
                    findProperties()
                        .then(function (selectedQRCodePropertyAlias) {
                            getQRCodeSettings(vm.currentNodeId, selectedQRCodePropertyAlias)
                                .then(function () {
                                    setDefaultSettings();
                                });
                        }),
                    getRequiredSettingsForFormats()
                ]).then(function () {
                    $scope.model.disabled = false;
                })
            ]).then(function () {
                watchSettings();
            });
        }

        init();
    }
})();