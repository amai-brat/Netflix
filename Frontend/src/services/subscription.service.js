import {baseSubscriptionUrl} from "../httpClient/baseUrl.js";
import {fetchAuth} from "../httpClient/fetchAuth.js";

export const subscriptionService = {
  getAllSubscriptions,
  getPurchasedSubscriptions,
  buySubscription,
  getSubscriptionById,
  unsubscribe
};

async function getAllSubscriptions() {
  const response = await fetch(`${baseSubscriptionUrl}subscription/getAllSubscriptions`);
  return {response, data: await response.json()};
}

async function getPurchasedSubscriptions() {
  return await fetchAuth("subscription/getUserSubscriptions", true, {}, baseSubscriptionUrl);
}

async function buySubscription(values) {
  return await fetchAuth("subscription/buySubscription", true, {
    method: "POST",
    headers: {
      "Content-Type": "application/json"
    },
    body: JSON.stringify(values)
  }, baseSubscriptionUrl);
}

async function getSubscriptionById(subscriptionId) {
  const response = await fetch(`${baseSubscriptionUrl}subscription/getSubscriptionById?subscriptionId=${subscriptionId}`);
  return {response, data: await response.json()};
}

async function unsubscribe(subscriptionId) {
  return await fetchAuth(`subscription/cancelSubscription`, true, {
    method: "DELETE",
    headers: {
      "Content-Type": "application/json"
    },
    body: JSON.stringify({subscriptionId: subscriptionId})
  }, baseSubscriptionUrl);
}