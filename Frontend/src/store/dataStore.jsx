import {makeAutoObservable} from 'mobx';
import initialState from "./initialState.jsx";

class DataStore {
    constructor() {
        makeAutoObservable(this)
    }
    data = initialState

    setChatSession = (session) => {
        this.data.chatSessions = session;
    }

    removeSession = () => {
        this.data.chatSessions = null;
    }

    setIsSignIn = (isSignIn) => {
        this.data.isSignIn = isSignIn
    }
}

const dataStore = new DataStore();
export const createDataStore = () => { return dataStore};