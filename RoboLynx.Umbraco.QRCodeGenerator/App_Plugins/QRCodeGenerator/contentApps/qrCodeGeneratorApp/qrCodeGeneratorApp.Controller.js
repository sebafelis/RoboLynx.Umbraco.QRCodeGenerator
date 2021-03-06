(function () {
    'use strict';

    angular.module("umbraco")
        .controller("RoboLynx.Umbraco.QRCodeContentAppController", qrCodeContentApp);

    qrCodeContentApp.$inject = ['$scope', 'editorState', 'eventsService', 'contentResource', 'RoboLynx.Umbraco.QRCodeGeneratorResources', '$q', 'notificationsService'];

    function qrCodeContentApp($scope, editorState, eventsService, contentResource, qrCodeGeneratorResources, $q, notificationsService) {

        var vm = this;

        var defaultSettings = {},
            requierdSettingsForFormats = {},
            unwatchSettingsModel,
            unwatchQRCodePropertyAlias,
            unwatchAppActive,
            unwatchContentState,
            onContentSaved,
            initSubscription;

        vm.appActive = false;
        vm.currentNodeId = editorState.current.id;
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

        function getActiveVariant(variants) {
            return _.findWhere(variants, { active: true });
        }

        function getCurrentCulture(variants) {
            var culture = "invariant";
            var activeVariant = getActiveVariant(variants);
            if (activeVariant && activeVariant.language) {
                culture = activeVariant.language.culture;
            }
            return culture;
        }

        function setDefaultSettings() {
            var tempSettings = angular.copy(vm.settingsModel);
            for (var property in defaultSettings) {
                var propertyModel = _.findWhere(tempSettings, { alias: property });
                if (propertyModel) {
                    propertyModel.value = defaultSettings[propertyModel.alias];
                }
            }
            vm.settingsModel = tempSettings;
        }

        function unwatchSettings() {
            if (unwatchSettingsModel) {
                unwatchSettingsModel();
                unwatchSettingsModel = null;
            }

            if (onContentSaved) {
                onContentSaved();
                onContentSaved = null;
            }
        }

        function watchSettings() {
            var callGetQRCode = function () {
                if (vm.selectedQRCodePropertyAlias) {
                    var culture = getCurrentCulture($scope.content.variants);
                    getQRCode(vm.currentNodeId, vm.selectedQRCodePropertyAlias, culture, mapSettings(vm.settingsModel));
                }
            }

            if (!unwatchSettingsModel) {
                unwatchSettingsModel = $scope.$watchGroup(generateExpressionsToWatchSettingsValue(), function () {
                    callGetQRCode();
                }, true);
            }

            if (!onContentSaved) {
                onContentSaved = eventsService.on("content.saved", function () {
                    callGetQRCode();
                });
            }
        }

        function generateExpressionsToWatchSettingsValue() {
            return _.map(vm.settingsModel, function (item, index) {
                return "vm.settingsModel[" + index + "].value";
            });
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
                        notificationsService.error("Error", "Could not get required settings for formats. " + error);
                        vm.qrCodeError = error;
                        return $q.reject(error);
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
                        notificationsService.error("Error", "Could not get default settings. " + error);
                        vm.qrCodeError = error;
                        return $q.reject(error);
                    });
        }

        function getQRCode(contentId, propertyAlias, culture, settings) {
            vm.qrCodeLoaded = false;
            return qrCodeGeneratorResources.getQRCodeAsBase64(contentId, propertyAlias, culture, settings)
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
                        notificationsService.error("Error", "Could not get QR Code. " + error);
                        vm.qrCodeError = error;
                        return $q.reject(error);
                    });
        }


        function findProperties() {
            var culture = getCurrentCulture($scope.content.variants);
            return contentResource.getById(vm.currentNodeId, culture).then(function (node) {
                var prop = _.flatten(_.pluck(_.flatten(node.variants[0].tabs, true), 'properties'));
                vm.qrCodeProperties = _.filter(prop,
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

        function oninit() {

            $scope.model.disabled = true;

            unwatchQRCodePropertyAlias = $scope.$watch("vm.selectedQRCodePropertyAlias", function (newValue, oldValue) {
                if (newValue && oldValue != newValue) {
                    unwatchSettings();

                    $q.all([
                        getRequiredSettingsForFormats(),
                        getQRCodeSettings(vm.currentNodeId, vm.selectedQRCodePropertyAlias)
                    ]).then(function () {
                        setDefaultSettings();
                        watchSettings();
                    });
                }
            });

            unwatchContentState = $scope.$watch("variantContent.state", function (newValue, oldValue) {
                $scope.model.disabled = newValue != "Published";
            }, true);

            unwatchAppActive = $scope.$watch("model.active", function (newValue, oldValue) {
                if (newValue == true) {
                    unwatchSettings();
                    findProperties();
                    unwatchAppActive();
                }
            });
        }

        $scope.$on('$destroy', function () {
            initSubscription.unsubscribe();

            if (unwatchQRCodePropertyAlias) {
                unwatchQRCodePropertyAlias();
            }

            unwatchSettings();

            if (unwatchContentState) {
                unwatchContentState();
            }

            if (unwatchAppActive) {
                unwatchAppActive();
            }
        });

        oninit();
    }
})();