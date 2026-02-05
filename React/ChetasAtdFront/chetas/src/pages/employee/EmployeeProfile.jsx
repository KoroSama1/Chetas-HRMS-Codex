import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { useAuth } from "../../auth/AuthContext";
import { employeeApi } from "../../api/employee.api";
import { ArrowLeftIcon } from "@heroicons/react/24/solid";


const EmployeeProfile = () => {
  const { logout } = useAuth();
  const navigate = useNavigate();
  const [profile, setProfile] = useState(null);
  const [loading, setLoading] = useState(true);
  const [activeTab, setActiveTab] = useState("overview");

  useEffect(() => {
    const loadProfile = async () => {
      try {
        const res = await employeeApi.getProfile();
        setProfile(res.data);
      } catch (err) {
        console.error("Profile load failed:", err);
      } finally {
        setLoading(false);
      }
    };
    loadProfile();
  }, []);

  if (loading) {
    return (
      <div className="max-w-4xl mx-auto p-6 animate-pulse">
        <div className="h-48 bg-gray-200 rounded-2xl mb-6" />
        <div className="h-4 bg-gray-200 rounded w-2/3 mb-3" />
        <div className="h-4 bg-gray-200 rounded w-1/2" />
      </div>
    );
  }

  if (!profile) return <p className="p-6">Profile not found</p>;

  return (
    <div
      className="min-h-screen flex items-center justify-center px-3 sm:px-6 relative"
      style={{
        backgroundImage: "url('/images/imageEmployee2.png')",
        backgroundSize: "cover",
        backgroundPosition: "center",
        backgroundRepeat: "no-repeat"
      }}
    >
      {/* Main Card */}
      <div className="w-full max-w-4xl mx-auto bg-white/85 backdrop-blur-sm rounded-2xl shadow-xl overflow-hidden">
         

         {/* ===== BACK BUTTON ===== */}
      <button
     onClick={() => navigate("/emp/home")}   // or navigate(-1)
     title="Back"
     className="absolute top-4 left-4 z-20
             flex items-center gap-2
             rounded-full bg-white/90 p-2
             shadow hover:bg-white transition
             active:scale-95"
>
  <ArrowLeftIcon className="h-5 w-5 text-gray-800" />
</button>

        {/* ===== HEADER ===== */}
        <div className="relative h-40 sm:h-48 bg-gradient-to-r from-indigo-600 via-purple-600 to-pink-600">
          <div className="absolute inset-0 opacity-20 bg-[radial-gradient(circle_at_top,_white,_transparent)]" />

          {/* Avatar */}
          <div className="absolute -bottom-14 sm:-bottom-16 left-1/2 -translate-x-1/2">
            <img
              src={profile.employeePhotoPath}
              alt={profile.fullName}
              className="w-24 h-24 sm:w-32 sm:h-32 rounded-full object-cover border-4 border-white shadow-lg"
              onError={(e) => (e.target.src = "/default-user.png")}
            />
          </div>
        </div>

        {/* ===== BASIC INFO ===== */}
        <div className="pt-16 sm:pt-20 text-center px-4 sm:px-8">
          <h2 className="text-xl sm:text-3xl font-bold text-gray-800">
            {profile.fullName}
          </h2>
          <p className="text-sm sm:text-base text-gray-500">
            {profile.designation}
          </p>

          <div className="flex flex-wrap justify-center gap-2 mt-3">
            <span className="px-3 py-1 text-xs rounded-full bg-green-100 text-green-700">
              Active Employee
            </span>
            <span className="px-3 py-1 text-xs rounded-full bg-indigo-100 text-indigo-700">
              {profile.role}
            </span>
          </div>

          <div className="flex justify-center gap-3 mt-5">
            <button
              onClick={logout}
              className="px-4 sm:px-5 py-2 rounded-lg bg-red-100 text-red-600 hover:bg-red-200 transition text-sm sm:text-base"
            >
              Logout
            </button>
          </div>
        </div>

        {/* ===== TABS ===== */}
        <div className="mt-6 sm:mt-8 px-4 sm:px-8">
          <div className="flex justify-center sm:justify-start gap-6 border-b overflow-x-auto">
            {["overview", "work", "contact"].map((tab) => (
              <button
                key={tab}
                onClick={() => setActiveTab(tab)}
                className={`pb-3 capitalize font-medium transition whitespace-nowrap
                  ${
                    activeTab === tab
                      ? "border-b-2 border-indigo-600 text-indigo-600"
                      : "text-gray-500 hover:text-gray-700"
                  }`}
              >
                {tab}
              </button>
            ))}
          </div>

          {/* ===== TAB CONTENT ===== */}
         <div className="mt-6 grid grid-cols-1 sm:grid-cols-2 gap-4 sm:gap-6">


            {activeTab === "overview" && (
              <InfoCard title="Employee Details" full>
                <Info label="Employee Code" value={profile.employeeCode} />
                <Info label="Username" value={profile.username} />
                <Info
                  label="Date of Joining"
                  value={new Date(profile.dateOfJoining).toLocaleDateString()}
                />
                <Info label="Designation" value={profile.designation} />
              </InfoCard>
            )}

            {activeTab === "work" && (
              <InfoCard title="Work Information" full>
                <Info label="Role" value={profile.role} />
                <Info label="Department" value={profile.department} />
                <Info label="Region" value={profile.region} />
              </InfoCard>
            )}

            {activeTab === "contact" && (
              <InfoCard title="Contact Information" full>
                <Info
                  label="Email"
                  value={
                    <a
                      href={`mailto:${profile.emailId}`}
                      className="text-indigo-600 hover:underline"
                    >
                      {profile.emailId}
                    </a>
                  }
                />
                <Info
                  label="Phone"
                  value={
                    <a
                      href={`tel:${profile.phoneNumber}`}
                      className="text-indigo-600 hover:underline"
                    >
                      {profile.phoneNumber}
                    </a>
                  }
                />
              </InfoCard>
            )}
          </div>
        </div>

        <div className="h-6" />
      </div>
    </div>
  );
};

/* ===== Reusable Components ===== */

const InfoCard = ({ title, children, full }) => (
  <div
    className={`bg-white rounded-xl p-4 sm:p-5 shadow-sm hover:shadow-md transition
      ${full ? "sm:col-span-2" : ""}`}
  >

    <h3 className="font-semibold text-gray-700 mb-4 text-sm sm:text-base">
      {title}
    </h3>
    <div className="space-y-3">{children}</div>
  </div>
);

const Info = ({ label, value }) => (
  <div className="flex justify-between gap-4 text-xs sm:text-sm">
    <span className="text-gray-500 shrink-0">
      {label}
    </span>
    <span className="font-medium text-gray-800 text-right max-w-[220px] break-all">
      {value}
    </span>
  </div>
);

export default EmployeeProfile;
