import {baseUrl} from "../Pages/Shared/HttpClient/baseUrl.js";
import {fetchAuth} from "../httpClient/fetchAuth.js";

export const contentService = {
  getContentInfo,
  createReview
};

async function getContentInfo(id) {
  const response = await fetch(`${baseUrl}content/${id}`);
  return {response, data: await response.json()};
}

async function createReview(reviewData) {
  const {response, data} = await fetchAuth("reviews/assign", false, {
    method: "POST",
    headers: {
      "Content-Type": "application/json"
    },
    body: JSON.stringify(reviewData)
  });
  
  return {response, data};
}