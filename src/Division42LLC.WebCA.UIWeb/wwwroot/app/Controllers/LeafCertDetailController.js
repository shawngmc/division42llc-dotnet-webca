(function () {

    var app = angular.module("app");

    var LeafCertDetailController = function ($scope, $rootScope, $routeParams, $location, RestService) {

        $scope.thumbprint = $routeParams.id;

        $scope.isLoading = false;

        var onGetSuccess = function (response) {
            $scope.isLoading = false;
            $scope.cert = response;
        };

        var onError = function (response) {
            $scope.error = response;
            $scope.isLoading = false;
        };

        $scope.getLeafCert = function () {
            $scope.isLoading = true;

            RestService.get("/api/leaf/get/" + $scope.thumbprint)
                .then(onGetSuccess, onError);
        };

        $scope.deleteCertificate = function () {
            $scope.isLoading = true;

            $(".modal").modal("hide");

            setTimeout(function () {
                RestService.get("/api/leaf/delete/" + $scope.thumbprint)
                    .then(function () {
                        $location.path("/leafcerts");
                    }, onError);
            },750);
        };

        $scope.getLeafCert();
    };

    app.controller("LeafCertDetailController", LeafCertDetailController);

}());