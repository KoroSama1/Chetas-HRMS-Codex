import { useState } from "react";
import { useAuth } from "../../auth/AuthContext";
import Swal from "sweetalert2";

const Login = () => {
  const { login, loading } = useAuth();

  const [form, setForm] = useState({
    username: "",
    password: ""
  });

  const handleSubmit = async (e) => {
    e.preventDefault();

    try {
      await login(form);

      Swal.fire({
        icon: "success",
        title: "Login Successful",
        text: "Welcome to Chetas HRMS",
        width: 300,          // 👈 makes popup small
        padding: "1em",
        timer: 1500,
        showConfirmButton: false,
        customClass: {
        popup: "small-swal",
       },
      });

    } catch (error) {
      Swal.fire({
        icon: "error",
        title: "Login Failed",
        text: "Invalid username or password",
      });
    }
  };

  return (
    <div
      className="min-h-screen flex items-center justify-center px-4"
      style={{
        backgroundImage: "url('/images/imageEmployee2.png')",
        backgroundSize: "cover",
        backgroundPosition: "center",
        backgroundRepeat: "no-repeat"
      }}
    >
      {/* Transparent Glass Card */}
      <div className="bg-white/20 backdrop-blur-md rounded-2xl shadow-2xl p-8 w-full max-w-md border border-white/30">

        {/* Avatar / Logo */}
        <div className="flex flex-col items-center mb-6">
     <img 

     src="/ChetasLogo.webp" 
     alt="Logo" 
     
     className="w-24 h-auto mb-3"
 
     />
    <h1 className="text-xl font-semibold text-white">LOGIN FORM</h1>
   </div> 



        <form onSubmit={handleSubmit} className="space-y-5">

          {/* Username */}
          <div>
            <input
              type="text"
              placeholder="Enter Your Email"
              value={form.username}
              onChange={(e) => setForm({ ...form, username: e.target.value })}
              className="w-full px-4 py-2 bg-transparent border-b border-white text-white placeholder-white/70 focus:outline-none focus:border-green-400"
              required
            />
          </div>

          {/* Password */}
          <div>
            <input
              type="password"
              placeholder="Enter Your Password"
              value={form.password}
              onChange={(e) => setForm({ ...form, password: e.target.value })}
              className="w-full px-4 py-2 bg-transparent border-b border-white text-white placeholder-white/70 focus:outline-none focus:border-green-400"
              required
            />
          </div>

          {/* Remember me */}
          
          
          {/* Button */}
          <button
            type="submit"
            disabled={loading}
            className="w-full py-2 bg-green-600 hover:bg-green-700 text-white rounded-full font-semibold transition"
          >
            {loading ? "Logging in..." : "LOGIN"}
          </button>
        </form>
      </div>
    </div>
  );
};

export default Login;
