import MasterPage from "./commonMcomponent/MasterPage";
import { MASTERS } from "./commonMcomponent/CommonComponents";

const DepartmentMaster = () => {
  return <MasterPage config={MASTERS.department} />;
};

export default DepartmentMaster;