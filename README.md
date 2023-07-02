# Azure Static Web APP Blazor Application with Auth0 support

This code is based on Microsoft's Azure Static Web App Blazor Starter Template. [Github](https://github.com/MicrosoftDocs/mslearn-staticwebapp-dotnet)
For more Infos on the basic setup and architecture please check the original template out.

I just added support for Auth0 authentication based on this blog entry from the [Auth0-Blog](https://auth0.com/blog/support-auth0-in-azure-static-web-apps-for-blazor-wasm/).

Additionally I extended that work to also support custom roles from Auth0.

## Changes starting the blog entry

1. Add a new Azure Function retreives the role information from Auth0
2. Add `rolesSource` entry to `auth` in `staticwebapp.config.json`
3. Configurate Auth0 for custom roles and use of the managment api
4. Add custom roles there where you want to check them

## Add Azure Function for roles

You need to add a new azure function with an HTTP-Trigger receiving a post request.

![image](https://github.com/rene2204/Auth0CustomRoles/assets/64254506/b690a045-991c-4b0e-87a2-eda3d3fc725c)


The body of the post request contains a json similar to this following format:
```json
{
  "identityProvider": "auth0",
  "userId": "72137ad3-ae00-42b5-8d54-aacb38576d76",
  "userDetails": "ellen@contoso.com",
  "claims": [
      {
          "typ": "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress",
          "val": "ellen@contoso.com"
      },
      {
          "typ": "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname",
          "val": "Contoso"
      },
      {
          "typ": "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname",
          "val": "Ellen"
      },
      {
          "typ": "name",
          "val": "Ellen Contoso"
      },
      {
          "typ": "http://schemas.microsoft.com/identity/claims/objectidentifier",
          "val": "7da753ff-1c8e-4b5e-affe-d89e5a57fe2f"
      },
      {
          "typ": "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier",
          "val": "72137ad3-ae00-42b5-8d54-aacb38576d76"
      },
      {
          "typ": "http://schemas.microsoft.com/identity/claims/tenantid",
          "val": "3856f5f5-4bae-464a-9044-b72dc2dcde26"
      },
      {
          "typ": "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name",
          "val": "ellen@contoso.com"
      },
      {
          "typ": "ver",
          "val": "1.0"
      }
  ],
  "accessToken": "eyJ0eXAiOiJKV..."
}
```


The result of this message has to be successful response with a list of the roles in following json format:
```json
{
  "roles": [
    "Reader",
    "Contributor"
  ]
}
```


For more details check out [this learn entry](https://learn.microsoft.com/en-us/azure/static-web-apps/authentication-custom?tabs=aad%2Cfunction#manage-roles)

## Modify `staticwebapp.config.json`

```json
{
  "auth": {
    "rolesSource": "/api/GetRoles",
    "identityProviders": {
      ...
    }
  }
}
```

You just need to add the path to the newly written azure function for `rolesSource` inside of `auth`.

## Template Structure

- **Client**: The Blazor WebAssembly sample application
- **Api**: A C# Azure Functions API, which the Blazor application will call
- **Shared**: A C# class library with a shared data model between the Blazor and Functions application

## Deploy to Azure Static Web Apps

This application can be deployed to [Azure Static Web Apps](https://docs.microsoft.com/azure/static-web-apps), to learn how, check out [this quickstart guide](https://aka.ms/blazor-swa/quickstart).
