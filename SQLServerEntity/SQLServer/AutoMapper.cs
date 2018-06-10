using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace Framework.Automap.SQLServer
{
    public class AutoMapper<T>
    {
        public static T Map(DataRow dr)
        {
            var result = Activator.CreateInstance<T>();

            foreach (var item in result.GetType().GetProperties())
            {
                PropertyInfo properties = result.GetType().GetProperty(item.Name);

                if (dr.Table.Columns.Contains(item.Name) && dr[item.Name] != DBNull.Value)
                {
                    properties.SetValue(result, dr[item.Name], null);
                }

            }

            return result;
        }

        public static List<T> Map(DataTable tbl)
        {
            var listResult = new List<T>();

            if (tbl.Rows.Count > 0)
            {
                foreach (DataRow dr in tbl.Rows)
                {
                    var item = Activator.CreateInstance<T>();

                    item = Map(dr);

                    listResult.Add(item);
                }
            }
            return listResult;
        }

        public static T MapToObject(Object X)
        {
            var resutlReturn = Activator.CreateInstance<T>();

            foreach (var item in X.GetType().GetProperties())
            {
                PropertyInfo propertieX = X.GetType().GetProperty(item.Name);


                var propertieResult = resutlReturn.GetType().GetProperty(item.Name);

                if (propertieResult != null)
                {
                    propertieResult.SetValue(resutlReturn, propertieX.GetValue(X, null), null);
                }
            }

            return resutlReturn;
        }
    }
}
