angular.module("auth.controller", [])

  .controller("LoginController", LoginController)
  .controller("AccountController", AccountController)
  .controller("registerController", registerController)
  .controller("resetController", resetController);

function LoginController($scope, $state, AuthService, message) {
  $scope.model = {};
  $scope.login = (user) => {
    AuthService.login(user).then(x => {
      if (x.roles[0] == "Company") {
        $state.go("dashboard");
      } else if (x.roles[0] == "Approval1") {
        $state.go("approval");
      } else {
        $state.go("home");
      }
    })
  }
  $scope.reset = (email) => {
    message.dialog("Yakin ingin mereset password?", "YA").then(x => {
      AuthService.resetPassword(email).then(res => {
        message.info("Periksa Email anda!!", "OK");
      })
    })
  }
}

function AccountController($scope, $state, AuthService) {
  if (AuthService.userIsLogin()) {
    var user = AuthService.getUser();
    if (user.roles[0] == "Company") {
      $state.go("dashboard");
    } else if (user.roles[0] == "Approval1") {
      $state.go("approval");
    } else {
      $state.go("home");
    }
  }
}

function registerController($scope, $state, AuthService, message) {

}

function resetController($scope, $state, AuthService, $stateParams, message) {
  $scope.model = {}
  console.log($stateParams);
  $scope.error = false;
  $scope.reset = () => {
    $.LoadingOverlay("show");
    AuthService.changePassword($scope.model, $stateParams.token).then(res=>{
      $.LoadingOverlay("hide");
      message.dialog("Reset password berhasil, silahkan login kembali").then(x=>{
        $state.go("login");
      })
    })    
  }
  $scope.check = ()=>{
    if($scope.model.Password!==$scope.model.ConfirmPassword){
      $scope.error = true;
    }else{
      $scope.error = false;
    }
  }
  
}

