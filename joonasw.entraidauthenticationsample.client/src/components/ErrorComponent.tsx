import { MsalAuthenticationResult } from "@azure/msal-react"

export const ErrorComponent: React.FC<MsalAuthenticationResult> = ({ error }) => {
    return <h6>An error occurred: {error ? error.errorCode : "Unknown error"}</h6>
}