import { useNavigate } from "react-router-dom";
const HRQuickActions = () => {
    const navigate = useNavigate();
  return (
    <div className="grid grid-cols-3 gap-6">
      <button 
        onClick= {() => navigate("/hr/emp-checkin")}
        className="bg-orange-500 text-white py-6 text-xl rounded hover:bg-orange-600"
      >
        Emp Check-in 🔔
      </button>

      <button 
        onClick= {() => navigate("/hr/emp-checkout")}
        className="bg-orange-500 text-white py-6 text-xl rounded hover:bg-orange-600"
      >
        Emp Check-out 🔔
      </button>

      <button className="bg-orange-500 text-white py-6 text-xl rounded hover:bg-orange-600">
        Help Queries 🔔
      </button>

      <button className="col-span-2 bg-orange-500 text-white py-6 text-xl rounded hover:bg-orange-600">
        Emp Profiles
      </button>

      <button className="col-span-1 bg-orange-500 text-white py-6 text-xl rounded hover:bg-orange-600">
        HR Attendance
      </button>
    </div>
  );
};

export default HRQuickActions;