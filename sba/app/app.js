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
                    'minlength': 'Name ({0}) must be more than 5 chars.',
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

            //$scope.uNameValidators = {
            //    'required': {
            //        'value': null,
            //        'async': false,
            //        'fn': null,
            //        'message': 'Name is missing.'
            //    },
            //    'minlength': {
            //        'value': 5,
            //        'async': false,
            //        'fn': null,
            //        'message': 'Name must be more than 5 chars.'
            //    },
            //    'mustContainsSpace': {
            //        'value': null,
            //        'async': false,
            //        'fn': function (ctrl, scope, q, modelValue, viewValue) {
            //            if (ctrl.$isEmpty(modelValue)) {
            //                // consider empty models to be valid
            //                return true;
            //            }
            //            if (modelValue.indexOf(' ') > 0) {
            //                return true;
            //            }
            //            else {
            //                return false;
            //            }
            //        },
            //        'message': 'Name must contain at lease one space.'
            //    },
            //    'notDuplicated': {
            //        'value': null,
            //        'async': true,
            //        'fn': function (ctrl, scope, q, modelValue, viewValue) {
            //            if (ctrl.$isEmpty(modelValue)) {
            //                // consider empty model valid
            //                return q.when();
            //            }
            //
            //            var def = q.defer();
            //
            //            $timeout(function() {
            //                // Mock a delayed response
            //                if (scope.names.indexOf(modelValue) === -1) {
            //                    // The username is available
            //                    def.resolve();
            //                } else {
            //                    def.reject();
            //                }
            //
            //            }, 2000);
            //
            //            return def.promise;
            //        },
            //        'message': 'Name must contain at lease one space.'
            //    }
            //};
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
                    //var ngClass = ''
                    //    + '{'
                    //    + '\'has-error\':' + formModel + '.$dirty && ' + formModel + '.$invalid,'
                    //    + '\'has-success\':' + formModel + '.$dirty && ' + formModel + '.$valid,'
                    //    + '\'has-warning\':' + formModel + '.$dirty && ' + formModel + '.$pending'
                    //    + '}';
                    //element.attr('data-ng-class', ngClass);
                    //element.attr('popover-trigger', 'mouseover');
                    //element.attr('popover-placement', 'right');
                    //element.removeAttr('sb-validation');
                    //element.removeAttr('x-sb-validation');
                    //element.removeAttr('data-sb-validation');

                    var errorMessages = {};
                    var errorMessageAttributeProfixing = 'sbErrorMessage';
                    Object.getOwnPropertyNames(attrs).forEach(function (attrName) {
                        if (attrName.substr(0, errorMessageAttributeProfixing.length) === errorMessageAttributeProfixing) {
                            var validatorName = attrName.substr(errorMessageAttributeProfixing.length);
                            validatorName = validatorName.charAt(0).toLowerCase() + validatorName.slice(1);
                            errorMessages[validatorName] = attrs[attrName];
                        }
                    });

                    console.log(errorMessages);

                    element.popover({
                        content: function () {
                            return scope.getErrorMessage(ctrl.$name, ctrl.$viewValue);
                        },
                        placement: 'right',
                        trigger: 'hover'
                    });

                    scope.$watch(
                        function (scope) {
                            var model = scope[form.$name][ctrl.$name];

                            if (model.$dirty) {
                                if (model.$valid) {
                                    return 0; // success
                                }
                                else if (model.$pending) {
                                    return 1; // pending
                                }
                                else if (model.$invalid) {
                                    return -1; // error
                                }
                                else {
                                    return 0; // success
                                }
                            }
                            else {
                                return 65535; // origin
                            }
                        },
                        function (value) {
                            attrs.$removeClass('has-error');
                            attrs.$removeClass('has-success');
                            attrs.$removeClass('has-warning');
                            switch (value) {
                                case -1: // error
                                    attrs.$addClass('has-error');
                                    break;
                                case 1: // pending
                                    attrs.$addClass('has-warning');
                                    break;
                                default : // success
                                    attrs.$addClass('has-success');
                                    break;
                            }
                        });

                    //$compile(element)(scope);

                    //console.log(element.contents());
                    //var e = $compile(element.contents())(scope);
                    //console.log(e[0].outerHTML);

                    //element.replaceWith($compile(element)(scope));
                    //element.replaceWith(element);

                    var spanHtml = ''
                        + '<span class="glyphicon glyphicon-ok form-control-feedback has-success-icon" ng-show="' + formModel + '.$dirty && ' + formModel + '.$valid"></span>'
                        + '<span class="glyphicon glyphicon-remove form-control-feedback has-error-icon" ng-show="' + formModel + '.$dirty && ' + formModel + '.$invalid"></span>'
                        + '<span class="glyphicon glyphicon-refresh form-control-feedback has-warning-icon" ng-show="' + formModel + '.$dirty && ' + formModel + '.$pending"></span>';
                    element.parent().append($compile(spanHtml)(scope));
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
        });
})(window.angular);