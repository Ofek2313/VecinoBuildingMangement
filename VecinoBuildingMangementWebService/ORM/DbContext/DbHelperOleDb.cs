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

            
            this.oleDbConnection.ConnectionString = $@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={Directory.GetCurrentDirectory()}\App_Data\\BuildingMangement.accdb;Persist Security Info=True";


      
            this.dbCommand = new OleDbCommand();
            this.dbCommand.Connection = this.oleDbConnection;
        }

        public void CloseConnection() // Closes a conncetion to the database.
        {
           
           this.oleDbConnection.Close();
        }

        public void Commit() //Commit changes that were made while the transcation was open.
        {
            this.dbTransaction.Commit();
        }

        //CRUD Opeartions
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
        

        public void OpenConnection() // Open a conncetion to the database.
        {
       
            this.oleDbConnection.Open();
        }

        // Open Transaction for treating multiple SQL actions as a single unit, that ensures that the database will be in sync.
        public void OpenTransaction()
        {
            this.dbTransaction = this.oleDbConnection.BeginTransaction();
            this.dbCommand.Transaction = this.dbTransaction;
        }

        public void RollBack() // Cancel the operations made while the transaction was open.
        {
            this.dbTransaction.Rollback();
        }

       
        public void AddParameter(string name, object value) // Prevent Sql Injection
        {
            this.dbCommand.Parameters.Add(new OleDbParameter(name, value));
        }
        public void ClearParameters()
        {
            this.dbCommand.Parameters.Clear();
        }
        public string GetLastId(string sql) // used for getting the id of the last inserted row in the database.
        {
            this.dbCommand.CommandText = sql;
            return this.dbCommand.ExecuteScalar().ToString();
        }
        public object ExecuteScalar(string sql) // Function that returns the first column of the first row of the records. 
        {
            this.dbCommand.CommandText = sql;

            object result = this.dbCommand.ExecuteScalar();

            this.dbCommand.Parameters.Clear();

            return result;

        }
    }
}
