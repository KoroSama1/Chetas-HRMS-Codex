import { useAuth } from "../../auth/AuthContext";

const AdminHome = () => {
  const { user, logout } = useAuth();

  return (
    <div style={{ padding: 40 }}>
      <h1>Welcome to Admin panel</h1>
      <p>Employee ID: {user?.employeeId}</p>

      <button onClick={logout}>Logout</button>
    </div>
  );
};

export default AdminHome;
