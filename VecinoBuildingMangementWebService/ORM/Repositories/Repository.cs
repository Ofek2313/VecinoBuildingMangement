
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
       
        public Repository()
        {
            this.dbHelperOleDb = new DbHelperOleDb();
            this.modelCreators = new ModelCreators();
        }
    }
}
