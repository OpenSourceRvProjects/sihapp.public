var angularApp = angular.module('sihappAngularApp', ['ngSanitize', 'ui.select']);

interface AppointmentListScope extends ng.IScope {
    backendData: Sihapp.WebProject.Models.Appointments.AppointmentBackendDataVM;
    showAddAppointmentModal();
    appointmentGroups: Sihapp.WebProject.Models.Appointments.GroupedAppointmentVM[];
    newAppointmentTime: Date;
    newAppointmentDate: Date;
    selectedPatient: Sihapp.WebProject.Models.Common.TextValueVM;
    saveAppointment();
    anotherClinicMen: boolean;
    selectedClinicMen: Sihapp.WebProject.Models.Common.TextValueVM;
    selectedAppointmentStatus: Sihapp.WebProject.Models.Common.TextValueVM;
    responseMsg: string;
    search();
    processing: boolean;
    openAppointmentStatus();
    openConsult();
    selectedAppointmentID: System.Guid;
    selectAppointment(appointment: Sihapp.WebProject.Models.Appointments.AppointmentItemsVM)
    hideOpenConsultButton: boolean;
    goToConsult();
    selectedConsultID: System.Guid;
    openSendReminderPopUp(appointment: Sihapp.WebProject.Models.Appointments.AppointmentItemsVM);
    selectedAppointment: Sihapp.WebProject.Models.Appointments.AppointmentItemsVM;
    sendReminderEmail();
    statusModel: StatusModel;
    searchPatients(keyboard: string);
    changePatientListMode();
    ui_selectMode: boolean;
}

interface StatusModel {
    SelectedStatus: Sihapp.WebProject.Models.Common.TextValueVM;

}

angularApp.controller('appointmentListCtrl', function ($scope: AppointmentListScope, $http: ng.IHttpService) {
    //alert($scope.backendData.UserType);

    $scope.anotherClinicMen = false;
    $scope.selectedClinicMen = <Sihapp.WebProject.Models.Common.TextValueVM>{};
    $scope.selectedAppointmentStatus = <Sihapp.WebProject.Models.Common.TextValueVM>{};

    $scope.ui_selectMode = true;

    $scope.changePatientListMode = function () {
        $scope.ui_selectMode = !$scope.ui_selectMode;
    }

    $scope.statusModel = <StatusModel>{};
    $scope.statusModel.SelectedStatus = <Sihapp.WebProject.Models.Common.TextValueVM>{};
    $scope.openSendReminderPopUp = function (appointment: Sihapp.WebProject.Models.Appointments.AppointmentItemsVM) {
        $scope.selectedAppointment = appointment;
        $('#appointmentReminder').modal('show');
    }


    $scope.showAddAppointmentModal = function () {
        $('#addAppointmentModal').modal('show');
    }

    $scope.openAppointmentStatus = function () {
        $scope.responseMsg = "";
        $('#appointmentPanel').modal('show');
    }

    $scope.saveAppointment = function () {
        if ($scope.processing)
            return;

        $scope.processing = true;

        $scope.responseMsg = "";
        $http({
            method: 'POST',
            url: '../Appointment/AddAppointment',
            data: {
                patientID: $scope.selectedPatient.Value,
                anotherClinicMen: $scope.anotherClinicMen,
                clinicMenID: $scope.selectedClinicMen.Value,
                newAppointmentDate: $scope.newAppointmentDate,
                newAppointmentTime: $scope.newAppointmentTime,
            }
        }).
            success(function (data: string, status, headers, config) {
                debugger;
                $scope.responseMsg = data;
                $scope.processing = false;
                if ($scope.responseMsg.length <= 0) {
                    $scope.search();
                    $('#addAppointmentModal').modal('hide');
                }

            }).error(function (status, headers, config) {
                $scope.processing = false;
            });

    }


    $scope.sendReminderEmail = function () {
        $scope.responseMsg = "";
        $http({
            method: 'POST',
            url: '../Appointment/SendAppointmentReminder',
            data: {
                appointmentID: $scope.selectedAppointment.AppointmentID,
            }
        }).
            success(function (data: string, status, headers, config) {
                $('#appointmentReminder').modal('hide');
            }).error(function (status, headers, config) {

            });

    }


    $scope.selectAppointment = function (appointment: Sihapp.WebProject.Models.Appointments.AppointmentItemsVM) {

        $scope.selectedAppointmentID = appointment.AppointmentID;
        $scope.hideOpenConsultButton = appointment.HasActiveConsult;



        var obj = _.filter($scope.backendData.AppointmentStatusCodes
            , function (ap) { return ap.Value === appointment.SelectedStatus.Value });
        if (obj.length > 0)
            $scope.selectedAppointmentStatus = obj[0];
        $scope.statusModel.SelectedStatus = obj[0];

        if (appointment.HasActiveConsult)
            $scope.selectedConsultID = appointment.ConsultID;
        $('#appointmentPanel').modal('show');

    }

    $scope.goToConsult = function () {
        window.location.href = "../../Consult/Consult?consultID=" + $scope.selectedConsultID;

    }


    $scope.openConsult = function () {

        $http({
            method: 'POST',
            url: '../Consult/OpenConsult',
            data: {
                appointmentID: $scope.selectedAppointmentID,
            }
        }).
            success(function (data: System.Guid, status, headers, config) {
                debugger;
                var redirectGuid = data;
                $('#addAppointmentModal').modal('hide');

                window.location.href = "../../Consult/Consult?consultID=" + redirectGuid;

            }).error(function (status, headers, config) {

            });
    }




    $scope.search = function () {
        debugger;

        if ($scope.processing)
            return;

        // $scope.pagination = <Sihapp.WebProject.Models.Common.PaginationVM>{ Page: $scope.currentPage, PageSize: $scope.itemsPerPage };
        $scope.processing = true;
        $http({
            method: 'POST',
            url: '../Appointment/GetPagedAppointments',
            data: {
                startDate: $scope.backendData.StartDate,
                endDate: $scope.backendData.EndDate,
                patientID: null,
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



    if ($scope.backendData.Option == 1) {
        $scope.showAddAppointmentModal();
    }

    $scope.search();

    $scope.searchPatients = function (keyword: string) {
        $http({
            method: 'POST',
            url: '../Patients/GetPatientsByKeywords',
            data: {
                keyword: keyword

            }
        }).
            success(function (data: Sihapp.WebProject.Models.Common.TextValueVM[], status, headers, config) {
                $scope.backendData.PatientList = data;

            });
    }

});
