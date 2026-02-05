import { useState, useMemo, useEffect } from "react";
import { hrApi } from "../../../../api/hr.api";
import { masterApi } from "../../../../api/master.api";
import { XCircleIcon } from "@heroicons/react/24/outline";
import Swal from "sweetalert2";


/* ---------- debounce helper ---------- */
const debounce = (fn, delay = 400) => {
  let timer;
  return (...args) => {
    clearTimeout(timer);
    timer = setTimeout(() => fn(...args), delay);
  };
};

const CreateEmployeeModal = ({ onClose, onCreated }) => {
  const [photoBase64, setPhotoBase64] = useState(null);

  const [form, setForm] = useState({
    employeeCode: "",
    fullName: "",
    username: "",
    password: "",
    emailId: "",
    phoneNumber: "",
    dateOfJoining: "",
    roleId: 3,
    regionId: "",
    departmentId: "",
    designationId: "",
  });

  const[regions, setRegions] = useState([]);
  const[departments, setDepartments] = useState([]);
  const[designations, setDesignations] = useState([]);
  /* ---------- fetching Master data for Dropdowns ---------- */
  useEffect(() => {
    const loadMasters = async () => {
      try{
        const [regionsRes, deptRes, desigRes] = await Promise.all([
          masterApi.getAll("Region"),
          masterApi.getAll("Department"),
          masterApi.getAll("Designation"),
        ]);

        setRegions(regionsRes.data);
        setDepartments(deptRes.data);
        setDesignations(desigRes.data);
      }catch (err){
        console.error("Failed to load master data", err);
      }
    };
    loadMasters();
  }, []);

  const [errors, setErrors] = useState({});
  const [checking, setChecking] = useState({});

  /* ---------- backend validation ---------- */
  const validateField = async (field, value, apiFn) => {
    if (!value) {
      setErrors((p) => ({ ...p, [field]: null }));
      return;
    }

    setChecking((p) => ({ ...p, [field]: true }));

    try {
      const res = await apiFn(value);
      setErrors((p) => ({
        ...p,
        [field]: res.data.isValid ? null : res.data.message,
      }));
    } catch (err) {
      setErrors((p) => ({
        ...p,
        [field]:
          err.response?.data?.message ||
          "Validation service unavailable",
      }));
    } finally {
      setChecking((p) => ({ ...p, [field]: false }));
    }
  };

  /* ---------- debounced validators ---------- */
  const debouncedUsernameCheck = useMemo(
    () =>
      debounce((v) =>
        validateField("username", v, hrApi.validateUsername)
      ),
    []
  );

  const debouncedEmailCheck = useMemo(
    () =>
      debounce((v) =>
        validateField("emailId", v, hrApi.validateEmail)
      ),
    []
  );

  const debouncedPhoneCheck = useMemo(
    () =>
      debounce((v) =>
        validateField("phoneNumber", v, hrApi.validatePhone)
      ),
    []
  );

  const debouncedCodeCheck = useMemo(
    () =>
      debounce((v) =>
        validateField(
          "employeeCode",
          Number(v),
          hrApi.validateEmployeeCode
        )
      ),
    []
  );

  /* ---------- handlers ---------- */
  const handleChange = (e) => {
    const { name, value } = e.target;

    setForm((p) => ({
      ...p,
      [name]: ["roleId", "regionId", "departmentId", "designationId"].includes(
        name
      )
        ? Number(value)
        : value,
    }));
    setErrors((p) => ({ ...p, _form: null }));
  };

  const handlePhotoChange = (e) => {
    const file = e.target.files[0];
    if (!file) return;

    const reader = new FileReader();
    reader.onloadend = () => setPhotoBase64(reader.result);
    reader.readAsDataURL(file);
  };

  const handleSubmit = async () => {
    if (
      !form.employeeCode ||
      !form.fullName.trim() ||
      !form.username.trim() ||
      !form.password ||
      !form.emailId.trim() ||
      !form.phoneNumber.trim() ||
      !form.dateOfJoining
    ) {
      setErrors((p) => ({
        ...p,
        _form: "Please fill all required fields",
      }));
      return;
    }

    if (!form.regionId || !form.departmentId || !form.designationId) {
      setErrors((p) => ({
        ...p,
        _form: "Please select Region, Department and Designation",
      }));
      return;
    }

    if (!photoBase64) {
      setErrors((p) => ({
        ...p,
        _form: "Employee photo is required",
      }));
      return;
    }

    if (Object.values(errors).some(Boolean)) {
      setErrors((p) => ({
        ...p,
        _form: "Please fix validation errors",
      }));
      return;
    }

    try {
      await hrApi.createEmployee({
      ...form,
     dateOfJoining: new Date(form.dateOfJoining).toISOString(),
     photoBase64,
     });

   // ✅ Small Success Popup (Top Right)
   await Swal.fire({
   icon: "success",
   title: "Created",
   text: "Employee created successfully",
   position: "top-end",
   timer: 1200,
   showConfirmButton: false,
   width: 240,
   backdrop: false,

  customClass: {
    popup: "rounded-lg shadow-md p-3",
    title: "text-sm font-semibold text-green-700",
    htmlContainer: "text-xs text-gray-600",
  },
});

onCreated();
onClose();

    } catch (err) {
      setErrors((p) => ({
        ...p,
        _form: err.response?.data?.message || "Failed to create employee",
      }));
    }
  };

  return (
    <div className="fixed inset-0 bg-black/40 flex items-center justify-center">
      <div className="bg-white w-full max-w-4xl rounded-xl shadow-lg p-6 max-h-[85vh] flex flex-col">
        <div className="flex items-center justify-between mb-6">
          <h2 className="text-xl font-semibold">Create Employee</h2>
          <button
            onClick={onClose}
            className="rounded-full p-1 text-gray-500 hover:bg-gray-100 hover:text-gray-800 transition"
            aria-label="Close"
          >
            <XCircleIcon className="h-8 w-8" />
          </button>
        </div>

        {errors._form && (
          <div className="mb-4 rounded border border-red-300 bg-red-50 px-4 py-2 text-sm text-red-700">
            {errors._form}
          </div>
        )}

        <div className="overflow-y-auto px-6 pb-4">
          <div className="grid grid-cols-2 gap-4">

            <div>
              <label className="block text-sm font-medium mb-1">Employee Code *</label>
              <input
                name="employeeCode"
                type="number"
                placeholder="Employee Code"
                onChange={handleChange}
                onKeyUp={(e) => {
                  const v = Number(e.target.value);
                  if (!Number.isNaN(v)) {
                    debouncedCodeCheck(v);
                  }
                }}
                className={`border p-2 rounded w-full ${
                  errors.employeeCode ? "border-red-500" : ""
                }`}
              />
              {errors.employeeCode && (
                <p className="text-red-600 text-xs mt-1">
                  {errors.employeeCode}
                </p>
              )}
            </div>

            <div>
              <label className="block text-sm font-medium mb-1">Full Name *</label>
              <input
                name="fullName"
                placeholder="Full Name"
                onChange={handleChange}
                className="border p-2 rounded w-full"
              />
            </div>

            <div>
              <label className="block text-sm font-medium mb-1">Username *</label>
              <input
                name="username"
                placeholder="Username"
                onChange={handleChange}
                onKeyUp={(e) => debouncedUsernameCheck(e.target.value)}
                className={`border p-2 rounded w-full ${
                  errors.username ? "border-red-500" : ""
                }`}
              />
              {errors.username && (
                <p className="text-red-600 text-xs mt-1">
                  {errors.username}
                </p>
              )}
            </div>

            <div>
              <label className="block text-sm font-medium mb-1">Password *</label>
              <input
                type="text"
                name="password"
                placeholder="Password"
                onChange={handleChange}
                className="border p-2 rounded w-full"
              />
            </div>

            <div>
              <label className="block text-sm font-medium mb-1">Email *</label>
              <input
                type="email"
                name="emailId"
                placeholder="Email"
                onChange={handleChange}
                onKeyUp={(e) => debouncedEmailCheck(e.target.value)}
                className={`border p-2 rounded w-full ${
                  errors.emailId ? "border-red-500" : ""
                }`}
              />
              {errors.emailId && (
                <p className="text-red-600 text-xs mt-1">
                  {errors.emailId}
                </p>
              )}
            </div>

            <div>
              <label className="block text-sm font-medium mb-1">Phone Number *</label>
              <input
                name="phoneNumber"
                type="number"
                placeholder="Phone"
                onChange={handleChange}
                onKeyUp={(e) => debouncedPhoneCheck(e.target.value)}
                className={`border p-2 rounded w-full ${
                  errors.phoneNumber ? "border-red-500" : ""
                }`}
              />
              {errors.phoneNumber && (
                <p className="text-red-600 text-xs mt-1">
                  {errors.phoneNumber}
                </p>
              )}
            </div>

            <div>
              <label className="block text-sm font-medium mb-1">Date of Joining *</label>
              <input
                type="date"
                name="dateOfJoining"
                onChange={handleChange}
                className="border p-2 rounded w-full"
              />
            </div>

            <div>
              <label className="block text-sm font-medium mb-1">Role</label>
              <select
                name="roleId"
                value={form.roleId}
                onChange={handleChange}
                className="border p-2 rounded w-full"
              >
                <option value={3}>EMPLOYEE</option>
                <option value={2}>HR</option>
              </select>
            </div>

            <div>
              <label className="block text-sm font-medium mb-1">Region *</label>
              <select
                name="regionId"
                value={form.regionId}
                onChange={handleChange}
                className="border p-2 rounded w-full"
              >
                <option value="">Select Region</option>
                {regions.map((r) => (
                  <option key={r.id} value={r.id}>
                    {r.name}
                  </option>
                ))}
              </select>
            </div>

            <div>
              <label className="block text-sm font-medium mb-1 ">Department *</label>
              <select
                name="departmentId"
                value={form.departmentId}
                onChange={handleChange}
                className="border p-2 rounded w-full"
              >
                <option value="">Select Department</option>
                {departments.map((d) => (
                  <option key={d.id} value={d.id}>{d.name}</option>
                ))}
              </select>
            </div>

            <div>
              <label className="block text-sm font-medium mb-1">Designation *</label>
              <select
                name="designationId"
                value={form.designationId}
                onChange={handleChange}
                className="border p-2 rounded w-full mb-3"
              >
                <option value="">Select Designation</option>
                {designations.map((d) => (
                  <option key={d.id} value={d.id}>{d.name}</option>
                ))}
              </select>

              <label className="block text-sm font-medium mb-1">Employee Photo *</label>
              <input
                type="file"
                accept="image/*"
                onChange={handlePhotoChange}
                className="border p-2 rounded w-full"
              />
            </div>

            {photoBase64 && (
              <img
                src={photoBase64}
                alt="Preview"
                className="w-35 h-35 ml-10 object-cover rounded border"
              />
            )}
          </div>
        </div>

        <div className="flex justify-end gap-3 mt-8">
          <button
            onClick={onClose}
            className="px-4 py-2 border rounded"
          >
            Cancel
          </button>
          <button
            onClick={handleSubmit}
            className="px-4 py-2 bg-blue-600 text-white rounded"
          >
            Create
          </button>
        </div>
      </div>
    </div>
  );
};

export default CreateEmployeeModal;
