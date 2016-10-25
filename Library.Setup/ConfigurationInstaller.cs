using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Configuration;
using System.Security.Principal;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Win32;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;



namespace JMFE.ESB.Extensions.Library.Setup
{
   
    public class ConfigurationInstaller 
    {
        public ConfigurationInstaller()
        {
          //  InitializeComponent();
        }


        public string TargetDir { get; set; }

        public  void Install()
        {
        //   base.Install(stateSaver);
            MessageBox.Show("Trying to access Registry");
            string esbInstallPath = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\BizTalk ESB Toolkit").GetValue("InstallPath").ToString();

            MessageBox.Show(esbInstallPath);

            string vsInstallPath = Registry.LocalMachine.OpenSubKey(@"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Microsoft\VisualStudio\12.0").GetValue("InstallDir").ToString();

            MessageBox.Show(vsInstallPath);

            string configPath = esbInstallPath;
            if (string.IsNullOrEmpty(esbInstallPath))
                throw new InstallException("ESB Install Path is empty, Installation can not continue, verify the Microsoft ESB 2.3 Registry InstallPath key is correctly configured to a location where Microsoft BizTalk ESB Toolkit is located.");

            if (string.IsNullOrEmpty(vsInstallPath))
                throw new InstallException("Visual Studio Install Dir is empty, Installation can not continue, verify the Microsoft Visual Studio 2013 key is correctly configured to a location where Microsoft Visual Studio 2013 is located.");

            string itineraryDSLLibPath = string.Format("{0}Extensions\\Microsoft.Practices.Services.Itinerary.DslPackage\\Lib\\JMFE.ESB.Extensions.Library.dll", vsInstallPath);
            
            string itineraryDSLPath = string.Format("{0}Extensions\\Microsoft.Practices.Services.Itinerary.DslPackage", vsInstallPath);

            MessageBox.Show(itineraryDSLPath);
            //// Use the for each loop to determine what the context parameters are
            //foreach (string key in Context.Parameters.Keys)
            //{
            //    Context.LogMessage(string.Format("KeyName: {0} = \nValue: {1}", key, Context.Parameters[key]));
            //    MessageBox.Show(string.Format("KeyName: {0} = \nValue: {1}", key, Context.Parameters[key]));
            //}

            string libraryToCopy = string.Empty;
            var targetDirInvalid = TargetDir; // Context.Parameters["targetDir"].ToString();

            MessageBox.Show("TargetDir: " + targetDirInvalid);

            var targetDir = targetDirInvalid.Substring(0, targetDirInvalid.LastIndexOf('\\') ) ;
            libraryToCopy = targetDir + "JMFE.ESB.Extensions.Library.dll";

            MessageBox.Show("Library To Copy: " + libraryToCopy);
            
            MessageBox.Show(itineraryDSLLibPath);

            // Copy Extensions to Itinerary Lib Path
            File.Copy(libraryToCopy, itineraryDSLLibPath, true);

            MessageBox.Show("Copied file to DSL Lib Path");

            // First Copy MSMQ Property Manifest
            libraryToCopy = targetDir  +
                                   "AdapterProviders\\MSMQ\\Xml\\MSMQPropertyManifest.xml";

            // Copy Property Manifests to Itinerary DSL folder
            File.Copy(libraryToCopy, itineraryDSLPath + "MSMQPropertyManifest.xml", true);
            //File.Copy(libraryToCopy, vsExtensionsPath + "MSMQPropertyManifest.xml", true);


            //      // Next Copy WSS Property Manifest
            libraryToCopy = targetDir +
                                   "AdapterProviders\\SharePoint\\Xml\\Windows SharePoint ServicesPropertyManifest.xml";

            // Copy Property Manifests to Itinerary DSL folder
            File.Copy(libraryToCopy, itineraryDSLPath + "Windows SharePoint ServicesPropertyManifest.xml", true);
            //File.Copy(libraryToCopy, vsExtensionsPath + "Windows SharePoint ServicesPropertyManifest.xml", true);

            // Next Copy HTTP Property Manifest
            libraryToCopy = targetDir  +
                                   "AdapterProviders\\HTTP\\Xml\\HTTPPropertyManifest.xml";

            // Copy Property Manifests to Itinerary DSL folder
            File.Copy(libraryToCopy, itineraryDSLPath + "HTTPPropertyManifest.xml", true);
            //File.Copy(libraryToCopy, vsExtensionsPath + "HTTPPropertyManifest.xml", true);


            // Next Copy ActiveDirecctory Adapter Property Manifest
            libraryToCopy = targetDir +
                                   "AdapterProviders\\ACTIVEDIRECTORY\\Xml\\ACTIVEDIRECTORYPropertyManifest.xml";

            // Copy Property Manifests to Itinerary DSL folder
            File.Copy(libraryToCopy, itineraryDSLPath + "ACTIVEDIRECTORYPropertyManifest.xml", true);
           // File.Copy(libraryToCopy, vsExtensionsPath + "ACTIVEDIRECTORYPropertyManifest.xml", true);


            string esbConfigPath = string.Format("{0}esb.config", configPath);
            XmlDocument esbConfigDoc = new XmlDocument();
            esbConfigDoc.Load(esbConfigPath);

            #region "BRE Resolver"

            XmlNode resolversNode = esbConfigDoc.SelectSingleNode(
                "/*[local-name()='configuration']/*[local-name()='esb']/*[local-name()='resolvers']");

            XmlNode extendedResolver = esbConfigDoc.CreateNode(XmlNodeType.Element, "resolver", string.Empty);
            XmlAttribute nameAttribute = esbConfigDoc.CreateAttribute("name");
            nameAttribute.Value = "BRE.EXT";
            XmlAttribute typeAttribute = esbConfigDoc.CreateAttribute("type");
            typeAttribute.Value =
                "Microsoft.Practices.ESB.Resolver.Unity.ResolveProvider, Microsoft.Practices.ESB.Resolver.Unity, Version=2.1.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35";

            extendedResolver.Attributes.Append(nameAttribute);
            extendedResolver.Attributes.Append(typeAttribute);
            extendedResolver.InnerXml = "<resolverConfig><add name=\"unitySectionName\" value=\"esb.resolver\" /><add name=\"unityContainerName\" value=\"BRE.EXT\" /></resolverConfig>";

            resolversNode.AppendChild(extendedResolver);

           // MessageBox.Show("Finished BRE REsolver");
            
            #endregion

              

            #region "Adapter Provider Registration"
            // Now Get the Adapter Providers
            // <adapterProvider name="MSMQ" type="JMFE.ESB.Extensions.AdapterProviders.MSMQ.MSMQAdapterProvider, JMFE.ESB.Extensions.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5f6c2d4ce791c4c" moniker="msmq" />
            XmlNode adapterProvidersNode = esbConfigDoc.SelectSingleNode(
                "/*[local-name()='configuration']/*[local-name()='esb']/*[local-name()='adapterProviders']");
            XmlNode extendedAdapterProvider = esbConfigDoc.CreateNode(XmlNodeType.Element, "adapterProvider", string.Empty);

            XmlAttribute apNameAttribute = esbConfigDoc.CreateAttribute("name");
            apNameAttribute.Value = "MSMQ";
            XmlAttribute apTypeAttribute = esbConfigDoc.CreateAttribute("type");
            apTypeAttribute.Value =
                "JMFE.ESB.Extensions.Library.AdapterProviders.MSMQ.MSMQAdapterProvider, JMFE.ESB.Extensions.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5f6c2d4ce791c4c";

            XmlAttribute apMonikerAttribute = esbConfigDoc.CreateAttribute("moniker");
            apMonikerAttribute.Value = "msmq";

            extendedAdapterProvider.Attributes.Append(apNameAttribute);
            extendedAdapterProvider.Attributes.Append(apTypeAttribute);
            extendedAdapterProvider.Attributes.Append(apMonikerAttribute);

            adapterProvidersNode.AppendChild(extendedAdapterProvider);
            
            //MessageBox.Show("Finished MSMQ Adapter Provider");

            // Now Get the WSS Adapter Providers
            // <adapterProvider name="WSS" type="JMFE.ESB.Extensions.AdapterProviders.SharePoint.WSSAdapterProvider, JMFE.ESB.Extensions.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=b01aa156b4424a13" moniker="msmq" />

            XmlNode wssAdapterProvider = esbConfigDoc.CreateNode(XmlNodeType.Element, "adapterProvider", string.Empty);
            XmlAttribute wssNameAttribute = esbConfigDoc.CreateAttribute("name");
            wssNameAttribute.Value = "Windows SharePoint Services";
            XmlAttribute wssTypeAttribute = esbConfigDoc.CreateAttribute("type");
            wssTypeAttribute.Value =
                "JMFE.ESB.Extensions.Library.AdapterProviders.SharePoint.WSSAdapterProvider, JMFE.ESB.Extensions.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5f6c2d4ce791c4c";

            XmlAttribute wssMonikerAttribute = esbConfigDoc.CreateAttribute("moniker");
            wssMonikerAttribute.Value = "wss";

            wssAdapterProvider.Attributes.Append(wssNameAttribute);
            wssAdapterProvider.Attributes.Append(wssTypeAttribute);
            wssAdapterProvider.Attributes.Append(wssMonikerAttribute);

            adapterProvidersNode.AppendChild(wssAdapterProvider);
            
            //MessageBox.Show("Finished WSS Adapter Provider");

            // Now Get the HTTP Adapter Providers
            // <adapterProvider name="HTTP" type="JMFE.ESB.Extensions.AdapterProviders.HTTP.HTTPAdapterProvider, JMFE.ESB.Extensions.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5f6c2d4ce791c4c" moniker="http" />

            XmlNode httpAdapterProvider = esbConfigDoc.CreateNode(XmlNodeType.Element, "adapterProvider", string.Empty);
            XmlAttribute httpNameAttribute = esbConfigDoc.CreateAttribute("name");
            httpNameAttribute.Value = "HTTP";
            XmlAttribute httpTypeAttribute = esbConfigDoc.CreateAttribute("type");
            httpTypeAttribute.Value =
                "JMFE.ESB.Extensions.Library.AdapterProviders.HTTP.HTTPAdapterProvider, JMFE.ESB.Extensions.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5f6c2d4ce791c4c";

            XmlAttribute httpMonikerAttribute = esbConfigDoc.CreateAttribute("moniker");
            httpMonikerAttribute.Value = "http_legacy";

            httpAdapterProvider.Attributes.Append(httpNameAttribute);
            httpAdapterProvider.Attributes.Append(httpTypeAttribute);
            httpAdapterProvider.Attributes.Append(httpMonikerAttribute);

            adapterProvidersNode.AppendChild(httpAdapterProvider);
            
            //MessageBox.Show("Finished Http Legacy Adapter Provider");


            // Now Get the ACTIVEDIRECTORY Adapter Providers
            // <adapterProvider name="ACTIVEDIRECTORY" type="JMFE.ESB.Extensions.AdapterProviders.ACTIVEDIRECTORY.ACTIVEDIRECTORYAdapterProvider, JMFE.ESB.Extensions.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5f6c2d4ce791c4c" moniker="activedirectory" />

            XmlNode activedirectoryAdapterProvider = esbConfigDoc.CreateNode(XmlNodeType.Element, "adapterProvider", string.Empty);
            XmlAttribute activedirectoryNameAttribute = esbConfigDoc.CreateAttribute("name");
            activedirectoryNameAttribute.Value = "ACTIVEDIRECTORY";
            XmlAttribute activedirectoryTypeAttribute = esbConfigDoc.CreateAttribute("type");
            activedirectoryTypeAttribute.Value =
                "JMFE.ESB.Extensions.Library.AdapterProviders.ACTIVEDIRECTORY.ACTIVEDIRECTORYAdapterProvider, JMFE.ESB.Extensions.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5f6c2d4ce791c4c";

            XmlAttribute activedirectoryMonikerAttribute = esbConfigDoc.CreateAttribute("moniker");
            activedirectoryMonikerAttribute.Value = "activedirectory";

            activedirectoryAdapterProvider.Attributes.Append(activedirectoryNameAttribute);
            activedirectoryAdapterProvider.Attributes.Append(activedirectoryTypeAttribute);
            activedirectoryAdapterProvider.Attributes.Append(activedirectoryMonikerAttribute);

            adapterProvidersNode.AppendChild(activedirectoryAdapterProvider);

            //MessageBox.Show("Finished ActiveDirectory Adapter Provider");

            #endregion

            #region "BRE Type Aliases"

            // Now Get the Type Aliases
            //  <typeAlias alias="BRE_ResolverProvider" type="JMFE.ESB.Extensions.Resolvers.BRE_ResolverProvider, JMFE.ESB.Extensions.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=b01aa156b4424a13" />
            XmlNode typeAliasesNode = esbConfigDoc.SelectSingleNode(
                "/*[local-name()='configuration']/*[local-name()='esb.resolver']/*[local-name()='typeAliases']");

            XmlNode extendedTypeAlias = esbConfigDoc.CreateNode(XmlNodeType.Element, "typeAlias", string.Empty);
            XmlAttribute typeAliasAttribute = esbConfigDoc.CreateAttribute("alias");
            typeAliasAttribute.Value = "BRE_ResolverProvider";
            XmlAttribute typeTypeAttribute = esbConfigDoc.CreateAttribute("type");
            typeTypeAttribute.Value =
                "JMFE.ESB.Extensions.Library.Resolvers.Bre.BRE_ResolverProvider, JMFE.ESB.Extensions.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5f6c2d4ce791c4c";

            extendedTypeAlias.Attributes.Append(typeAliasAttribute);
            extendedTypeAlias.Attributes.Append(typeTypeAttribute);
            typeAliasesNode.AppendChild(extendedTypeAlias);

           // MessageBox.Show("Finished BRE Type Aliases");
            #endregion

               

            #region "BRE Unity Container"
            
            XmlNode containerNode = esbConfigDoc.SelectSingleNode(
                "/*[local-name()='configuration']/*[local-name()='esb.resolver']/*[local-name()='container']");

            XmlNode extendedContainer = esbConfigDoc.CreateNode(XmlNodeType.Element, "container", string.Empty);
            extendedContainer.InnerXml = "<types><type type=\"IResolveProvider\" mapTo=\"ItineraryResolveProvider\"/><type type=\"IFactProvider\" mapTo=\"ItineraryFactProvider\" name=\"ItineraryFactProvider\"><lifetime type=\"singleton\"/></type><type type=\"IRepositoryProvider\" mapTo=\"SqlRepositoryProvider\" name=\"CurrentRepositoryProvider\"><lifetime type=\"singleton\"/><typeConfig extensionType=\"Microsoft.Practices.Unity.Configuration.TypeInjectionElement,Microsoft.Practices.Unity.Configuration, Version=2.0.414.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\"><constructor><param name=\"connectionStringName\" parameterType=\"string\"><value value=\"ItineraryDb\"/></param><param name=\"cacheManagerName\" parameterType=\"string\"><value value=\"Itinerary Cache Manager\"/></param><param name=\"cacheTimeout\" parameterType=\"string\"><value value=\"120\"/></param></constructor></typeConfig></type><type type=\"IFactTranslator\" mapTo=\"DefaultFactTranslator\" name=\"DefaultFactTranslator\"><lifetime type=\"singleton\"/></type><type type=\"IFactTranslator\" mapTo=\"ItineraryFactTranslator\" name=\"ItineraryFactTranslator\"><lifetime type=\"singleton\"/><typeConfig extensionType=\"Microsoft.Practices.Unity.Configuration.TypeInjectionElement,Microsoft.Practices.Unity.Configuration, Version=2.0.414.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\"><constructor><param name=\"repositoryProvider\" parameterType=\"IRepositoryProvider\"><dependency name=\"CurrentRepositoryProvider\"/></param></constructor></typeConfig></type></types>";
            XmlAttribute containerNameAttribute = esbConfigDoc.CreateAttribute("name");
            containerNameAttribute.Value = "BRE.EXT";
            extendedContainer.Attributes.Append(containerNameAttribute);
            containerNode.AppendChild(extendedContainer);

           // MessageBox.Show("Finished BRE.EXT ");
            #endregion

           

            esbConfigDoc.Save(esbConfigPath);

        }

        public  void Uninstall()
        {
          
           // base.Uninstall(savedState);
            string esbInstallPath = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\BizTalk ESB Toolkit").GetValue("InstallPath").ToString();
            string vsInstallPath = Registry.LocalMachine.OpenSubKey(@"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Microsoft\VisualStudio\12.0").GetValue("InstallDir").ToString();
            string configPath = esbInstallPath;
            
            //For Debuggin purposes
            //foreach (string key in Context.Parameters.Keys)
            //{
            //    MessageBox.Show(string.Format("KeyName: {0} = \nValue: {1}", key, Context.Parameters[key]));
            //}

            string itineraryDSLLibPath = string.Format("{0}Extensions\\Microsoft.Practices.Services.Itinerary.DslPackage\\Lib\\JMFE.ESB.Extensions.Library.dll", vsInstallPath);
            string itineraryDSLPath = string.Format("{0}Extensions\\Microsoft.Practices.Services.Itinerary.DslPackage", vsInstallPath);


           
            if (File.Exists(itineraryDSLLibPath))
                File.Delete(itineraryDSLLibPath);

          

                
                // First Delete MSMQ Property Manifest
                string manifestToDelete = string.Format("{0}MSMQPropertyManifest.xml", itineraryDSLPath);

                if (File.Exists(manifestToDelete))
                    File.Delete(manifestToDelete);

                
                // Next Delete WSS Property Manifest
                manifestToDelete = string.Format("{0}Windows SharePoint ServicesPropertyManifest.xml", itineraryDSLPath);

                if (File.Exists(manifestToDelete))
                    File.Delete(manifestToDelete);

                
                // Next Delete HTTP Property Manifest
                manifestToDelete = string.Format("{0}HTTPPropertyManifest.xml", itineraryDSLPath);

                if (File.Exists(manifestToDelete))
                    File.Delete(manifestToDelete);

            
                // Next Delete AD Property Manifest
                manifestToDelete = string.Format("{0}ACTIVEDIRECTORYPropertyManifest.xml", itineraryDSLPath);

                if (File.Exists(manifestToDelete))
                    File.Delete(manifestToDelete);


            // Now get esb config file
            string esbConfigPath = string.Format("{0}esb.config", configPath);
            XmlDocument esbConfigDoc = new XmlDocument();
            esbConfigDoc.Load(esbConfigPath);

            XmlNode extResolversNode = esbConfigDoc.SelectSingleNode("/*[local-name()='configuration']/*[local-name()='esb']/*[local-name()='resolvers']");
            if (extResolversNode != null)
            {

                foreach (XmlNode node in extResolversNode.ChildNodes)
                {
                    if (node.Attributes["name"] != null)
                    {

                        if (node.Attributes["name"].Value == "BRE.EXT")
                        {
                            node.RemoveAll();
                            extResolversNode.RemoveChild(node);
                            continue;
                        }

                       
                    }
                }
            }
            XmlNode adapterProvidersNode = esbConfigDoc.SelectSingleNode("/*[local-name()='configuration']/*[local-name()='esb']/*[local-name()='adapterProviders']");

            if (adapterProvidersNode != null)
            {
                foreach (XmlNode node in adapterProvidersNode.ChildNodes)
                {
                    if (node.Attributes["name"] != null)
                    {
                        if (node.Attributes["name"].Value == "MSMQ")
                        {
                            node.RemoveAll();
                            adapterProvidersNode.RemoveChild(node);
                            continue;
                        }

                        // Remove WSS
                        if (node.Attributes["name"].Value == "Windows SharePoint Services")
                        {
                            node.RemoveAll();
                            adapterProvidersNode.RemoveChild(node);
                            continue;
                        }

                        // Remove HTTP
                        if (node.Attributes["name"].Value == "HTTP")
                        {
                            node.RemoveAll();
                            adapterProvidersNode.RemoveChild(node);
                            continue;
                        }

                        // Remove AD
                        if (node.Attributes["name"].Value == "ACTIVEDIRECTORY")
                        {
                            node.RemoveAll();
                            adapterProvidersNode.RemoveChild(node);
                            continue;
                        }
                    }
                }
            }

            XmlNode typeAliasesNode = esbConfigDoc.SelectSingleNode("/*[local-name()='configuration']/*[local-name()='esb.resolver']/*[local-name()='typeAliases']");

            if (typeAliasesNode != null)
            {

                foreach (XmlNode node in typeAliasesNode.ChildNodes)
                {
                    try
                    {
                        if (node.Attributes["alias"] != null)
                        {
                            if (node.Attributes["alias"].Value == "BRE_ResolverProvider")
                            {
                                node.RemoveAll();
                                typeAliasesNode.RemoveChild(node);
                                continue;
                            }


                           
                        }
                    }
                    catch (Exception)
                    {
                        // just keep looping
                    }
                }
            }

            XmlNode containersNode = esbConfigDoc.SelectSingleNode(
                "/*[local-name()='configuration']/*[local-name()='esb.resolver']/*[local-name()='container']");
            if (containersNode != null)
            {
                foreach (XmlNode node in containersNode.ChildNodes)
                {
                    try
                    {
                        if (node.Attributes["name"] != null)
                        {
                            if (node.Attributes["name"].Value == "BRE.EXT")
                            {
                                node.RemoveAll();
                                containersNode.RemoveChild(node);
                                continue;
                            }

                            
                        }
                    }
                    catch (Exception)
                    {
                        // keep looping

                    }
                }
            }

            esbConfigDoc.Save(esbConfigPath);

        }
    }
}
