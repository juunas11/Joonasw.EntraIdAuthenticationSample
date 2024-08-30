import { Configuration, PopupRequest } from "@azure/msal-browser";

export const msalConfig: Configuration = {
    auth: {
        clientId: "ce9bb3c6-e71c-4383-8f0d-2a939ade243d",
        authority: "https://login.microsoftonline.com/52a7d760-d554-4751-bb71-cc3585633f2e",
        redirectUri: "/",
        postLogoutRedirectUri: "/"
    },
    system: {
        allowNativeBroker: false
    }
};

// TODO: Check if openid and profile scopes are added
export const loginRequest: PopupRequest = {
    scopes: ["User.Read"]
};