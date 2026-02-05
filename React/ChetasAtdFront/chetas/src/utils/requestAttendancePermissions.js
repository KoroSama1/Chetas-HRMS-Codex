export const requestAttendancePermissions = async () => {
    try{
        //camera permission
        const stream = await navigator.mediaDevices.getUserMedia({ video: true });
        stream.getTracks().forEach(track => track.stop());

        //location permission
        // await new Promise((resolve, reject) => {
        //     navigator.geolocation.getCurrentPosition(resolve, reject);
        // });
        return true;
    } catch(err){
        return false;
    }
};