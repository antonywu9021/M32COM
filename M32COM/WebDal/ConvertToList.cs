using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Reflection;
using System.Globalization;
using System.Data.SqlClient;
using System.Web;
using System.Text.RegularExpressions;

using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

namespace System
{
    public class ConverttoList<T> where T : new()
    {
        
        public static List<T> DataTableToListObject(DataTable tab)    
        {
            List<T> List = new List<T>();
            try
            {
                if (tab.Rows.Count > 0)
                {
                    for (int i = 0; i < tab.Rows.Count; ++i)
                    {
                        T t = new T();
                        PropertyInfo[] info = t.GetType().GetProperties();
                        foreach (PropertyInfo pp in info)
                        {
                            string Name = pp.Name;
                            if (tab.Columns.Contains(Name))
                            {
                                string mm = tab.Rows[i][Name].ToString();
                                if (tab.Rows[i][Name].ToString() != "")
                                {
                                    if (pp.PropertyType == typeof(DateTime) || pp.PropertyType == typeof(DateTime))
                                    {
                                        DateTime date = DateTime.MaxValue;
                                        DateTime.TryParse(tab.Rows[i][pp.Name].ToString(),
                                            CultureInfo.CurrentCulture, DateTimeStyles.None, out date);

                                        pp.SetValue(t, date, null);
                                    }
                                    else
                                    {
                                        pp.SetValue(t, tab.Rows[i][pp.Name], null);
                                    }
                                }
                            }
                        }
                        List.Add(t);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
            
            return List;
        }
        public static T DataTableToDynamicObject(DataTable tab)   
        {
            T t = new T();
            if (tab.Rows.Count > 0)
            {
                for (int i = 0; i < tab.Rows.Count && i<1; ++i)
                {
                    PropertyInfo[] info = t.GetType().GetProperties();
                    foreach (PropertyInfo pp in info)
                    {
                        string Name = pp.Name;
                        if (tab.Columns.Contains(Name))
                        {
                            string mm = tab.Rows[i][Name].ToString();
                            if (tab.Rows[i][Name].ToString() != "")
                            {
                                    if (pp.PropertyType == typeof(DateTime) || pp.PropertyType == typeof(DateTime))
                                    {
                                        DateTime date = DateTime.MaxValue;
                                        DateTime.TryParse(tab.Rows[i][pp.Name].ToString(),
                                            CultureInfo.CurrentCulture, DateTimeStyles.None, out date);
                                        pp.SetValue(t, date, null);
                                    }
                                    else
                                    {
                                        pp.SetValue(t, tab.Rows[i][pp.Name], null);
                                    }

                            }
                        }
                    }

                }
            }
            return t;
        }
    }

    public static class Extension
    {
        public static List<T> ToList<T>(this DataSet ds) where T : new()
        {
            return ConverttoList<T>.DataTableToListObject(ds.Tables[0]);
        }

        public static List<T> ToList<T>(this DataTable dt) where T : new()
        {
            return ConverttoList<T>.DataTableToListObject(dt);
        }
        public static T ToObject<T>(this DataRow dr) where T : new()
        {
            T obj = new T();
            PropertyInfo[] properties = obj.GetType().GetProperties();
            foreach (PropertyInfo property in properties)
            {
                string propertyName = property.Name;
                if (dr.Table.Columns.Contains(propertyName))
                {
                    object Value = dr[propertyName];
                    if (!Value.Equals(DBNull.Value))
                    {
                        if (property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(DateTime))
                        {
                            DateTime date = DateTime.MaxValue;
                            DateTime.TryParse(Value.ToString(), CultureInfo.CurrentCulture, DateTimeStyles.None, out date);
                            property.SetValue(obj, date, null);
                        }
                        else
                        {
                            property.SetValue(obj, Value, null);
                        }
                    }
                }
            }
            return obj;
        }

        public static T Single<T>(this DataSet ds) where T : new()
        {
            var dt = ds.Tables[0];
            if (dt == null || dt.Rows.Count <= 0)
                return default(T);

            return ConverttoList<T>.DataTableToDynamicObject(dt);
        }
        public static T Single<T>(this DataTable dt) where T : new()
        {
            if (dt == null || dt.Rows.Count <= 0)
                return new T();

            return ConverttoList<T>.DataTableToDynamicObject(dt);
        }
        public static int InsertChanges<T>(this T model) where T : new()
        {
            string refinfo="";
            return InsertChanges<T>(model, out refinfo);
        }
        public static int InsertChanges<T>(this T model,out string refinfo) where T:new()
        {
            string Str = " insert into "+ DbHelperSQL._QianZhui + model.GetType().Name+" ( ";
            string parmeter = "";
            string result = "";
            DataTable tab = DbHelperSQL.Query("select top 1 * from "+ DbHelperSQL._QianZhui + model.GetType().Name).Tables[0];
            List<SqlParameter> listpar=new List<SqlParameter>();
                    PropertyInfo[] info = model.GetType().GetProperties();
                    foreach (PropertyInfo pp in info)
                    {
                        if (tab.Columns.Contains(pp.Name))
                        {
                            Str +=pp.GetValue(model, null) != null? (pp.Name.ToUpper() != "ID" ? pp.Name + "," : (pp.PropertyType == typeof(Guid)? pp.Name + ",":"")):"";
                            parmeter += pp.GetValue(model, null) != null ? (pp.Name.ToUpper() != "ID" ? "@"+pp.Name + "," : (pp.PropertyType == typeof(Guid) ? "@" + pp.Name + "," : "")) : "";// pp.GetValue(model, null) != null && pp.Name.ToUpper() != "ID" ? "@" + pp.Name + "," : "";
                        }
                    }
                    Str=Str.TrimEnd(',')+")  values("+parmeter.TrimEnd(',')+") select @@identity  ";
                    foreach (PropertyInfo pp in info)
                    {
                         if (tab.Columns.Contains(pp.Name) && pp.GetValue(model,null)!=null)
                            {
                                    if(pp.Name.ToUpper() == "ID")
                                    {
                                        if (pp.PropertyType == typeof(Guid))
                                        {
                                            result = Guid.NewGuid().ToString();
                                            listpar.Add(new SqlParameter("@" + pp.Name, SqlDbType.UniqueIdentifier) { Value =Guid.Parse(result) });
                                        }
                                    }
                                    else if ( pp.PropertyType == typeof(string))
                                    {
                                           listpar.Add(new SqlParameter("@"+pp.Name, DbType.String) { Value=pp.GetValue(model,null)});
                                    }
                                    else  if (pp.PropertyType == typeof(DateTime) || pp.PropertyType == typeof(DateTime))
                                    {
                                        DateTime date =DateTime.Parse("1990-01-01 00:00:00");
                                        DateTime.TryParse(pp.GetValue(model,null).ToString(),
                                            CultureInfo.CurrentCulture, DateTimeStyles.None, out date);
                                        date =DateTime.Parse(pp.GetValue(model, null).ToString())!=DateTime.MinValue && pp.GetValue(model, null).ToString() != "0001-01-01 00:00:00" ? Convert.ToDateTime(pp.GetValue(model, null)) : DateTime.Parse("1990-01-01 00:00:00");
                                        listpar.Add(new SqlParameter("@"+pp.Name, SqlDbType.DateTime) { Value=date==DateTime.MinValue? DateTime.Now:date});
                                    }
                                    else if(pp.PropertyType == typeof(int?) || pp.PropertyType == typeof(int))
                                    {
                                           listpar.Add(new SqlParameter("@"+pp.Name, SqlDbType.Int) { Value=pp.GetValue(model,null)});
                                    }
                                    else if(pp.PropertyType == typeof(decimal?) || pp.PropertyType == typeof(decimal))
                                    {
                                           listpar.Add(new SqlParameter("@"+pp.Name, SqlDbType.Decimal) { Value=pp.GetValue(model,null)});
                                    }
                                    else if(pp.PropertyType == typeof(bool?) || pp.PropertyType == typeof(bool))
                                    {
                                        listpar.Add(new SqlParameter("@"+pp.Name, SqlDbType.Bit) { Value=Convert.ToByte(pp.GetValue(model,null))});
                                    }
                                    else if (pp.PropertyType == typeof(Guid))
                                    {
                                        listpar.Add(new SqlParameter("@" + pp.Name, SqlDbType.UniqueIdentifier) { Value=pp.GetValue(model, null)});
                                    }
                              }
                    }
            SqlParameter[] par = listpar.ToArray();
            int ret= DbHelperSQL.ExecTranSql(Str,par);
            refinfo = result;
            return ret;
        }
        public static bool InsertListChanges<T>(this List<T> model) where T : new()
        {
            string result ="";
            foreach(var mm in model)
            {
                mm.InsertChanges(out result);
            }
            return true;
        }
        public static bool UpdateChanges<T>(this T model) where T : new()
        {
            string Str ="";
            List<SqlParameter> listpar = new List<SqlParameter>();
            PropertyInfo[] info = model.GetType().GetProperties();
            string ID="";
            foreach (PropertyInfo pp in info)
            {
                if (pp.Name.ToUpper() == "ID" && pp.GetValue(model, null) != null)
                {
                    ID=pp.GetValue(model, null).ToString();
                    break;
                }
                if (pp.Name.ToUpper() == "ID" && ID == "")
                {
                    throw new Exception("ID not Empty");
                }
            }
            DataTable tab = DbHelperSQL.Query("select * from " + DbHelperSQL._QianZhui + model.GetType().Name + " where ID='" + ID + "'").Tables[0];
            T DataModel = tab.Single<T>(); //DBQuery<T>.Single("ID=" + ID);
            if (DataModel != null)
            {
                foreach (PropertyInfo pp in info)
                {
                    string Datamodel = pp.GetValue(DataModel, null) == null ? "" : pp.GetValue(DataModel, null).ToString();
                    if (pp.GetValue(model, null) != null && tab.Columns.Contains(pp.Name) && pp.GetValue(model, null).ToString()!=Datamodel)
                    {
                        Str += pp.GetValue(model, null) != null && pp.Name.ToUpper() != "ID" ? pp.Name + "=@" + pp.Name + "," : "";
                    }
                }
            }
            Str =Str!=""?  " update " + DbHelperSQL._QianZhui + model.GetType().Name + " set  "+Str.TrimEnd(',') +"  where  ID=@ID ":"";
            if (Str != "")
            {
                foreach (PropertyInfo pp in info)
                {
                    string Datamodel = pp.GetValue(DataModel, null) == null ? "" : pp.GetValue(DataModel, null).ToString();
                    if (pp.Name.ToUpper() == "ID")
                    {
                          listpar.Add(new SqlParameter("@ID", (pp.PropertyType ==typeof(int)?SqlDbType.Int: SqlDbType.UniqueIdentifier)) { Value = pp.GetValue(model, null) });
                    }
                    else if (pp.GetValue(model, null) != null && tab.Columns.Contains(pp.Name) && pp.GetValue(model, null).ToString() !=Datamodel)
                    {
                        if (pp.PropertyType == typeof(string))
                        {
                            listpar.Add(new SqlParameter("@" + pp.Name,DbType.String) { Value = pp.GetValue(model, null) });
                        }
                        else if (pp.PropertyType == typeof(DateTime) || pp.PropertyType == typeof(DateTime))
                        {
                            DateTime date =DateTime.Parse("1990-01-01 00:00:00");
                            DateTime.TryParse(pp.GetValue(model, null).ToString(),
                                CultureInfo.CurrentCulture, DateTimeStyles.None, out date);
                            date = DateTime.Parse(pp.GetValue(model, null).ToString()) != DateTime.MinValue && pp.GetValue(model, null).ToString() != "0001-01-01 00:00:00" ? Convert.ToDateTime(pp.GetValue(model, null)) : DateTime.Parse("1990-01-01 00:00:00");
                            listpar.Add(new SqlParameter("@" + pp.Name, SqlDbType.DateTime) { Value = date });
                        }
                        else if (pp.PropertyType == typeof(int?) || pp.PropertyType == typeof(int))
                        {
                            listpar.Add(new SqlParameter("@" + pp.Name, SqlDbType.Int) { Value = pp.GetValue(model, null) });
                        }
                        else if (pp.PropertyType == typeof(decimal?) || pp.PropertyType == typeof(decimal))
                        {
                            listpar.Add(new SqlParameter("@" + pp.Name, SqlDbType.Decimal) { Value = pp.GetValue(model, null) });
                        }
                        else if (pp.PropertyType == typeof(bool?) || pp.PropertyType == typeof(bool))
                        {
                            listpar.Add(new SqlParameter("@" + pp.Name, SqlDbType.Bit) { Value = Convert.ToByte(pp.GetValue(model, null)) });
                        }
                        else if (pp.PropertyType == typeof(Guid?) || pp.PropertyType == typeof(Guid))
                        {
                            listpar.Add(new SqlParameter("@" + pp.Name, SqlDbType.UniqueIdentifier) { Value = pp.GetValue(model, null) });
                        }
                    }
                }
                 SqlParameter[] par = listpar.ToArray();
                 int ret = DbHelperSQL.ExecuteSql(Str, par);
                 return ret > 0 ? true : false;
            }
            else
            {
                return true;   
            }
           
        }
        public static bool UpdateListChanges<T>(this List<T> model) where T : new()
        {
            foreach (var mm in model)
            {
                mm.UpdateChanges();
            }
            return true;
        }
        public static bool DelChanges<T>(this T model) where T:new()
        {
            PropertyInfo[] info = model.GetType().GetProperties();
            string ID="";
            foreach (PropertyInfo pp in info)
            {
                if (pp.Name.ToUpper() == "ID" && pp.GetValue(model, null) != null)
                {
                    ID=pp.GetValue(model, null).ToString();
                    break;
                }
            }
            if (ID != "0" && ID != "")
            {
                return DbHelperSQL.ExecuteSql("delete  " + DbHelperSQL._QianZhui + model.GetType().Name + "  where  ID='" + ID + "'") >= 1 ? true : false;
            }else
            {
                return false;
            }
        }
        public static bool DelListChanges<T>(this List<T> model) where T:new()
        {
            if (model.Count > 0)
            {
                string ID = "";
                foreach (var mm in model)
                {
                    PropertyInfo[] info = mm.GetType().GetProperties();
                    foreach (PropertyInfo pp in info)
                    {
                        if (pp.Name.ToUpper() == "ID" && pp.GetValue(mm, null) != null && int.Parse(pp.GetValue(mm, null).ToString()) > 0)
                        {
                            ID += pp.GetValue(mm, null).ToString() + ",";
                            break;
                        }
                        else if (pp.Name.ToUpper() == "ID" && (pp.GetValue(model, null) == null || int.Parse(pp.GetValue(model, null).ToString()) <= 0))
                        {
                            throw new Exception("主键ID不能为null或者为空！");
                        }
                    }
                }
                return DbHelperSQL.ExecuteSql("delete  " + DbHelperSQL._QianZhui + model[0].GetType().Name + "  where  ID in(" + ID.TrimEnd(',') + ")") >= 1 ? true : false;
            }
            else
            {
                return false;
            }
        }
      
        static string GetNetIP()
        {
            string result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            if (string.IsNullOrEmpty(result))
                result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (string.IsNullOrEmpty(result))
                result = HttpContext.Current.Request.UserHostAddress;
            if (string.IsNullOrEmpty(result) || !Regex.IsMatch(result, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$"))
                return "127.0.0.1";
            return result;
        }

        public static string BaseEcode(this string Str)
        {
            return Convert.ToBase64String(Encoding.Default.GetBytes(Str));
        }
        public static string BaseDcode(this string Str)
        {
            return Encoding.Default.GetString(Convert.FromBase64String(Str));
        }
        public static SqlParameter[] GetParameter(this object t,string name)
        {
            return new SqlParameter[]{new SqlParameter(name,DbType.String){Value=t.ToString()}};
        }
        public static Dictionary<TKey, List<TSource>> ToTree<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            return source.ToTree(keySelector, x => x);
        }

        public static Dictionary<TKey, List<TElement>> ToTree<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector)
        {
            var result = new Dictionary<TKey, List<TElement>>();

            foreach (var item in source)
            {
                var key = keySelector(item);
                var element = elementSelector(item);
                if (result.ContainsKey(key))
                {
                    result[key].Add(element);
                }
                else
                {
                    result.Add(key, new List<TElement> { element });
                }
            }
            return result;
        }
    }
    public class M<T> where T : new()
    {

            public static List<T> Where(string where = "  1=1  ",params SqlParameter[] par)
            {
                T obj = new T();
                DataSet ds = DbHelperSQL.Query("select *  from  " + DbHelperSQL._QianZhui + obj.GetType().Name + "  where  " + where,par);
                return ds.Tables[0].ToList<T>();
            }
            public static List<T> WhereSql(string sql, params SqlParameter[] par)
            {
                T obj = new T();
                DataSet ds = DbHelperSQL.Query(sql,par);
                return ds.Tables[0].ToList<T>();
            }
            public static DataTable SqlRetuenTable(string sql, params SqlParameter[] par)
            {
                T obj = new T();
                return DbHelperSQL.Query(sql,par).Tables[0];
            }
            public static T SingleWhere(string where = "  1=1  ", params SqlParameter[] par)
            {
                T obj = new T();
                DataSet ds = DbHelperSQL.Query("select *  from  " + DbHelperSQL._QianZhui + obj.GetType().Name + "  where  " + where,par);
                return ds.Tables[0].Single<T>();
            }
            public static T SingleSql(string sql, params SqlParameter[] par)
            {
                T obj = new T();
                DataSet ds = DbHelperSQL.Query(sql,par);
                return ds.Tables[0].Single<T>();
            }
    }
    
   
    public class MsgJson
    {
        public virtual Boolean Success { get; set; }
        public virtual string Message { get; set; }

        public MsgJson()
        {

        }

        public MsgJson(bool success)
        {
            this.Success = success;
        }

        public MsgJson(string message)
        {
            this.Message = message;
        }

        public MsgJson(bool success, string message)
        {
            this.Success = success;
            this.Message = message;
        }
    }
}
