
using System.Data;
using System.Reflection;
using VecinoBuildingMangement.Models;
using VecinoBuildingMangementWebService.ORM.ModelCreators;

namespace VecinoBuildingMangementWebService
{
    public class Repository 
    {
        protected DbHelperOleDb dbHelperOleDb;
        protected ModelCreators modelCreators;
       
        public Repository(DbHelperOleDb dbHelperOleDb, ModelCreators modelCreators)
        {
            this.dbHelperOleDb = dbHelperOleDb;
            this.modelCreators = modelCreators;
        }
    }
}
