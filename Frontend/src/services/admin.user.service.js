import {fetchAuth} from "../httpClient/fetchAuth.js";

export const adminUserService = {
  makeModerator
}

async function makeModerator(userId) {
  return await fetchAuth("admin/usermanagement/changeRole", true, {
    method: "PATCH",
    headers: {
      "Content-Type": "application/json"
    },
    body: JSON.stringify({userId: userId, role: "moderator"})
  });
}