(function () {

    var app = angular.module("app", ["ngRoute"]);

    app.config(function ($routeProvider) {
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
            .when("/leafcerts/:id/details/", {
                templateUrl: "/app/Views/leaf-detail.html",
                controller: "LeafCertDetailController"
            })
            .when("/leafcerts/new/", {
                templateUrl: "/app/Views/leaf-new.html",
                controller: "LeafNewController"
            })
            .otherwise({ redirectTo: "/" });
    });

    app.run(function ($rootScope, RestService) {

        RestService.getApplicationDetails()
            .then(function (data) {
                $rootScope.app_name = data.applicationName;
                $rootScope.app_version = data.applicationVersion;
                $rootScope.app_built = data.applicationBuilt;
                $rootScope.hostname = data.hostname;
                $rootScope.uptime = data.uptime;
            });

        $rootScope.login = function () {
            $rootScope.isLoading = true;

            setTimeout(() => {
                $rootScope.$apply(function () {
                    $rootScope.loginError = "Unknown username or bad password.";
                    $rootScope.login.password = "";
                    $rootScope.isLoading = false;
                });
            }, 2000);
        };

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

    // sleep time expects milliseconds
    function sleep(time) {
        return new Promise((resolve) => setTimeout(resolve, time));
    }

}());