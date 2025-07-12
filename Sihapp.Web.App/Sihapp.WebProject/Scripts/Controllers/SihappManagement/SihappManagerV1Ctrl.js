var angularApp = angular.module('sihappAngularApp', []);
angularApp.controller('sihappManagerV1Ctrl', function ($scope, $http) {
    ///PAGINATION SECTION///
    $scope.itemsPerPage = 8;
    $scope.currentPage = 0;
    $scope.totalPages = 0;
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
        if ($scope.processing)
            return;
        $scope.pagination = { Page: $scope.currentPage, PageSize: $scope.itemsPerPage, SearchTerm: $scope.searchText };
        $scope.processing = true;
        $http({
            method: 'POST',
            url: '../SihappManagement/GetSihappAccounts',
            data: {
                pagination: $scope.pagination,
            }
        }).success(function (data, status, headers, config) {
            $scope.accounts = data.items;
            $scope.totalPages = data.TotalPages;
            $scope.Count = data.AccountCount;
            $scope.processing = false;
        });
    };
    $scope.deleteAccount = function (selectedAccount) {
        $scope.selectedAccount = selectedAccount;
        $('#deleteAccountModal').modal('show');
    };
    $scope.delete = function () {
        $scope.responseMessage = "";
        $http({
            method: 'POST',
            url: '../SihappManagement/DeletePemanetlyAccount',
            data: {
                clinicID: $scope.selectedAccount.ClinicID,
                masterKey: $scope.masterKey,
            }
        }).success(function (data, status, headers, config) {
            if (data.length > 0) {
                $scope.responseMessage = data;
            }
            else {
                $('#deleteAccountModal').modal('hide');
                $scope.masterKey = "";
                $scope.search();
            }
        });
    };
    //Inicialization
    $scope.search();
});
//# sourceMappingURL=SihappManagerV1Ctrl.js.map