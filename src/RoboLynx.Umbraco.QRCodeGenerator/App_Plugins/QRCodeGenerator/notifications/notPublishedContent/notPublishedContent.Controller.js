angular.module("umbraco").controller("RoboLynx.QRCodeGenerator.NotPublishedContentController",
	function ($scope, notificationsService) {

		$scope.open = function (not) {
			not.args.listener();
			notificationsService.remove(not);
		};

		$scope.close = function (not) {
			notificationsService.remove(not);
		};

	});