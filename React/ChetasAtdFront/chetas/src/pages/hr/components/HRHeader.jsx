import { useNavigate } from "react-router-dom";
import { useState } from "react";


const HRHeader = ({ onLogout }) => {
  const navigate = useNavigate();
  const [open, setOpen] = useState(false);

  return (
    <div className="flex items-center justify-between border-b-2 border-blue-500 pb-3 mb-6">
      {/* Left */}
      <div className="flex items-center gap-2">
        <span className="text-xl font-bold text-purple-600">HR Panel</span>
      </div>

      {/* Right */}
      <div className="flex items-center gap-3">
        <button 
          onClick={() => navigate("/hr")}
          className="px-4 py-1 border border-blue-500 text-blue-600 rounded hover:bg-blue-50">
          Dashboard
        </button>

        <div className="relative">
          <button
            onClick={() => setOpen(!open)}
            className="px-4 py-1 border border-blue-500 text-blue-600 rounded hover:bg-blue-50"
          >
            Masters ▾
          </button>

          <div
            className={`absolute top-8 right-0 bg-white border rounded shadow-md w-40 z-10 ${
              open ? "block" : "hidden"
            }`}
          >
            <div
              onClick={() => { navigate("/hr/emp-master"); setOpen(false); }}
              className="px-3 py-2 hover:bg-gray-100 cursor-pointer"
            >
              Employee
            </div>
            <div
              onClick={() => { navigate("/hr/dept-master"); setOpen(false); }}
              className="px-3 py-2 hover:bg-gray-100 cursor-pointer"
            >
              Department
            </div>
            <div
              onClick={() => { navigate("/hr/desg-master"); setOpen(false); }}
              className="px-3 py-2 hover:bg-gray-100 cursor-pointer"
            >
              Designation
            </div>
            <div
              onClick={() => { navigate("/hr/region-master"); setOpen(false); }}
              className="px-3 py-2 hover:bg-gray-100 cursor-pointer"
            >
              Region
            </div>
          </div>
        </div>

        <button
          onClick={onLogout}
          className="px-4 py-1 border border-red-500 text-red-500 rounded hover:bg-red-50"
        >
          Log Out
        </button>
      </div>
    </div>
  );
};

export default HRHeader;  