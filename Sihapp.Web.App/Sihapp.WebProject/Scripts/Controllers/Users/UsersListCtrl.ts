/// <reference path="../Utils/Utils.ts" />
var angularApp = angular.module('sihappAngularApp', []);

interface UsersListScope extends ng.IScope {

    search();
    selectedUser: Sihapp.WebProject.Models.Users.UserItemsVM;
    userItems: Sihapp.WebProject.Models.Users.UserItemsVM[];
    pagination: Sihapp.WebProject.Models.Common.PaginationVM;
    showAddUserModal();
    userType: string;
    backendData: Sihapp.WebProject.Models.Common.TextValueVM[];
    selectedPatient: Sihapp.WebProject.Models.Common.TextValueVM;
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

    ////ClinicMenData////
    cmName: string;
    cmLastName1: string;
    cmLastName2: string;
    cmBirthDate: Date;
    cmPhone: string;
    cmEmail: string;
    cmAddress: string;
    cmUserName: string;
    cmPassword: string;

    patientUserName: string;
    patientPassword: string;


    save();
    saveAuxiliar();
    savePatient();
    openModalChangePassword();
    password: string;
    confirmPassword: string;
    changePassword();
    selectUser(user: Sihapp.WebProject.Models.Users.UserItemsVM);
    errorMsg: string;
}

angularApp.controller('usersListCtrl', function ($scope: UsersListScope, $http: ng.IHttpService) {
    $scope.itemsPerPage = 10;
    $scope.currentPage = 0;
    $scope.totalPages = 0;
    $scope.searchText = "";
    $scope.pagination = <Sihapp.WebProject.Models.Common.PaginationVM>{ Page: $scope.currentPage, PageSize: $scope.itemsPerPage };
    debugger;

    $scope.search = function () {
        debugger;

        if ($scope.processing)
            return;

        $scope.pagination = <Sihapp.WebProject.Models.Common.PaginationVM>{ Page: $scope.currentPage, PageSize: $scope.itemsPerPage };
        $scope.processing = true;
        $http({
            method: 'POST',
            url: '../User/GetPagedUsers',
            data: {
                search: $scope.searchText,
                pagination: $scope.pagination
            }
        }).
            success(function (data: Sihapp.WebProject.Models.Users.PageableUsersVM, status, headers, config) {
                $scope.userItems = data.UserItems;
                $scope.totalPages = data.TotalPages;
                $scope.processing = false;
            });

    }
    $scope.showAddUserModal = function () {
        $('#addUserModal').modal('show');
    }
    ///CLINIC MEN ///
    $scope.save = function () {
        if ($scope.userType === "Profesional de salud") {
            $http({
                method: 'POST',
                url: '../ClinicMen/AddClinicMen',
                data: {
                    cmName: $scope.cmName,
                    cmLastName1: $scope.cmLastName1,
                    cmLastName2: $scope.cmLastName2,
                    cmBirthDate: $scope.cmBirthDate,
                    cmAddress: $scope.cmAddress,
                    cmPhone: $scope.cmPhone,
                    cmEmail: $scope.cmEmail,
                    cmUserName: $scope.cmUserName,
                    cmPassword: $scope.cmPassword,
                }
            }).
                success(function (status, headers, config) {
                    $('#addUserModal').modal('hide');
                    window.location.href = "../User/UsersList"
                    $scope.search();
                }).error(function (error, status, headers, config) {
                    $scope.errorMsg = Utils.HandleServerError(error);
                });
        }
    }

    $scope.saveAuxiliar = function () {

        $scope.errorMsg = "";
        $http({
            method: 'POST',
            url: '../AuxiliarPersonal/AddAuxiliarPersonal',
            data: {
                cmName: $scope.cmName,
                cmLastName1: $scope.cmLastName1,
                cmLastName2: $scope.cmLastName2,
                cmBirthDate: $scope.cmBirthDate,
                cmAddress: $scope.cmAddress,
                cmPhone: $scope.cmPhone,
                cmEmail: $scope.cmEmail,
                cmUserName: $scope.cmUserName,
                cmPassword: $scope.cmPassword,
            }
        }).
            success(function (status, headers, config) {
                $('#addUserModal').modal('hide');

                $scope.search();
                $scope.cmName = undefined;
                $scope.cmLastName1 = undefined;
                $scope.cmLastName2 = undefined;
                $scope.cmBirthDate = undefined;
                $scope.cmAddress = undefined;
                $scope.cmPhone = undefined;
                $scope.cmEmail = undefined;
                $scope.cmUserName = undefined;
                $scope.cmPassword = undefined;
                window.location.href = "../User/UsersList"
            }).error(function (error, status, headers, config) {
                $scope.errorMsg = error;
            });
    }


    $scope.savePatient = function () {

        $http({
            method: 'POST',
            url: '../Patients/AddUserPatient',
            data: {
                patientID: $scope.selectedPatient.Value,
                userName: $scope.patientUserName,
                password: $scope.patientPassword,
            }
        }).
            success(function (status, headers, config) {
                $scope.search();
                window.location.href = "../Patients/PatientsList"
                $('#addUserModal').modal('hide');
            }).error(function (error, status, headers, config) {
                $scope.errorMsg = error;
            });


    }


    ///PAGINATION SECTION///
    $scope.search();

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

    $scope.selectUser = function (user: Sihapp.WebProject.Models.Users.UserItemsVM) {
        $scope.errorMsg = "";
        $('#changePasswordModal').modal('show');
        $scope.selectedUser = user;
        $scope.password = undefined;
        $scope.confirmPassword = undefined;
    }



    $scope.changePassword = function () {

        if (!$scope.password || !$scope.confirmPassword) {
            $scope.errorMsg = "La contraseña y su confirmación es obligatoria";
            return;
        }

        if ($scope.password !== $scope.confirmPassword) {
            $scope.errorMsg = "La confirmación de la contraseña no coincide";
            return;
        }

        $http({
            method: 'POST',
            url: '../Account/ResetPassowrd',
            data: {
                userID: $scope.selectedUser.ID,
                newPassword: $scope.password
            }
        }).
            success(function (status, headers, config) {
                $('#changePasswordModal').modal('hide');
                window.location.href = "../User/UsersList"
            }).error(function (status, headers, config) {
            });
    }

});
