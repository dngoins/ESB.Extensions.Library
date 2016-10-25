using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.ESB.Exception.Management;
using System.Diagnostics;

namespace ESB.Extensions.Library.Resolvers.Bre
{

    /// <summary>
    /// Class used in both the BRE Resolver Provider, and Business Rules Composer to create, update and read BizTalk Message Context Property name, value information.
    /// </summary>
    public class MessageContextFactRetriever
    {
        private Dictionary<string, string> ctxtValues;

        /// <summary>
        /// Initializes the Context Value collection with Message Context values being passed in.
        /// </summary>
        /// <param name="ctxtValues">Collection of BizTalk Message Context values</param>
        public MessageContextFactRetriever(Dictionary<string, string> ctxtValues)
        {
            this.ctxtValues = ctxtValues;        
        }

        /// <summary>
        /// Returns current Message Context collection values
        /// </summary>
        /// <returns>Dictionary Collection of most current Message Context Values</returns>
        public Dictionary<string, string> GetDictionaryCollection()
        {
            return ctxtValues;
        }


        /// <summary>
        /// Retrieves the current entry in the Message Context Value collection if present, otherwise it returns an empty string
        /// </summary>
        /// <param name="name">The Name of the property to retrieve</param>
        /// <param name="strNamespace">The namespace of the property name to retrieve</param>
        /// <returns>String representation of the value</returns>
        public string GetItem(string name, string strNamespace)
        {
            string key = string.Format("{0}#{1}",  strNamespace, name);
            if (string.IsNullOrEmpty( name) || string.IsNullOrEmpty( strNamespace )) return string.Empty;
            try
            {
                return ctxtValues[key];
            }
            catch (Exception ex)
            {
                EventLogger.LogMessage(string.Format("MessageContextRetriever::GetItem threw an exception:\nFor Key:\n{0}\nDetails:\n{1}", key, ex.Message), EventLogEntryType.Error, 10000);
                return string.Empty;
            }
            
        }


        /// <summary>
        /// Method which updates, or adds a new name value property into the Message Context value collection
        /// </summary>
        /// <param name="name">Name of the property to create or update</param>
        /// <param name="strNamespace">Namespace of the property to create or update</param>
        /// <param name="value">Value of the property - name and namespace to be created or updated with</param>
        /// <param name="promote">Boolean value to determine if the property should be Property Promoted (which follows all the BizTalk Proper rules for property promotion), or distinguished. True means to property promote, which also means the namespace, name and value must adhere to the BizTalk proper rules for property promotion. This includes, an existing property schema, matching schema namespace, and 255 character limitation of a string data type, and other BizTalk Proper validation procedures.</param>
        public void SetItem(string name, string strNamespace, string value, bool promote)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(strNamespace)) return;


            bool bDistinguished = name.Contains("/");

            string key = string.Format("{0}#{1}",  strNamespace, name);
            try
            {
                if (!ctxtValues.ContainsKey(key))
                {
                    if (promote || !bDistinguished )
                        ctxtValues.Add(key + "!", value );
                    else
                    {
                        ctxtValues.Add(key, value);
                    }
                }
                else
                {
                    if (promote || !bDistinguished)
                    {
                        ctxtValues.Remove(key);
                        ctxtValues.Add(key + "!", value );
                    }
                    else
                        ctxtValues[key] = value;

                }
            }
            catch (Exception ex)
            {
                EventLogger.LogMessage(string.Format("MessageContextRetriever::SetItem threw an exception:\nFor Key:\n{0}\nValue:\n{1}\nErrorDetails:\n{2}", key, value, ex.Message), EventLogEntryType.Error, 10000);
                return;
            }
            
        }
    }
}
