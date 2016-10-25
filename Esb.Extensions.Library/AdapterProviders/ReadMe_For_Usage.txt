1. Rebuild and GAC this project
2. Open the ESB.Config file from the install folder
Add the following line in the Adapter Providers section:
	<adapterProvider name="MSMQ" type="ESB.Extensions.Library.AdapterProviders.MSMQ.MSMQAdapterProvider, ESB.Extensions.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=aeb660468fddc5d9" moniker="msmq" />
	<adapterProvider name="Windows SharePoint Services" type="ESB.Extensions.Library.AdapterProviders.SharePoint.WSSAdapterProvider, ESB.Extensions.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=aeb660468fddc5d9" moniker="wss" />
	<adapterProvider name="HTTP" type="ESB.Extensions.Library.AdapterProviders.HTTP.HTTPAdapterProvider, ESB.Extensions.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=aeb660468fddc5d9" moniker="http" />
	
[Optional - for static resolver use inside an itinerary ]
3. Copy the Property manifest xml files into the %ESB Install Folder%\Tools\Itinerary Designer\ folder

 C:\Program Files (x86)\Microsoft Visual Studio 11.0\Common7\IDE\Extensions\Microsoft.Practices.Services.Itinerary.DslPackage
 C:\Program Files (x86)\Microsoft Visual Studio 11.0\Common7\IDE\Extensions\Microsoft.Practices.Services.Itinerary.DslPackage\Lib
	
Note: ACTIVEDIRECTORY adapter provider is used specially for ACTIVEADAPTER.
see http://www.activeadapter.com for more information

	