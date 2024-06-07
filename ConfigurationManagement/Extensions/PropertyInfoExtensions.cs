using System;
using System.Linq;
using System.Reflection;

namespace ConfigurationManagement.Extensions
{
    public static class PropertyInfoExtensions
    {
        public static bool IsOfCompliantType(this PropertyInfo propertyInfo, Type[] applicableTypes)
        {
            var propertyType = propertyInfo.PropertyType;
            return 
                applicableTypes is not null && applicableTypes.Any(type => type == propertyType);
        }
    }
}
