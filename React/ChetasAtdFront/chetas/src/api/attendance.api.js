import api from "./axiosClient";

export const attendanceApi = {
  checkIn: (payload) => api.post("/employee/check-in", payload),
  checkOut: (payload) => api.post("/employee/check-out", payload),
};
