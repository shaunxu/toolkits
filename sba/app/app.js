(function(angular) {
    'use strict';
    angular
        .module('customTriggerExample', ['sb.validation', 'kendo.directives'])
        .controller('ExampleController', ['$scope', '$timeout', '$q', function($scope, $timeout) {
            $scope.names = [
                'Job 1',
                'Job 2',
                'Job 3'
            ];

            $scope.waveCountInputOptions = {
                format: '#'
            };

            $scope.job = {};

            $scope.submit = function (event) {
                event.preventDefault();

                //var result = $scope.form.$validate();
                //console.log(result);

                //$scope.form.$commitViewValue();

                if ($scope.form.$valid) {
                    $timeout(function() {
                        $scope.form.jobName.$setValidity('xxx', false);
                    }, 2000);
                }
                else {
                    alert('Something wrong. You need to fix it.');
                }
            };
        }])
        .directive('endMustGreaterThanStart', function () {
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
        });
})(window.angular);