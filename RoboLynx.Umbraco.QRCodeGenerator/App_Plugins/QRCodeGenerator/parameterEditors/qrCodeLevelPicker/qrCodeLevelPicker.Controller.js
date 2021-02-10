angular.module("umbraco")
    .controller("RoboLynx.Umbraco.QRCodeLevelPickerController",
        ['$scope', 'RoboLynx.Umbraco.QRCodeGeneratorResources',
            function ($scope, resources) {
                $scope.availableLevels = [];
                $scope.selectedItem = null;
                $scope.loaded = false;
                $scope.error = null;

                function init() {
                    resources.getQRCodeLevels().then(function (result) {
                        $scope.availableLevels = result;
                        $scope.loaded = true;
                        $scope.error = null;
                    }, function (error) {
                        $scope.error = error;
                    });
                }

                $scope.showInfo = function (value) {
                    $scope.selectedItem = _.find($scope.availableLevels, function (level) { return level.id == value });
                }

                init();
            }
        ]
    );