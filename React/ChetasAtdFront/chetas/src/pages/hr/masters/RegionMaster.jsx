import MasterPage from "./commonMcomponent/MasterPage";
import { MASTERS } from "./commonMcomponent/CommonComponents";

const RegionMaster = () => {
  return <MasterPage config={MASTERS.region} />;
};

export default RegionMaster;