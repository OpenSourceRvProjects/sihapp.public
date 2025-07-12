var angularApp = angular.module('sihappAngularApp', ['ngSanitize', 'ui.select']);

interface PatientRemissionScope extends ng.IScope {

    search();
    selectedPayment: Sihapp.WebProject.Models.RemissionNotes.PatientPaymentItemVM;
    pagination: Sihapp.WebProject.Models.Common.PaginationVM;
    backendData: Sihapp.WebProject.Models.Patients.Payments.PatientPaymentBackendDataVM;
    paymentItems: Sihapp.WebProject.Models.RemissionNotes.PagedPatientPaymentVM;

    ////PAGINATION VARS////
    next();
    prev();
    first();
    last();
    goToPage(page: number);
    pageLinks(): number[];
    itemsPerPage: number;
    currentPage: number;
    totalPages: number;
    searchText: string;
    processing: boolean;
    reset: boolean;
    showPaymentsPanel: boolean
    goBack();
    showDeletePopup(payment: Sihapp.WebProject.Models.RemissionNotes.PatientPaymentItemVM);
    responseMessage: string;
    delete();
}

angularApp.controller('patientRemissionCtrl', function ($scope: PatientRemissionScope, $http: ng.IHttpService) {

    $scope.itemsPerPage = 15;
    $scope.currentPage = 0;
    $scope.totalPages = 0;
    $scope.selectedPayment = <Sihapp.WebProject.Models.RemissionNotes.PatientPaymentItemVM>{};
    $scope.showPaymentsPanel = false;


    $scope.next = function () {
        if (($scope.currentPage + 1) == $scope.totalPages)
            return;
        $scope.currentPage = $scope.currentPage + 1;
        $scope.search();
    }

    $scope.prev = function () {
        if ($scope.currentPage == 0)
            return;

        $scope.currentPage = $scope.currentPage - 1;
        $scope.search();
    }

    $scope.first = function () {

        if ($scope.currentPage == 0)
            return;

        $scope.currentPage = 0;
        $scope.search();

    }

    $scope.last = function () {

        if (($scope.currentPage) == $scope.totalPages)
            return;
        $scope.currentPage = $scope.totalPages - 1;
        $scope.search();
    }

    $scope.goToPage = function (page) {

        $scope.currentPage = page;
        $scope.search();
    }

    $scope.pageLinks = function (): number[] {

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

        $scope.showPaymentsPanel = true;

        if ($scope.processing)
            return;

        if ($scope.reset)
            $scope.currentPage = 0;

        $scope.pagination = <Sihapp.WebProject.Models.Common.PaginationVM>{ Page: $scope.currentPage, PageSize: $scope.itemsPerPage, SearchTerm: $scope.searchText };
        $scope.processing = true;
        $http({
            method: 'POST',
            url: '../Patients/GetPagedPatientPayments',
            data: {
                pagination: $scope.pagination,
                remissionID: $scope.backendData.RemissionID
            }
        }).
            success(function (data: Sihapp.WebProject.Models.RemissionNotes.PagedPatientPaymentVM, status, headers, config) {
                debugger;
                $scope.paymentItems = data;
                $scope.processing = false;
                $scope.reset = false;
               
            });

    }

    $scope.goBack = function ()
    {
        $scope.showPaymentsPanel = false;

    }

    $scope.showDeletePopup = function (payment)
    {
        $scope.responseMessage = "";
        $scope.selectedPayment = payment;
        $('#deletePaymentModal').modal('show');

    }


    $scope.delete = function () {

        $scope.responseMessage = "";

        $http({
            method: 'POST',
            url: '../Patients/DeletePatientPayment',
            data: {
                RemissionPaymentID: $scope.selectedPayment.PaymentID,
               
            }
        }).
            success(function (data: string, status, headers, config) {
                $('#deletePaymentModal').modal('hide');
                $scope.search();
                $scope.processing = false;
            }).error(function (error, status, headers, config) {
                $scope.processing = false;
                $scope.responseMessage = error;
            });


    }
   

});
