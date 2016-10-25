using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.ESB.Resolver;
using System.Xml;
using System.Xml.Serialization;
using System.Globalization;
using System.Reflection;
using System.IO;
using Microsoft.XLANGs.BaseTypes;
using Microsoft.Practices.ESB.Exception.Management;
using Microsoft.RuleEngine;
using Microsoft.BizTalk.Message.Interop;
using Microsoft.BizTalk.Component.Interop;
using Microsoft.BizTalk.Streaming;
using System.Diagnostics;
using System.ComponentModel;
using Microsoft.Practices.ESB.Resolver.Itinerary.Facts;
using Microsoft.Practices.ESB.Resolver.Container;
using Microsoft.Practices.Unity;
using Microsoft.Practices.ESB.Resolver.Facts;


namespace ESB.Extensions.Library.Resolvers.Bre 
{
    /// <summary>
    /// BRE Resolver Provider
    /// 
    /// An ESB Resolver Provider which populates the ESB Resolution collection object with facts from BRE Policies to resolve itineraries, maps, and endpoint addresses.
    /// </summary>
    public class BRE_ResolverProvider : IResolveProvider, IResolveContainer
{
        Dictionary<string, string> ctxtValues;
        private static PolicyExecutor policyExecutor;

        
        public BRE_ResolverProvider()
        {
            ctxtValues = new Dictionary<string, string>();
        }

    // Methods
        /// <summary>
        /// Method to create descriptor object from configured areas such as itineraries and pipeline components
        /// </summary>
        /// <param name="config">String containing name, values for BRE Resolver properties</param>
        /// <param name="resolver">Resolver connection String</param>
        /// <returns>BRE Descriptor object populated with configuration values</returns>
    private BRE CreateResolverDescriptor(string config, string resolver)
    {
        BRE bre2;
        if (config == null)
        {
            throw new ArgumentNullException("config");
        }
        if (resolver == null)
        {
            throw new ArgumentNullException("resolver");
        }
        Dictionary<string, string> facts = null;
        try
        {
            BRE bre = new BRE();
            bre.useMsg = false;
            bre.useMsgCtxt = false;
            facts = ResolverMgr.GetFacts(config, resolver);
            bre.policy = ResolverMgr.GetConfigValue(facts, true, "policy");
            bre.version = ResolverMgr.GetConfigValue(facts, false, "version");
            string str = ResolverMgr.GetConfigValue(facts, false, "useMsg");
            string strUseMsgCtxt = ResolverMgr.GetConfigValue(facts, false, "useMsgCtxt");
            string strMsgCtxt = ResolverMgr.GetConfigValue(facts, false, "msgCtxtValues");
            if (!string.IsNullOrEmpty(str) && ((string.Compare(str, "true", true, CultureInfo.CurrentCulture) == 0) || (string.Compare(str, "false", true, CultureInfo.CurrentCulture) == 0)))
            {
                bre.useMsgSpecified = true;
                bre.useMsg = Convert.ToBoolean(str, NumberFormatInfo.CurrentInfo);
            }
            string str2 = ResolverMgr.GetConfigValue(facts, false, "recognizeMessageFormat");
            if (!string.IsNullOrEmpty(str2) && ((string.Compare(str2, "true", true, CultureInfo.CurrentCulture) == 0) || (string.Compare(str2, "false", true, CultureInfo.CurrentCulture) == 0)))
            {
                bre.recognizeMessageFormatSpecified = true;
                bre.recognizeMessageFormat = Convert.ToBoolean(str2, NumberFormatInfo.CurrentInfo);
            }
            if (!string.IsNullOrEmpty(strUseMsgCtxt) && ((string.Compare(strUseMsgCtxt, "true", true, CultureInfo.CurrentCulture) == 0) || (string.Compare(strUseMsgCtxt, "false", true, CultureInfo.CurrentCulture) == 0)))
            {
                bre.useMsgCtxtSpecified = true;
                bre.useMsgCtxt = Convert.ToBoolean(strUseMsgCtxt, NumberFormatInfo.CurrentInfo);
            }

            if (bre.useMsgCtxt )
            {
                if (!string.IsNullOrEmpty(strMsgCtxt) )
                {
                    bre.msgCtxtValues  = strMsgCtxt;
                }   
            }

            string strUsePolicyBroker = ResolverMgr.GetConfigValue(facts, false, "usePolicyBroker");
            string strUseDebugInterceptor = ResolverMgr.GetConfigValue(facts, false, "useDebugInterceptor");
            string strDebugOutput = ResolverMgr.GetConfigValue(facts, false, "debugOutputPath");
            if (!string.IsNullOrEmpty(strUsePolicyBroker) && ((string.Compare(strUsePolicyBroker, "true", true, CultureInfo.CurrentCulture) == 0) || (string.Compare(strUsePolicyBroker, "false", true, CultureInfo.CurrentCulture) == 0)))
            {
                bre.usePolicyBrokerSpecified = true;
                bre.usePolicyBroker  = Convert.ToBoolean(strUsePolicyBroker, NumberFormatInfo.CurrentInfo);
            }

            if (!string.IsNullOrEmpty(strUseDebugInterceptor) && ((string.Compare(strUseDebugInterceptor, "true", true, CultureInfo.CurrentCulture) == 0) || (string.Compare(strUseDebugInterceptor, "false", true, CultureInfo.CurrentCulture) == 0)))
            {
                bre.useDebugInterceptorSpecified = true;
                bre.useDebugInterceptor = Convert.ToBoolean(strUseDebugInterceptor, NumberFormatInfo.CurrentInfo);
            }

            if (bre.useDebugInterceptor)
                bre.debugOutputPath = strDebugOutput;

            string strUseRepromotion = ResolverMgr.GetConfigValue(facts, false, "useRepromotion");
            if (!string.IsNullOrEmpty(strUseRepromotion) && ((string.Compare(strUseRepromotion, "true", true, CultureInfo.CurrentCulture) == 0) || (string.Compare(strUseRepromotion, "false", true, CultureInfo.CurrentCulture) == 0)))
            {
                bre.useRepromotionSpecified  = true;
                bre.useRepromotion = Convert.ToBoolean(strUseRepromotion, NumberFormatInfo.CurrentInfo);
            }

            bre2 = bre;
        }
        catch (Exception exception)
        {
            EventLogger.Write(MethodBase.GetCurrentMethod(), exception);
            throw;
        }
        finally
        {
            if (facts != null)
            {
                facts.Clear();
                facts = null;
            }
        }
        return bre2;
    }
        /// <summary>
        /// Resolve implementation for use with Resolver Web Service. This method is typically used with unit tests to test for the correct Resolved entities, such as itinerary, maps and endpoint addresses. This method invokes the BRE Policies that were configured through the Config and Resolver connection string values.
        /// </summary>
        /// <param name="config">string containing name, value property values</param>
        /// <param name="resolver">Resolver connection string</param>
        /// <param name="message">Xml document containing the message to pass to the BRE policies if configured properly</param>
        /// <returns>Resolver Dictionary Collection containing resolved entries, such as itinerary name, map name, and endpoint address resolution values</returns>
    Dictionary<string, string> IResolveProvider.Resolve(string config, string resolver, XmlDocument message)
    {
        Dictionary<string, string> dictionary;
        if (string.IsNullOrEmpty(config))
        {
            throw new ArgumentNullException("config");
        }
        if (string.IsNullOrEmpty(resolver))
        {
            throw new ArgumentNullException("resolver");
        }
        if (message == null )
        {
            throw new ArgumentNullException("message");
        }

        Resolution resolution = new Resolution();
        try
        {
            BRE bre = this.CreateResolverDescriptor(config, resolver);
            if ( bre.recognizeMessageFormat || bre.useMsgCtxt)
            {
                throw new ResolveException("The attributes  recognizeMessageFormat and useMsgCtxt are only supported inside of pipelines. They are not supported when passing only and XmlDocument.");
            }
            IBaseMessage msg = null;
            dictionary = ResolveRules(config, resolver, message, resolution, bre, null, null, ref msg );
        }
        catch (Exception exception)
        {
            EventLogger.Write(MethodBase.GetCurrentMethod(), exception);
            throw;
        }
        finally
        {
            if (resolution != null)
            {
                resolution = null;
            }
        }
        return dictionary;
    }

        /// <summary>
        /// Resolve implementation for use within a Pipeline component. This method is typically used with one of the ESB Pipeline components such as the Itinerary Selector, or ESB Dispatcher to resolve entities, such as itinerary, maps and endpoint addresses. This method invokes the BRE Policies that were configured through the Config and Resolver connection string values.
        /// </summary>
    /// <param name="config">string containing name, value property values</param>
    /// <param name="resolver">Resolver connection string</param>
    /// <param name="message">BizTalk IBaseMessage class which is used to pass to the BRE policies if configured properly</param>
        /// <param name="pipelineContext">BizTalk Pipeline configuration</param>
    /// <returns>Resolver Dictionary Collection containing resolved entries, such as itinerary name, map name, and endpoint address resolution values</returns>
    Dictionary<string, string> IResolveProvider.Resolve(string config, string resolver, IBaseMessage message, IPipelineContext pipelineContext)
    {
        Dictionary<string, string> dictionary;
        
        if (string.IsNullOrEmpty(config))
        {
            throw new ArgumentNullException("config");
        }
        if (string.IsNullOrEmpty(resolver))
        {
            throw new ArgumentNullException("resolver");
        }
        if (message == null)
        {
            throw new ArgumentNullException("message");
        }
        if (pipelineContext == null)
        {
            throw new ArgumentNullException("pipelineContext");
        }
        Resolution resolution = new Resolution();
        XmlDocument document = new XmlDocument();
        try
        {
            
            ResolverMgr.SetContext(resolution, message, pipelineContext);
            BRE bre = this.CreateResolverDescriptor(config, resolver);

            Stream originalDataStream = message.BodyPart.GetOriginalDataStream();
            if (!originalDataStream.CanSeek)
            {
                ReadOnlySeekableStream stream2 = new ReadOnlySeekableStream(originalDataStream);
                message.BodyPart.Data = stream2;
                pipelineContext.ResourceTracker.AddResource(stream2);
                originalDataStream = stream2;
            }
            if (originalDataStream.Position != 0L)
            {
                originalDataStream.Position = 0L;
            }
            document.Load(originalDataStream);
            originalDataStream.Position = 0L;

            if (bre.useMsg && (message.BodyPart != null))
            {
                if (bre.useMsgCtxt)
                {
                    // Check for Message Context values
                    string strMsgCtxtValues = bre.msgCtxtValues;
                    if (string.IsNullOrEmpty(strMsgCtxtValues ))
                    {
                        // get all context values
                        object objValue = null;
                        ctxtValues.Clear();
                        IBaseMessageContext bmCtxt = message.Context;
                        
                        for (int i = 0; i < bmCtxt.CountProperties; i++ )
                        {
                            string strName = string.Empty;
                            string strNamespace = string.Empty ;
                            string key = string.Empty;  
                            try
                            {
                                objValue = bmCtxt.ReadAt(i, out strName, out strNamespace);
                                key = string.Format("{0}#{1}", strNamespace, strName);
                                if (objValue != null)
                                {
                                    // check to see if already in collection
                                    if (!ctxtValues.ContainsKey(key))
                                        ctxtValues.Add(key, objValue.ToString());
                                    else
                                    {
                                        ctxtValues[key] = objValue.ToString();
                                    }

                                }
                                else
                                {
                                    if (!ctxtValues.ContainsKey(key))
                                        ctxtValues.Add(key, string.Empty);
                                    else
                                    {
                                        ctxtValues[key] = string.Empty;
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                EventLogger.LogMessage(string.Format("Namespace: {0}\nName: {1}\n caused an exception:\n{2}\nThis item was not added to the collection.", strNamespace, strName, ex.Message), EventLogEntryType.Error, 1000);
                            }

                        }

                        // Now ready body part properties

                        for (int p = 0; p < message.PartCount; p++)
                        {
                            string partName = string.Empty;
                            var part = message.GetPartByIndex(p, out partName);

                            for (int pp = 0; pp < part.PartProperties.CountProperties; pp++)
                            {
                                string strName = string.Empty;
                                string strNamespace = string.Empty;
                                string key = string.Empty;
                                try
                                {
                                    objValue = part.PartProperties.ReadAt(pp, out strName, out strNamespace);
                                    key = string.Format("{0}#{1}", strNamespace, strName);
                                    if (objValue != null)
                                    {
                                        // check to see if already in collection
                                        if (!ctxtValues.ContainsKey(key))
                                            ctxtValues.Add(key, objValue.ToString());
                                        else
                                        {
                                            ctxtValues[key] = objValue.ToString();
                                        }
                                    }
                                    else
                                    {
                                        if (!ctxtValues.ContainsKey(key))
                                            ctxtValues.Add(key, string.Empty);
                                        else
                                        {
                                            ctxtValues[key] = string.Empty;
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    EventLogger.LogMessage(
                                        string.Format(
                                            "Namespace: {0}\nName: {1}\n caused an exception:\n{2}\nThis item was not added to the collection.",
                                            strNamespace, strName, ex.Message), EventLogEntryType.Error, 1000);
                                }

                            }
                        }


                    }
                    else
                    {
                        // otherwise get specific context values by checking for |
                        bool hasPipeDelimiter = strMsgCtxtValues.Contains("|");
                        string[] msgCtxtValues = null;
                        string msgCtxtValue = string.Empty;
                        if (hasPipeDelimiter)
                        {
                            msgCtxtValues = strMsgCtxtValues.Split('|');
                            object objValue = null;
                            ctxtValues.Clear();
                            string key;
                            foreach (string str in msgCtxtValues )
                            {
                                string[] ns_values = str.Split('#');
                                try
                                {
                                    objValue = message.Context.Read(ns_values[1], ns_values[0]);
                                    key = str;
                                    if (objValue != null)
                                    {
                                        if (!ctxtValues.ContainsKey(key))
                                            ctxtValues.Add(key, objValue.ToString());
                                        else
                                        {
                                            ctxtValues[key] = objValue.ToString();
                                        }
                                    }
                                    else
                                    {
                                        if (!ctxtValues.ContainsKey(key))
                                            ctxtValues.Add(key, string.Empty);
                                        else
                                        {
                                            ctxtValues[key] = string.Empty;
                                        }
                                    }
                                }
                                catch(Exception ex)
                                {
                                    EventLogger.LogMessage(string.Format("Namespace: {0}\nName: {1}\n caused an exception:\n{2}\nThis item was not added to the collection.", ns_values[0], ns_values[1], ex.Message), EventLogEntryType.Error, 1000);
                                }
                                
                            }
                        }
                        else
                        {
                            object objValue;
                            string key;
                            string[] ns_Values = new string[] {string.Empty, string.Empty };
                            try
                            {
                                ns_Values = strMsgCtxtValues.Split('#');
                                objValue = message.Context.Read(ns_Values[1], ns_Values[0]);
                                key = strMsgCtxtValues;
                                if (objValue != null)
                                {
                                    if (!ctxtValues.ContainsKey(key))
                                        ctxtValues.Add(key, objValue.ToString());
                                }
                                else
                                {
                                    if (!ctxtValues.ContainsKey(key))
                                        ctxtValues.Add(key, string.Empty);

                                }
                            }
                            catch (Exception ex)
                            {
                                EventLogger.LogMessage(string.Format("Namespace: {0}\nName: {1}\n caused an exception:\n{2}\nThis item was not added to the collection.", ns_Values[0], ns_Values[1], ex.Message), EventLogEntryType.Error, 1000);
                            }
                        }
                    }
                }
                
            }
            dictionary = ResolveRules(config, resolver, document, resolution, bre, ctxtValues, pipelineContext, ref message );
                            
        }
        catch (Exception exception)
        {
            EventLogger.Write(MethodBase.GetCurrentMethod(), exception);
            throw;
        }
        finally
        {
            if (resolution != null)
            {
                resolution = null;
            }
        }
        return dictionary;
    }


    /// <summary>
    /// Resolve implementation for use within an Orchestration. This method is typically used by creating an instance of the BRE Resolver Provider class inside an orchestration expression to resolve entities, such as itinerary, maps and endpoint addresses. This method invokes the BRE Policies that were configured through the Config and Resolver connection string values.
    /// </summary>
    /// <param name="resolverInfo">Resolver collection of entries</param>
    /// <param name="message">BizTalk XLangMessage class which is used to pass to the BRE policies if configured properly</param>
    /// <returns>Resolver Dictionary Collection containing resolved entries, such as itinerary name, map name, and endpoint address resolution values</returns>
    public Dictionary<string, string> Resolve(ResolverInfo resolverInfo, XLANGMessage message)
    {
        Dictionary<string, string> dictionary;
        if (message == null)
        {
            throw new ArgumentNullException("message");
        }
        Resolution resolution = new Resolution();
        try
        {
            
            BRE bre = this.CreateResolverDescriptor(resolverInfo.Config, resolverInfo.Resolver);
            if ( bre.recognizeMessageFormat || bre.useMsgCtxt )
            {
                throw new ResolveException("The attributes  recognizeMessageFormat and useMsgCtxt are not supported from orchestrations.");
            }
            ResolverMgr.SetContext(resolution, message);
            XmlDocument document = null;
            if (bre.useMsg)
            {
                document = (XmlDocument)message[0].RetrieveAs(typeof(XmlDocument));
            }
            
            
            IBaseMessage msg = null;
            dictionary = ResolveRules(resolverInfo.Config, resolverInfo.Resolver, document, resolution, bre, null, null, ref msg);
        }
        catch (Exception exception)
        {
            EventLogger.Write(MethodBase.GetCurrentMethod(), exception);
            throw;
        }
        finally
        {
            if (resolution != null)
            {
                resolution = null;
            }
        }
        return dictionary;
    }
        /// <summary>
        /// Method used to update BizTalk Pipeline message from the BRE Policy. BRE Policies work with Xml messages. BizTalk Messages are immutable. Thus if in order for the BRE Policy to update the BizTalk Message, a new message needs to be created, copied, or cloned containing the updated Xml content, along with the BizTalk Promoted properties from the context.
        /// </summary>
        /// <param name="xmlMessage">Message containing the modified content</param>
        /// <param name="pc">Pipeline context containing the current pipeline configuration</param>
        /// <param name="baseMsg">BizTalk BaseMessage which is to be cloned/copied/or modified</param>
        private static void UpdateMessage(string xmlMessage, IPipelineContext pc, ref IBaseMessage baseMsg)
        {
            if (pc == null || baseMsg == null) return;
            IBaseMessagePart bodyPart = pc.GetMessageFactory().CreateMessagePart();
            MemoryStream omstrm = new MemoryStream(Encoding.UTF8.GetBytes(xmlMessage ));
            omstrm.Seek(0L, SeekOrigin.Begin);
            bodyPart.Data = omstrm;
            pc.ResourceTracker.AddResource(omstrm);
            IBaseMessage outMsg = pc.GetMessageFactory().CreateMessage();
            outMsg.AddPart("body", bodyPart, true);
            outMsg.Context = PipelineUtil.CloneMessageContext(baseMsg.Context);
            baseMsg = outMsg;
        }

        /// <summary>
        /// Updates the BizTalk BaseMessage and Message Context with any new or modified values from the executed BRE Policies.
        /// </summary>
        /// <param name="msgCtxt">BizTalk BaseMessage Context value collection to update</param>
        /// <param name="bre">BRE Descriptor with possible values to read for updating the Message context</param>
        /// <param name="pCtxt">PipelineContext</param>
        /// <param name="baseMsg">BizTalk BaseMessage to update</param>
        private static void UpdateContextProperties(MessageContextFactRetriever msgCtxt, BRE bre, IPipelineContext pCtxt,  ref IBaseMessage baseMsg)
        {
            try
            {
                if (pCtxt == null || baseMsg == null) return;
                IBaseMessageContext baseMsgCtxt = baseMsg.Context;
                foreach (var updatedCtxt in msgCtxt.GetDictionaryCollection())
                {
                    string[] NameNameSpaceValues = updatedCtxt.Key.Split('#');

                    // no need to check for old values just overwrite and add
                    // Check to see if we need to promote it
                    string name = NameNameSpaceValues[1];
                    bool shouldPromote = name.Contains("!");
                    bool isDistinguished = name.Contains("/");

                    string namesp = NameNameSpaceValues[0];
                    
                    // check to determine if we should promote and not distinguished
                    if (shouldPromote && !isDistinguished)
                    {
                       
                        string correctName = name;

                        // remove ! char from key name before promoting
                        if (shouldPromote)
                            correctName = name.Substring(0, name.Length - 1);

                        // however check to see if already promoted or not
                        bool isAlreadyPromoted = false;
                        var ovalue = baseMsgCtxt.Read(correctName, namesp);
                        if (ovalue != null)
                        {
                            isAlreadyPromoted = baseMsgCtxt.IsPromoted(correctName, namesp);
                        }

                        if (ovalue != null && isAlreadyPromoted)
                        {
                            // we need to remove and re - promote
                            baseMsgCtxt.Write(correctName, namesp, null);
                            baseMsgCtxt.Promote(correctName, namesp, null);
                            baseMsgCtxt.Promote(correctName, namesp, updatedCtxt.Value);
                        }
                        else
                        {
                            // it's not already promoted and we should promote if we can, 
                            // this assumes there is a valid property schema, name, and data type associated with it for promotion validation...
                            // dangerous operation which could cause cyclic loop by re-promoting a property that was slated to be demoted *wasPromote*...
                            if (bre.useRepromotionSpecified)
                            {
                                if (bre.useRepromotion)
                                {
                                    try
                                    {
                                        baseMsgCtxt.Write(correctName, namesp, null);
                                        baseMsgCtxt.Promote(correctName, namesp, null);
                                        baseMsgCtxt.Promote(correctName, namesp, updatedCtxt.Value);
                                    }
                                    catch (Exception ex)
                                    {
                                        EventLogger.LogMessage(
                                            string.Format(
                                                "Namespace: {0}\nName: {1}\n caused an exception:\n{2}\nThis item was not promoted.",
                                                namesp, correctName, ex.Message), EventLogEntryType.Error, 1000);

                                    }
                                }
                            }

                        }
                    }
                    else if (shouldPromote && isDistinguished )
                    {
                        // can't promote a distinguished field that contains a "/" in it's name, there's no way for BizTalk to validate it using normal BizTalk Property Schemas...
                        // do nothing.
                    }
                    else if (isDistinguished)
                    {
                        // We don't need to promote it, only write it (Distinguished)
                        // we need to remove and re-write it
                        baseMsgCtxt.Write(name, namesp, null);
                        baseMsgCtxt.Write(name, namesp, updatedCtxt.Value);
                    }
                    //else niether promote nore write so do nothing...
                }
                pCtxt.ResourceTracker.AddResource(baseMsgCtxt);
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("BRE_ResolverProvider::UpdateContextProperties", ex.ToString(),
                                    EventLogEntryType.Error, 10000);
                throw;
            }
        }

        /// <summary>
        /// Method which executes the BRE Policy which is configured using the config and resolver connection string
        /// </summary>
        /// <param name="config">string containing name, value property values</param>
        /// <param name="resolver">Resolver connection string</param>
        /// <param name="message">Xml document containing the message to pass to the BRE policies if configured properly</param>
        /// <param name="resolution">Resolution object</param>
        /// <param name="bre">BRE Descriptor object</param>
        /// <param name="ctxtValues">Dictionary collection of BizTalk Message Context property value pairs</param>
        /// <param name="pCtxt">BizTalk Pipeline Context</param>
        /// <param name="baseMsg">BizTalk BaseMessage</param>
        /// <returns>Resolver Dictionary Collection containing resolved entries, such as itinerary name, map name, and endpoint address resolution values</returns>
    private static Dictionary<string, string> ResolveRules(string config, string resolver, XmlDocument message, Resolution resolution, BRE bre, Dictionary<string, string> ctxtValues, IPipelineContext pCtxt, ref IBaseMessage baseMsg)
    {
        Dictionary<string, string> dictionary3;
        int num = 0;
        int num2 = 0;
        Policy policy = null;
        Dictionary<string, string> dictionary = null;
        string[] strArray = null;
        object[] objArray = null;
        TypedXmlDocument document = null;
        ItineraryFact itineraryInfo = new ItineraryFact();

        string documentSpecNameField = "Microsoft.Practices.ESB.ResolveProviderMessage";
        Dictionary<string, string> resolverDictionary = new Dictionary<string, string>();
        if (!resolver.Contains(@":\"))
        {
            resolver = resolver + @":\";
        }
        try
        {
            EventLogger.Write("Resolution strong name is {0}.", new object[] { resolution.DocumentSpecNameField });
            if (!string.IsNullOrEmpty(resolution.DocumentSpecNameField) && bre.recognizeMessageFormat)
            {
                int index = resolution.DocumentSpecNameField.IndexOf(",", StringComparison.CurrentCultureIgnoreCase);
                if ((index > 0) && (index < resolution.DocumentSpecNameField.Length))
                {
                    documentSpecNameField = resolution.DocumentSpecNameField.Substring(0, index);
                }
                else
                {
                    documentSpecNameField = resolution.DocumentSpecNameField;
                }
            }


            // add root node as non promoted value for purpose of using with Orchestrations if needed
            if (ctxtValues == null)
            {
                ctxtValues = new Dictionary<string, string>();
            }
            ctxtValues.Add( "Microsoft.Practices.ESB.ResolveProviderMessage#RootNode", message.DocumentElement.LocalName.ToUpperInvariant());

             MessageContextFactRetriever customFactRetriever = new MessageContextFactRetriever(ctxtValues);

            EventLogger.Write("DocType for typed xml document is {0}.", new object[] { documentSpecNameField });
            if (!string.IsNullOrEmpty(bre.version))
            {
                strArray = bre.version.Split(".".ToCharArray());
                if (strArray != null)
                {
                    num = Convert.ToInt16(strArray[0], NumberFormatInfo.CurrentInfo);
                    num2 = Convert.ToInt16(strArray[1], NumberFormatInfo.CurrentInfo);
                }
            }
            if (bre.useMsg)
            {
                EventLogger.Write("Xml document Content is {0}.", message.OuterXml );
                objArray = new object[4];
                document = new TypedXmlDocument(documentSpecNameField, message);

                objArray[0] = resolution;
                objArray[1] = document;
                objArray[2] = itineraryInfo;
                objArray[3] = customFactRetriever;
            }
            else
            {
                objArray = new object[] { resolution, itineraryInfo , customFactRetriever   };
            }
            
            bool useDebugInterceptor = false;
            string outputPath = string.Empty;
            if (bre.useDebugInterceptorSpecified)
            {
                useDebugInterceptor = bre.useDebugInterceptor;
                outputPath = bre.debugOutputPath;
                policyExecutor = new PolicyExecutor(useDebugInterceptor , outputPath);
            }
            else
                policyExecutor = new PolicyExecutor(false, string.Empty);

            // Check to see if we need to use the Policy Broker
            if (bre.usePolicyBrokerSpecified )
            {
                if (bre.usePolicyBroker)
                {
                    PolicyBroker broker = new PolicyBroker(useDebugInterceptor, outputPath);
                    PolicyBrokerInfoCollection _policyInfos = broker.GetPolicies(message, resolution, bre.policy, itineraryInfo, customFactRetriever, num, num2);
                    foreach (var policyBrokerInfo in _policyInfos.PolicyInfos )
                    {
                        var policyInfo = policyBrokerInfo.Value;
                        int foundPolicyMajor = 0;
                        int foundPolicyMinor = 0;
                        strArray = policyInfo.Version.Split(".".ToCharArray());
                        if (strArray != null)
                        {
                            foundPolicyMajor = Convert.ToInt16(strArray[0], NumberFormatInfo.CurrentInfo);
                            foundPolicyMinor = Convert.ToInt16(strArray[1], NumberFormatInfo.CurrentInfo);
                        }
                        policyExecutor.ExecutePolicy(policyInfo.PolicyName, objArray, foundPolicyMajor, foundPolicyMinor);
                        
                    }
                }
                else
                {
                    // don't use policy broker
                    policyExecutor.ExecutePolicy(bre.policy, objArray, num, num2);
                }
            }
            else
            {
                // don't use policy broker
                policyExecutor.ExecutePolicy(bre.policy, objArray, num, num2);
  
            }

            if (objArray[0] == null)
            {
                throw new ResolveException("Resolution is not configured correctly after applying BRE Custom Resolver;\nPlease check the Business Rule Policy");
            }
            
            // Check for Itinerary fact values
            SetAllResolutionDictionaryEntries(objArray, resolverDictionary, bre);
            dictionary3 = resolverDictionary;

            if (bre.useMsg)
            {
                TypedXmlDocument doc = (TypedXmlDocument)objArray[1];
                dictionary3.Add("message", doc.Document.OuterXml);

                if (bre.useMsgCtxt)
                {
                    MessageContextFactRetriever customRetriever = (MessageContextFactRetriever)objArray[3];
                    dictionary3.Add("contextValues", GetContextValuesString(customRetriever.GetDictionaryCollection()));
                    
                    // Update the context properties
                    UpdateContextProperties(customRetriever, bre, pCtxt, ref baseMsg);
                }

                // Modify the Current Message in pipeline context
                UpdateMessage(doc.Document.OuterXml, pCtxt, ref baseMsg);
            }
            else
            {
                if (bre.useMsgCtxt)
                {
                    MessageContextFactRetriever customRetriever = (MessageContextFactRetriever)objArray[2];
                    dictionary3.Add("contextValues", GetContextValuesString(customRetriever.GetDictionaryCollection()));

                    // Update the context properties
                    UpdateContextProperties(customRetriever, bre, pCtxt, ref baseMsg );
                }
                
            }

        }
        catch (Exception exception)
        {
            EventLogger.Write(MethodBase.GetCurrentMethod(), exception);
            throw;
        }
        finally
        {
            if (objArray != null)
            {
                objArray = null;
            }
            if (document != null)
            {
                document = null;
            }
            if (resolution != null)
            {
                resolution = null;
            }
            if (bre != null)
            {
                bre = null;
            }
            if (strArray != null)
            {
                strArray = null;
            }
            if (policy != null)
            {
                policy.Dispose();
                policy = null;
            }
            if (dictionary != null)
            {
                dictionary.Clear();
                dictionary = null;
            }
            if (resolverDictionary != null)
            {
                resolverDictionary = null;
            }
            if (ctxtValues != null)
            {
                ctxtValues = null;
            }
        }
        return dictionary3;
    }
   
   
        /// <summary>
        /// This method populates the fact dictionary with all the resolved values from the resolvexxx() methods.
        /// </summary>
        /// <param name="facts">Fact array containing resolved entries</param>
        /// <param name="factDictionary">Fact Dictionary which will be polulated with resolved entries</param>
        /// <param name="bre">BRE Descriptor object</param>
        private static void SetAllResolutionDictionaryEntries(object[] facts, Dictionary<string, string> factDictionary, BRE bre)
   {
       Resolution res = (Resolution)facts[0];
       ItineraryFact itin = null;
       if (bre.useMsg )
         itin = (ItineraryFact)facts[2];
       else
         itin = (ItineraryFact)facts[1];
        
       try
       {
           var factTranslators = from translator in container.ResolveAll<IFactTranslator>()
                                 select translator;

           foreach (var trans in factTranslators)
           {
               // Check facts
               if (itin != null)
               {
                   // Don't translate the itinerary facts
                   // Check to see if we should translate
                   if (string.IsNullOrEmpty(itin.Name))
                       if (trans is ItineraryFactTranslator || trans is ItineraryContentsFactTranslator)
                           continue;
               }

               if (res != null)
               {
                   bool isNotTransportResolution = string.IsNullOrEmpty(res.TransportType) &&
                                                   string.IsNullOrEmpty(res.TransportLocation);
                   bool isNotTransformationResolution = string.IsNullOrEmpty(res.TransformType);

                   if (isNotTransformationResolution && isNotTransportResolution)
                       if (trans is DefaultFactTranslator)
                           continue;
               }

               trans.TranslateFact(facts, factDictionary);
           }

            factDictionary["Resolver.Success"] = "true";
       }
       catch
       {
           factDictionary["Resolver.Success"] = "false";
       }
   }

        /// <summary>
        /// Method used to convert dictionary collection of changed context values from executed BRE Policy to a xml formatted string for use with passing along with ESB Resolver Config entries.
        /// </summary>
        /// <param name="ctxtChangedValues">Dictionary Collection of changed context values from BRE Policy</param>
        /// <returns>Xml formatted string containing Context name value pairs</returns>
     private static string GetContextValuesString(Dictionary<string, string> ctxtChangedValues)
        {
            if (ctxtChangedValues == null)
                return "<contextValues />";

            StringBuilder sb = new StringBuilder();
            sb.Append("<contextValues>![CDATA[");
            int startCnt = 0;
            foreach (var o in ctxtChangedValues)
            {
                if (startCnt == 0)
                    sb.Append(string.Format("{0}={1}", o.Key, o.Value.ToString()));
                else
                    sb.Append(string.Format("|{0}={1}", o.Key, o.Value.ToString()));
                startCnt++;
            }
            sb.Append("]]</contextValues>");
            return sb.ToString();
        }


     #region IResolveContainer Members

        private static IUnityContainer container;
     public void Initialize(Microsoft.Practices.Unity.IUnityContainer container)
     {
         BRE_ResolverProvider.container = container;
     }

     #endregion
}
    
}