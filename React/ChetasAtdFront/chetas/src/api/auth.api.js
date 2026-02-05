import api from "./axiosClient";

export const authApi = {
  login: (credentials) => api.post("/auth/login", credentials),

  refresh: () => api.post("/auth/refresh"),

  me: () => api.get("/auth/me"),

  logout: () => api.post("/auth/logout"),
};
