
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace WebApp.Helpers
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum UserType
    {
        None, Administrator, Manager, Approval1, HSE, Company, Gate
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum StatusPengajuan
    {
        Baru, Proccess, Complete , Cancel=-1
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum AttackStatus
    {
        Baru, Perpanjangan, Complete
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum StatusPersetujuan
    {
        Proccess, Approved, Complete, Reject, Fixed, Cancel=-1
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum ExpireStatus
    {
        None, WillExpire, Expire
    }

}