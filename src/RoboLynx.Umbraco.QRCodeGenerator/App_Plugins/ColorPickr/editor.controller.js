angular.module("umbraco").controller("Our.Umbraco.ColorPickr.Controller", function ($scope, angularHelper) {

    const vm = this;
    let colorPickerRef = null;

    vm.setup = setup;
    vm.cancel = cancel;
    vm.save = save;

    vm.validateMandatory = validateMandatory;

    /**
     * Method required by the valPropertyValidator directive
     */
    function validateMandatory() {
        return {
            isValid: !$scope.model.validation.mandatory || ($scope.color != null && $scope.color.length > 0),
            errorMsg: "Value cannot be empty",
            errorKey: "required"
        };
    }

    /** configure some defaults on init */
    function configureDefaults() {
        $scope.model.config.inlineMode = $scope.model.config.inlineMode ? Object.toBoolean($scope.model.config.inlineMode) : false;
        $scope.model.config.theme = $scope.model.config.theme || 'classic';

        if (!$scope.model.config.swatches) {
            $scope.model.config.swatches = [];
        }
        else {
            $scope.model.config.swatches = _.pluck($scope.model.config.swatches, "hexa");
        }

        // Setup default
        $scope.options = {
            inline: $scope.model.config.inlineMode,
            theme: $scope.model.config.theme,
            showAlways: $scope.model.config.inlineMode,
            swatches: $scope.model.config.swatches
        };

        if ($scope.model.value) {
            $scope.color = $scope.model.value;
        }
        else {
            $scope.color = null;
        }
    }

    function setup(instance) {
        colorPickerRef = instance;
    }

    function cancel(color) {
        
    }

    function save(color) {
        $scope.color = color ? color.hexa : null;
        $scope.model.value = color ? color.hexa : null;
    }

    function init() {
        configureDefaults();
    }

    init();

});