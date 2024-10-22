import {fetchAuth} from "../httpClient/fetchAuth.js";
import {baseSupportUrl} from "../httpClient/baseUrl.js";

//TODO: заменить url если отличаются

export const supportService = {
    getUserSupportMessagesHistory
};


async function getUserSupportMessagesHistory() {
    const {response, data} = await fetchAuth('get/messages', true, {}, baseSupportUrl)
    return {response, data}
}
