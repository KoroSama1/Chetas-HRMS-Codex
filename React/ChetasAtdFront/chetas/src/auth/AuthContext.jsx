import { createContext, useContext, useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { authApi } from "../api/auth.api";
import { tokenManager } from "./tokenManager";

const AuthContext = createContext(null);

export const AuthProvider = ({ children }) => {
  const [user, setUser] = useState(null);
  const [loading, setLoading] = useState(true);
  const navigate = useNavigate();

  const redirectByRole = (role) => {
    switch (role) {
      case "SUPERADMIN":
        navigate("/admin", { replace: true });
        break;
      case "HR":
        navigate("/hr", { replace: true });
        break;
      case "EMPLOYEE":
        navigate("/employee", { replace: true });
        break;
      default:
        navigate("/login", { replace: true });
    }
  };

  /* ================= RESTORE SESSION ================= */
  useEffect(() => {
    const restoreSession = async () => {
      try {
        const refreshRes = await authApi.refresh();
        tokenManager.set(refreshRes.data.accessToken);

        const meRes = await authApi.me();

        const userData = {
          employeeId: meRes.data.employeeId,
          role: meRes.data.role,
        };

        setUser(userData);
        
        redirectByRole(userData.role);

      } catch {
        tokenManager.clear();
        setUser(null);
        navigate("/login", { replace: true });
      } finally {
        setLoading(false);
      }
    };

    restoreSession();
  }, []);

  /* ================= LOGIN ================= */
  const login = async (credentials) => {
    try {
      setLoading(true);

      const loginRes = await authApi.login(credentials);
      tokenManager.set(loginRes.data.accessToken);

      const meRes = await authApi.me();

      const userData = {
        employeeId: meRes.data.employeeId,
        role: meRes.data.role,
      };
      

      setUser(userData);
      if(window.location.pathname === "/login"){
        redirectByRole(userData.role);
      }
    } catch (err) {
      tokenManager.clear();
      throw err;
    } finally {
      setLoading(false);
    }
  };

  /* ================= LOGOUT ================= */
  const logout = async () => {
    try {
      await authApi.logout();
    } catch {
      // ignore
    } finally {
      tokenManager.clear();
      setUser(null);
      navigate("/login", { replace: true });
      window.history.replaceState(null,"","/login");
    }
  };

  return (
    <AuthContext.Provider
      value={{
        user,
        loading,
        isAuthenticated: !!user,
        login,
        logout,
      }}
    >
      {/* {!loading && children} */}
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => useContext(AuthContext);
  

