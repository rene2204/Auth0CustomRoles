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

## Template Structure

- **Client**: The Blazor WebAssembly sample application
- **Api**: A C# Azure Functions API, which the Blazor application will call
- **Shared**: A C# class library with a shared data model between the Blazor and Functions application

## Deploy to Azure Static Web Apps

This application can be deployed to [Azure Static Web Apps](https://docs.microsoft.com/azure/static-web-apps), to learn how, check out [our quickstart guide](https://aka.ms/blazor-swa/quickstart).
