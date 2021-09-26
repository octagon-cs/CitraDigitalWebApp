angular
    .module('perusahaan.controller', [])
    .controller('companyController', companyController)
    .controller('dashboardController', dashboardController)
    .controller('profilePerusahaanController', profilePerusahaanController)
    .controller('kendaraanController', kendaraanController)
    .controller('pengajuanController', pengajuanController)
    .controller('tambahPengajuanController', tambahPengajuanController)
    .controller('kimsController', kimsController)
    ;

function companyController($scope, ProfilePerusahaanServices, message, $state, AuthService, helperServices, CompanyServices) {
    $scope.hideside = () => {

    }
    $scope.warningkim = 0;
    $scope.warningkendaraan = 0;
    $scope.profile = {};
    if (!AuthService.userIsLogin()) {
        $state.go("login");
    } else {
        $scope.profile = AuthService.getProfile();
        if (!$scope.profile) {
            setTimeout(() => {
                ProfilePerusahaanServices.get().then(x => {
                    // $scope.$emit("SendDown", "true");
                    $scope.profile = x;
                    $scope.profile.logo = $scope.profile.logo;
                    AuthService.addProfile($scope.profile);
                    CompanyServices.getAllKim().then(res => {
                        res.forEach(element => {
                            var carCreated = moment(element.truck.carCreated + "-01-01", 'YYYY-MM-DD');
                            var newDate = moment();
                            var duration = moment.duration(newDate.diff(carCreated));
                            var text = "";
                            if (element.ageOfKIM.months == 0 && element.ageOfKIM.days > 0) {
                                $scope.warningkim += 1;
                            } else if (element.ageOfKIM.months <= 0 && element.ageOfKIM.days <= 0) {
                                $scope.warningkim += 1;
                            }

                            if (element.truck.assDriverAge.years >= 54 && element.truck.assDriverAge.months >= 11 && element.truck.assDriverAge.days >= 1 && element.truck.assDriverAge.days <= 31) {
                                $scope.warningkendaraan += 1;
                            }

                            if (element.truck.driverAge.years >= 54 && element.truck.driverAge.months >= 11 && element.truck.driverAge.days >= 1 && element.truck.driverAge.days <= 31) {
                                $scope.warningkendaraan += 1;
                            }

                            if (element.truck.driverAge.years >= 54 && element.truck.driverAge.months >= 11 && element.truck.driverAge.days >= 1 && element.truck.driverAge.days <= 31) {
                                $scope.warningkendaraan += 1;
                            }

                            if (duration._data.years == 10 && duration._data.months == 11 && (duration._data.days > 1 && duration._data.days < 31)) {
                                $scope.warningkendaraan += 1;
                            } else if (duration._data.years > 10) {
                                $scope.warningkendaraan += 1;
                            }
                        });
                        if ($scope.warningkendaraan > 0 || $scope.warningkim > 0) {
                            message.warning("Terdapat Berkas yang akan expire");
                        }
                    })
                }, (err) => {
                    message.dialogmessage("Mohon isi Profile terlebih dahulu").then(x => {
                        $state.go("profileperusahaan");
                    });
                })
            }, 500);
        } else {
            setTimeout(() => {
                CompanyServices.getAllKim().then(res => {
                    res.forEach(element => {
                        var carCreated = moment(element.truck.carCreated + "-01-01", 'YYYY-MM-DD');
                        var newDate = moment();
                        var duration = moment.duration(newDate.diff(carCreated));
                        var text = "";
                        if (element.ageOfKIM.months == 0 && element.ageOfKIM.days > 0) {
                            $scope.warningkim += 1;
                        } else if (element.ageOfKIM.months <= 0 && element.ageOfKIM.days <= 0) {
                            $scope.warningkim += 1;
                        }

                        if (element.truck.assDriverAge.years >= 54 && element.truck.assDriverAge.months >= 11 && element.truck.assDriverAge.days >= 1 && element.truck.assDriverAge.days <= 31) {
                            $scope.warningkendaraan += 1;
                        }

                        if (element.truck.driverAge.years >= 54 && element.truck.driverAge.months >= 11 && element.truck.driverAge.days >= 1 && element.truck.driverAge.days <= 31) {
                            $scope.warningkendaraan += 1;
                        }

                        if (element.truck.driverAge.years >= 54 && element.truck.driverAge.months >= 11 && element.truck.driverAge.days >= 1 && element.truck.driverAge.days <= 31) {
                            $scope.warningkendaraan += 1;
                        }

                        if (duration._data.years == 10 && duration._data.months == 11 && (duration._data.days > 1 && duration._data.days < 31)) {
                            $scope.warningkendaraan += 1;
                        } else if (duration._data.years > 10) {
                            $scope.warningkendaraan += 1;
                        }
                    });
                    if ($scope.warningkendaraan > 0 || $scope.warningkim > 0) {
                        message.warning("Terdapat Berkas yang akan expire");
                    }
                })
            }, 300);
        }
        // $scope.$emit("SendUp", $scope.profile);
    };
    $scope.logout = () => {
        AuthService.logOff();
    }

    $scope.reset = (email) => {
        message.dialog("Yakin ingin mereset password?", "YA").then(x => {
            $.LoadingOverlay("show");
            AuthService.resetPassword(email).then(res => {
                $.LoadingOverlay("hide");
                message.dialog("Periksa Email anda!!", "OK").then(x => {
                    AuthService.logOff();
                })
            })
        })
    }

}

function dashboardController($scope, CompanyServices, AuthService, helperServices) {
    $scope.datas = [];
    $scope.Title = 'Daftar User';
    // $scope.$on("SendUp", function (evt, data) {
    //     $scope.title = data.title;
    //     $scope.header = data.header;
    //     $scope.breadcrumb = data.breadcrumb;
    //     console.log(data);
    // });
    $scope.profile = AuthService.getProfile();

    CompanyServices.get().then(dash => {
        $scope.data = dash;
        console.log($scope.data);
    })
}

function profilePerusahaanController($scope, helperServices, message, AuthService, StorageService, ProfilePerusahaanServices) {
    $scope.url = helperServices.url;
    $scope.statusProfile = false;
    $scope.test;
    $scope.model = {};
    ProfilePerusahaanServices.get().then(x => {
        $scope.statusProfile = true;
        $scope.model = x;
    }, err => {
        $scope.statusProfile = false;
    })
    $scope.simpan = () => {
        // var photo = {};
        // var ext = $scope.model.logoData.filename.split(".");
        // photo.fileExtention = ext[1];
        // photo.fileType = "Photo";
        // photo.fileName = $scope.model.logoData.filename;
        // photo.data = $scope.model.logoData.data;
        // $scope.model.logoData = photo;
        if ($scope.model.id) {
            ProfilePerusahaanServices.put($scope.model).then(x => {
                message.dialogconfirm("Profile Perusahaan telah di perbaharui!!! Silahkan Login Ulang untuk melanjutkan", "OK").then(x => {
                    AuthService.logOff()
                })
                // message.info("Berhasil");
                // $scope.statusProfile = true;
                // $scope.model.logo = x.logo;
                // StorageService.remove("profile");
                // AuthService.addProfile(x);
            })
        } else {
            ProfilePerusahaanServices.post($scope.model).then(x => {
                message.dialogconfirm("Profile Perusahaan telah dibuat!!! Silahkan Login Ulang Untuk Melanjutkan", "OK").then(x => {
                    AuthService.logOff()
                    message.info("Berhasil Menyimpan");
                })
            })
        }
        console.log($scope.model);
    }
    $scope.edit = () => {
        $scope.statusProfile = false;
    }
    $scope.showDataLogo = (item) => {
        console.log(item);
    }
}


function kendaraanController($scope, KendaraanServices, helperServices, message) {
    $scope.url = helperServices.url;
    $scope.datas = [];
    $scope.model = {};
    $scope.model.driverLicense = {};
    $scope.model.assdriverLicense = {};
    $scope.model.vehicleRegistration = {};
    $scope.model.keurDLLAJR = {};
    $scope.model.assdriverIDCard = {};
    $scope.model.driverIDCard = {};
    $scope.kims = [];
    KendaraanServices.get().then(x => {
        x.forEach(element => {
            element.driverDateOfBirth = new Date(element.driverDateOfBirth);
            element.assDriverDateOfBirth = new Date(element.assDriverDateOfBirth);
            if (element.kim) {
                var carCreated = moment(element.carCreated + "-01-01", 'YYYY-MM-DD');
                var newDate = moment();
                var duration = moment.duration(newDate.diff(carCreated));
                var text = "";

                if (element.assDriverAge.years >= 54 && element.assDriverAge.months >= 11 && element.assDriverAge.days >= 1 && element.assDriverAge.days <= 31) {
                    element.color = 'yellow';
                    text += "- Umur Assistant Driver akan melewati batas<br>";
                }

                if (element.driverAge.years >= 54 && element.driverAge.months >= 11 && element.driverAge.days >= 1 && element.driverAge.days <= 31) {
                    element.color = 'yellow';
                    text += "- Umur Driver akan melewati batas<br>";
                }
                
                if (duration._data.years == 10 && duration._data.months == 11 && (duration._data.days > 1 && duration._data.days < 31)) {
                    element.color = 'yellow';
                    text += "- Umur kendaraan anda akan melewati batas<br>";
                } else if (duration._data.years > 10) {
                    element.color = 'red';
                    text += "- Umur kendaraan telah melewati batas<br>";
                }
                element.title = text;
            }
            $scope.datas.push(element);
        });
        console.log(x);
    })
    $scope.setFile = (item) => {
        console.log(item);
    }
    $scope.simpan = () => {
        if ($scope.model.id) {
            $.LoadingOverlay("show");
            KendaraanServices.put($scope.model).then(x => {
                $.LoadingOverlay("hide");
                message.dialogmessage("Data berhasil di tambahkan", "OK").then(x => {
                    $('#myTab li:first-child a').tab('show')
                    document.location.reload();
                });

                // $scope.model = {};
                // $scope.model.driverLicense = {};
                // $scope.model.assdriverLicense = {};
                // $scope.model.vehicleRegistration = {};
                // $scope.model.keurDLLAJR = {};
                // $scope.model.assdriverIDCard = {};
                // $scope.model.driverIDCard = {};
            })
        } else {
            $.LoadingOverlay("show");
            KendaraanServices.post($scope.model).then(x => {
                message.dialogmessage("Data berhasil di tambahkan").then(x => {
                    $.LoadingOverlay("hide");
                    $('#myTab li:first-child a').tab('show')
                    document.location.reload()
                });
                // $scope.model = {};
                // $scope.model.driverLicense = {};
                // $scope.model.assdriverLicense = {};
                // $scope.model.vehicleRegistration = {};
                // $scope.model.keurDLLAJR = {};
                // $scope.model.assdriverIDCard = {};
                // $scope.model.driverIDCard = {};
            })
        }
    }
    $scope.edit = (item) => {
        $('#myTab li:last-child a').tab('show')
        $scope.model = {};
        $scope.model = item
        $scope.model.driverLicense.berlaku = new Date($scope.model.driverLicense.berlaku);
        $scope.model.driverLicense.hingga = new Date($scope.model.driverLicense.hingga);
        $scope.model.assdriverLicense.berlaku = new Date($scope.model.assdriverLicense.berlaku);
        $scope.model.assdriverLicense.hingga = new Date($scope.model.assdriverLicense.hingga);
        $scope.model.vehicleRegistration.berlaku = new Date($scope.model.vehicleRegistration.berlaku);
        $scope.model.vehicleRegistration.hingga = new Date($scope.model.vehicleRegistration.hingga);
        $scope.model.keurDLLAJR.berlaku = new Date($scope.model.keurDLLAJR.berlaku);
        $scope.model.keurDLLAJR.hingga = new Date($scope.model.keurDLLAJR.hingga);
        $scope.model.assdriverIDCard.berlaku = new Date($scope.model.assdriverIDCard.berlaku);
        $scope.model.assdriverIDCard.hingga = new Date($scope.model.assdriverIDCard.hingga);
        $scope.model.driverIDCard.berlaku = new Date($scope.model.driverIDCard.berlaku);
        $scope.model.driverIDCard.hingga = new Date($scope.model.driverIDCard.hingga);
        $scope.model.carCreated = $scope.model.carCreated.toString();
        console.log($scope.model);
    }
    $scope.showKims = (item) => {
        $scope.kims = item;
        $("#dataKims").modal('show');
    }

}

function pengajuanController($scope, PengajuanServices, message) {
    $scope.datas = [];

    PengajuanServices.get().then(x => {
        $scope.datas = x;
        console.log(x);
    })
    $scope.deleteItem = (item) => {
        PengajuanServices.deleted(item).then(result => {
            message.info('Berhasil');
        })
    }
    $scope.checkStatus = (item) => {
        var status = true;
        item.forEach(element => {
            status = element.status == 'Reject' ? false : true;
        });
        return status;
    }

}

function tambahPengajuanController($scope, KendaraanServices, helperServices, PengajuanServices, message, $state, $stateParams, ListPemeriksaanServices, approvalServices) {
    $scope.url = helperServices.url;
    $scope.jenisPengajuan = helperServices.jenisPengajuan;
    $scope.id = $stateParams.id;
    $scope.kendaraan = [];
    $scope.model = {};
    $scope.model.items = []
    $scope.listPemeriksaan = [];
    $scope.truck = {};
    $.LoadingOverlay("show");
    KendaraanServices.get().then(x => {
        $scope.$applyAsync(x => {
            $(".truck").select2();
        })
        $scope.kendaraan = x;
        // var test = atob(x[0].fileAssDriverLicense.data);
        // console.log(test);
        $scope.model.company = { id: $scope.kendaraan[0].companyId };
        PengajuanServices.get().then(itemPengajuan => {
            if ($stateParams.id == null) {
                var d = new Date();
                if (itemPengajuan.length == 0) {
                    $scope.model.letterNumber = "1/" + $scope.kendaraan[0].company.name.toUpperCase().replace(/\s/g, '') + "/" + helperServices.toRoman(d.getMonth()) + "/" + d.getFullYear();
                } else {
                    var itemnomor = itemPengajuan[itemPengajuan.length - 1];
                    var arraynomor = itemnomor.letterNumber.split('/');
                    $scope.model.letterNumber = (parseInt(arraynomor[0]) + 1) + "/" + $scope.kendaraan[0].company.name.toUpperCase().replace(/\s/g, '') + "/" + helperServices.toRoman(d.getMonth()) + "/" + d.getFullYear()
                    console.log($scope.model);
                }
            } else {
                $scope.model = itemPengajuan.find(x => x.id == $stateParams.id);
                ListPemeriksaanServices.get().then(pemeriksaan => {
                    $scope.listPemeriksaan = pemeriksaan;
                    console.log($scope.model);
                })
                // console.log($scope.model);
            }
            $.LoadingOverlay("hide");
        })
    })
    $scope.setItem = (item) => {
        if ($stateParams.id == null) {
            $scope.$applyAsync(x => {
                var itemPengajuan = { truck: item, }
                $scope.model.items.push(itemPengajuan);
                console.log($scope.model);
            })
        } else {
            $scope.$applyAsync(x => {
                item.attackStatus = item.attackStatus;
                item.pengajuanId;
                var truck = {};
                truck.truck = item;
                $scope.model.items.push(angular.copy(truck));
                console.log($scope.model);
            })
        }
    }
    $scope.deleteItem = (item) => {
        var index = $scope.model.items.indexOf(item);
        $scope.model.items.splice(index, 1);
        console.log($scope.model);
    }
    $scope.simpan = () => {
        console.log($scope.model);
        message.dialogmessage("Pastikan berkas kendaraan anda telah lengkap. Yakin mengajukan berkas ??", "Ya", "Tidak").then(x => {
            $.LoadingOverlay("show");
            if ($scope.model.id) {
                PengajuanServices.put($scope.model).then(x => {
                    message.info('Berhasil');
                    $state.go("pengajuan");
                    $.LoadingOverlay("hide");
                })
            } else {
                PengajuanServices.post($scope.model).then(x => {
                    message.info('Berhasil');
                    $state.go("pengajuan");
                    $.LoadingOverlay("hide");
                })
            }
        })
    }
    $scope.detailPemeriksaan = (item) => {
        console.log(item);
        $scope.truck = item;
        $scope.listPemeriksaan.forEach(elementPemeriksaan => {
            elementPemeriksaan.hasilPemeriksaan = item.hasilPemeriksaan.filter(x => x.itemPemeriksaan.pemeriksaan.id == elementPemeriksaan.id);
        });
        console.log($scope.listPemeriksaan);
        $scope.pemeriksaan1 = item.persetujuans.find(x => x.approvedBy == 'Administrator');
        $scope.pemeriksaan2 = item.persetujuans.find(x => x.approvedBy == 'Approval1');
        $scope.pemeriksaan3 = item.persetujuans.find(x => x.approvedBy == 'HSE');
        $scope.pemeriksaan4 = item.persetujuans.find(x => x.approvedBy == 'Manager');
        $("#showPemeriksaan").modal('show');
    }
    $scope.pengajuan = (item) => {
        $.LoadingOverlay("show");
        message.dialogmessage("Pastikan anda telah melengkapi berkas atau perlengkapan yang tidak 'Valid', Yakin mengajukan ulang berkas ??", "Ya", "Tidak").then(x => {
            approvalServices.post(item).then(res => {
                message.info("Berhasil");
                $state.go("pengajuan");
                $.LoadingOverlay("hide");
            })
        })
    }
}
function kimsController($scope, helperServices, CompanyServices) {
    $scope.datas = [];
    CompanyServices.getAllKim().then(res => {
        res.forEach(element => {
            var text = "";
            if (element.ageOfKIM.months == 0 && element.ageOfKIM.days > 0) {
                element.color = 'yellow';
                text += "KIM Anda akan segera expire dalam " + element.ageOfKIM.days + " hari lagi<br>";
            } else if (element.ageOfKIM.months <= 0 && element.ageOfKIM.days <= 0) {
                element.color = 'red';
                text += "KIM telah expire";
            }
            element.title = text;
        });
        console.log(res);
        $scope.datas = res;
        $.LoadingOverlay("hide");
    })
}