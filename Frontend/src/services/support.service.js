import {fetchAuth} from "../httpClient/fetchAuth.js";
import {baseSupportHubUrl, baseSupportUrl} from "../httpClient/baseUrl.js";

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

async function uploadChatFiles(formData) {
    const {response, data} = await fetchAuth('send-message-with-file', true, {
        method: "POST",
        body: formData
    }, baseSupportHubUrl)
    return {response, data}
}