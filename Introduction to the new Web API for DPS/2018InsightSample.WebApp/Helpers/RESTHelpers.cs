using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;

namespace InsightAPISample.WebApp.Helpers
{
    public static class RESTHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fields"></param>
        /// <returns></returns>
        public static string GetFieldFilterParamString(string[] fields)
        {
            System.Text.StringBuilder paramString = new System.Text.StringBuilder();
            if (fields != null && fields.Count() > 0)
            {
                paramString.Append($"fieldFilter={HttpUtility.UrlEncode(string.Join(",", fields))}");
            }

            return paramString.ToString();
        }

        /// <summary>
        /// returns a portion of the url string for a set of search conditions
        /// </summary>
        /// <param name="fields"></param>
        /// <returns></returns>
        public static string GetSearchFilterParamString(IList<FilterHash> fields)
        {
            System.Text.StringBuilder paramString = new System.Text.StringBuilder();
            if (fields != null && fields.Count() > 0)
            {
                for (int i = 0; i < fields.Count(); i++)
                {
                    paramString.Append($"&filterHash[{i}][seq]={i+1}");
                    paramString.Append($"&filterHash[{i}][name]={fields[i].name}");
                    paramString.Append($"&filterHash[{i}][value]={fields[i].value}");
                    paramString.Append($"&filterHash[{i}][tablename]={fields[i].tablename}");
                    paramString.Append($"&filterHash[{i}][type]={fields[i].type}");
                    paramString.Append($"&filterHash[{i}][opp]={HttpUtility.UrlEncode(fields[i].opp)}");
                    paramString.Append($"&filterHash[{i}][condition]={fields[i].condition}");
                    if (fields[i].searchlevel > 0)
                    {
                        paramString.Append($"&filterHash[{i}][searchlevel]={fields[i].searchlevel}");
                    }
                }
            }

            return paramString.ToString();

        }

        
    }

    /// <summary>
    /// a set of search filter conditions
    /// </summary>
    public class FilterHash
    {
        /// <summary>
        /// the column name in the table
        /// </summary>
        public string name { get; set; } = "";
        /// <summary>
        /// the value that is filtered on
        /// </summary>
        public string value { get; set; } = "";
        /// <summary>
        /// the table name for this field
        /// </summary>
        public string tablename { get; set; } = "";
        /// <summary>
        /// field type: can be numeric, dropdown, etc...
        /// </summary>
        public string type { get; set; } = "dropdown";
        /// <summary>
        /// comparer: =, <= !=, ...
        /// </summary>
        public string opp { get; set; } = "=";
        /// <summary>
        /// not sure
        /// </summary>
        public string condition { get; set; } = "";
        /// <summary>
        /// only used for projects: wbs level to search for
        /// </summary>
        public int searchlevel { get; set; } = 0;
    }
}
