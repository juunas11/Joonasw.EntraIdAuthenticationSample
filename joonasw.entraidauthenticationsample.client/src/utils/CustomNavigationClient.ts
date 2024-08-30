import { NavigationClient, NavigationOptions } from "@azure/msal-browser";
import { NavigateFunction } from "react-router-dom";

export class CustomNavigationClient extends NavigationClient {
    private navigate: NavigateFunction;

    constructor(navigate: NavigateFunction) {
        super();
        this.navigate = navigate;
    }

    async navigateInternal(url: string, options: NavigationOptions): Promise<boolean> {
        const relativePath = url.replace(window.location.origin, "");
        if (options.noHistory) {
            this.navigate(relativePath, { replace: true });
        } else {
            this.navigate(relativePath);
        }

        return false;
    }
}