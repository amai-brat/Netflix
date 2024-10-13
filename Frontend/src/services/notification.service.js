import {fetchAuth} from "../httpClient/fetchAuth.js";

export const notificationService = {
  getNotificationHistory,
};

async function getNotificationHistory() {
  const {response, data} = await fetchAuth(`comment/notifications`, true);
  return {response, data};
}

// не используется из-за signalr
// eslint-disable-next-line no-unused-vars
async function setNotificationRead(notificationId){
  const {response, data} = await fetchAuth(`comment/set/readed?notificationId=${notificationId}`, false, {
    method: "POST"
  });
  return {response, data};
} 