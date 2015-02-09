(function(angular) {
    'use strict';
    angular
        .module('customTriggerExample', ['ngMessages', 'ui.bootstrap', 'ui.directives'])
        .controller('ExampleController', ['$scope', '$timeout', '$q', function($scope, $timeout, $q) {

            $scope.getErrorMessage = function (name, value) {
                var control = $scope.form[name];
                if (control) {
                    if (control.$dirty && control.$invalid) {
                        var message = '';
                        Object.getOwnPropertyNames(control.$error).forEach(function (key) {
                            if ($scope.errorMessages[name][key]) {
                                message = $scope.errorMessages[name][key];
                            }
                        });
                        message = message.format(value);
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
                    'minlength': 'Name ({0}) must be more than 2 chars.',
                    'mustContainsSpace': 'Name ({0}) must contain at lease one space.',
                    'notDuplicated': 'Name ({0}) was duplicated.'
                }
            };

            $scope.names = [
                'Shaun Xu',
                'Wang Chong'
            ];

            $scope.submit = function (event) {
                event.preventDefault();

                if ($scope.form.$valid) {
                    alert('Everything is good. Submit!');
                }
                else {
                    alert('Something wrong. You need to fix it.');
                }
            };
        }])
        .directive('mustContainsSpace', function () {
            return {
                require: 'ngModel',
                link: function(scope, elm, attrs, ctrl) {
                    ctrl.$validators.mustContainsSpace = function (modelValue, viewValue) {
                        if (ctrl.$isEmpty(modelValue)) {
                            // consider empty models to be valid
                            return true;
                        }
                        if (modelValue.indexOf(' ') > 0) {
                            return true;
                        }
                        else {
                            return false;
                        }
                    };
                }
            };
        })
        .directive('notDuplicated', function ($q, $timeout) {
            return {
                require: 'ngModel',
                link: function(scope, elm, attrs, ctrl) {
                    ctrl.$asyncValidators.notDuplicated = function (modelValue, viewValue) {
                        if (ctrl.$isEmpty(modelValue)) {
                            // consider empty model valid
                            return $q.when();
                        }

                        var def = $q.defer();

                        $timeout(function() {
                            // Mock a delayed response
                            if (scope.names.indexOf(modelValue) === -1) {
                                // The username is available
                                def.resolve();
                            } else {
                                def.reject();
                            }

                        }, 2000);

                        return def.promise;
                    };
                }
            };
        })
        .run(function () {
            if (!String.prototype.format) {
                String.prototype.format = function() {
                    var args = arguments;
                    var result = this.replace(/{(\d+)}/g, function(match, number) {
                        return typeof args[number] != 'undefined' ? args[number] : match;
                    });
                    return result;
                };
            }
        });
})(window.angular);