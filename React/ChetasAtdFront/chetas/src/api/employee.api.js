import api from "./axiosClient";

export const employeeApi = {
  getProfile: () => api.get("/employee/profile"),
  getPerformance: () => api.get("/employee/attendance/summary/current-month"),
};
