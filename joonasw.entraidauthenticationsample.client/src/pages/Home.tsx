import { InteractionType } from "@azure/msal-browser";
import { MsalAuthenticationTemplate, useAccount } from "@azure/msal-react";
import { ErrorComponent } from "../components/ErrorComponent";
import { LoadingComponent } from "../components/LoadingComponent";
import { loginRequest } from "../authConfig";
import { useQuery } from "@tanstack/react-query";
import { msalInstance } from "../main";

interface Forecast {
    date: string;
    temperatureC: number;
    temperatureF: number;
    summary: string;
}

const HomeContent = () => {
    const account = useAccount();
    const { data, isLoading } = useQuery<Forecast[]>({
        queryKey: ['weather', account?.homeAccountId],
        initialData: [],
        enabled: !!account,
        queryFn: async () => {
            const tokenResponse = await msalInstance.acquireTokenSilent({
                ...loginRequest,
                account: account!
            });
            const headers = new Headers();
            headers.append("Authorization", `Bearer ${tokenResponse.accessToken}`);

            const response = await fetch('http://localhost:5167/weatherforecast', {
                headers: headers,
                method: 'GET'
            });
            return await response.json();
        }
    });
    useQuery({
        queryKey: ['graph', account?.homeAccountId],
        enabled: !!account,
        queryFn: async () => {
            const tokenResponse = await msalInstance.acquireTokenSilent({
                ...loginRequest,
                account: account!
            });
            const headers = new Headers();
            headers.append("Authorization", `Bearer ${tokenResponse.accessToken}`);

            const response = await fetch('http://localhost:5167/graph/me', {
                headers: headers,
                method: 'GET'
            });
            return await response.json();
        }
    });

    const contents = isLoading
        ? <p><em>Loading... Please refresh once the ASP.NET backend has started. See <a href="https://aka.ms/jspsintegrationreact">https://aka.ms/jspsintegrationreact</a> for more details.</em></p>
        : <table className="table table-striped" aria-labelledby="tableLabel">
            <thead>
                <tr>
                    <th>Date</th>
                    <th>Temp. (C)</th>
                    <th>Temp. (F)</th>
                    <th>Summary</th>
                </tr>
            </thead>
            <tbody>
                {data.map(forecast =>
                    <tr key={forecast.date}>
                        <td>{forecast.date}</td>
                        <td>{forecast.temperatureC}</td>
                        <td>{forecast.temperatureF}</td>
                        <td>{forecast.summary}</td>
                    </tr>
                )}
            </tbody>
        </table>;

    return (
        <>
            <div>
                <h1 id="tableLabel">Weather forecast</h1>
                <p>This component demonstrates fetching data from the server.</p>
                {contents}
            </div>
            <div>
                <h2>ID token claims for signed in user</h2>
                {account && <pre id="userClaims">{JSON.stringify(account.idTokenClaims, null, 2)}</pre>}
            </div>
        </>
    );
}

export function Home() {
    const authRequest = {
        ...loginRequest
    };
    return (
        <MsalAuthenticationTemplate
            interactionType={InteractionType.Redirect}
            errorComponent={ErrorComponent}
            loadingComponent={LoadingComponent}
            authenticationRequest={authRequest}>
            <HomeContent />
        </MsalAuthenticationTemplate>
    );
}