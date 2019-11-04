using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace InsightAPISample.WebApp.Helpers
{
    public static class XMLHelpers
    {
        #region XML Extensions
        public static string EmptyIfNull(this string item)
        {
            if (string.IsNullOrEmpty(item)) { return ""; } else { return item; }
        }

        /// <summary>
        /// checks if a certain element contains a child element by name
        /// </summary>
        /// <param name="parentElement"></param>
        /// <param name="elementName"></param>
        /// <returns></returns>
        public static bool ElementExists(this XElement parentElement, string elementName)
        {
            try
            {
                var foundElement = parentElement.Element(elementName);
                if (foundElement != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }

        }

        /// <summary>
        /// checks if a document contains a certain element
        /// </summary>
        /// <param name="parentDocument"></param>
        /// <param name="elementName"></param>
        /// <returns></returns>
        public static bool ElementExists(this XDocument parentDocument, string elementName)
        {
            try
            {
                var foundElement = parentDocument.Element(elementName);
                if (foundElement != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parentElement"></param>
        /// <param name="elementName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string TryGetElementValue(this XElement parentElement, string elementName, string defaultValue = null, int maxLength = 0)
        {
            var foundElement = parentElement.Element(elementName);
            if (foundElement != null)
            {
                if (maxLength > 0 && foundElement.Value.Length > maxLength)
                {
                    return foundElement.Value.Substring(0, maxLength);
                }
                else
                {
                    return foundElement.Value;
                }

            }

            return defaultValue;
        }
        #endregion

        public static JObject StoredProcXMLToJObject(string xml)
        {
            XDocument xmlContent;

            try
            {
                xmlContent = XDocument.Load(new System.IO.StringReader(xml));
            }
            catch (Exception)
            {
                return new JObject();
            }

            if (xmlContent.ElementExists("NewDataSet"))
            {
                JTokenWriter jWriter = new JTokenWriter();
                jWriter.WriteStartObject();
                int i = 0;
                string tableName = "Table";
                while (xmlContent.Element("NewDataSet").ElementExists(tableName))
                {
                    //write a property for each table
                    jWriter.WritePropertyName(tableName);
                    //write an array for each collection of table entities
                    jWriter.WriteStartArray();

                    foreach (XElement item in xmlContent.Element("NewDataSet").Elements(tableName))
                    {
                        jWriter.WriteStartObject();
                        foreach (XElement prop in item.Elements())
                        {
                            jWriter.WritePropertyName(prop.Name.LocalName);
                            jWriter.WriteValue(prop.Value);
                        }
                        jWriter.WriteEndObject();
                    }
                    jWriter.WriteEndArray();

                    i++;
                    tableName = $"Table{i}";
                }

                jWriter.WriteEndObject();

                return (JObject)jWriter.Token;
            }

            return new JObject();
        }


        public static Dictionary<string, List<Dictionary<string, object>>> 
            StoredProcXMLToDictionary(string xml)
        {
            var dict = new Dictionary<string, List<Dictionary<string, object>>>();

            XDocument xmlContent;

            try
            {
                xmlContent = XDocument.Load(new System.IO.StringReader(xml));
            }
            catch (Exception)
            {
                return dict;
            }

            if (xmlContent.ElementExists("NewDataSet"))
            {
                int i = 0;
                string tableName = "Table";

                //table level
                while (xmlContent.Element("NewDataSet").ElementExists(tableName))
                {
                    //row level
                    var rows = new List<Dictionary<string, object>>();
                    foreach (XElement item in xmlContent.Element("NewDataSet").Elements(tableName))
                    {
                        Dictionary<string, object> props = new Dictionary<string, object>();
                        foreach (XElement prop in item.Elements())
                        {
                            //item level
                            props.Add(prop.Name.LocalName, prop.Value);
                        }
                        rows.Add(props);
                    }

                    dict.Add(tableName, rows);

                    i++;
                    tableName = $"Table{i}";
                }
            }

            return dict;
        }
    }
}
