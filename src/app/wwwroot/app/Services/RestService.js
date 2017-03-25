(function() {

    var RestService = function($http) {

        var getApplicationDetails = function() {
            return $http.get("/api/meta")
                .then(function(response) {
                    return response.data;
                });
        };

        var get = function(url) {
            return $http.get(url)
                .then(function(response) {
                    return response.data;
                });
        };

        var post = function(url, data) {
            return $http.post(url, data)
                .then(function(response) {
                    return response.data;
                });
        };

        return {
            getApplicationDetails: getApplicationDetails,
            get,
            post
        };
    };

    var module = angular.module("app");
    module.factory("RestService", RestService);

}());