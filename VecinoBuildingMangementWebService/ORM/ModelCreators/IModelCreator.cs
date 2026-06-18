using System.Data;

namespace VecinoBuildingMangementWebService
{
    public interface IModelCreator
    {
        T CreateModel<T>(IDataReader dataReader, List<string> extraIgnore = null) where T : new();
    }
}
