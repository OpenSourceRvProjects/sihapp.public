var angularApp = angular.module('sihappAngularApp', []);

interface SettingsScope extends ng.IScope {

    editMode: boolean;
    onEditMode();
    offEditMode();
    model: Sihapp.WebProject.Models.ClinicMen.ClinicMenSettingsVM;
    updateSettings();
}

angularApp.controller('settingsCtrl', function ($scope: SettingsScope, $http: ng.IHttpService) {

    $scope.editMode = false;
    $scope.onEditMode = function () {
        $scope.editMode = true;
    }

    $scope.offEditMode = function () {
        $scope.editMode = false;
        window.location.href = "../ClinicMen/Settings"
    }

    $scope.updateSettings = function () {
        $http({
            method: 'POST',
            url: '../ClinicMen/UpdateSettings',
            data: {
                settings: $scope.model,
            }
        }).
            success(function (status, headers, config) {
                $scope.offEditMode();
            }).error(function (status, headers, config) {
            });

    }

});

