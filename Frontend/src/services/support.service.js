import {fetchAuth} from "../httpClient/fetchAuth.js";
import {baseSupportUrl} from "../httpClient/baseUrl.js";

export const supportService = {
    getUserSupportMessagesHistory,
    getSupportUsersUnansweredMessagesHistory,
    getSupportUserMessagesHistory
};


async function getUserSupportMessagesHistory() {
    const {response, data} = await fetchAuth('get/user/messages', true, {}, baseSupportUrl)
    return {response, data}
}

async function getSupportUserMessagesHistory(id) {
    const {response, data} = await fetchAuth(`get/support/messages/${id}`, true, {}, baseSupportUrl)
    return {response, data}
}

async function getSupportUsersUnansweredMessagesHistory() {
    const {response, data} = await fetchAuth('get/support/messages', true, {}, baseSupportUrl)
    return {response, data}
}