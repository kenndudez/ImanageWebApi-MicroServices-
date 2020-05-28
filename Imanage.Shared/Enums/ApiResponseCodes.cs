using System;
using System.ComponentModel;
using System.Reflection;

namespace Imanage.Shared.Enums
{
    public enum ApiResponseCodes
    {
        [Description("AN ERROR OCURRED")]
        EXCEPTION = -5,
        [Description("UNAUTHORIZED ACCESS")]
        UNAUTHORIZED = -4,
        [Description("NOT FOUND")]
        NOT_FOUND = -3,
        [Description("BAD REQUEST")]
        INVALID_REQUEST = -2,
        [Description("SERVER ERROR OCCURED")]
        ERROR = -1,
        [Description("FAIL")]
        FAIL = 2,
        [Description("SUCCESS")]
        OK = 1,
    }

    //public static class ResponseCodeHelper
    //{

    //    public static string GetDescription(this Enum value)
    //    {
    //        Type type = value.GetType();
    //        string name = Enum.GetName(type, value);
    //        if (name != null)
    //        {
    //            FieldInfo field = type.GetField(name);
    //            if (field != null)
    //            {
    //                DescriptionAttribute attr =
    //                    Attribute.GetCustomAttribute(field,
    //                        typeof(DescriptionAttribute)) as DescriptionAttribute;
    //                if (attr != null)
    //                {
    //                    return attr.Description;
    //                }
    //            }
    //        }
    //        return string.Empty;
    //    }
    //}
}