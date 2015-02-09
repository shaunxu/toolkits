(function(angular) {
    'use strict';
    angular
        .module('customTriggerExample', ['ngMessages', 'ui.bootstrap', 'ui.directives'])
        .controller('ExampleController', ['$scope', '$timeout', '$q', function($scope, $timeout, $q) {

            //$scope.getValidationIconNgClass = function (ctrl) {
            //    return {
            //        'has-error': ctrl.$dirty && ctrl.$invalid,
            //        'has-success': ctrl.$dirty && ctrl.$valid,
            //        'has-warning': ctrl.$dirty && ctrl.$pending
            //    };
            //};

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
        .directive('sbValidation', function ($compile) {
            return {
                restrict: 'A',
                compile: function () {
                    return function ($scope, $element, $attr) {
                        var name = $attr['name'];
                        var errorMessages = $attr['errorMessages'];
                        var form = $attr['formName'];
                        var messages = $scope[errorMessages][name];

                        $element.removeAttr('sb-validation');
                        $element.removeAttr('x-sb-validation');
                        $element.removeAttr('data-sb-validation');
                    };
                }
            };
        })
        .directive('sbValidationBorder', function ($compile) {
            return {
                restrict: 'A',
                compile: function () {
                    return function ($scope, $element, $attr) {
                        var formModel = $attr['formModel'];
                        var term =
                            '{' +
                                '\'has-error\':' + formModel + '.$dirty && ' + formModel + '.$invalid,' +
                                '\'has-success\':' + formModel + '.$dirty && ' + formModel + '.$valid,' +
                                '\'has-warning\':' + formModel + '.$dirty && ' + formModel + '.$pending' +
                            '}';
                        $element.attr('data-ng-class', term);
                        $element.removeAttr('sb-validation-border');
                        $element.removeAttr('x-sb-validation-border');
                        $element.removeAttr('data-sb-validation-border');
                        var e = $compile($element)($scope);
                        $element.replaceWith(e);
                    };
                }
            };
        })
        .directive('sbValidationIcon', function ($compile) {
            return {
                restrict: 'E',
                compile: function () {
                    return function ($scope, $element, $attr) {
                        var formModel = $attr['formModel'];
                        var html =
                            '<span class="glyphicon glyphicon-ok form-control-feedback" ng-show="' + formModel + '.$dirty && ' + formModel + '.$valid"></span>' +
                            '<span class="glyphicon glyphicon-remove form-control-feedback" ng-show="' + formModel + '.$dirty && ' + formModel + '.$invalid"></span>' +
                            '<span class="glyphicon glyphicon-refresh form-control-feedback" ng-show="' + formModel + '.$dirty && ' + formModel + '.$pending"></span>';
                        var e = $compile(html)($scope);
                        $element.replaceWith(e);
                    };
                }
            };
        })
        .run(function ($rootScope) {
            if (!String.prototype.format) {
                String.prototype.format = function() {
                    var args = arguments;
                    var result = this.replace(/{(\d+)}/g, function(match, number) {
                        return typeof args[number] != 'undefined' ? args[number] : match;
                    });
                    return result;
                };
            }

            $rootScope.getValidationBorderNgClass = function (ctrl) {
                return {
                    'has-error': ctrl.$dirty && ctrl.$invalid,
                    'has-success': ctrl.$dirty && ctrl.$valid,
                    'has-warning': ctrl.$dirty && ctrl.$pending
                };
            };
        });
})(window.angular);