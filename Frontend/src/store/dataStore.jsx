import {makeAutoObservable} from 'mobx';
import initialState from "./initialState.jsx";

class DataStore {
    constructor() {
        makeAutoObservable(this)
    }
    data = initialState

    setConnection = (connection) => {
        this.data.connection = connection
    }
    
    setChatSession = (session) => {
        this.data.chatSession = session;
    }

    removeSession = () => {
        this.data.chatSession = null;
    }

    setIsSignIn = (isSignIn) => {
        this.data.isSignIn = isSignIn
    }
}

const dataStore = new DataStore();
export const createDataStore = () => { return dataStore};