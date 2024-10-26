import { useEffect } from 'react';
import * as signalR from '@microsoft/signalr';
import { useDataStore } from '../store/dataStoreProvider';
import { authenticationService } from '../services/authentication.service';

const useSupportConnection = (baseSupportUrl) => {
    const store = useDataStore(); 

    useEffect(() => {
        const supportConnection = new signalR.HubConnectionBuilder()
            .withUrl(baseSupportUrl + "hub/support", {
                accessTokenFactory: async () => await authenticationService.refreshTokenIfNotExpired(),
            })
            .configureLogging(signalR.LogLevel.Information)
            .build();

        supportConnection.start()
            .then(() => {
                store.setSupportConnection(supportConnection);
            })
            .catch(err => console.error(err));

        return () => {
            supportConnection.stop().catch(err => console.error('Error stopping connection:', err));
        };
    }, [baseSupportUrl, store]);
};

export default useSupportConnection;
