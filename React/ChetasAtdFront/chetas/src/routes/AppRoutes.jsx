import { Routes, Route, Navigate } from "react-router-dom";
import Login from "../pages/auth/Login";

// Dashboards
import EmployeeHome from "../pages/employee/EmployeeHome";
import HRHome from "../pages/hr/HRHome";
import AdminHome from "../pages/superadmin/AdminHome";

// Employee Pages
import CheckIn from "../pages/attendance/CheckIn";
import ConfirmCheckIn from "../pages/attendance/ConfirmCheckIn";
import CheckOut from "../pages/attendance/CheckOut";
import ConfirmCheckOut from "../pages/attendance/ConfirmCheckOut";
import EmployeeProfile from "../pages/employee/EmployeeProfile";

// Hr Pages
import EmpCheckIn from "../pages/hr/EmpCheckIn";
import EmpCheckOut from "../pages/hr/EmpCheck-Out";
import EmployeeMaster from "../pages/hr/masters/EmployeeMaster"
import DesignationMaster from "../pages/hr/masters/DesignationMaster";
import DepartmentMaster from "../pages/hr/masters/DepartmentMaster";
import RegionMaster from "../pages/hr/masters/RegionMaster";

// Routes
import PublicRoute from "./PublicRoute";
import RoleProtectedRoute from "./RoleProtectedRoute";



const AppRoutes = () => {
  return (
    <Routes>
      {/* ==================== PUBLIC ==================== */}
      <Route
        path="/login"
        element={
          <PublicRoute>
            <Login />
          </PublicRoute>
        }
      />


      {/* ==================== EMPLOYEE ==================== */}
      <Route
        path="/employee"
        element={
          <RoleProtectedRoute allowedRoles={["EMPLOYEE"]}>
            <EmployeeHome />
          </RoleProtectedRoute>
        }
      />

      <Route
        path="/attendance/check-in"
        element={
          <RoleProtectedRoute allowedRoles={["EMPLOYEE"]}>
            <CheckIn />
          </RoleProtectedRoute>
        }
      />

      <Route
        path="/attendance/confirm-check-in"
        element={
          <RoleProtectedRoute allowedRoles={["EMPLOYEE"]}>
            <ConfirmCheckIn />
          </RoleProtectedRoute>
        }
      />

      <Route
        path="/attendance/check-out"
        element={
          <RoleProtectedRoute allowedRoles={["EMPLOYEE"]}>
            <CheckOut />
          </RoleProtectedRoute>
        }
      />

      <Route
        path="/attendance/confirm-check-out"
        element={
          <RoleProtectedRoute allowedRoles={["EMPLOYEE"]}>
            <ConfirmCheckOut />
          </RoleProtectedRoute>
        }
      />

      <Route
        path="/emp/self-profile"
        element={
          <RoleProtectedRoute allowedRoles={["EMPLOYEE"]}>
            <EmployeeProfile />
          </RoleProtectedRoute>
        }
      />


      {/* ==================== HR ==================== */}
      <Route
        path="/hr"
        element={
          <RoleProtectedRoute allowedRoles={["HR"]}>
            <HRHome />
          </RoleProtectedRoute>
        }
      />

      <Route
        path="/hr/emp-checkin"
        element={
          <RoleProtectedRoute allowedRoles={["HR"]}>
            <EmpCheckIn />
          </RoleProtectedRoute>
        }
      />

      <Route
        path="/hr/emp-checkout"
        element={
          <RoleProtectedRoute allowedRoles={["HR"]}>
            <EmpCheckOut />
          </RoleProtectedRoute>
        }
      />

      <Route
        path="/hr/emp-master"
        element={
          <RoleProtectedRoute allowedRoles={["HR"]}>
            <EmployeeMaster/>
          </RoleProtectedRoute>
        }
      />


      {/* ===== HR MASTERS ===== */}
      <Route
        path="/hr/dept-master"
        element={
          <RoleProtectedRoute allowedRoles={["HR"]}>
            <DepartmentMaster />
          </RoleProtectedRoute>
        }
      />

      <Route
        path="/hr/desg-master"
        element={
          <RoleProtectedRoute allowedRoles={["HR"]}>
            <DesignationMaster />
          </RoleProtectedRoute>
        }
      />
 
      <Route
        path="/hr/region-master"
        element={
          <RoleProtectedRoute allowedRoles={["HR"]}>
            <RegionMaster />
          </RoleProtectedRoute>
        }
      />


      {/* ==================== ADMIN ==================== */}
      <Route
        path="/admin"
        element={
          <RoleProtectedRoute allowedRoles={["SUPERADMIN"]}>
            <AdminHome />
          </RoleProtectedRoute>
        }
      />


      {/* ==================== FALLBACK ==================== */}
      <Route path="*" element={<Navigate to="/login" replace />} />
    </Routes>
  );
};

export default AppRoutes;
