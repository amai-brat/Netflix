import {fetchAuth} from "../httpClient/fetchAuth.js";
import {baseSupportUrl} from "../httpClient/baseUrl.js";

export const supportService = {
    getUserSupportMessagesHistory,
    getSupportUsersUnansweredMessagesHistory,
    getSupportUserMessagesHistory,
    uploadChatFiles,
};


async function getUserSupportMessagesHistory() {
    const {response, data} = await fetchAuth('support/chats/user/messages', true, {}, baseSupportUrl)
    return {response, data}
}

async function getSupportUserMessagesHistory(id) {
    const {response, data} = await fetchAuth(`support/chats/${id}/messages`, true, {}, baseSupportUrl)
    return {response, data}
}

async function getSupportUsersUnansweredMessagesHistory() {
    const {response, data} = await fetchAuth('support/chats/unanswered', true, {}, baseSupportUrl)
    return {response, data}
}

async function uploadChatFiles(id, formData) {
    const {response, data} = await fetchAuth(`support/chats/${id}/files/upload`, true, {
        method: "POST",
        body: formData
    }, baseSupportUrl)
    return {response, data}
}