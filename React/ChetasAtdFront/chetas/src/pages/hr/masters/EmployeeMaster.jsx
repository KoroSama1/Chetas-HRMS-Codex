import { useState, useEffect } from "react";
import EmployeeTable from "./empComponents/EmployeeTable";
import CreateEmployeeModal from "./empComponents/CreateEmployeeModal";
import UpdateEmployeeModal from "./empComponents/UpdateEmployeeModal";
import HRHeader from "../components/HRHeader";
import { hrApi } from "../../../api/hr.api";
import { useAuth } from "../../../auth/AuthContext";import Swal from "sweetalert2";



const EmployeeMaster = () => {
  const [showCreate, setShowCreate] = useState(false);
  const [showEdit, setShowEdit] = useState(false);
  const [selectedEmployee, setSelectedEmployee] = useState(null);
  const [employees, setEmployees] = useState([]);
  const [loading, setLoading] = useState(true);
  const { logout } = useAuth();

  const fetchEmployees = async () => {
    setLoading(true);
    try{
      const res = await hrApi.getEmployees();
      setEmployees(res.data);
    }catch (err){
      console.error("Failed to load employees", err);
    }finally{
      setLoading(false);
    }
  };
  useEffect(() => {
    fetchEmployees();
  }, []);

  const handleDelete = async (employeeId) => {
  // 🔔 Confirmation Popup (Tailwind Styled)
  const result = await Swal.fire({
    icon: "warning",
    title: "Delete Employee?",
    text: "Are you sure you want to delete this employee?",
    showCancelButton: true,
    confirmButtonText: "Yes, Delete",
    cancelButtonText: "Cancel",
    width: 260,
    backdrop: false,

    customClass: {
      popup: "rounded-lg shadow-lg p-3",
      title: "text-sm font-semibold text-gray-800",
      htmlContainer: "text-xs text-gray-600",
      confirmButton: "bg-red-600 hover:bg-red-700 text-white px-3 py-1 rounded text-xs",
      cancelButton: "bg-gray-200 hover:bg-gray-300 text-gray-800 px-3 py-1 rounded text-xs ml-2",
    },
  });

  if (!result.isConfirmed) return;

  try {
    await hrApi.deleteEmployee(employeeId);

    setEmployees(prev => prev.filter(e => e.employeeId !== employeeId));

    // ✅ Success Popup (Top Right, Tailwind Styled)
    Swal.fire({
      icon: "success",
      title: "Deleted",
      text: "Employee deleted successfully",
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

  } catch (err) {
    const msg = err.response?.data?.message || "Failed to delete employee";

    // ❌ Error Popup (Top Right, Tailwind Styled)
    Swal.fire({
      icon: "error",
      title: "Delete Failed",
      text: msg,
      position: "top-end",
      timer: 1500,
      showConfirmButton: false,
      width: 240,
      backdrop: false,

      customClass: {
        popup: "rounded-lg shadow-md p-3",
        title: "text-sm font-semibold text-red-700",
        htmlContainer: "text-xs text-gray-600",
      },
    });
  }
};


  return (
    <div className="p-6 bg-gray-50 min-h-screen">
        <HRHeader onLogout={logout} />
      {/* Header */}
      <div className="flex justify-between items-center mb-6">
        <h1 className="text-2xl font-semibold">Employees</h1>

        <div className="flex gap-3">
          <button
            onClick={() => setShowCreate(true)}
            className="bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700"
          >
            + Create Employee
          </button>
        </div>
      </div>

      {/* Filters (UI only) */}
      <div className="flex gap-3 mb-4">
        <button className="border px-3 py-1 rounded">Region</button>
        <button className="border px-3 py-1 rounded">Department</button>
        <button className="border px-3 py-1 rounded">Role</button>
      </div>

      {/* Table */}
      <EmployeeTable 
        employees={employees}
        loading={loading}
        onEdit={(emp) =>{
          setSelectedEmployee(emp);
          setShowEdit(true);
        }} 
        onDelete={handleDelete}
      />

      {/* Create Modal */}
      {showCreate && (
        <CreateEmployeeModal onClose={() => setShowCreate(false)} 
          onCreated={fetchEmployees}
        />
      )}

      {showEdit && (
        <UpdateEmployeeModal employee={selectedEmployee} onClose={() => setShowEdit(false)} 
          onUpdated={fetchEmployees}
        />
      )}
    </div>
  );
};

export default EmployeeMaster;
