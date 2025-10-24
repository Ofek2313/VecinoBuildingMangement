using System.Data;

namespace VecinoBuildingMangementWebService
{
    public interface IDbHelper
    {
        void OpenConnection();

        void CloseConnection();

        // dataReader - הוא אוביקט של recordset
        IDataReader Select(string sql);

        //CRUD
        int Update(string sql);

        int Delete(string sql);

        int Insert(string sql);

        void OpenTransaction();

        void Commit();

        void RollBack();



    }
}
