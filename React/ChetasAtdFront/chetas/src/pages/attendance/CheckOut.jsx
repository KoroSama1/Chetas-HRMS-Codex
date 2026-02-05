import { useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { useCamera } from "../../hooks/useCamera";
import { getCurrentLocation } from "../../hooks/useGeolocation";
import { ArrowLeftIcon } from "@heroicons/react/24/solid";



const CheckOut = () => {
  const navigate = useNavigate();
  const {
    videoRef,
    canvasRef,
    startCamera,
    stopCamera,
    captureImage
  } = useCamera();

  useEffect(() => {
    startCamera().catch(()=>{
      alert("camera permission not available");
      navigate("/employee");
    });
    return () => stopCamera();
  }, []);

  const handleCapture = async () => {
    const video = videoRef.current;
    if(!video || video.readyState !== 4 || video.videoWidth === 0 || video.videoHeight === 0){
      alert("Camera not ready");
      return;
    }
    
    const imageBase64 = captureImage();
    stopCamera();
    try{
      const location = await getCurrentLocation();
  
      navigate("/attendance/confirm-check-out", {
        state: {
          imageBase64,
          location
        }
      });
    }catch(err){
      alert("Location Permission is required to check-out");
      startCamera();
    }
  };

  return (
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

      {/* ===== MAIN CARD ===== */}
      <div
        className="w-full max-w-md bg-white rounded-3xl shadow-2xl
                   p-6 sm:p-8 text-center
                   animate-fadeIn"
      >
        {/* ===== HEADING ===== */}
        <h2 className="text-2xl font-extrabold text-black mb-4">
          Check Out
        </h2>

        {/* ===== CAMERA PREVIEW ===== */}
        <div
          className="rounded-2xl overflow-hidden shadow-lg mb-6
                     border border-purple-200
                     animate-scaleIn"
        >
          <video
            ref={videoRef}
            autoPlay
            playsInline
            muted
            className="w-full h-auto"
          />
        </div>

        <canvas ref={canvasRef} hidden />

        {/* ===== CAPTURE BUTTON ===== */}
        <button
          onClick={handleCapture}
          className="w-full py-4 rounded-2xl font-extrabold text-black
                     bg-gradient-to-br from-purple-200 to-purple-300
                     shadow-xl transition
                     hover:-translate-y-1 hover:shadow-2xl
                     hover:from-purple-300 hover:to-purple-400
                     active:scale-95"
        >
          📸 Capture
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

          @keyframes fadeIn {
            from {
              opacity: 0;
              transform: translateY(20px);
            }
            to {
              opacity: 1;
              transform: translateY(0);
            }
          }

          @keyframes scaleIn {
            from {
              opacity: 0;
              transform: scale(0.95);
            }
            to {
              opacity: 1;
              transform: scale(1);
            }
          }
        `}
      </style>
    </div>
  );
};

export default CheckOut;
