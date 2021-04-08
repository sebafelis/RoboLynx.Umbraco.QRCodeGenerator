angular.module("umbraco").controller("Our.Umbraco.ColorPickr.PrevalueEditors.MultiColorsController", function ($scope, assetsService, localizationService, angularHelper) {

    const vm = this;

    vm.cancel = cancel;
    vm.save = save;

    vm.add = add;
    vm.remove = remove;

    function init() {
        
        $scope.hasError = false;
        $scope.focusOnNew = false;

        if (!$scope.model.value) {
            $scope.model.value = [];
        }

        if (angular.isArray($scope.model.value)) {
            _.map($scope.model.value, function(x) {
                x.rgbaCode = convertRGBtoObj(x.rgba);
                return x;
            });
        }

        $scope.color = $scope.color || null;
    }

    function cancel(color) {

    }

    function save(color) {
        $scope.color = color;
    }

    function remove(item, evt) {
        evt.preventDefault();

        $scope.model.value = _.reject($scope.model.value, function (x) {
            return x.hexa === item.hexa;
        });
        
    };

    function add(evt) {
        evt.preventDefault();

        console.log("$scope.color", $scope.color);
        
        if ($scope.color) {
            var exists = _.find($scope.model.value, function(item) {
                return item.hexa.toUpperCase() === $scope.color.hexa.toUpperCase();
            });

            if (!exists) {
                $scope.model.value.push({
                    hexa: $scope.color.hexa,
                    rgba: $scope.color.rgba,
                    rgbaCode: convertRGBtoObj($scope.color.rgba)
                });

                $scope.hasError = false;
                $scope.focusOnNew = true;
                return;
            }
        }

        //there was an error, do the highlight (will be set back by the directive)
        $scope.hasError = true;            
    };

    function convertRGBtoObj(colorString)
    {
      const rgbKeys = ['r', 'g', 'b', 'a'];
      let rgbObj = {};
      let color = colorString.replace(/^rgba?\(|\s+|\)$/g,'').split(',');
    
      for (let i in rgbKeys)
        rgbObj[rgbKeys[i]] = color[i] || 1;
    
      return rgbObj;
    }

    $scope.sortableOptions = {
        axis: 'y',
        containment: 'parent',
        cursor: 'move',
        items: '> div.control-group',
        tolerance: 'pointer',
        update: function (e, ui) {
            angularHelper.getCurrentForm($scope).$setDirty();
        }
    };

    init();

});