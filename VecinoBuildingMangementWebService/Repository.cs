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
