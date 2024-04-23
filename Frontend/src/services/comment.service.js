import {fetchAuth} from "../httpClient/fetchAuth.js";

export const commentService = {
  createComment
};

async function createComment(reviewId, commentData) {
  const {response, data} = await fetchAuth(`comment/assign?reviewId=${reviewId}`, true, {
    method: "POST",
    headers: {
      "Content-Type": "application/json"
    },
    body: JSON.stringify(commentData)
  });
  
  return {response, data};
}