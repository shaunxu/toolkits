var app = angular.module('app', [ 'kendo.directives' ]);

app.controller('MainCtrl', function ($scope, $http) {
    $scope.validatorOptions = {
        rules: {
            'greater-date': function (input) {
                if (input.is('[id=end]')) {
                    return !$scope.end || !$scope.start || ($scope.end > $scope.start);
                }
                return true;
            },
            'job-name-duplicated': function (input) {
                if (input.is('[id=jobname')) {
                    var thisName = $scope.jobname;
                    if (thisName) {
                        $http.get('api/jobs.json')
                            .success(function (jobs) {
                                var duplicated = false;
                                jobs.forEach(function (job) {
                                    if (thisName === job) {
                                        duplicated = true;
                                    }
                                });
                                var result = !duplicated;
                                return result;
                            })
                            .error(function () {
                                return false;
                            });
                    }
                    else {
                        return true;
                    }
                }
                else {
                    return true;
                }
            }
        },
        messages: {
            'job-name-duplicated': function (input) {
                return 'Duplicated job name "' + input.val() + '" for {0}';
            }
        }
    };

    $scope.waveCountOptions = {
        format: '#',
        min: 1,
        max: 100,
        step: 1
    };

    $scope.validate = function(event) {
        $scope.data = [
            "12 Angry Men",
            "Il buono, il brutto, il cattivo.",
            "Inception",
            "One Flew Over the Cuckoo's Nest",
            "Pulp Fiction",
            "Schindler's List",
            "The Dark Knight",
            "The Godfather",
            "The Godfather: Part II",
            "The Shawshank Redemption"
        ];

        event.preventDefault();

        if ($scope.jobValidator.validate()) {
            $scope.validationMessage = "Awesome! Your job information is valid.";
        } else {
            $scope.validationMessage = "Oops! Something wrong here.";
        }
    }
});