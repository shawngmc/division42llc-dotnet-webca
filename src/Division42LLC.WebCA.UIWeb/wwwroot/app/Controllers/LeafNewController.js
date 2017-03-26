(function() {

    var app = angular.module("app");

    var LeafNewController = function ($scope, $rootScope, $location, RestService) {

        $scope.isLoading = false;
        $scope.request = {};

        var onCreateSuccess = function (response) {
            $scope.isLoading = false;

            $rootScope.refreshCA();
            $location.path = "#/leafcerts";
        };

        var onError = function (response) {
            $scope.error = response;
            $scope.isLoading = false;

            $rootScope.refreshCA();
        };

        $scope.createLeaf = function () {
            $scope.isLoading = true;

            RestService.post("/api/leaf/create", $scope.request)
                .then(onCreateSuccess, onError);

        };

    };

    app.controller("LeafNewController", LeafNewController);

}());