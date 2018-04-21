using System.Reflection;

namespace IziCast.Core
{
    public class ReflectionHelper
    {
        public static T GetInternalStaticProperty<T, TPropertyClass>(string propertyName)
        {
            var propertyInfo = typeof(TPropertyClass).GetProperty(propertyName, BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetProperty | BindingFlags.FlattenHierarchy);

            return (T)propertyInfo.GetValue(null);
        }

        public static T GetInternalProperty<T>(object obj, string propertyName)
        {
            var propertyInfo = obj.GetType().GetProperty(propertyName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetProperty);

            return (T)propertyInfo.GetValue(obj);
        }

        public static void SetPrivateField<TObj>(TObj obj, string fieldName, object newValue)
        {
            var fieldInfo = typeof(TObj).GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetField);

            fieldInfo.SetValue(obj, newValue);
        }
    }
}

