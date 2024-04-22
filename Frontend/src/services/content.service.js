import {baseUrl} from "../Pages/Shared/HttpClient/baseUrl.js";
import {fetchAuth} from "../httpClient/fetchAuth.js";

export const contentService = {
  getContentInfo,
  getReviewsCount,
  getReviews,
  createReview
};

async function getContentInfo(id) {
  const response = await fetch(`${baseUrl}content/${id}`);
  return {response, data: await response.json()};
}

async function getReviewsCount(contentId) {
  const response = await fetch(`${baseUrl}reviews/count/${contentId}`);
  return {response, data: await response.json()};
}

async function getReviews(contentId, itemOffset, itemsPerPage, sort) {
  const response = await fetch(`${baseUrl}reviews/${contentId}/?offset=${itemOffset}&limit=${itemsPerPage}&sort=${sort}`);
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