 import api from "./axiosClient";

export const masterApi = {
  getAll: (master) =>
    api.get(`/${master}`),

  create: (master, data) =>
    api.post(`/${master}`, data),

  update: (master, id, data) =>
    api.put(`/${master}/${id}`, data),

  remove: (master, id) =>
    api.delete(`/${master}/${id}`),
};
