# Sample app for Entra ID authentication in a SPA + API

I made this app for Techorama 2024 in the Netherlands.

## How to run this sample

You will need to have .NET 8 SDK installed (with Visual Studio etc. or otherwise).

Register a new app in your Entra ID tenant with:

- Single-page app platform, redirect URI: `https://localhost:5173`
- Expose an API, add a scope `Forecasts.Read`
- API permissions, add MS Graph `User.Read` scope
- Add a client secret

The file appsettings.json in the Server project must be updated for your app registration:

- EntraId:TenantId (your Entra tenant ID)
- EntraId:ClientId (your app registration client ID)
- EntraId:ClientSecret (your app registration client secret)
  - You should not store the secret in this value; store it in user secrets/Key Vault etc.

Also update client/src/authConfig.ts with your values:

- clientId in MSAL config (your app registration client ID)
- authority in MSAL config (update with your Entra tenant ID)
- scopes in both loginRequest and apiRequest objects (the Forecasts.Read scope you added to your app registration)

You should now be able to run the server project.
It runs the client project as well, so you don't need to run it separately.
