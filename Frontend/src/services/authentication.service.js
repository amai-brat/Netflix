import {baseUrl} from "../httpClient/baseUrl.js";
import {fetchAuth} from "../httpClient/fetchAuth.js";
import {jwtDecode} from "jwt-decode";

export const authenticationService = {
  signin,
  signup,
  logout,
  externalSignIn,
  enableTwoFactorAuth,
  getWhetherTwoFactorEnabled,
  sendTwoFactorToken,
  getUser,
  refreshToken,
  refreshTokenIfNotExpired,
  isCurrentUserModerator,
  isCurrentUserAdmin,
  isCurrentUserSupport
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

async function externalSignIn(provider, code){
  try {
    const resp = await fetch(`${baseUrl}auth/external/${provider}`, {
      method: "POST",
      credentials: "include",
      headers: {
        "Content-Type": "application/json"
      },
      body: JSON.stringify({Code: code})
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

async function getWhetherTwoFactorEnabled() {
  const {response, data} = await fetchAuth("auth/is-enabled-2fa", true);
  return {response, data};
}

async function enableTwoFactorAuth() {
  return await fetchAuth("auth/enable-2fa", true, {method: "POST"});
}

async function sendTwoFactorToken(token, rememberMe) {
  try {
    const resp = await fetch(`${baseUrl}auth/send-2fa`, {
      method: "POST",
      credentials: "include",
      headers: {
        "Content-Type": "application/json"
      },
      body: JSON.stringify({token, rememberMe})
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

async function refreshToken() {
  try {
    let response = await fetch(`${baseUrl}auth/refresh-token`, {
      method: "POST",
      credentials: "include"
    });

    if (response.ok) {
      let data = await response.text();
      sessionStorage.setItem('accessToken', data);
      return data;
    } else {
      const error = await response.json()
      return {ok: false, data: error};
    }
  } catch (e) {
    console.log(e);
    return {ok: false, data: e.message}
  }
}

async function refreshTokenIfNotExpired() {
  let token = sessionStorage.getItem("accessToken");
  if (!token || jwtDecode(token).exp + 10 < new Date() / 1000) {
      try {
          const response = await fetch(`${baseUrl}auth/refresh-token`, {
              method: "POST",
              credentials: "include"
          });
          if (response.ok) {
            token = await response.text();
            sessionStorage.setItem('accessToken', token);
          }
      }
      // eslint-disable-next-line no-empty
      catch {}
  }

  return token;
}

function getUser() {
  const token = sessionStorage.getItem("accessToken");
  if (!token) {
    return null
  }
  return jwtDecode(token);
}

function isCurrentUserModerator() {
  const roles = getUser()?.role;
  if (!roles) return false;
  return roles.includes("moderator") || roles.includes("admin");
}

function isCurrentUserAdmin() {
  return getUser()?.role?.includes("admin") ?? false;
}

function isCurrentUserSupport() {
  return getUser()?.role?.includes("support") ?? false;
}