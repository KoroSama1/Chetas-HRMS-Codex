import axios from "axios";
import { tokenManager } from "../auth/tokenManager";

const api = axios.create({
  baseURL: import.meta.env.VITE_API_BASE_URL, 
  withCredentials: true, 
});

/* ================= REQUEST INTERCEPTOR ================= */
api.interceptors.request.use(
  (config) => {
    const token = tokenManager.get();
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => Promise.reject(error)
);

/* ================= RESPONSE INTERCEPTOR ================= */
let isRefreshing = false;
let refreshQueue = [];

api.interceptors.response.use(
  (response) => response,
  async (error) => {
    if (!error.response) {
      return Promise.reject(error);
    }
    
    const originalRequest = error.config;


    const isAuthEndpoint =
      originalRequest.url.includes("/auth/login") ||
      originalRequest.url.includes("/auth/refresh") ||
      originalRequest.url.includes("/auth/me");

    if (
      error.response.status === 401 &&
      !originalRequest._retry &&
      !isAuthEndpoint
    ) {
      originalRequest._retry = true;

      // If refresh already running, queue request
      if (isRefreshing) {
        return new Promise((resolve, reject) => {
          refreshQueue.push({ resolve, reject });
        }).then(() => api(originalRequest));
      }

      isRefreshing = true;

      try {
        // 🔥 REFRESH TOKEN (cookie-based)
        const refreshRes = await api.post("/auth/refresh");

        // 🔥 THIS WAS THE MISSING LINE
        tokenManager.set(refreshRes.data.accessToken);

        // Ensure retried request uses the new access token
        originalRequest.headers.Authorization = `Bearer ${refreshRes.data.accessToken}`;

        // Resolve queued requests
        refreshQueue.forEach((p) => p.resolve());
        refreshQueue = [];

        return api({
          ...originalRequest,
          headers: {
            ...originalRequest.headers,
            Authorization: `Bearer ${refreshRes.data.accessToken}`,
          },
        });
      } catch (refreshError) {
        // Refresh failed → logout
        tokenManager.clear();
        refreshQueue.forEach((p) => p.reject(refreshError));
        refreshQueue = [];
        window.location.href = "/login";
        return Promise.reject(refreshError);
      } finally {
        isRefreshing = false;
      }
    }

    return Promise.reject(error);
  }
);

export default api;
