import {baseUrl} from "../httpClient/baseUrl.js";

export const authenticationService = {
  signin,
  signup
};

async function signin(values){
  try {
    const resp = await fetch(`${baseUrl}auth/signin`, {
      method: "POST",
      credentials: "include",
      headers: {
        "Content-Type": "application/json"
      },
      body: JSON.stringify(values)
    });
    
    if (resp.ok) {
      const token = await resp.text();
      sessionStorage.setItem("accessToken", token);
      return {ok: true, data: token};
    } else {
      const error = await resp.json()
      return {ok: false, data: error};
    }
  } catch (e) {
    console.log(e);
    return {ok: false, data: e.message}
  }
}

async function signup(values) {
  try {
    const resp = await fetch(`${baseUrl}auth/signup`, {
      method: "POST",
      credentials: "include",
      headers: {
        "Content-Type": "application/json"
      },
      body: JSON.stringify(values)
    });

    if (resp.ok) {
      const token = await resp.text();
      return {ok: true, data: token};
    } else {
      const error = await resp.json()
      return {ok: false, data: error};
    }
  } catch (e) {
    console.log(e);
    return {ok: false, data: e.message}
  }
}