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
    
    setSupportConnection = (connection) => {
        this.data.supportConnection = connection
    }
}

const dataStore = new DataStore();
export const createDataStore = () => { return dataStore};