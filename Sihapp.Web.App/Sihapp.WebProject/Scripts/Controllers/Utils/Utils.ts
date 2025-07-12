module Utils {
    export function HandleServerError(error: any): string {
        alert(error);
        var htmlObject = document.createElement('div');
        htmlObject.innerHTML = error;
        var x = htmlObject.getElementsByTagName("title")[0];
        return x.innerHTML;
    }
}

////export class PaginationEngine {

   
////    var itemsPerPage: number;
////var currentPage: number;
////var totalPages: number;
////var searchText: string;
////var processing: boolean;


//export function next() {
//    if ((currentPage + 1) == totalPages)
//        return;
//    currentPage = currentPage + 1;
//    //$scope.search();
//}

//export function prev() {
//    if (currentPage == 0)
//        return;

//    currentPage = currentPage - 1;
//    //$scope.search();
//}

//export function first() {

//    if (currentPage == 0)
//        return;

//    currentPage = 0;
//    //$scope.search();

//}

//export function last() {

//    if ((currentPage) == totalPages)
//        return;
//    currentPage = totalPages - 1;
//    $scope.search();
//}

//$scope.goToPage = function (page) {

//    $scope.currentPage = page;
//    $scope.search();
//}

//$scope.pageLinks = function (): number[] {

//    var result = [];
//    for (var i = $scope.currentPage; i < $scope.currentPage + 4; i++) {

//        result.push(i);
//    }

//    return result;
//};



//}