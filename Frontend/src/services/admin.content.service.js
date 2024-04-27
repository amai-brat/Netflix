import {fetchAuth} from "../httpClient/fetchAuth.js";

export const adminContentService = {
  deleteContent,
  getEditSerialContent,
  getEditMovieContent,
  addMovie,
  addSerial,
  updateMovie,
  updateSerial
};

async function deleteContent(idToDelete) {
  return await fetchAuth(`content/delete/${idToDelete}`, true, {
    method: "DELETE",
    headers: {
      "Content-Type": "application/json"
    }
  });
}
 
async function getEditSerialContent(editSerialId) {
  return await fetchAuth(`content/admin/serial/${editSerialId}`, true, {
    method: "GET",
    headers: {
      "Content-Type": "application/json"
    }
  });
}

async function getEditMovieContent(editMovieId) {
  return await fetchAuth(`content/admin/movie/${editMovieId}`, true, {
    method: "GET",
    headers: {
      "Content-Type": "application/json"
    }
  });
}

async function addMovie(values) {
  const formData = new FormData();
  for (const key in values) {
    if (values[key] instanceof Array) {
      if (key === "personsInContent") {
        values[key].forEach((item, index) => formData.append(`${key}[${index}].Name`, item.name));
        values[key].forEach((item, index) => formData.append(`${key}[${index}].Profession`, item.profession));
      } else if (key === "allowedSubscriptions") {
        values[key].forEach((item, index) => formData.append(`${key}[${index}].Name`, item.name));
        values[key].forEach((item, index) => formData.append(`${key}[${index}].MaxResolution`, item.maxResolution));
      } else {
        values[key].forEach((item, index) => formData.append(`${key}[${index}]`, item));
      }
    } else {
      formData.append(key, values[key]);
    }
  }
  return await fetchAuth(`content/movie/add`, true, {
    method: "POST",
    body: formData
  });
}


async function addSerial(values) {
  return await fetchAuth(`content/serial/add`, true, {
    method: "POST",
    headers: {
      "Content-Type": "application/json"
    },
    body: JSON.stringify(values)
  });
}

async function updateMovie(id, values) {
  const formData = new FormData();
  for (const key in values) {
    if (values[key] instanceof Array) {
      if (key === "personsInContent"){
        values[key].forEach((item,index) => formData.append(`${key}[${index}].Name`, item.name));
        values[key].forEach((item,index) => formData.append(`${key}[${index}].Profession`, item.profession));
      } else if(key === "allowedSubscriptions"){
        values[key].forEach((item,index) => formData.append(`${key}[${index}].Name`, item.name));
        values[key].forEach((item,index) => formData.append(`${key}[${index}].MaxResolution`, item.maxResolution));
      }
      else{
        values[key].forEach((item,index) => formData.append(`${key}[${index}]`, item));
      }
    } else {
      formData.append(key, values[key]);
    }
  }
  return await fetchAuth(`content/movie/update/${id}`, true, {
    method: "POST",
    body: formData
  });
}

async function updateSerial(id, values) {
  return await fetchAuth(`content/serial/update/${id}`, true, {
    method: "POST",
    headers: {
      "Content-Type": "application/json"
    },
    body: JSON.stringify(values)
  });
}