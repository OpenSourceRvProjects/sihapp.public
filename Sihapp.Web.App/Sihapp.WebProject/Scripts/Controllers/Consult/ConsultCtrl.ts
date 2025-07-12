var angularApp = angular.module('sihappAngularApp', ['angularFileUpload']);

interface ConsultListScope extends ng.IScope {

    model: Sihapp.WebProject.Models.ConsultVM;
    closeConsult();
    showCloseConfirmModal();
    cost: number;
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
    cannotUpload: boolean;
    getConsultImageQty();
}

angularApp.controller('consultCtrl', function ($scope: ConsultListScope, $http: ng.IHttpService, FileUploader) {

    // alert($scope.model.PatientName);
    
    $scope.imageSrc = "";
    $scope.cannotUpload = false;
   

    var uploader = $scope.uploader = new FileUploader({
        url: 'SaveConsultImage',
        params: {
            consultId: $scope.model.ID,
            method: 'POST', //method
        }
    });

    //uploader.onAfterAddingFile = function ($files) {
    //    console.info('onAfterAddingFile', $files);
    //};


    uploader.onBeforeUploadItem = function (item) {
        item.formData = [{ consultId: $scope.model.ID }];
    };

    uploader.onCompleteItem = function (fileItem, response: any, status, headers) {
        debugger;
        console.info('onCompleteItem', fileItem, response, status, headers);
        $scope.consultImg = response;
    };


    $scope.resetImageUploader = function () {
        $scope.consultImg = null;
        $scope.getConsultImageQty();
    }

    $scope.getConsultImageQty = function ()
    {

        $http({
            method: 'POST',
            url: '../Consult/GetConsultImgesQuantity',
            data: {
                consultId: $scope.model.ID,
            }
        }).
            success(function (data: boolean, status, headers, config) {
                $scope.cannotUpload = data;
            }).error(function (error, status, headers, config) {
                alert(error);
            });


    }

    $scope.saveConsultPatientImg = function () {


        $http({
            method: 'POST',
            url: '../Consult/SavePatientImg',
            data: {
                consultID: $scope.model.ID,
                img: $scope.consultImg,
                notes: $scope.model.Notes
            }
        }).
            success(function (data: string, status, headers, config) {
                $scope.consultImg = undefined;

            }).error(function (error, status, headers, config) {
                alert(error);
            });

    }


    $scope.saveNotes = function () {
        $http({
            method: 'POST',
            url: '../Consult/SaveNotes',
            data: {
                consultID: $scope.model.ID,
                notes: $scope.model.Notes
            }
        }).
            success(function (data: string, status, headers, config) {
                alert("Notas guardadas con éxito");

            }).error(function (status, headers, config) {

            });

    }

    $scope.cost = 0;
    debugger;
    $scope.resetImageUploader();

    $scope.showCloseConfirmModal = function () {
        $('#closeConfirmation').modal('show');

    }


    $scope.closeConsult = function () {
        $scope.errorMsg = "";
        $http({
            method: 'POST',
            url: '../Consult/CloseConsult',
            data: {
                consultID: $scope.model.ID,
                notes: $scope.model.Notes,
                amount: $scope.cost,
            }
        }).
            success(function (data: string, status, headers, config) {
                debugger;
                

                $scope.errorMsg = data;
                if ($scope.errorMsg.length == 0) {
                    $('#closeConfirmation').modal('hide');
                    window.location.href = "../../Appointment/AppointmentsList";
                }

            }).error(function (status, headers, config) {

            });

    }

    $scope.getConsultImages = function ()
    {
        $http({
            method: 'POST',
            url: '../Consult/GetConsultImagesByConsultId',
            data: {
                id: $scope.model.ID,
            }
        }).
            success(function (data: Sihapp.WebProject.Models.Consult.ConsultImageVM[], status, headers, config) {
                $scope.consultImages = data;
            });
    }
 
});


