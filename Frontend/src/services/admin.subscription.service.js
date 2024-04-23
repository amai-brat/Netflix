import {fetchAuth} from "../httpClient/fetchAuth.js";

export const adminSubscriptionService = {
  getSubscriptions,
  deleteSubscription,
  getContentsForSubscription,
  addSubscription,
  editSubscription
};

async function getSubscriptions() {
  const {response, data} = await fetchAuth(`admin/subscription/all`, true);
  return {response, data};
}

async function deleteSubscription(subscriptionId) {
  const {response, data} = await fetchAuth(`admin/subscription/delete/${subscriptionId}`, true, {
    method: "DELETE"
  });
  
  return {response, data};
}

async function getContentsForSubscription(){
  return await fetchAuth("admin/subscription/contents", true);
}

async function addSubscription(values) {
  return await fetchAuth("admin/subscription/add", true, {
    method: "POST",
    headers: {
      "Content-Type": "application/json"
    },
    body: JSON.stringify(values)
  });
}

async function editSubscription(dto) {
  return await fetchAuth("admin/subscription/edit", true, {
    method: "PUT",
    headers: {
      "Content-Type": "application/json"
    },
    body: JSON.stringify(dto)
  });
}