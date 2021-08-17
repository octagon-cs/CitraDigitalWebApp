angular.module("auth.controller", [])

  .controller("LoginController", LoginController)
  .controller("AccountController", AccountController)

function LoginController($scope, $state, AuthService) {
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
}

function AccountController($scope, $state, AuthService) {
 if(AuthService.userIsLogin())
  {
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

