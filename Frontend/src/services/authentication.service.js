import {baseUrl} from "../httpClient/baseUrl.js";
import {fetchAuth} from "../httpClient/fetchAuth.js";
import {jwtDecode} from "jwt-decode";

export const authenticationService = {
  signin,
  signup,
  logout,
  getUser
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

async function logout() {
  try {
    const response = await fetch(`${baseUrl}auth/revoke-token`,{
      method: "POST",
      credentials: "include"
    });
    if (response.ok) {
      sessionStorage.removeItem("accessToken");
      return {response, data: await response.text()};
    }
    
    return {response, data: await response.json()}
  } catch (e) {
    console.log(e);
  }
}

function getUser() {
  const token = sessionStorage.getItem("accessToken");
  if (!token) {
    return null
  }
  return jwtDecode(token);
}