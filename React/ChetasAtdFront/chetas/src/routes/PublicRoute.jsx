import { Navigate } from "react-router-dom";
import { useAuth } from "../auth/AuthContext";

const PublicRoute = ({ children }) => {
  const { user } = useAuth();

  if (user) {
    // Already logged in → kick out of login page
    switch (user.role) {
      case "SUPERADMIN":
        return <Navigate to="/admin" replace />;
      case "HR":
        return <Navigate to="/hr" replace />;
      case "EMPLOYEE":
        return <Navigate to="/employee" replace />;
      default:
        return <Navigate to="/login" replace />;
    }
  }

  return children;
};

export default PublicRoute;
