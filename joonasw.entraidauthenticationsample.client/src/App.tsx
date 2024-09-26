import './App.css';
import { IPublicClientApplication } from '@azure/msal-browser';
import { MsalProvider } from '@azure/msal-react';
import { Home } from './pages/Home';
import { Route, Routes, useNavigate } from 'react-router-dom';
import { CustomNavigationClient } from './utils/CustomNavigationClient';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';

const queryClient = new QueryClient({
    defaultOptions: {
        queries: {
            refetchOnWindowFocus: false,
            retry: false
        }
    }
});

interface AppProps {
    pca: IPublicClientApplication
}

function App({ pca }: AppProps) {
    // These are needed in order for MSAL to navigate between client-side routes
    const navigate = useNavigate();
    const navigationClient = new CustomNavigationClient(navigate);
    pca.setNavigationClient(navigationClient);

    return (
        <MsalProvider instance={pca}>
            <QueryClientProvider client={queryClient}>
                <Routes>
                    <Route path="/" element={<Home />} />
                </Routes>
            </QueryClientProvider>
        </MsalProvider>
    )
}

export default App;