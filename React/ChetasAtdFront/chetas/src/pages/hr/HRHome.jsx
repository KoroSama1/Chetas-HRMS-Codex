import { useAuth } from "../../auth/AuthContext";
import HRHeader from "./components/HRHeader";
import HRStatsChart from "./components/HRStatsChart";
import HRQuickActions from "./components/HRQuickActions";

const HRHome = () => {
  const { logout } = useAuth();

  return (
    <div className="p-8 bg-gray-50 min-h-screen">
      <HRHeader onLogout={logout} />

      <HRStatsChart />

      <HRQuickActions />
    </div>
  );
};

export default HRHome;