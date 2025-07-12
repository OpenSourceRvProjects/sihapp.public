var angularApp = angular.module('sihappAngularApp', ['angularFileUpload']);

interface PatientExpedientScope extends ng.IScope {

    model: Sihapp.WebProject.Models.Patients.Expedient.PatientExpedientViewBagVM;
    appointmentGroups: Sihapp.WebProject.Models.Appointments.GroupedAppointmentVM[];
    errorMsg: string;
    imageSrc: any;
    progress: any;
    showPic();
    uploader: any;
    consultImg: number[];
    saveConsultPatientImg();
    saveNotes();
    getConsultImages();
    consultImages: Sihapp.WebProject.Models.Consult.ConsultImageVM[];
    resetImageUploader();
    searchAppointments();
    processing: boolean;
    printPDF();

}

angularApp.controller('patientExpedientCtrl', function ($scope: PatientExpedientScope, $http: ng.IHttpService, FileUploader) {

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
                //search: $scope.searchText,
                //pagination: $scope.pagination
            }
        }).
            success(function (data: Sihapp.WebProject.Models.Appointments.PageableAppointmentsVM, status, headers, config) {
                $scope.appointmentGroups = data.Groups;
                // $scope.totalPages = data.TotalPages;
                $scope.processing = false;
            });

    }

    $scope.printPDF = function ()
    {

        $http({
            method: 'GET',
            url: '../Patients/pdf',
          
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
            }
            }).
            success(function (data: any, status, headers, config) {
                debugger;
                var file = new Blob([data], { type: 'application/pdf' });
                var fileurl = URL.createObjectURL(file);
                window.open(fileurl);
            });

    }

});


