(function() {

    var app = angular.module("app");

    var CAController = function($scope, $rootScope, RestService) {

        $scope.isLoading = false;
        $scope.request = {};
        
        var onCreateSuccess = function(response) {
            $scope.isLoading = false;

            $rootScope.refreshCA();
        };

        var onWipeSuccess = function (response) {
            $scope.isLoading = false;

            $rootScope.refreshCA();
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

            $(".modal").hide();
            RestService.get("/api/ca/wipe")
                .then(onWipeSuccess, onError);

        };
    };

    app.controller("CAController", CAController);

}());