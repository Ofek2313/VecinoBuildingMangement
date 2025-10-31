using System.Data;

namespace VecinoBuildingMangementWebService
{
    public interface IModelCreator<T>
    {
        T CreateModel(IDataReader dataReader);

    }
}
