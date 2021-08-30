angular.module("helper.service", [])
    .factory("helperServices", helperServices);


function helperServices() {
    var service = {};
    service.url = "";
    //service.url = "http://192.168.1.2/";
    service.jenisPengajuan = ['Baru', 'Perpanjangan'];
    // service.roles = [{role:'Company', nama: 'Company'}, {role:'Approval1', nama: 'Checker'}, {role:'HSE', nama: 'HSSE'}, {role:'Manager', nama:'Manager'}, {role:'Gate', nama: 'Gate'}];
    service.roles = [{role:'Company', nama: 'Company'}, {role:'Approval1', nama: 'Checker'}, {role:'HSE', nama: 'HSSE'}, {role:'Manager', nama:'Manager'}];
    function toRoman(num) {
        if (typeof num !== 'number')
            return false;

        var digits = String(+num).split(""),
            key = ["", "C", "CC", "CCC", "CD", "D", "DC", "DCC", "DCCC", "CM",
                "", "X", "XX", "XXX", "XL", "L", "LX", "LXX", "LXXX", "XC",
                "", "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX"],
            roman_num = "",
            i = 3;
        while (i--)
            roman_num = (key[+digits.pop() + (i * 10)] || "") + roman_num;
        return Array(+digits.join("") + 1).join("M") + roman_num;
    }
    return { url: service.url, base: service.base, jenisPengajuan: service.jenisPengajuan, roles: service.roles, toRoman: toRoman };
}