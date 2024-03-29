angular.module("auth.service", [])

    .factory("AuthService", AuthService)


    ;




function AuthService($http, $q, StorageService, $state, helperServices, message) {

    var service = {};

    return {
        login: login,
        logOff: logoff,
        userIsLogin: userIsLogin,
        getUser: getUser,
        // userIsLogin: userIsLogin,
        userInRole: userInRole,
        getHeader: getHeader,
        getToken: getToken,
        getUserId: getUserId,
        addProfile : addProfile,
        getProfile : getProfile,
        resetPassword:resetPassword,
        changePassword: changePassword,
        confirmEmail: confirmEmail,
    }

    function login(user) {
        var def = $q.defer();
        $http({
            method: 'POST',
            url: helperServices.url + "api/users/authenticate",
            data: user,
            headers: {
                'Content-Type': 'application/json'
            }
        }).then(res => {
            var user = res.data;
            StorageService.addObject("user", user);
            def.resolve(user);
        }, err => {
            def.reject(err);
            message.error(err.data);
        });
        return def.promise;
    }

    function getHeader() {
        try {
            if (userIsLogin()) {
                return {
                    'Content-Type': 'application/json',
                    'Authorization': 'Bearer ' + getToken()
                }
            }
            throw new Error("Not Found Token");
        } catch {
            return {
                'Content-Type': 'application/json'
            }
        }
    }

    function getHeaderToken(token) {
        return {
            'Content-Type': 'application/json',
            'Authorization': 'Bearer ' + token
        }
    }

    function logoff() {
        StorageService.clear();
        $state.go("login");

    }

    function getUser() {
        if (userIsLogin) {
            var result = StorageService.getObject("user");
            return result;
        }
    }

    function getToken() {
        if (userIsLogin) {
            var result = StorageService.getObject("user");
            return result.token;
        }
    }

    function getUserId() {
        if (userIsLogin) {
            var result = StorageService.getObject("user");
            return result.id;
        }
    }

    function userIsLogin() {
        var result = StorageService.getObject("user");
        if (result) {
            return true;
        }
    }

    function userInRole(role) {
        var result = StorageService.getItem("user");
        if (result && result.roles.find(x => x.name = role)) {

            return true;
        }
    }
    
    function addProfile(profile) {
        StorageService.addObject("profile", profile);
    }

    function getProfile() {
        var result = StorageService.getObject("profile");
        if (result) {
            return result;
        }
    }

    function resetPassword(email) {
        var def = $q.defer();
        $http({
            method: 'GET',
            url: helperServices.url + "api/users/forgotpassword/" + email,
            headers: {
                'Content-Type': 'application/json'
            }
        }).then(res => {
            def.resolve(res.data);
        }, err => {
            $.LoadingOverlay("hide");
            def.reject(err);
            message.error(err.data);
        });
        return def.promise;
    }

    function changePassword(params, token) {
        var def = $q.defer();
        var Auth = getHeaderToken(token)
        $http({
            method: 'PUT',
            url: helperServices.url + "api/users/changepassword/",
            headers: Auth,
            data: params
        }).then(res => {
            def.resolve(res.data);
        }, err => {
            $.LoadingOverlay("hide");
            def.reject(err);
            message.error(err.data);
        });
        return def.promise;
    }
    function confirmEmail(params, token) {
        var def = $q.defer();
        var Auth = getHeaderToken(token)
        $http({
            method: 'PUT',
            url: helperServices.url + "api/users/confirmemail/",
            headers: Auth,
            data: params
        }).then(res => {
            def.resolve(res.data);
        }, err => {
            $.LoadingOverlay("hide");
            def.reject(err);
            message.error(err.data);
        });
        return def.promise;
    }
}