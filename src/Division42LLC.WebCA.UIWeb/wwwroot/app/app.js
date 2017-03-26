(function() {

    var app = angular.module("app", ["ngRoute"]);

    app.config(function($routeProvider) {
        $routeProvider
            .when("/", {
                templateUrl: "/app/Views/home.html",
                controller: "HomeController"
            })
            .when("/ca", {
                templateUrl: "/app/Views/ca.html",
                controller: "CAController"
            })
            .when("/leafcerts", {
                templateUrl: "/app/Views/leaf-index.html",
                controller: "LeafCertsController"
            })
            .when("/leafcerts/new", {
                templateUrl: "/app/Views/leaf-new.html",
                controller: "LeafNewController"
            })
            .otherwise({ redirectTo: "/" });
    });

    app.run(function($rootScope, RestService) {

        RestService.getApplicationDetails()
            .then(function(data) {
                $rootScope.app_name = data.app_name;
                $rootScope.app_version = data.app_version;
                $rootScope.app_built = data.app_built;
                $rootScope.hostname = data.hostname;
                $rootScope.uptime = data.uptime;
            });

        $rootScope.refreshCA = function () {
            RestService.get("/api/ca/get")
                .then(function (response) {
                    $rootScope.ca = response;

                    if (response.status === "OK") {
                        $rootScope.caCertValid = true;
                    } else {
                        $rootScope.caCertValid = false;
                    }
                });
        }

        $rootScope.refreshCA();
    });

}());