angular.module("umbraco")
    .controller("RoboLynx.Umbraco.QRCodeTypePickerController",
        ['$scope', 'RoboLynx.Umbraco.QRCodeGeneratorResources',
            function ($scope, resources) {
                $scope.availableTypes = [];
                $scope.selectedItem = null;
                $scope.loaded = false;
                $scope.error = null;

                function init() {
                    resources.getQRCodeTypes().then(function (result) {
                        $scope.availableTypes = result;
                        $scope.loaded = true;
                        $scope.error = null;
                    }, function (error) {
                        $scope.error = error;
                    });
                }

                $scope.showInfo = function (value) {
                    $scope.selectedItem = _.find($scope.availableTypes, function (type) { return type.id == value });
                }

                init();
            }
        ]
    );