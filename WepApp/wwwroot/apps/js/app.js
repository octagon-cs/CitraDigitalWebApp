angular.module("app", [
    "app.router",
    "datatables",
    "swangular",
    "message.service",

    "auth.service",
    "storage.services",
    "helper.service",

    "app.conponent",

    "auth.controller",
    "admin.controller",
    "admin.service",
    "perusahaan.controller",
    "perusahaan.service",
    "approval.controller",
    "approval.service",
    "naif.base64",
    "datatables",
    "ui.select2",
    "ngLocale",
    "ui.utils.masks",
    "720kb.datepicker"


])
    .controller("homeController", homeController)
    .directive("compareTo", compareTo);


function homeController($scope, AuthService) {
    $scope.logOff = function () {
        AuthService.logOff();
    }
}

function compareTo() {
    return {
        require: "ngModel",
        scope:
        {
            confirmPassword: "=compareTo"
        },
        link: function (scope, element, attributes, modelVal) {
            modelVal.$validators.compareTo = function (val) {
                return val == scope.confirmPassword;
            };
            scope.$watch("confirmPassword", function () {
                modelVal.$validate();
            });
        }
    };
};