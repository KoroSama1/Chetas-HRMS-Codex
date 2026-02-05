import { useLocation, useNavigate } from "react-router-dom";
import { attendanceApi } from "../../api/attendance.api";
import { useState } from "react";
import { ArrowLeftIcon } from "@heroicons/react/24/solid";
import Swal from "sweetalert2";





const ConfirmCheckOut = () => {
  const navigate = useNavigate();
  const { state } = useLocation();
  const [error, setError] = useState(null);
  const [submitting, setSubmitting] = useState(false);

  if (!state) {
    return (
      <div className="min-h-screen flex items-center justify-center bg-sky-200 text-gray-700">
        No data found
      </div>
    );
  }

  const { imageBase64, location } = state;

  const isValidImage = imageBase64 && imageBase64.startsWith("data:image") && imageBase64.length > 10000;

  /* ===== PREVIEW TIME (UI ONLY) ===== */
  const previewNow = new Date();
  const previewDate = previewNow.toLocaleDateString();
  const previewTime = previewNow.toLocaleTimeString();

  /* ===== CONFIRM CHECK-OUT ===== */
  const handleConfirm = async () => {
    setSubmitting(true);
    setError(null);

    // Capture exact click time (authoritative time is still backend)
    const clickTime = new Date();

  try {
    const res = await attendanceApi.checkOut({
      photoBase64: imageBase64,
      latitude: location.latitude,
      longitude: location.longitude,
      locationAddress: location.address,
        clientTime: clickTime.toISOString(), // optional (logs/debug)
    });

    // ✅ Success Popup
    await Swal.fire({
      icon: "success",
      title: "Check-Out Successful",
      text: "Check-Out Marked Successfully",
      timer: 2000,
      showConfirmButton: false,
    });

    // After popup, go to Employee Home
    navigate("/employee");

  } catch (err) {
    const msg =
      err.response?.data?.error?.message || "Attendance Failed";

    // ❌ Error Popup
    Swal.fire({
      icon: "error",
      title: "Check-Out Failed",
      text: msg,
    });

    setError(msg);
      setSubmitting(false);
  }
};

  return (
    /* ===== PAGE BACKGROUND (LIGHT BLUE – SLIGHTLY DARK) ===== */
    <div
  className="min-h-screen flex items-center justify-center px-4 relative"
  style={{
    backgroundImage: "url('/images/imageEmployee2.png')",
    backgroundSize: "cover",
    backgroundPosition: "center",
    backgroundRepeat: "no-repeat"
  }} 
>

{/* ===== BACK BUTTON ===== */}
<button
  onClick={() => navigate("/attendance/check-out")}   // or navigate(-1)
  title="Back"
  className="absolute top-4 left-4 z-20
             flex items-center gap-2
             rounded-full bg-white/90 p-2
             shadow hover:bg-white transition
             active:scale-95"
>
  <ArrowLeftIcon className="h-5 w-5 text-gray-800" />
</button>

      {/* ===== MAIN CARD ===== */}
      <div
        className="w-full max-w-md bg-white rounded-3xl
                   shadow-xl p-6 sm:p-8 text-center
                   animate-fadeIn"
      >
        {/* ===== HEADING ===== */}
        <h2 className="text-2xl font-extrabold text-black mb-4">
          Confirm Check-Out
        </h2>

        {/* ===== ERROR ===== */}
        {error && (
          <div className="mb-4 rounded-xl bg-red-100 text-red-700
                          font-semibold p-3 animate-slideDown">
            ❌ {error}
          </div>
        )}

        {/* ===== IMAGE PREVIEW ===== */}
        <div
          className="mb-4 rounded-2xl overflow-hidden
                     border border-sky-300 shadow-md animate-scaleIn"
        >
          <img
            src={imageBase64}
            alt="Captured"
            className="w-full h-auto"
          />
        </div>

        {/* ===== LOCATION + PREVIEW TIME ===== */}
        <p className="mb-6 text-sm text-black leading-relaxed">
          <span className="font-semibold">Location:</span>{" "}
          {location?.address}
          <br />
          <span className="font-semibold">Preview Date:</span>{" "}
          {previewDate}
          <br />
          <span className="font-semibold">Preview Time:</span>{" "}
          {previewTime}
        </p>

        {/* ===== MARK ATTENDANCE BUTTON ===== */}
        <button
          onClick={handleConfirm}
          disabled={submitting || !isValidImage}
          className={`w-full py-4 rounded-2xl font-extrabold text-black
            bg-gradient-to-br from-purple-200 to-purple-300
            shadow-lg transition
            hover:-translate-y-1 hover:shadow-xl
            hover:from-purple-300 hover:to-purple-400
            active:scale-95
            ${submitting ? "opacity-60 cursor-not-allowed" : ""}`}
        >
          {submitting ? "⏳ Marking Check-Out..." : "✅ Mark Check-Out"}
        </button>
      </div>

      {/* ===== ANIMATIONS ===== */}
      <style>
        {`
          .animate-fadeIn {
            animation: fadeIn 0.6s ease-out;
          }
          .animate-scaleIn {
            animation: scaleIn 0.5s ease-out;
          }
          .animate-slideDown {
            animation: slideDown 0.4s ease-out;
          }

          @keyframes fadeIn {
            from { opacity: 0; transform: translateY(20px); }
            to { opacity: 1; transform: translateY(0); }
          }

          @keyframes scaleIn {
            from { opacity: 0; transform: scale(0.96); }
            to { opacity: 1; transform: scale(1); }
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

export default ConfirmCheckOut;
