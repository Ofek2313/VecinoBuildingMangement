namespace VecinoBuildingMangementWebService
{
    public interface IRepository<T>  // All the CRUD actions
    {
        bool Create(T model);
        bool Update(T model);
        bool Delete(string id);
        
        List<T> GetAll();

        T GetById(string id);

    }
}
