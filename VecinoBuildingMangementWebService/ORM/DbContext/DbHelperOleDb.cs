using System.Data;
using System.Data.OleDb;


namespace VecinoBuildingMangementWebService
{
  
    public class DbHelperOleDb : IDbHelper
    {
        //יוצר קשר עם מוסד הנתונים
        OleDbConnection oleDbConnection;

        //אחראי לשלוח פקודות למוסד נתונים ומחזיר תשובה ממוסד נתונים
        OleDbCommand dbCommand;

        OleDbTransaction dbTransaction;

        public DbHelperOleDb()
        {
            this.oleDbConnection = new OleDbConnection();
            this.oleDbConnection.ConnectionString = $@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\Ofek\source\repos\VecinoBuildingMangement\VecinoBuildingMangementWebService\App_Data\BuildingMangement.accdb";


            //this.oleDbConnection.ConnectionString = $@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={Directory.GetCurrentDirectory()}\App_Data\\BuildingMangement.accdb;Persist Security Info=True";
            this.dbCommand = new OleDbCommand();
            this.dbCommand.Connection = this.oleDbConnection;
        }

        public void CloseConnection()
        {
           
           this.oleDbConnection.Close();
        }

        public void Commit()
        {
            this.dbTransaction.Commit();
        }

        public int Delete(string sql)
        {
            this.dbCommand.CommandText = sql;
            int records = this.dbCommand.ExecuteNonQuery();
            this.dbCommand.Parameters.Clear();
            return records;
        }

        public int Insert(string sql)
        {
            this.dbCommand.CommandText = sql;
            int records = this.dbCommand.ExecuteNonQuery();
            this.dbCommand.Parameters.Clear();
            return records;
        }

        public void OpenConnection()
        {
       
            this.oleDbConnection.Open();
        }

        public void OpenTransaction()
        {
            this.dbTransaction = this.oleDbConnection.BeginTransaction();
        }

        public void RollBack()
        {
            this.dbTransaction.Rollback();
        }

        public IDataReader Select(string sql)
        {
            this.dbCommand.CommandText = sql;
            IDataReader dataReader = this.dbCommand.ExecuteReader();
            this.dbCommand.Parameters.Clear();
            return dataReader;
        }

        public int Update(string sql)
        {
            this.dbCommand.CommandText = sql;
            int records = this.dbCommand.ExecuteNonQuery();
            this.dbCommand.Parameters.Clear();
            return records;
        }
        public void AddParameter(string name, object value)
        {
            this.dbCommand.Parameters.Add(new OleDbParameter(name, value));
        }
    }
}
