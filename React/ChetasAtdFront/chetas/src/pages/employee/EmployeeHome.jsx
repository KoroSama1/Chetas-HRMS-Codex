import { useEffect, useState } from "react";
import { useNavigate, useLocation } from "react-router-dom";
import { useAuth } from "../../auth/AuthContext";
import { employeeApi } from "../../api/employee.api";
import Chart from "react-apexcharts";
import { requestAttendancePermissions } from "../../utils/requestAttendancePermissions";

const EmployeeHome = () => {
  const { logout } = useAuth();
  const navigate = useNavigate();
  const { state } = useLocation();
  const successMessage = state?.successMessage;

  const [profile, setProfile] = useState(null);
  const [loading, setLoading] = useState(true);
  const [performance, setPerformance] = useState(null)
  const [permissionLoading, setPermissionLoading] = useState(false);

  /* ===== REPORT STATES ===== */
  const [reportOpen, setReportOpen] = useState(false);
  const [reportType, setReportType] = useState("");
  const [startDate, setStartDate] = useState("");
  const [endDate, setEndDate] = useState("");

  /* ===== HELP STATES ===== */
  const [helpOpen, setHelpOpen] = useState(false);
  const [helpText, setHelpText] = useState("");

  useEffect(() => {
    const loadData = async () => {
      try{
        const [profileRes,perfRes] = await Promise.all([
          employeeApi.getProfile(),
          employeeApi.getPerformance(),
        ]);
        setProfile(profileRes.data);
        setPerformance(perfRes.data);
      }finally{
        setLoading(false);
      }
    };
    loadData();
  }, []);

  const handlePrint = () => window.print();

  const handleAttendance = async (type) => {
    if(permissionLoading) return;

    setPermissionLoading(true);
    const allowed = await requestAttendancePermissions();
    setPermissionLoading(false);
    
    if(!allowed){
      alert("Camera and Location permission is required");
      return;
    }
    navigate(
      type === "in" ? "/attendance/check-in" : "/attendance/check-out"
    );
  };

  const handleHelpSubmit = () => {
    alert("Help request submitted!");
    setHelpText("");
    setHelpOpen(false);
  };

  // need to do changes here
  // const series = [ setPerformance]; // Present, Absent

  const series = performance
    ? [performance.presentDays, performance.absentDays]
    : [];
  const options = {
    labels: ["Present Days", "Absent Days"],
    colors: ["#22c55e", "#ef4444"],
    legend: {
      position: "bottom",
    },
    dataLabels: {
      enabled: false,
    },
  };

  if (loading) {
    return (
      <div className="min-h-screen flex items-center justify-center text-white text-lg">
        Loading dashboard...
      </div>
    );
  }

  return (
    <div
      className="min-h-screen flex items-center justify-center px-3 sm:px-6 relative overflow-x-hidden"
      style={{
        backgroundImage: "url('/images/imageEmployee2.png')",
        backgroundSize: "cover",
        backgroundPosition: "center",
        backgroundRepeat: "no-repeat"
      }}
    >
      {/* ===== MAIN CARD ===== */}
      <div
        className="
          w-full max-w-sm sm:max-w-md md:max-w-xl
          bg-white/95 backdrop-blur-lg
          rounded-2xl shadow-2xl
          p-5 sm:p-7
          relative
        "
      >
        {/* ===== HEADER ===== */}
<div className="flex justify-between items-center gap-3 mb-5">
  
  {/* LEFT: Employee Info */}
  <div className="flex flex-col sm:flex-row flex-wrap gap-1 sm:gap-6 text-xs sm:text-sm font-bold text-gray-700">
    <span>
      <span className="ml-1 text-black">
        {profile?.fullName}
      </span>
    </span>

    <span>
      - 
      <span className="ml-1 text-black">
        EMP{profile?.employeeCode}
      </span>
    </span>
  </div>

  {/* RIGHT: Profile + Logout */}
  <div className="flex items-center gap-3">
    <div 
      onClick={() => navigate("/emp/self-profile")}
      className="rounded-full bg-blue-100 p-2 text-xl cursor-pointer"
    >
      👤
    </div>

    <button
      onClick={logout}
      title="Logout"
      className="rounded-full border-2 border-black p-2 text-xl font-bold
                 transition hover:scale-110 hover:bg-purple-200"
    >
      ⏻
    </button>


    
  </div>

</div>
        {/* ===== SUCCESS MESSAGE ===== */}
        {successMessage && (
          <div className="mb-5 rounded-lg bg-cyan-50 text-cyan-800 font-semibold p-3">
            ✅ {successMessage}
          </div>
        )}

        {/* ===== PROFILE / CHART ===== */}
        <div className="mb-6 rounded-xl bg-slate-100 p-3 sm:p-4 space-y-1">
          <div className="bg-white rounded-xl shadow p-4">
            <Chart
              options={options}
              series={series}
              type="donut"
              height={200}
            />

            {/* ===== STATS ROW ===== */}
            <div className="mt-4 grid grid-cols-3 gap-2 sm:gap-3 text-center">
              <div className="rounded-lg bg-green-50 p-3">
                <p className="text-sm text-gray-500">Present</p>
                <p className="text-xl font-bold text-green-600">
                  {performance?.presentDays ?? 0}
                </p>
              </div>

              <div className="rounded-lg bg-red-50 p-3">
                <p className="text-sm text-gray-500">Absent</p>
                <p className="text-xl font-bold text-red-500">
                  {performance?.absentDays ?? 0}
                </p>
              </div>

              <div className="rounded-lg bg-indigo-50 p-3">
                <p className="text-sm text-gray-500">Total</p>
                <p className="text-xl font-bold text-indigo-600">
                  {performance?.totalDays ?? 0}
                </p>
              </div>
            </div>

          </div>
        </div>

        {/* ===== CHECK IN / CHECK OUT ===== */}
        <div className="flex flex-col sm:flex-row gap-3 sm:gap-4 mb-6">
          <button
            onClick={() => handleAttendance("in")}
            className="
              flex-1 rounded-xl sm:rounded-2xl
              py-3 sm:py-4
              font-extrabold text-black
              text-sm sm:text-base
              bg-gradient-to-br from-purple-200 to-purple-300
              shadow-xl transition
              hover:-translate-y-1 hover:shadow-2xl
              active:scale-95
            "
            disabled={permissionLoading}
            >
            ⬆️ Check In
          </button>

          <button
            onClick={() => handleAttendance("out")}
            disabled={permissionLoading}
            className="flex-1 rounded-2xl py-4 font-extrabold text-black
                      bg-gradient-to-br from-purple-200 to-purple-300
                      shadow-xl transition hover:-translate-y-1 hover:shadow-2xl active:scale-95"
          >
            ⬇️ Check Out
          </button>

        </div>

        {/* ===== REPORT DROPDOWN ===== */}
        <div className="mb-4">
          <button
            onClick={() => setReportOpen(!reportOpen)}
            className="
              w-full rounded-xl
              py-3
              font-bold text-black
              text-sm sm:text-base
              bg-gradient-to-br from-indigo-200 to-indigo-300
              shadow-md transition hover:-translate-y-1
            "
          >
            📊 Reports
          </button>

          {reportOpen && (
            <div className="mt-3 rounded-xl bg-slate-100 p-4 space-y-2 animate-slideDown">
              <button
                onClick={() => setReportType("month")}
                className="w-full text-left px-3 py-2 rounded-lg hover:bg-indigo-200 font-semibold text-sm"
              >
                📅 Current Month
              </button>

              <button
                onClick={() => setReportType("year")}
                className="w-full text-left px-3 py-2 rounded-lg hover:bg-indigo-200 font-semibold text-sm"
              >
                📆 Current Year
              </button>

              <button
                onClick={() => setReportType("custom")}
                className="w-full text-left px-3 py-2 rounded-lg hover:bg-indigo-200 font-semibold text-sm"
              >
                🗓️ Custom (Weekly / Date Range)
              </button>
            </div>
          )}
        </div>

        {/* ===== CUSTOM REPORT ===== */}
        {reportType === "custom" && (
          <div className="mb-5 rounded-xl bg-purple-50 p-4 space-y-3 animate-slideDown">
            <div className="flex gap-3">
              <input
                type="date"
                value={startDate}
                onChange={(e) => setStartDate(e.target.value)}
                className="w-1/2 rounded-lg border px-3 py-2 text-sm"
              />
              <input
                type="date"
                value={endDate}
                onChange={(e) => setEndDate(e.target.value)}
                className="w-1/2 rounded-lg border px-3 py-2 text-sm"
              />
            </div>
          </div>
        )}

        {/* ===== PRINT BUTTON ===== */}
        {reportType && (
          <button
            onClick={handlePrint}
            className="
              w-full rounded-xl
              py-3
              font-bold text-black
              text-sm sm:text-base
              bg-gradient-to-br from-green-200 to-green-300
              shadow-md transition hover:-translate-y-1 active:scale-95
            "
          >
            🖨️ Print Report
          </button>
        )}
      </div>

      {/* ===== HELP BUTTON ===== */}
      <button
        onClick={() => setHelpOpen(true)}
        title="Help"
        className="
          fixed bottom-4 sm:bottom-6
          right-4 sm:right-6
          rounded-full
          bg-gradient-to-br from-green-300 to-green-400
          text-black p-4 shadow-2xl text-xl
          transition hover:scale-110
        "
      >
        ❓
      </button>

      {/* ===== HELP MODAL ===== */}
      {helpOpen && (
        <div className="fixed inset-0 bg-black/40 flex items-center justify-center z-50">
          <div className="bg-white rounded-xl p-6 w-[90%] max-w-md animate-slideDown">
            <h3 className="text-lg font-bold mb-3">
              Need Help?
            </h3>

            <textarea
              rows="4"
              value={helpText}
              onChange={(e) => setHelpText(e.target.value)}
              placeholder="Describe your issue..."
              className="w-full border rounded-lg p-3 mb-4 focus:outline-none focus:ring-2 focus:ring-green-400 text-sm"
            />

            <div className="flex justify-end gap-3">
              <button
                onClick={() => setHelpOpen(false)}
                className="px-4 py-2 rounded-lg bg-gray-200 font-semibold text-sm"
              >
                Cancel
              </button>
              <button
                onClick={handleHelpSubmit}
                className="px-4 py-2 rounded-lg bg-green-400 font-semibold text-black text-sm"
              >
                Submit
              </button>
            </div>
          </div>
        </div>
      )}

      {/* ===== ANIMATION ===== */}
      <style>
        {`
          .animate-slideDown {
            animation: slideDown 0.3s ease-out;
          }
          @keyframes slideDown {
            from { opacity: 0; transform: translateY(-10px); }
            to { opacity: 1; transform: translateY(0); }
          }
        `}
      </style>
    </div>
  );
};

export default EmployeeHome;
