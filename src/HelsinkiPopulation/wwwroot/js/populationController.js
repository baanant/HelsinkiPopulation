// populationController.js
(function () {

    "use strict";

    // Getting the existing module
    angular.module("app")
      .controller("populationController", populationController);
    

    //TODO! Fix rendering problems.
    function populationController($http) {

        var vm = this;
        vm.chartlabels = [];
        vm.chartseries = [];
        vm.years = [];
        vm.errorMessage = "";
        vm.options = {};
        //Call this on the first page load. API then checks if cached year exists and retrieves the data accordingly.
        //Notice! The dropdown item should be also refreshed.
        vm.Initialize = function()
        {
            vm.errorMessage = "";
            $http({
                url: '/api/population/',
                method: 'GET',
                params:null
            }
            ).success(function (data) {
                vm.labels = Object.keys(data.Data);
                vm.data = [Object.keys(data.Data).map(function (key) {
                    return data.Data[key];
                })];

                vm.year = data.Year;
            });
        }

        vm.Initialize();
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
                vm.labels = Object.keys(data.Data);
                vm.data = [Object.keys(data.Data).map(function (key) {
                    return data.Data[key];
                })];
                vm.year = data.Year;

            });
        }
    };





})();