var angularApp = angular.module('sihappAngularApp', ['angularFileUpload']);
angularApp.controller('patientExpedientCtrl', function ($scope, $http, FileUploader) {
    $scope.processing = false;
    $scope.searchAppointments = function () {
        debugger;
        if ($scope.processing)
            return;
        // $scope.pagination = <Sihapp.WebProject.Models.Common.PaginationVM>{ Page: $scope.currentPage, PageSize: $scope.itemsPerPage };
        $scope.processing = true;
        $http({
            method: 'POST',
            url: '../Appointment/GetPagedAppointments',
            data: {
                startDate: $scope.model.StartDateFilter,
                endDate: $scope.model.EndDateFilter,
                patientID: $scope.model.ID,
            }
        }).
            success(function (data, status, headers, config) {
            $scope.appointmentGroups = data.Groups;
            // $scope.totalPages = data.TotalPages;
            $scope.processing = false;
        });
    };
    $scope.printPDF = function () {
        $http({
            method: 'GET',
            url: '../Patients/pdf',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
            }
        }).
            success(function (data, status, headers, config) {
            debugger;
            var file = new Blob([data], { type: 'application/pdf' });
            var fileurl = URL.createObjectURL(file);
            window.open(fileurl);
        });
    };
});
//# sourceMappingURL=PatientExpedientCtrl.js.map