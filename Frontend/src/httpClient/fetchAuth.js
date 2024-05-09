import {baseUrl} from "./baseUrl.js";
import {jwtDecode} from "jwt-decode";

let isRefreshing = false;

const originalRequest = async (url, config, isJsonResponse, base)=> {
  url = `${base}${url}`
  let response = await fetch(url, config);
  let data;
  if (isJsonResponse) {
    try {
      data = await response.json();
    } catch (e) {
    }
  } else {
    data = await response.text();
  }
  
  return {response, data}
}

export const fetchAuth = async (url, isJsonResponse = false, config = {}, base = baseUrl) => {
  let accessToken = sessionStorage.getItem("accessToken");
  if (!config.headers) {
    config = {...config, headers: {}};
  }
  if (isTokenValid(accessToken)) {
    config['headers'].Authorization = `Bearer ${accessToken}`;
    let {response, data} = await originalRequest(url, config, isJsonResponse, base);
    if (response.status === 401) {
      accessToken = await refreshToken();
      config['headers'].Authorization = `Bearer ${accessToken}`;

      let newResponse = await originalRequest(url, config, isJsonResponse, base)
      response = newResponse.response
      data = newResponse.data
    }
    
    return {response, data}
  }
  accessToken = await refreshToken();
  config['headers'].Authorization = `Bearer ${accessToken}`;
  return await originalRequest(url, config, isJsonResponse, base);
}

const refreshToken = async () => {
  if (isRefreshing) {
    await new Promise(resolve => setTimeout(resolve, 100));
    return sessionStorage.getItem("accessToken");
  }
  
  isRefreshing = true;
  let response = await fetch(`${baseUrl}auth/refresh-token`, {
    method: "POST",
    credentials: "include"
  });
  let data = await response.text();
  isRefreshing = false;
  if (response.ok) {
    sessionStorage.setItem('accessToken', data);
    return data;
  }
  
  return "";
}

const isTokenValid = (token) => {
  try {
    const decoded = jwtDecode(token);
    return decoded.exp > new Date() / 1000;
  } catch {
    return false;
  }
}