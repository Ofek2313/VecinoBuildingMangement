using System.Data;
using System.Reflection;
using VecinoBuildingMangementWebService.ORM.ModelCreators;

namespace VecinoBuildingMangementWebService
{
    public class GenericRepository<T> :IRepository<T> where T : class, new()
    {

        protected DbHelperOleDb dbHelperOleDb;

        public GenericRepository(DbHelperOleDb dbHelperOleDb)
        {
            this.dbHelperOleDb = dbHelperOleDb;
        }
        protected ModelCreator<T> modelCreator;
        protected ModelCreator<T> ModelCreator
        {
            get
            {
                if (modelCreator == null)
                    modelCreator = new ModelCreator<T>();
                return modelCreator;
            }
        }




        public bool Create(T model)
        {
            string sql = "";
            Type type = typeof(T);
            var ignore = new[] { "HasErrors", "IsValid" };
            PropertyInfo[] properties = type.GetProperties().Skip(1).Where(p => !ignore.Contains(p.Name)).ToArray();
            string prop = string.Join(",", properties.Select(p => p.Name));
            string val = string.Join(",", properties.Select(p => "@" + p.Name));
            sql = @$"Insert Into {type.Name}({prop})
                            Values({val})";
            foreach (PropertyInfo property in properties)
            {
                dbHelperOleDb.AddParameter(@$"@{property.Name}", property.GetValue(model).ToString());
            }
            Console.WriteLine(sql);
            return this.dbHelperOleDb.Insert(sql) > 0;

        }

        public bool Delete(string id)
        {
            Type type = typeof(T);
            var ignore = new[] { "HasErrors", "IsValid" };
            PropertyInfo[] properties = type.GetProperties().Where(p => !ignore.Contains(p.Name)).ToArray();
            string sql = $@"Delete from {type.Name} where {properties[0].Name}=@{properties[0].Name}";
            this.dbHelperOleDb.AddParameter($@"@{properties[0].Name}", id);
            return this.dbHelperOleDb.Delete(sql) > 0;
        }


        public List<T> GetAll()
        {
            Type type = typeof(T);

            string sql = $"Select * From {type.Name}";

            List<T> list = new List<T>();
            using (IDataReader reader = this.dbHelperOleDb.Select(sql))
            {
                while (reader.Read())
                {

                    list.Add(this.ModelCreator.CreateModel(reader));

                }
            }

            return list;
        }

        public T GetById(string id)
        {
            Type type = typeof(T);
            var ignore = new[] { "HasErrors", "IsValid" };
            PropertyInfo[] properties = type.GetProperties().Where(p => !ignore.Contains(p.Name)).ToArray();
            string sql = @$"Select * From {type.Name} Where {properties[0].Name}=@{properties[0].Name}";
            dbHelperOleDb.AddParameter(@$"@{properties[0].Name}", id);

            using (IDataReader dataReader = this.dbHelperOleDb.Select(sql))
            {
                dataReader.Read();
                return this.ModelCreator.CreateModel(dataReader);
            }

        }

        public bool Update(T model)
        {
            Type type = typeof(T);
            var ignore = new[] { "HasErrors", "IsValid" };
            PropertyInfo[] properties = type.GetProperties().Skip(1).Where(p => !ignore.Contains(p.Name)).ToArray();
            string sql = $@"Update {type.Name} set ";
            string val = string.Join(", ", properties.Select(p => $@"{p.Name} = @{p.Name}"));

            PropertyInfo id = type.GetProperties().Where(p => !ignore.Contains(p.Name)).ToArray()[0];
            sql += val;
            sql += @$"WHERE {id.Name} = @{id.Name}";
            this.dbHelperOleDb.AddParameter($@"@{id.Name}", id.GetValue(model).ToString());

            foreach (PropertyInfo property in properties)
            {
                dbHelperOleDb.AddParameter(@$"@{property.Name}", property.GetValue(model).ToString());
            }
            Console.WriteLine(sql);
            return this.dbHelperOleDb.Update(sql) > 0;
        }
    }
}

