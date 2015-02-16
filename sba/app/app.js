(function(angular) {
    'use strict';
    var app = angular.module('customTriggerExample', ['sb.validation', 'kendo.directives']);
    app.controller('ExampleController', ['$scope', '$timeout', function($scope, $timeout) {
            $scope.user = {
                username: '',
                password: '',
                rememberUserName: false
            };

            $scope.submitting = false;

            $scope.login = function (event) {
                event.preventDefault();

                if ($scope.loginForm.$valid) {
                    $scope.submitting = true;
                    $timeout(function () {
                        $scope.submitting = false;
                        angular.forEach($scope.loginForm, function (v, k) {

                        });
                        $scope.loginForm.username.$setValidity('authFailed', false);
                    }, 2000);
                }
            };
        }]
    );

    app.directive('authFailed', function () {
        return {
            require: 'ngModel',
            link: function(scope, elm, attrs, ctrl) {
                ctrl.$validators.serverSideOnly = function () {
                    return true;
                }
            }
        };
    });

    app.directive('endMustGreaterThanStart', [function () {
            return {
                require: 'ngModel',
                link: function(scope, elm, attrs, ctrl) {
                    ctrl.$validators.endMustGreaterThanStart = function (value) {
                        if (ctrl.$isEmpty(value)) {
                            return true;
                        }
                        else if (ctrl.$isEmpty(scope.job.start)) {
                            return true;
                        }
                        else {
                            return value > scope.job.start;
                        }
                    };

                    scope.$watch('job.start', function () {
                        ctrl.$validate();
                    });
                }
            };
        }]
    );
    app.directive('notDuplicated', ['$q', '$timeout', function ($q, $timeout) {
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
        }]
    );
})(window.angular);