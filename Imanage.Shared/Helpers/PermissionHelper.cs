using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Imanage.Shared.Helpers
{
    public static class PermissionHelper
    {
        public static string GetPermissionDescription(this Permission value)
        {
            Type type = value.GetType();

            string name = Enum.GetName(type, value);
            if (name != null)
            {
                FieldInfo field = type.GetField(name);
                if (field != null)
                {
                    DescriptionAttribute attr = Attribute.GetCustomAttribute(field,
                             typeof(DescriptionAttribute)) as DescriptionAttribute;

                    if (attr != null)
                    {
                        return attr.Description;
                    }
                }
            }

            return null;
        }

        public static string GetPermissionCategory(this Permission value)
        {
            Type type = value.GetType();
            string name = Enum.GetName(type, value);

            if (name != null)
            {
                FieldInfo field = type.GetField(name);
                if (field != null)
                {
                    CategoryAttribute attr = Attribute.GetCustomAttribute(field,
                             typeof(CategoryAttribute)) as CategoryAttribute;

                    if (attr != null)
                    {
                        return attr.Category;
                    }
                }
            }

            return null;
        }
    }
}
