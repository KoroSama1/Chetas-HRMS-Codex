import { useEffect, useState } from "react";



const MasterForm = ({ onSubmit, selected }) => {
  const [name, setName] = useState("");
  const [error, setError] = useState("");

  useEffect(() => {
    if (selected) {
      setName(selected.name);
      setError("");
    } else {
      setName("");
    }
  }, [selected]);

 const handleSubmit = async (e) => {
  e.preventDefault();

  if (!name.trim()) {
    setError("Name is required");
    return;
  }

  setError("");

  try {
    // Just call parent, no popup here
    await onSubmit({ name });

    setName("");

  } catch (err) {
    const msg =
      err?.response?.data?.message ||
      "Operation failed. Please try again.";

    setError(msg);
  }
};



  return (
    <form
      onSubmit={handleSubmit}
      className="space-y-4"
    >
      {/* Title */}
      <div>
        <h3 className="text-lg font-semibold text-gray-800">
          {selected ? "Update" : "Create"} Item
        </h3>
        <p className="text-sm text-gray-500">
          Enter the name and submit the form
        </p>
      </div>

      {/* Input */}
      <div>
        <label className="block text-sm font-medium text-gray-700 mb-1">
          Name <span className="text-red-500">*</span>
        </label>

        <input
          type="text"
          value={name}
          maxLength={100}
          placeholder="Enter name"
          onChange={(e) => {
            setName(e.target.value);
            if (error) setError("");
          }}
          className={`w-full px-3 py-2 border rounded-md text-sm
            focus:outline-none focus:ring-2
            ${
              error
                ? "border-red-500 focus:ring-red-300"
                : "border-gray-300 focus:ring-blue-400"
            }
          `}
        />

        {/* Helper / Error text */}
        {error ? (
          <p className="text-xs text-red-500 mt-1">{error}</p>
        ) : (
          <p className="text-xs text-gray-400 mt-1">
            Maximum 100 characters
          </p>
        )}
      </div>

      {/* Action buttons */}
      <div className="flex gap-2 pt-2">
        <button
          type="submit"
          className={`flex-1 py-2 rounded-md text-sm font-medium text-white transition
            ${
              selected
                ? "bg-orange-500 hover:bg-orange-600"
                : "bg-blue-600 hover:bg-blue-700"
            }
          `}
        >
          {selected ? "Update" : "Create"}
        </button>

        {selected && (
          <button
            type="button"
            onClick={() => {
              setName("");
              setError("");
            }}
            className="flex-1 py-2 rounded-md text-sm font-medium border border-gray-300 text-gray-600 hover:bg-gray-50"
          >
            Clear
          </button>
        )}
      </div>
    </form>
  );
};

export default MasterForm;
