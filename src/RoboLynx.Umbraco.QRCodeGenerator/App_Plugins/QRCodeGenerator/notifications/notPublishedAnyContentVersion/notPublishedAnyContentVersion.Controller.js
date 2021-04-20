angular.module("umbraco").controller("RoboLynx.QRCodeGenerator.NotPublishedAnyContentVersionController",
	function ($scope, notificationsService) {

		$scope.close = function (not) {
			notificationsService.remove(not);
		};

	});