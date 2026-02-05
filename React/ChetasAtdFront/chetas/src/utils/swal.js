import Swal from "sweetalert2";

const SwalAlert = {
  success: (title, text) => {
    return Swal.fire({
      icon: "success",
      title: title || "Success",
      text: text || "",
      timer: 2000,
      showConfirmButton: false
    });
  },

  error: (title, text) => {
    return Swal.fire({
      icon: "error",
      title: title || "Error",
      text: text || "",
    });
  },

  info: (title, text) => {
    return Swal.fire({
      icon: "info",
      title: title || "Info",
      text: text || "",
    });
  }
};

export default SwalAlert;
