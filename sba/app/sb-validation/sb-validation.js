angular.module('sb.validation', [])
    .provider('sbValidationOptions', function () {
        'use strict';

        this.options = {
            tooltip: {
                placement: 'right',
                css: 'popover'
            },
            styles: {
                error: 'has-error',
                success: 'has-has-success',
                pending: 'has-warning'
            },
            messages: {
                required: 'This field is required.',
                minlength: 'This field must contains at least {1} chars.',
                maxlength: 'This field must contains at most {1} chars.',
                min: 'This field must contains at least {1} chars.',
                max: 'This field must contains at least {1} chars.',
                pattern: 'This field only supports input in pattern as "{1}".',
                email: 'Input is not a valid email format'
            },
            messageSuffixing: 'sbErrorMessage'
        };

        this.$get = function () {
            var opts = this.options;
            return opts;
        };

        this.setTooltip = function (opt) {
            this.options.tooltip = opt;
        };

        this.setStyles = function (opt) {
            this.options.styles =opt;
        };

        this.registerMessage = function (name, content) {
            this.options.messages[name] = content;
        };

        this.setMessageSuffixing = function (val) {
            this.messageSuffixing = val;
        }
    })
    .directive('sbValidation', ['$compile', 'sbValidationOptions', function ($compile, sbValidationOptions) {
        'use strict';
        return {
            restrict: 'A',
            require: ['ngModel', '^form'],
            priority: 0,
            terminal: true,
            link: function (scope, element, attrs, ctrls) {
                var ngModelCtrl = ctrls[0];
                var ngFormCtrl = ctrls[1];
                var modelFullName = ngFormCtrl.$name + '.' + ngModelCtrl.$name;
                // copy global messages and inject value from attributes
                var messages = angular.copy(sbValidationOptions.messages);
                angular.forEach(messages, function (v, k) {
                    if(attrs[k]) {
                        messages[k] = {
                            content: v,
                            value: attrs[k]
                        };
                    }
                });
                // load error messages from attributes
                var errorMessageAttributeSuffixing = sbValidationOptions.messageSuffixing;
                angular.forEach(attrs, function (attr, attrName) {
                    if (attrName.substr(0, errorMessageAttributeSuffixing.length) === errorMessageAttributeSuffixing) {
                        var validatorName = attrName.substr(errorMessageAttributeSuffixing.length);
                        validatorName = validatorName.charAt(0).toLowerCase() + validatorName.slice(1);
                        var validatorValue = attrs[validatorName] || '';
                        var errorMessage = attr;
                        messages[validatorName] = {
                            content: errorMessage,
                            value: validatorValue
                        };
                    }
                });
                var getErrorMessage = function () {
                    if (ngModelCtrl.$dirty) {
                        var validatorName = null;
                        Object.getOwnPropertyNames(ngModelCtrl.$error).forEach(function (vn) {
                            if (!validatorName && !!ngModelCtrl.$error[vn]) {
                                validatorName = vn;
                            }
                        });
                        var errorMessage = messages[validatorName] || { content: validatorName, value: '' };
                        var stringFormat = function(format) {
                            var args = Array.prototype.slice.call(arguments, 1);
                            return format.replace(/{(\d+)}/g, function (match, number) {
                                return typeof args[number] != 'undefined' ? args[number] : match;
                            });
                        };
                        return stringFormat(errorMessage.content, ngModelCtrl.$name, errorMessage.value, ngModelCtrl.$viewValue);
                    }
                    else {
                        return null;
                    }
                };

                element.popover({
                    content: function () {
                        return getErrorMessage();
                    },
                    placement: sbValidationOptions.tooltip.placement,
                    trigger: 'hover'
                });

                scope.$watch(
                    function (scope) {
                        var model = scope[ngFormCtrl.$name][ngModelCtrl.$name];
                        if (model.$dirty) {
                            if (model.$valid) {
                                return sbValidationOptions.styles.success; // success
                            }
                            else if (model.$pending) {
                                return sbValidationOptions.styles.pending; // pending
                            }
                            else if (model.$invalid) {
                                return sbValidationOptions.styles.error; // error
                            }
                            else {
                                return sbValidationOptions.styles.success; // success
                            }
                        }
                        else {
                            return null; // origin
                        }
                    },
                    function (css) {
                        attrs.$removeClass(sbValidationOptions.styles.success);
                        attrs.$removeClass(sbValidationOptions.styles.pending);
                        attrs.$removeClass(sbValidationOptions.styles.error);
                        if (css) {
                            attrs.$addClass(css);
                        }
                    });

                var spanHtml = ''
                    + '<span class="glyphicon glyphicon-ok form-control-feedback has-success-icon" ng-show="' + modelFullName + '.$dirty && ' + modelFullName + '.$valid"></span>'
                    + '<span class="glyphicon glyphicon-remove form-control-feedback has-error-icon" ng-show="' + modelFullName + '.$dirty && ' + modelFullName + '.$invalid"></span>'
                    + '<span class="glyphicon glyphicon-refresh form-control-feedback has-warning-icon" ng-show="' + modelFullName + '.$dirty && ' + modelFullName + '.$pending"></span>';
                element.parent().append($compile(spanHtml)(scope));
            }
        };
    }]);