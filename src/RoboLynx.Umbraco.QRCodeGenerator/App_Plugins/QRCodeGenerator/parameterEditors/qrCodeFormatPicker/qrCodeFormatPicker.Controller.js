angular.module("umbraco")
    .controller("RoboLynx.Umbraco.QRCodeFormatPicker",
        ['$scope', 'RoboLynx.Umbraco.QRCodeGeneratorResources',
            function ($scope, resources) {
                $scope.availableFormats = [];
                $scope.selectedItem = null;
                $scope.loaded = false;
                $scope.error = null;

                function init() {
                    resources.getQRCodeFormats()
                        .then(function (result) {
                            $scope.availableFormats = result;
                            $scope.loaded = true;
                            $scope.error = null;
                        }, function (error) {
                            $scope.error = error;
                        });
                }

                $scope.showInfo = function (value) {
                    $scope.selectedItem = _.find($scope.availableFormats, function (format) { return format.id == value });
                }

                $scope.change = function (value) {
                    if ($scope.model.change) {
                        $scope.model.change(value);
                    }
                    $scope.showInfo(value);
                }

                init();
            }
        ]
    );