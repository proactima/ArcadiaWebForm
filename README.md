# Arcadia Example Web Form

This project demonstrates how to authenticate, get a valid access token and create a simple article in Arcadia (UXRisk backend). It is written in ASP.Net Core, using C#. Arcadia have no specific bindings to any framework/language, so any framework/language will work as long as it can talk http.

## Prerequisites
In order for the example to work a few things need to be done.

### Register the Forms App

1. Sign in to the [Azure portal](https://portal.azure.com).
2. On the top bar, click on your account and under the **Directory** list, choose the Active Directory tenant where you wish to register your application.
3. Click on **More Services** in the left hand nav, and choose **Azure Active Directory**.
4. Click on **App registrations** and choose **Add**.
5. Enter a friendly name for the application, for example 'ArcadiaForms' and select 'Web App/API' as the Application Type. For the redirect URI, enter `https://localhost:44390/signin-oidc`. Click on **Create** to create the application.
6. While still in the Azure portal, choose your application, click on **Settings** and choose **Properties**.
7. Find the Application ID value and copy it to the clipboard.
8. Configure Permissions for your application - in the Settings menu, choose the 'Required permissions' section, click on **Add**, then **Select an API**, and type 'UXRisk' in the textbox. Then, click on  **Select Permissions** and select 'Access UXRisk API'.
9. Create a new application secret under API Settings/Keys. Name the key something to remember it by and copy it out (ypu will need it in the next step).

### Configure Forms App
1. Open appsettings.json file
2. Set 'ClientId' to be the object id from AAD
3. Set 'TenantId' to be the tenant id from your AAD
4. Set 'SecretKey' to be the secret key from step 9 above

### Trust the IIS Express SSL certificate

Since the web API is SSL protected, the client of the API (the web app) will refuse the SSL connection to the web API unless it trusts the API's SSL certificate.  Use the following steps in Windows Powershell to trust the IIS Express SSL certificate.  You only need to do this once.  If you fail to do this step, calls to the TodoListService will always throw an unhandled exception where the inner exception message is:

"The underlying connection was closed: Could not establish trust relationship for the SSL/TLS secure channel."

To configure your computer to trust the IIS Express SSL certificate, begin by opening a Windows Powershell command window as Administrator.

Query your personal certificate store to find the thumbprint of the certificate for `CN=localhost`:

```
PS C:\windows\system32> dir Cert:\LocalMachine\My


    Directory: Microsoft.PowerShell.Security\Certificate::LocalMachine\My


Thumbprint                                Subject
----------                                -------
C24798908DA71693C1053F42A462327543B38042  CN=localhost
```

Next, add the certificate to the Trusted Root store:

```
PS C:\windows\system32> $cert = (get-item cert:\LocalMachine\My\C24798908DA71693C1053F42A462327543B38042)
PS C:\windows\system32> $store = (get-item cert:\Localmachine\Root)
PS C:\windows\system32> $store.Open("ReadWrite")
PS C:\windows\system32> $store.Add($cert)
PS C:\windows\system32> $store.Close()
```

You can verify the certificate is in the Trusted Root store by running this command:

`PS C:\windows\system32> dir Cert:\LocalMachine\Root`

## What does it do?
In 'Startup.cs' authentication is setup, including how to get a valid access token to call Arcadia API. This is standard [Open Connect ID](http://openid.net/connect/) setup for ASP.Net Core. If you wish to use another framework you will have to figure out how to do that in your chosen framework.

The authentication setup includes using a token cache. This is to avoid having to fetch a new token on every request. The example includes an in-memory token cache; this is fine for development and simple applications running on a single server. However if you want to deploy to multiple servers you should consider using a [https://docs.microsoft.com/en-us/azure/architecture/multitenant-identity/token-cache](distributed token cache).

The 'ProfileAttribute' filter is executed on every request and ensures that a valid token is present for API communication. It also loads the users profile and makes it possible to show the username and tenant name in the header of the page. Please note that for this example the user is simply signed out if something goes wrong (invalid token etc.). In a production environment you might want to handle token and cookie expiration better, there are [https://docs.microsoft.com/nb-no/aspnet/core/security/authentication/cookie](documentation) on this from Microsoft.

The OpportunityController is setup with input/output and viewmodels, conversion happens via [http://automapper.org/](Automapper), using profiles. This makes it easy to code and avoids [https://www.hanselman.com/blog/ASPNETOverpostingMassAssignmentModelBindingSecurity.aspx](overposting security issues).

In appsettings.json you can set the value of 'UseRemote' to false to use a fake api caller; it will return dummy data and accept any input. This is useful for debugging layout and such.

## Using Arcadia API
In order to create an article, such as an opportunity in this example, you always need a valid Arcadia Id first. The 'ICallApi' interface has a simple method to aquire such an id. Please note that the id might look like a guid; but it could be any string of equal or shorter length. Using that id you can PUT pretty much any valid JSON to the Api. The 'StoreArticleAsync' method will store an opportunity in the correct fashion. It's worth noting that all responses comes in the form of 

```
{
    "results": [
        { 
            // reponse object
        }
    ]
}
```
An article can have (basically) any number of fields, but for references (1:n, 1:1) there are specific rules. In the example this is illustrate through the class 'ArcadiaLink'; it holds the type name (unit, location, status etc.) and the reference id(s). In the example the reference is limited to entity references (e_[reference_name]_ids), but by using an article reference (a_[reference_name]_ids) two articles can also be linked. Please note that the child of the two (the one referenced in the values property of the link) need also to refer to the parent in the property 'parentid' and 'parenttype'. This allowes our backend to traverse a graph of articles both ways.
