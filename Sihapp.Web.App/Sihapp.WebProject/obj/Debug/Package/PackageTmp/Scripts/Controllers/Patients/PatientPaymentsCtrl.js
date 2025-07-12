var angularApp = angular.module('sihappAngularApp', ['ngSanitize', 'ui.select']);
angularApp.controller('patientPaymentsCtrl', function ($scope, $http) {
    $scope.itemsPerPage = 15;
    $scope.currentPage = 0;
    $scope.totalPages = 0;
    $scope.selectedRemission = {};
    $scope.next = function () {
        if (($scope.currentPage + 1) == $scope.totalPages)
            return;
        $scope.currentPage = $scope.currentPage + 1;
        $scope.search();
    };
    $scope.prev = function () {
        if ($scope.currentPage == 0)
            return;
        $scope.currentPage = $scope.currentPage - 1;
        $scope.search();
    };
    $scope.first = function () {
        if ($scope.currentPage == 0)
            return;
        $scope.currentPage = 0;
        $scope.search();
    };
    $scope.last = function () {
        if (($scope.currentPage) == $scope.totalPages)
            return;
        $scope.currentPage = $scope.totalPages - 1;
        $scope.search();
    };
    $scope.goToPage = function (page) {
        $scope.currentPage = page;
        $scope.search();
    };
    $scope.pageLinks = function () {
        var result = [];
        if ($scope.totalPages < $scope.currentPage + 4) {
            for (var i = $scope.currentPage; i < $scope.totalPages; i++) {
                result.push(i);
            }
        }
        else {
            for (var i = $scope.currentPage; i < $scope.currentPage + 4; i++) {
                result.push(i);
            }
        }
        return result;
    };
    $scope.search = function () {
        debugger;
        if ($scope.processing)
            return;
        if ($scope.reset)
            $scope.currentPage = 0;
        $scope.pagination = { Page: $scope.currentPage, PageSize: $scope.itemsPerPage, SearchTerm: $scope.searchText };
        $scope.processing = true;
        var patientID = $scope.selectedPatient !== undefined ? $scope.selectedPatient.Value : null;
        $http({
            method: 'POST',
            url: '../Patients/GetPagedRemisionNotes',
            data: {
                pagination: $scope.pagination,
                startDate: $scope.backendData.StartDate,
                endDate: $scope.backendData.EndDate,
                patientID: patientID
            }
        }).
            success(function (data, status, headers, config) {
            debugger;
            $scope.remissions = data.RemissionList;
            $scope.totalPages = data.TotalPages;
            $scope.totalPending = data.TotalPending;
            $scope.totalPayed = data.TotalPayed;
            $scope.totalGrandTotal = data.TotalGrandAmount;
            $scope.processing = false;
            $scope.reset = false;
        });
    };
    $scope.openPaymentPopUp = function (remission) {
        $scope.selectedRemission = remission;
        $scope.paymentNotes = "Pago de la consulta # " + remission.ConsultNumber
            + " del paciente " + remission.PatientName + " con " + remission.ClinicMen;
        $('#addPaymentModal').modal('show');
    };
    $scope.addPayment = function () {
        debugger;
        $scope.errorMsg = "";
        if ($scope.processing)
            return;
        $scope.processing = true;
        $http({
            method: 'POST',
            url: '../Patients/AddPatientPayment',
            data: {
                remission: $scope.selectedRemission,
                comments: $scope.paymentNotes,
                date: new Date(),
            }
        }).
            success(function (data, status, headers, config) {
            debugger;
            $scope.processing = false;
            $scope.reset = true;
            $scope.search();
            $('#addPaymentModal').modal('hide');
        }).error(function (error, status, headers, config) {
            $scope.errorMsg = error;
            $scope.processing = false;
        });
    };
    $scope.openDetails = function (remission) {
        window.location.href = "../../Patients/PatientRemissionDetails?remissionID=" + remission.ID;
    };
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
            $scope.backendData.Patients = data;
        });
    };
});
//# sourceMappingURL=PatientPaymentsCtrl.js.map