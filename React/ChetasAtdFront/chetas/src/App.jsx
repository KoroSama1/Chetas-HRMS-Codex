import { useAuth } from "./auth/AuthContext";
import AppRoutes from "./routes/AppRoutes";

export default function App() {
  const { loading } = useAuth();

  // if (loading) {
  //   return (
  //     <div className="h-screen flex items-center justify-center">
  //       <p>Loading...</p>
  //     </div>
  //   );
  // }
  if (loading) {
    return (
      <div className="h-screen flex items-center justify-center bg-white">
        <p className="text-[2.5rem] font-mono font-semibold text-gray-800">
          Loading
          <span className="inline-block w-[1ch] animate-pulse">.</span>
          <span className="inline-block w-[1ch] animate-pulse delay-200">.</span>
          <span className="inline-block w-[1ch] animate-pulse delay-400">.</span>
        </p>
      </div>
    );
  }

  return <AppRoutes />;
}
