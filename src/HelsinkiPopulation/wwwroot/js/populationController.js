// populationController.js
(function () {

    "use strict";

    // Getting the existing module
    angular.module("app")
      .controller("populationController", populationController);

    function populationController($http) {

        var vm = this;
        vm.chartlabels = [];
        vm.chartseries = [];
        vm.years = [];
        vm.errorMessage = "";
        vm.options = {};
        //Call this on the first page load. API then checks if cached year exists and retrieves the data accordingly.
        //Notice! The return value should also contain the cached year so that the year dropdown could be refreshed.
        $http({
            url: '/api/population/',
            method: 'GET'
        }
        ).success(function (data) {
            vm.errorMessage = "";
            vm.chartlabels = Object.keys(data);
            vm.chartseries = Object.values(data);
        });

        //Gets the parsed year data from the API which fetches the data from dev.helsinki.fi.
        $http({
            url: '/api/population/years',
            method: 'GET'
        }
        ).success(function (data) {
            vm.errorMessage = "";
            vm.years = data;
        });

        //Workaround for Chart.js defect whic cut off part of the labels on diagram.
        vm.options = {
            scaleLabel: function (object) {
                return "      " + object.value;
            },
            responsive: true,
            pointDot: false
        };


        //Update the chart according to the selected year. Calls the API which calls dev.helsinki.fi and parses the data.
        vm.UpdateData = function (group) {
            vm.errorMessage = "";
            var year = {
                id: group.Year
            }
            $http({
                url: '/api/population/',
                method: 'GET',
                params: year
            }
            ).success(function (data) {
                vm.labels = Object.keys(data);
                vm.data = [Object.keys(data).map(function (key) {
                    return data[key];
                })];


            });
        }
    };





})();