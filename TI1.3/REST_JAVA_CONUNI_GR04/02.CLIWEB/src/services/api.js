import axios from 'axios';

const API_BASE_URL = 'http://localhost:8080/WS_ConUni_REST_JAVA_GR04/webresources/ConUni';

const api = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
    'Accept': 'application/json'
  }
});

export const conversions = {
  cmToIn: (value) => api.get(`/cm-to-in?value=${value}`),
  inToCm: (value) => api.get(`/in-to-cm?value=${value}`),
  cToF: (value) => api.get(`/c-to-f?value=${value}`),
  fToC: (value) => api.get(`/f-to-c?value=${value}`),
  kgToLb: (value) => api.get(`/kg-to-lb?value=${value}`),
  lbToKg: (value) => api.get(`/lb-to-kg?value=${value}`)
};

export default api;