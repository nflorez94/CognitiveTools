using System.ComponentModel;

namespace Data
{
    public enum EnumSpeechToTextRequestParameters
    {
        [Description("@RequestId")]
        RequestId,

        [Description("@UserId")]
        UserId,

        [Description("@RequestDate")]
        RequestDate,

        [Description("@RequestedFromCountry")]
        RequestedFromCountry,

        [Description("@RequestFile")]
        RequestFile,

        [Description("@Extension")]
        Extension,

        [Description("@StateId")]
        StateId,

        [Description("@FileSize")]
        FileSize,

        [Description("@Response")]
        Response,

        [Description("@ResponseSize")]
        ResponseSize,

        [Description("@AudioLanguaje")]
        AudioLanguaje,
    }
}