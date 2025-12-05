using Microsoft.AspNetCore.Hosting.Server;
using System.Data;
using System.Data.OleDb;
using static System.Runtime.InteropServices.JavaScript.JSType;


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

            oleDbConnection.ConnectionString =
            this.oleDbConnection.ConnectionString = $@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={Directory.GetCurrentDirectory()}\App_Data\\BuildingMangement.accdb;Persist Security Info=True";


            // Set the Connection String statically
            //this.oleDbConnection.ConnectionString =
            //    $@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\USER\source\repos\VecinoBuildingMangement\VecinoBuildingMangementWebService\App_Data\BuildingMangement.accdb;Persist Security Info=False";
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
            this.dbCommand.Transaction = this.dbTransaction;
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
        public void ClearParameters()
        {
            this.dbCommand.Parameters.Clear();
        }
        public string GetLastId(string sql)
        {
            this.dbCommand.CommandText = sql;
            return this.dbCommand.ExecuteScalar().ToString();
        }
       
    }
}
