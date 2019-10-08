using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.ComponentModel;

namespace TP_DSCCR.Models.Enums
{
    public static class EnumEx
    {
        public static string GetDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : value.ToString();
        }

        //經由列舉值取得Description之值
        //GetDescription(Family.Brother)

        //經由字串值取得Description之值
        //GetDescription(((Family) Enum.Parse(typeof(Family), "Brother")))

        //經由列舉型別的基底型別數值取得Description之值
        //GetDescription((Family) Enum.ToObject(typeof(Family), 2))
    }

}