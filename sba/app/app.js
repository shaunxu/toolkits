(function(angular) {
    'use strict';
    angular
        .module('customTriggerExample', [])
        .controller('ExampleController', ['$scope', '$timeout', '$q', function($scope) {

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
                            console.log(modelValue);
                            console.log('pass');
                            console.log(ctrl);
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
                            return $q.when(undefined);
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
                require: ['ngModel', '^form'],
                priority: 0,
                terminal: true,
                link: function (scope, element, attrs, ctrls) {
                    var ctrl = ctrls[0];
                    var form = ctrls[1];
                    var formModel = form.$name + '.' + ctrl.$name;

                    var errorMessages = {};
                    var errorMessageAttributeProfixing = 'sbErrorMessage';
                    Object.getOwnPropertyNames(attrs).forEach(function (attrName) {
                        if (attrName.substr(0, errorMessageAttributeProfixing.length) === errorMessageAttributeProfixing) {
                            var validatorName = attrName.substr(errorMessageAttributeProfixing.length);
                            validatorName = validatorName.charAt(0).toLowerCase() + validatorName.slice(1);
                            errorMessages[validatorName] = attrs[attrName];
                        }
                    });
                    var getErrorMessage = function () {
                        if (ctrl.$dirty) {
                            var validatorName = null;
                            Object.getOwnPropertyNames(ctrl.$error).forEach(function (vn) {
                                if (!validatorName && !!ctrl.$error[vn]) {
                                    validatorName = vn;
                                }
                            });
                            var errorMessage = errorMessages[validatorName] || validatorName;
                            var stringFormat = function(format) {
                                var args = arguments;
                                var result = format.replace(/{(\d+)}/g, function (match, number) {
                                    return typeof args[number] != 'undefined' ? args[number] : match;
                                });
                                return result;
                            };
                            return stringFormat(errorMessage, ctrl.$name, ctrl.$viewValue);
                        }
                        else {
                            return null;
                        }
                    };

                    element.popover({
                        content: function () {
                            return getErrorMessage();
                        },
                        placement: 'right',
                        trigger: 'hover'
                    });

                    scope.$watch(
                        function (scope) {
                            var model = scope[form.$name][ctrl.$name];
                            if (model.$dirty) {
                                if (model.$valid) {
                                    return 'has-success'; // success
                                }
                                else if (model.$pending) {
                                    return 'has-warning'; // pending
                                }
                                else if (model.$invalid) {
                                    return 'has-error'; // error
                                }
                                else {
                                    return 'has-success'; // success
                                }
                            }
                            else {
                                return null; // origin
                            }
                        },
                        function (css) {
                            attrs.$removeClass('has-error');
                            attrs.$removeClass('has-success');
                            attrs.$removeClass('has-warning');
                            if (css) {
                                attrs.$addClass(css);
                            }
                        });

                    var spanHtml = ''
                        + '<span class="glyphicon glyphicon-ok form-control-feedback has-success-icon" ng-show="' + formModel + '.$dirty && ' + formModel + '.$valid"></span>'
                        + '<span class="glyphicon glyphicon-remove form-control-feedback has-error-icon" ng-show="' + formModel + '.$dirty && ' + formModel + '.$invalid"></span>'
                        + '<span class="glyphicon glyphicon-refresh form-control-feedback has-warning-icon" ng-show="' + formModel + '.$dirty && ' + formModel + '.$pending"></span>';
                    element.parent().append($compile(spanHtml)(scope));
                }
            };
        });
})(window.angular);