var angularApp = angular.module('sihappAngularApp', []);
angularApp.controller('patientsListCtrl', function ($scope, $http) {
    $scope.message = "Lista de pacientes registrados";
    $scope.createUser = false;
    $scope.responseMessage = "";
    $scope.itemsPerPage = 10;
    $scope.currentPage = 0;
    $scope.totalPages = 0;
    $scope.searchText = "";
    $scope.reset = false;
    $scope.PatientsQty;
    $scope.customFieldsValuesToRequest = [];
    $scope.validateAddPatient = function () {
        var result = "";
        if ($scope.name === undefined || !$scope.name)
            result = "*El nombre de tu paciente es obligatorio\n";
        if ($scope.lastName1 === undefined || !$scope.lastName1)
            result += " *El apellido paterno de tu paciente es obligatorio\n";
        if ($scope.lastName2 === undefined || !$scope.lastName2)
            result += " *El apellido materno de tu paciente es obligatorio\n";
        if ($scope.birthDate === undefined || !$scope.birthDate)
            result += " *La fecha de nacimiento de tu paciente es obligatoria\n";
        if ($scope.address === undefined || !$scope.address)
            result += " *La dirección de tu paciente es obligatoria\n";
        if ($scope.email === undefined || !$scope.email)
            result += " *El correo de tu paciente es obligatorio\n";
        if ($scope.phone === undefined || !$scope.phone)
            result += " *El telefono de tu paciente es obligatorio\n";
        return result;
    };
    debugger;
    ///PAGINATION SECTION///
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
        $http({
            method: 'POST',
            url: '../Patients/GetPagedPatients',
            data: {
                search: $scope.searchText,
                pagination: $scope.pagination
            }
        }).
            success(function (data, status, headers, config) {
            $scope.patientItems = data.PatientItems;
            $scope.totalPages = data.TotalPages;
            $scope.processing = false;
            $scope.PatientsQty = data.PatientsQty;
            $scope.reset = false;
        });
    };
    $scope.getPresetFields = function () {
        debugger;
        if ($scope.processing)
            return;
        $scope.pagination = { Page: $scope.currentPage, PageSize: $scope.itemsPerPage, SearchTerm: $scope.searchText };
        $scope.processing = true;
        $http({
            method: 'GET',
            url: '../Patients/GetPatientCustomFieldsByClinic',
            params: {}
        }).
            success(function (data, status, headers, config) {
            $scope.presetFields = data;
            $scope.processing = false;
        });
    };
    $scope.searchPanel = function () {
        $scope.reset = true;
        $scope.search();
    };
    $scope.savePatient = function () {
        debugger;
        if ($scope.processing)
            return;
        $scope.responseMessage = "";
        if ($scope.validateAddPatient().length > 0) {
            $scope.responseMessage = $scope.validateAddPatient();
            return;
        }
        $scope.processing = true;
        $scope.responseMessage = "";
        $http({
            method: 'POST',
            url: '../Patients/AddPatient',
            data: {
                name: $scope.name,
                lastName1: $scope.lastName1,
                lastName2: $scope.lastName2,
                birthDate: $scope.birthDate,
                weight: $scope.weight,
                heigth: $scope.heigth,
                address: $scope.address,
                phone: $scope.phone,
                email: $scope.email,
                createUser: $scope.createUser,
                userName: $scope.userName,
                password: $scope.password,
                customFields: $scope.customFieldsValuesToRequest,
            }
        }).
            success(function (data, status, headers, config) {
            debugger;
            $scope.search();
            window.location.href = "../Patients/PatientsList";
            $('#addPatientModal').modal('hide');
            $scope.name = undefined;
            $scope.lastName1 = undefined;
            $scope.lastName2 = undefined;
            $scope.birthDate = undefined;
            $scope.weight = undefined;
            $scope.heigth = undefined;
            $scope.address = undefined;
            $scope.phone = undefined;
            $scope.email = undefined;
            $scope.createUser = undefined;
            $scope.userName = undefined;
            $scope.password = undefined;
            $scope.processing = false;
            $scope.customFieldsValuesToRequest = [];
        }).error(function (error, status, headers, config) {
            debugger;
            $scope.responseMessage = error;
            $scope.processing = false;
        });
    };
    $scope.showAddPatientModal = function () {
        $scope.getPresetFields();
        $('#addPatientModal').modal('show');
    };
    $scope.openDeletePopUp = function (patient) {
        $scope.selectedPatient = patient;
        $('#deletePatientModal').modal('show');
    };
    $scope.goToExpedient = function (id) {
        window.location.href = "../Patients/PatientExpedientByID?id=" + id;
    };
    $scope.delete = function () {
        $scope.responseMessage = "";
        $http({
            method: 'POST',
            url: '../Patients/DeletePatient',
            data: {
                patientID: $scope.selectedPatient.ID,
            }
        }).
            success(function (data, status, headers, config) {
            $scope.responseMessage = data;
            if ($scope.responseMessage.length <= 0) {
                window.location.href = "../Patients/PatientsList";
                $('#deletePatientModal').modal('hide');
            }
            $scope.processing = false;
        }).error(function (status, headers, config) {
            $scope.processing = false;
        });
    };
    $scope.addCustomFieldToView = function () {
        var newCustomField;
        newCustomField = {};
        newCustomField.FieldType = $scope.customFieldsTypes[0].Value;
        newCustomField.FieldText = "";
        newCustomField.AllowAllEntities = true;
        $scope.customFieldsValuesToRequest.push(newCustomField);
    };
    //Inicialization
    $scope.search();
});
//# sourceMappingURL=PatientsListCtrl.js.map