angular.module("umbraco")
    .controller("RoboLynx.Umbraco.QRCodeGeneratorController",
        ["$scope", "dialogService", "editorState",
            function ($scope, dialogService, editorState) {
                $scope.openGenerator = function(propertyAlias) {
                    dialogService.open({
                        template: "/App_Plugins/QRCodeGenerator/dialogs/generateQRCodeDialog/generateQRCodeDialog.html",
                        show: true,
                        dialogData: { contentId: editorState.current.id, propertyAlias: propertyAlias }
                    });
                }
            }
        ]
	);