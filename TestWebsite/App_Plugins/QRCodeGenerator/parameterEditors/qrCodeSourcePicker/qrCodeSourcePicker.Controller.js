angular.module("umbraco")
    .controller("RoboLynx.Umbraco.QRCodeSourcePickerController",
        ['$scope', 'RoboLynx.Umbraco.QRCodeGeneratorResources',
            function ($scope, resources) {
                $scope.availableSources = [];
                $scope.qrCodeParamsForm = { codeSource: null };
                $scope.selectedItem = null;
                $scope.loaded = false;

                function init() {
                    resources.getQRCodeSources().success(function (result) {
                        $scope.availableSources = result;
                        $scope.loaded = true;
                    });
                }

                $scope.showInfo = function (value) {
                    $scope.selectedItem = _.find($scope.availableSources, function (source) { return source.id == value });
                }

                init();
            }
        ]
    );