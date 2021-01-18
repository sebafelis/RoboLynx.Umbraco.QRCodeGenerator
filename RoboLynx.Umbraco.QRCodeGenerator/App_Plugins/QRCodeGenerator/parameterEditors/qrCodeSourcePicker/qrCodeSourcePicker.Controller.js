angular.module("umbraco")
    .controller("RoboLynx.Umbraco.QRCodeSourcePickerController",
        ['$scope', 'RoboLynx.Umbraco.QRCodeGeneratorResources',
            function ($scope, resources) {
                $scope.availableSources = [];
                $scope.selectedItem = null;
                $scope.loaded = false;
                $scope.error = null;

                function init() {
                    resources.getQRCodeSources().then(function (result) {
                        $scope.availableSources = result;
                        $scope.loaded = true;
                        $scope.error = null;
                    }, function (error) {
                        $scope.error = error;
                    });
                }

                $scope.showInfo = function (value) {
                    $scope.selectedItem = _.find($scope.availableSources, function (source) { return source.id == value });
                }

                init();
            }
        ]
    );