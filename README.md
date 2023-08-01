# Azure Static Web App Blazor Application with Auth0 support

This code is based on Microsoft's Azure Static Web App Blazor Starter Template. [Github](https://github.com/MicrosoftDocs/mslearn-staticwebapp-dotnet)
For more Infos on the basic setup and architecture please check the original template out.

I just added support for Auth0 authentication based on this blog entry from the [Auth0-Blog](https://auth0.com/blog/support-auth0-in-azure-static-web-apps-for-blazor-wasm/).

Additionally I extended that work to also support custom roles from Auth0 (or any other custom OpenID Authentication).

## Changes starting the blog entry

1. Add a new Azure Function retreives the role information from Auth0
2. Add `rolesSource` entry to `auth` in `staticwebapp.config.json`
3. Configurate Auth0 for custom roles and use of the managment api
4. Add custom roles there where you want to check them

### Add Azure Function for roles

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
     ...
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

### Get roles from Auth0 management api


### Modify `staticwebapp.config.json`

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

### Configure Auth0 

1. Add API for the used Auth0 Application
2. Authorize (Machine-to-Machine) Connection
3. Assign the read rights for user, role, role-assing, ...
4. Save changes


### Add custom roles to your application

If you want to make a whole page available only for users with the role 'admin' the code looks like this:


![image](https://github.com/rene2204/Auth0CustomRoles/assets/64254506/69c3df0e-c26d-42cb-803a-e7e6708e533d)

You just need to add attribute `Authorize` including the parameter for the the role with `@attribute` on the top part of the page.

If you only want to make some parts of the page or component visible for authorized users you can use `AuthorizeView`.

![image](https://github.com/rene2204/Auth0CustomRoles/assets/64254506/422f567b-c53d-4a99-b228-e88f01295bf0)

You can also define two diffrent kind of child elements accordingly if or if not you are authorized with `Authorized` and `NotAuthorized` as children of `AuthorizeView`.
For more details check out [this link.](https://learn.microsoft.com/en-us/aspnet/core/blazor/security/?view=aspnetcore-7.0)

## Template Structure

- **Client**: The Blazor WebAssembly sample application
- **Api**: A C# Azure Functions API, which the Blazor application will call
- **Shared**: A C# class library with a shared data model between the Blazor and Functions application

## Deploy to Azure Static Web Apps

This application can be deployed to [Azure Static Web Apps](https://docs.microsoft.com/azure/static-web-apps), to learn how, check out [this quickstart guide](https://aka.ms/blazor-swa/quickstart).
