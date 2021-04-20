//used for the media picker dialog
angular.module("umbraco").controller("RoboLynx.QRCodeGenerator.OpenDirtyContentController",
	function ($scope, $location, notificationsService) {

		$scope.open = function (not) {
			not.args.listener();
			notificationsService.remove(not);
		};

		$scope.close = function (not) {
			notificationsService.remove(not);
		};

	});