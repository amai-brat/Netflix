import { createContext, useContext } from 'react';
import { observer, useLocalObservable } from 'mobx-react';
import { createDataStore} from './dataStore.jsx';

const Context = createContext(null);

export const DataStoreProvider = observer(({ children}) => {
    const store = useLocalObservable(createDataStore);
    return <Context.Provider value={store}>{children}</Context.Provider>;
});

export const useDataStore = () => {
    const store = useContext(Context);
    if (!store) throw new Error('Use App store within provider!');
    return store;
};