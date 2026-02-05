import api from "./axiosClient";

export const hrApi = {
    getRegions: () => api.get("/hr/regions"),
    // getSubRegions: (regionId) => api.get(`/hr/subregions?regionId=${regionId}`),

    getCheckIn: (regionId) => api.get(`/hr/pending-checkin?regionId=${regionId}`),
    getCheckOut: (regionId) => api.get(`/hr/pending-checkout?regionId=${regionId}`),

    verifyAttendance: (attendanceId, status , type) =>
        api.post("/hr/verify", { attendanceId, status,type}),


    //Masters
    getEmployees : () => api.get("/hr/all-employees"),
    createEmployee: (payload) => api.post("/hr/employee", payload),
    updateEmployee: (employeeId, payload) => api.put(`/hr/employee/${employeeId}`, payload),
    deleteEmployee: (employeeId) => api.delete(`/hr/employee/${employeeId}`),

    validateUsername: (value) => api.get(`/hr/validate/username?value=${value}`),
    validateEmail: (value) => api.get(`/hr/validate/email?value=${value}`),
    validatePhone: (value) => api.get(`/hr/validate/phone?value=${value}`),
    validateEmployeeCode: (value) =>
        api.get(`/hr/validate/employee-code?value=${value}`),
};