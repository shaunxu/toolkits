(function(angular) {
    'use strict';
    angular.module('customTriggerExample', ['ngMessages', 'ui.bootstrap'])
        .controller('ExampleController', ['$scope', function($scope) {
            $scope.user = {};
            $scope.getErrorMessage = function (name) {
                var control = $scope.form[name];
                if (control) {
                    if (control.$dirty && control.$invalid) {
                        var message = '';
                        Object.getOwnPropertyNames(control.$error).forEach(function (key) {
                            if ($scope.errorMessages[name][key]) {
                                message = $scope.errorMessages[name][key];
                            }
                        });
                        return message;
                    }
                    else {
                        return undefined;
                    }
                }
                else {
                    return undefined;
                }
            };
            $scope.errorMessages = {
                'uName': {
                    'required': 'Name is missing.',
                    'minlength': 'Name must be more than 2 chars.'
                }
            };
        }]);
})(window.angular);