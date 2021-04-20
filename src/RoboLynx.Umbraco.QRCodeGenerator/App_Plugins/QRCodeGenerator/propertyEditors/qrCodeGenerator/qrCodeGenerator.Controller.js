angular.module("umbraco")
    .controller("RoboLynx.Umbraco.QRCodeGeneratorController",
        ["$scope", "dialogService", "editorState", "notificationsService",
            function ($scope, dialogService, editorState, notificationsService) {
                $scope.openGenerator = function (propertyAlias) {
                    // get the content item form
                    var contentForm = angular.element('form[name=contentForm]').children().scope().$parent.contentForm;

                    if (!editorState.current.hasPublishedVersion) {
                        if (!notificationsService.hasView()) {
                            notificationsService.add({
                                type: 'error',
                                view: '/App_Plugins/QRCodeGenerator/notifications/notPublishedAnyContentVersion/notPublishedAnyContentVersion.html',
                                sticky: true
                            });
                        }
                        return false;
                    }

                    // if we have a dirty property show a notification and cancel opening 
                    if (contentForm.$dirty) {
                        if (!notificationsService.hasView()) {
                            notificationsService.add({
                                type: 'warning',
                                view: '/App_Plugins/QRCodeGenerator/notifications/openDirtyContent/openDirtyContent.html',
                                sticky: true,
                                args: {
                                    listener: function () { openGenerateQRCodeDialog(propertyAlias); }
                                }
                            });
                        }
                        return false;
                    }

                    if (!editorState.current.published) {
                        if (!notificationsService.hasView()) {
                            notificationsService.add({
                                type: 'warning',
                                view: '/App_Plugins/QRCodeGenerator/notifications/notPublishedContent/notPublishedContent.html',
                                sticky: true,
                                args: {
                                    listener: function () { openGenerateQRCodeDialog(propertyAlias); }
                                }
                            });
                        }
                        return false;
                    }

                    openGenerateQRCodeDialog(propertyAlias);
                }

                function openGenerateQRCodeDialog(propertyAlias) {
                    dialogService.open({
                        template: '/App_Plugins/QRCodeGenerator/dialogs/generateQRCodeDialog/generateQRCodeDialog.html',
                        show: true,
                        dialogData: { contentId: editorState.current.id, propertyAlias: propertyAlias }
                    });
                }
            }
        ]
    );