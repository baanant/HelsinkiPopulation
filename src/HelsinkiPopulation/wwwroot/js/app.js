// app.js
(function () {

    "use strict";

    // Creating the Module
    angular.module("app", ["ngRoute", "chart.js", "angular-loading-bar"])
      .config(function ($routeProvider) {
          $routeProvider.when("/", {
              controller: "populationController",
              controllerAs: "vm",
              templateUrl: "/views/populationView.html"
          });

          $routeProvider.otherwise({ redirectTo: "/" });

      });

})();