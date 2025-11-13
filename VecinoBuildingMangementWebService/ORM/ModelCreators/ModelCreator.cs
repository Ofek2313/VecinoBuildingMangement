using System.Data;
using System.Reflection;
using VecinoBuildingMangement.Models;

namespace VecinoBuildingMangementWebService.ORM.ModelCreators
{
    public class ModelCreator<T> : IModelCreator<T> where T : new()
    {
        public T CreateModel(IDataReader dataReader)
        {
            Type type = typeof(T);
            var ignore = new[] { "HasErrors", "IsValid" };
            PropertyInfo[] properties = type.GetProperties().Where(p => !ignore.Contains(p.Name)).ToArray();
            T t = new T();
            Type propType;
            foreach (PropertyInfo property in properties)
            {
                propType = property.PropertyType;
                property.SetValue(t, Convert.ChangeType(dataReader[$@"{property.Name}"],propType));
            }
            return t;
        }
    }
}
