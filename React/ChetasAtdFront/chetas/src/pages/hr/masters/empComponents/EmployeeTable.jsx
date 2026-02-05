// import { useEffect, useState } from "react";
// import { hrApi } from "../../../../api/hr.api";
import { PencilSquareIcon, TrashIcon } from "@heroicons/react/24/outline";

const EmployeeTable = ({ employees, loading, onEdit, onDelete }) => {
  // const [employees, setEmployees] = useState([]);
  // const [loading, setLoading] = useState(true);

  // useEffect(() => {
  //   const fetchEmployees = async () => {
  //     try{
  //       const res = await hrApi.getEmployees();
  //       setEmployees(res.data);
  //     }catch (err){
  //       console.error("Failed to load employees", err);
  //     }finally{
  //       setLoading(false);
  //     }
  //   };
  //   fetchEmployees();
  // }, []);

  if(loading){
    return <div className="p-4">Loading employees...</div>;
  }

  return (
    <div className="bg-white rounded shadow overflow-x-auto">
      <table className="w-full text-sm">
        <thead className="bg-gray-100 text-left">
          <tr>
            <th className="p-3">Emp Code</th>
            <th className="p-3">Name</th>
            <th className="p-3">Username</th>
            <th className="p-3">Email</th>
            <th className="p-3">Designation</th>
            <th className="p-3">Department</th>
            <th className="p-3">Role</th>
            <th className="p-3">Region</th>
            <th className="p-3">DOJ</th>
            <th className="p-3">Action</th>
          </tr>
        </thead>

        <tbody>
          {employees.map((emp) => (
            <tr key={emp.employeeId} className="border-t hover:bg-gray-50 transition">
            <td className="p-3">EMP-{emp.employeeCode}</td>
            <td className="p-3">{emp.fullName}</td>
            <td className="p-3">{emp.username}</td>
            <td className="p-3">{emp.emailId}</td>
            <td className="p-3">{emp.designation}</td>
            <td className="p-3">{emp.department}</td>
            <td className="p-3">{emp.role}</td>
            <td className="p-3">{emp.region}</td>
            <td className="p-3">
              {new Date(emp.dateOfJoining).toLocaleDateString()}
            </td>
            <td className="p-3">
              <div className="flex gap-2">
                <button
                  title="Update"
                  onClick={() => onEdit(emp)}
                  className="p-2 rounded-md bg-blue-50 text-blue-600 hover:bg-blue-100 transition"
                >
                  <PencilSquareIcon className="w-5 h-5" />
                </button>

                <button
                  title="Delete"
                  onClick={() => onDelete(emp.employeeId)}
                  className="p-2 rounded-md bg-red-50 text-red-600 hover:bg-red-100 transition"
                >
                  <TrashIcon className="w-5 h-5" />
                </button>
              </div>
            </td>
          </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default EmployeeTable;
