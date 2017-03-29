(function() {

    var app = angular.module("app");

    var CAController = function($scope, $rootScope, $location, RestService) {

        $scope.isLoading = false;
        $scope.request = {};
        
        var onCreateSuccess = function(response) {
            $scope.isLoading = false;

            $rootScope.refreshCA();
        };

        var onWipeSuccess = function (response) {
            $scope.isLoading = false;

            $rootScope.refreshCA();
            $location.path("/");
        };

        var onError = function(response) {
            $scope.error = response;
            $scope.isLoading = false;

            $rootScope.refreshCA();
        };
        
        $scope.createCA = function() {
            $scope.isLoading = true;

            RestService.post("/api/ca/create", $scope.request)
                .then(onCreateSuccess, onError);

        };

        $scope.wipeCA = function () {
            $scope.isLoading = true;

            $(".modal").modal("hide");

            setTimeout(function () {
                RestService.get("/api/ca/wipe")
                    .then(onWipeSuccess, onError);
            }, 500);

        };
    };

    app.controller("CAController", CAController);

}());