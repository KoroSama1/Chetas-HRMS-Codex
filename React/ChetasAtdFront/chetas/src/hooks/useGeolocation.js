export const getCurrentLocation = () =>
  new Promise((resolve, reject) => {
    navigator.geolocation.getCurrentPosition(
      async (pos) => {
        const {latitude, longitude} = pos.coords;
        try{
          const res = await fetch(`https://nominatim.openstreetmap.org/reverse?format=json&lat=${latitude}&lon=${longitude}`);
          const data = await res.json();
          resolve({
            latitude,
            longitude,
            address:data.display_name || "Unknown location",
          });
        }catch(err){
          resolve({
            latitude,
            longitude,
            address: "Auto GPS",
          });
        }
      }, 
      reject
    );
  });
