import {fetchAuth} from "../httpClient/fetchAuth.js";

export const userService = {
  getReviewsPagesCount,
  getReviews,
  getFavourites,
  getPersonalInfo,
  changeProfilePicture,
  changeEmail,
  changeBirthDay,
  changePassword
};

async function getReviewsPagesCount(search) {
  const {data} = await fetchAuth(`user/get-reviews-pages-count?input=${search}`,true);
  return +data;
}

async function getReviews(sort, search, page) {
  const {data} = await fetchAuth(`user/get-reviews?sort=${sort}&input=${search}&page=${page - 1}`, true);
  return data;
}

async function getFavourites() {
  const {response, data} = await fetchAuth(`user/get-favourites`, true);
  return {response, data};
}

async function getPersonalInfo() {
  const {response, data} = await fetchAuth(`user/get-personal-info`, true);
  return {response, data};
}

async function changeProfilePicture(formData) {
  const {response, data} = await fetchAuth("user/change-profile-picture", true, {
    method: "PATCH",
    body: formData
  });
  return {response, data};
}

async function changeEmail(email) {
  return await fetchAuth("user/change-email", true, {
    method: "PATCH",
    headers: {
      "Content-Type": "application/json"
    },
    body: JSON.stringify(email)
  });
}

async function changeBirthDay(birthday) {
  return await fetchAuth("user/change-birthday", true, {
    method: "PATCH",
    headers: {
      "Content-Type": "application/json"
    },
    body: JSON.stringify(birthday)
  });
}

async function changePassword(previousPassword, newPassword) {
  return await fetchAuth("user/change-password", true, {
    method: "PATCH",
    headers: {
      "Content-Type": "application/json"
    },
    body: JSON.stringify({previousPassword, newPassword})
  });
}