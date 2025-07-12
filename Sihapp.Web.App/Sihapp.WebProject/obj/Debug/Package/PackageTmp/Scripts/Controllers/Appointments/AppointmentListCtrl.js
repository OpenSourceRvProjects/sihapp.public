var angularApp = angular.module('sihappAngularApp', ['ngSanitize', 'ui.select']);
angularApp.controller('appointmentListCtrl', function ($scope, $http) {
    //alert($scope.backendData.UserType);
    $scope.anotherClinicMen = false;
    $scope.selectedClinicMen = {};
    $scope.selectedAppointmentStatus = {};
    $scope.ui_selectMode = true;
    $scope.changePatientListMode = function () {
        $scope.ui_selectMode = !$scope.ui_selectMode;
    };
    $scope.statusModel = {};
    $scope.statusModel.SelectedStatus = {};
    $scope.openSendReminderPopUp = function (appointment) {
        $scope.selectedAppointment = appointment;
        $('#appointmentReminder').modal('show');
    };
    $scope.showAddAppointmentModal = function () {
        $('#addAppointmentModal').modal('show');
    };
    $scope.openAppointmentStatus = function () {
        $scope.responseMsg = "";
        $('#appointmentPanel').modal('show');
    };
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
            success(function (data, status, headers, config) {
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
    };
    $scope.sendReminderEmail = function () {
        $scope.responseMsg = "";
        $http({
            method: 'POST',
            url: '../Appointment/SendAppointmentReminder',
            data: {
                appointmentID: $scope.selectedAppointment.AppointmentID,
            }
        }).
            success(function (data, status, headers, config) {
            $('#appointmentReminder').modal('hide');
        }).error(function (status, headers, config) {
        });
    };
    $scope.selectAppointment = function (appointment) {
        $scope.selectedAppointmentID = appointment.AppointmentID;
        $scope.hideOpenConsultButton = appointment.HasActiveConsult;
        var obj = _.filter($scope.backendData.AppointmentStatusCodes, function (ap) { return ap.Value === appointment.SelectedStatus.Value; });
        if (obj.length > 0)
            $scope.selectedAppointmentStatus = obj[0];
        $scope.statusModel.SelectedStatus = obj[0];
        if (appointment.HasActiveConsult)
            $scope.selectedConsultID = appointment.ConsultID;
        $('#appointmentPanel').modal('show');
    };
    $scope.goToConsult = function () {
        window.location.href = "../../Consult/Consult?consultID=" + $scope.selectedConsultID;
    };
    $scope.openConsult = function () {
        $http({
            method: 'POST',
            url: '../Consult/OpenConsult',
            data: {
                appointmentID: $scope.selectedAppointmentID,
            }
        }).
            success(function (data, status, headers, config) {
            debugger;
            var redirectGuid = data;
            $('#addAppointmentModal').modal('hide');
            window.location.href = "../../Consult/Consult?consultID=" + redirectGuid;
        }).error(function (status, headers, config) {
        });
    };
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
            }
        }).
            success(function (data, status, headers, config) {
            $scope.appointmentGroups = data.Groups;
            // $scope.totalPages = data.TotalPages;
            $scope.processing = false;
        });
    };
    if ($scope.backendData.Option == 1) {
        $scope.showAddAppointmentModal();
    }
    $scope.search();
    $scope.searchPatients = function (keyword) {
        $http({
            method: 'POST',
            url: '../Patients/GetPatientsByKeywords',
            data: {
                keyword: keyword
            }
        }).
            success(function (data, status, headers, config) {
            $scope.backendData.PatientList = data;
        });
    };
});
//# sourceMappingURL=AppointmentListCtrl.js.map