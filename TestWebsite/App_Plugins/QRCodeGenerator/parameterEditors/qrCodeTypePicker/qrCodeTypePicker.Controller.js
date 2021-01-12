angular.module("umbraco")
    .controller("RoboLynx.Umbraco.QRCodeTypePickerController",
        ['$scope', 'RoboLynx.Umbraco.QRCodeGeneratorResources',
            function ($scope, resources) {
                $scope.availableTypes = [];
                $scope.qrCodeParamsForm = { codeType: null };

                function init() {
                    resources.getQRCodeTypes().success(function (result) {
                        $scope.availableTypes = result;
                    });
                }

                init();
            }
        ]
    );