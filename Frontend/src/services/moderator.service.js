import {fetchAuth} from "../httpClient/fetchAuth.js";

export const moderatorService = {
  deleteComment,
  deleteReview
}

async function deleteComment(commentId) {
  return await fetchAuth(`admin/usermanagement/deleteComment/${commentId}`, true, {
    method: "DELETE"
  });
}

async function deleteReview(reviewId) {
  return await fetchAuth(`admin/usermanagement/deleteReview/${reviewId}`, true, {
    method: "DELETE"
  });
}