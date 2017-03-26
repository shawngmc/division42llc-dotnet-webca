(function() {

    var app = angular.module("app");

    var CAController = function($scope, $rootScope, RestService) {

        $scope.isLoading = false;
        $scope.request = {};

        var onGetSuccess = function(response) {
            $rootScope.ca = response;

            $rootScope.ca.certificate = response.certificate
                .replace("BEGIN|CERT", "BEGIN CERT")
                .replace("END|CERT", "END CERT")
                .replace(new RegExp("|", 'g'), "\n\n\n");
            alert($rootScope.ca.certificate);
            $scope.isLoading = false;
            $rootScope.caCertValid = response.isValid;
        };

        var onPostSuccess = function(response) {
            $rootScope.ca = response;
            $scope.isLoading = false;
            //$rootScope.caCertValid = response.isValid;

            $scope.getCurrent();
        };

        var onError = function(response) {
            $scope.error = response;
            $scope.isLoading = false;
            $rootScope.caCertValid = false;
        };

        $scope.getCurrent = function() {
            $scope.isLoading = true;

            RestService.get("/api/ca")
                .then(onGetSuccess, onError);
        };

        $scope.createCA = function() {
            $scope.isLoading = true;

            RestService.post("/api/ca/init", $scope.request)
                .then(onPostSuccess, onError);

        };
    };

    app.controller("CAController", CAController);

}());