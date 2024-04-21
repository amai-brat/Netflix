import {fetchAuth} from "../httpClient/fetchAuth.js";

export const userService = {
  getReviewsPagesCount,
  getReviews
};

async function getReviewsPagesCount(search) {
  const {data} = await fetchAuth(`user/get-reviews-pages-count?input=${search}`,true);
  return +data;
}

async function getReviews(sort, search, page) {
  const {data} = await fetchAuth(`user/get-reviews?sort=${sort}&input=${search}&page=${page - 1}`, true);
  return data;
}