angular.module("umbraco")
    .controller("RoboLynx.Umbraco.QRCodeGeneratorController",
        ["$scope", "dialogService",
            function ($scope, dialogService) {
                $scope.openGenerator = function(contentId, propertyAlias) {
                    dialogService.open({
                        template: "/App_Plugins/QRCodeGenerator/dialogs/generateQRCodeDialog/generateQRCodeDialog.html",
                        show: true,
                        dialogData: { contentId: contentId, propertyAlias: propertyAlias }
                    });
                }
            }
        ]
	);