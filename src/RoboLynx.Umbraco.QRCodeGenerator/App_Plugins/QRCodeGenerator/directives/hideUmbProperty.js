(function () {
    'use strict';

    angular
        .module('umbraco')
        .directive('hideUmbProperty', hideUmbProperty);

    hideUmbProperty.$inject = ['$window'];

    function hideUmbProperty($window) {
        // Usage:
        //     <hideUmbProperty></hideUmbProperty>
        // Creates:
        //
        var directive = {
            link: link,
            restrict: 'E',
            scope: {
                name: '='
            },
        };
        return directive;

        function link(scope, element, attrs) {
            if (scope.name) {
                $(element).closest('umb-property[data-element="property-' + scope.name + ']').hide();
            }
            else {
                $(element).closest('umb-property').hide();
            }
        }
    }
})();