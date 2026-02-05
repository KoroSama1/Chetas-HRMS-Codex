import { useEffect, useState } from "react";
import { useAuth } from "../../auth/AuthContext";
import { hrApi } from "../../api/hr.api";
import HRHeader from "./components/HRHeader";

const EmpCheckOut = () => {
    const {logout} = useAuth();
    const [regions, setRegions] = useState([]);
    const [activeRegion, setActiveRegion] = useState(null);
    const [attendance, setAttendance] = useState([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const loadRegions = async () => {
            try{
                const res = await hrApi.getRegions();
                setRegions(res.data);

                if(res.data.length > 0){
                    const defaultRegion = res.data[0];
                    setActiveRegion(defaultRegion.regionId);
                    await loadAttendance(defaultRegion.regionId);
                }
            }catch (err){
                console.error("Failed to load regions:",err);
            }finally{
                setLoading(false);
            }
        };
        loadRegions();
    }, []);

    const loadAttendance = async (regionId) => {
        try{
            const res = await hrApi.getCheckOut(regionId);
            setAttendance(res.data);
        }catch(err){
            console.error("Failed to load attendance:",err);
        }
    };

    const handleRegionClick = (regionId) => {
        setActiveRegion(regionId);
        loadAttendance(regionId);
    };

    const handleAcceptClick = async (attendanceId) => {
    try {
        await hrApi.verifyAttendance(attendanceId, "Confirmed" ,"CheckOut");
        setAttendance(prev =>
        prev.filter(a => a.attendanceId !== attendanceId)
        );
    } catch (err) {
        alert("Failed to approve checkout");
        console.error(err);
    }
    };

    const handleRejectClick = async (attendanceId) => {
    try {
        await hrApi.verifyAttendance(attendanceId, "Rejected" ,"CheckOut");
        setAttendance(prev =>
        prev.filter(a => a.attendanceId !== attendanceId)
        );
    } catch (err) {
        alert("Failed to reject checkout");
        console.error(err);
    }
    };

    if (loading) return <p className="p-6">Loading..</p>;

    return(
        <div className="p-6">
            <HRHeader onLogout={logout}/>
            <div className="flex gap-3 mb-6">
                {regions.map((r) => (
                    <button
                        key={r.regionId}
                        onClick={() => handleRegionClick(r.regionId)}
                        className={`px-4 py-2 border rounded ${activeRegion === r.regionId?"bg-blue-600 text-white":"bg-white"}`}
                    >
                        {r.regionName}
                    </button>
                ))}
            </div>

            <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
                {attendance.map((a) => (
                    <div
                        key={a.attendanceId}
                        className="border rounded-lg p-4 flex flex-col items-center"
                    >
                        <img src={a.checkOutPhotoUrl} alt={a.employeeName} 
                            className="w-32 h-32 object-cover rounded mb-4 border"
                            onError={(e) =>{
                                e.target.src = "/default-user.png";
                            }}
                        />
                        <p className="font-semibold">Name : {a.employeeName}</p>
                        <p>Location : {a.checkOutLocationAddress}</p>
                        <p>Time : {a.checkOutTime}</p>

                        <div className="flex gap-4 mt-4">
                            <button
                                onClick={() => handleAcceptClick(a.attendanceId)}
                                className="bg-green-500 text-white px-4 py-2 rounded"
                            >
                            ✓
                            </button>

                            <button
                                onClick={() => handleRejectClick(a.attendanceId)}
                                className="bg-red-500 text-white px-4 py-2 rounded"
                            >
                            ✕
                            </button>
                        </div>
                    </div>
                ))}
            </div>
        </div>
    );
};

export default EmpCheckOut;