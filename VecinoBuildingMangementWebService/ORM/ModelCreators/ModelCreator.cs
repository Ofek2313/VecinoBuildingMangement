using System.Data;
using System.Reflection;
using VecinoBuildingMangement.Models;

namespace VecinoBuildingMangementWebService.ORM.ModelCreators
{
    public class ModelCreator 
    {
        public T CreateModel<T>(IDataReader dataReader ,List<string> extraIgnore = null ) where T : new()
        {
            Type type = typeof(T);
            
            List<string> ignore =  new List<string>{ "HasErrors", "IsValid", "IsValidationEnabled" };
            if (extraIgnore != null)
            {
                ignore.AddRange(extraIgnore);
            }
            PropertyInfo[] properties = type.GetProperties().Where(p => !ignore.Contains(p.Name)).ToArray();
            T t = new T();
            Type propType;
            foreach (PropertyInfo property in properties)
            {
                propType = property.PropertyType;
              

                if(propType.IsEnum)
                {
                    property.SetValue(t, Enum.ToObject(propType, Convert.ToInt32(dataReader[$@"{property.Name}"])));
                }
                else
                {
                    property.SetValue(t, Convert.ChangeType(dataReader[$@"{property.Name}"], propType));
                }
            }
            return t;
        }
    }
}
