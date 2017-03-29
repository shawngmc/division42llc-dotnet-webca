(function () {

    var app = angular.module("app");

    var LeafCertsController = function ($scope, $rootScope, $location, RestService) {

        $scope.isLoading = false;
        $scope.currentThumbprint = "";

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

        $scope.setCurrentThumbprint = function (thumbprint) {
            $scope.currentThumbprint = thumbprint;
        }

        $scope.deleteCertificate = function () {

            $(".modal").modal("hide");
            setTimeout(function () {
                RestService.get("/api/leaf/delete/" + $scope.currentThumbprint)
                    .then(function () { $scope.getLeafCerts() }, onError);
            }, 500);
        };

        $scope.getLeafCerts = function () {
            $scope.isLoading = true;

            RestService.get("/api/leaf/")
                .then(onGetSuccess, onError);

        };

        $scope.getLeafCerts();
    };

    app.controller("LeafCertsController", LeafCertsController);

}());