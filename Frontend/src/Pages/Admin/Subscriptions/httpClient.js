import {baseUrl} from "../../Shared/HttpClient/baseUrl.js";

export const getSubscriptions = async () => {
  try {
    const response = await fetch(baseUrl + "admin/subscription/all", {
      method: "GET",
      headers: {
        // TODO: auth token
        // "Authorization": "Bearer [token]"
      }
    });

    if (response.ok) {
      const data = await response.json();
      return {ok: true, data: data}
    } else {
      return {ok: false, data: await response.text()}
    }
  } catch (e) {
    console.log(e);
  }
}