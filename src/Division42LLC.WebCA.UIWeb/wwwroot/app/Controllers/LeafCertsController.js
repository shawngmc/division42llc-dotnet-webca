(function() {

    var app = angular.module("app");

    var LeafCertsController = function ($scope, $rootScope, $location, RestService) {

        $scope.isLoading = false;

        var onGetSuccess = function (response) {
            $scope.isLoading = false;
            $scope.certs = response;
            //$rootScope.refreshCA();
        };

        var onError = function (response) {
            $scope.error = response;
            $scope.isLoading = false;

            $rootScope.refreshCA();
        };

        $scope.getLeafCerts = function () {
            $scope.isLoading = true;

            RestService.get("/api/leaf/getall")
                .then(onGetSuccess, onError);

        };

        $scope.getLeafCerts();
    };

    app.controller("LeafCertsController", LeafCertsController);

}());