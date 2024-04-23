import {baseUrl} from "../httpClient/baseUrl.js";
import {fetchAuth} from "../httpClient/fetchAuth.js";

export const contentService = {
  getContentInfo,
  getReviewsCount,
  getReviews,
  createReview,
  likeReview,
  addToFavourites,
  removeFromFavourites,
  getSections,
  getPromos,
  getContentsByFilter,
  getContentTypes,
  getGenres
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

async function likeReview(reviewId) {
  const {response, data} = await fetchAuth(`reviews/like/${reviewId}`, true, {
    method: "POST"
  });
  
  return {response, data};
}

async function addToFavourites(contentId) {
  const {response, data} = await fetchAuth(`content/favourite/add`, true, {
    method: "POST",
    headers: {
      "Content-Type": "application/json"
    },
    body: JSON.stringify(contentId)
  });
  
  return {response, data};
}

async function removeFromFavourites(contentId) {
  const {response, data} = await fetchAuth(`content/favourite/remove`, true, {
    method: "POST",
    headers: {
      "Content-Type": "application/json"
    },
    body: JSON.stringify(contentId)
  });
  
  return {response, data};
}

async function getSections() {
  const response = await fetch(`${baseUrl}content/sections`);
  return {response, data: await response.json()};
}

async function getPromos() {
  const response = await fetch(`${baseUrl}content/promos`);
  return {response, data: await response.json()};
}

async function getContentsByFilter(queryParams) {
  const response = await fetch(`${baseUrl}content/filter?${queryParams}`);
  return {response, data: await response.json()};
}

async function getContentTypes() {
  const response = await fetch(`${baseUrl}content/types`);
  return {response, data: await response.json()};
}

async function getGenres() {
  const response = await fetch(`${baseUrl}content/genres`);
  return {response, data: await response.json()};
}