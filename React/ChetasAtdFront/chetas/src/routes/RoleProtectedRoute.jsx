import { Navigate } from "react-router-dom";
import { useAuth } from "../auth/AuthContext";

const RoleProtectedRoute = ({ allowedRoles, children }) => {
    const { user } = useAuth();

    if(!user){
        return <Navigate to="/login" replace/>;
    }

    if(!allowedRoles.includes(user.role)){
        return <Navigate to="/login" replace/>;
    }

    return children;
};

export default RoleProtectedRoute;