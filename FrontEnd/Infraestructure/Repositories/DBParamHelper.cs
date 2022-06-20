using Commons;
using System.ComponentModel;

namespace Frontend.Repositories
{
    public static class DBParamHelper
    {
        public static string BuilderFunction(EnumSchemas squema, [System.Runtime.CompilerServices.CallerMemberName] string? name = null)
        {
            if (!string.IsNullOrEmpty(name))
            {
                return squema switch
                {
                    EnumSchemas.DBO => $"{EnumHelper.GetEnumDescription(EnumSchemas.DBO)}.{name}",
                    EnumSchemas.SETTING => $"{EnumHelper.GetEnumDescription(EnumSchemas.SETTING)}.{name}",
                    EnumSchemas.SECURITY => $"{EnumHelper.GetEnumDescription(EnumSchemas.SECURITY)}.{name}",

                    _ => name,
                };
            }
            else
                return name;
        }

        public enum EnumSchemas
        {
            [Description("dbo")]
            DBO,

            [Description("setting")]
            SETTING,

            [Description("security")]
            SECURITY
        }
    }
}
