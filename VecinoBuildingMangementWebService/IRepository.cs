namespace VecinoBuildingMangementWebService
{
    public interface IRepository<T>
    {
        bool Create();
        bool Update();
        bool Delete();
        
        List<T> GetAll();

        T GetById(string id);

    }
}
