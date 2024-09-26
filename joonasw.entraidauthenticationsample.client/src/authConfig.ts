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

export const loginRequest: PopupRequest = {
    // openid, profile, and offline_access scopes are added by default
    scopes: ["api://ce9bb3c6-e71c-4383-8f0d-2a939ade243d/Forecasts.Read User.Read"]
};

export const apiRequest = {
    scopes: ["api://ce9bb3c6-e71c-4383-8f0d-2a939ade243d/Forecasts.Read"]
};