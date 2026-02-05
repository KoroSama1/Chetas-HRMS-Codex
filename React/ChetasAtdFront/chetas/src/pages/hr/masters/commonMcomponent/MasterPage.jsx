import { useEffect, useRef, useState, useMemo } from "react";
import { masterApi } from "../../../../api/master.api";
import MasterForm from "./Masterform";
import MasterTable from "./MasterTable";
import { useAuth } from "../../../../auth/AuthContext";
import HRHeader from "../../components/HRHeader";
import Swal from "sweetalert2";


const MasterPage = ({ config }) => {
  const [data, setData] = useState([]);
  const [filteredData, setFilteredData] = useState([]);
  const [selected, setSelected] = useState(null);
  const [loading, setLoading] = useState(false);

  const [search, setSearch] = useState("");
  const [message, setMessage] = useState({ type: "", text: "" });

  const [deleteId, setDeleteId] = useState(null);
 
  const [sortOrder, setSortOrder] = useState("asc");
  const [page, setPage] = useState(1);
  const [pageSize, setPageSize] = useState(5);

  const [lastRefreshed, setLastRefreshed] = useState(null);

  const searchRef = useRef(null);
  const { logout } = useAuth();

  /* ================= MESSAGE ================= */
  const showMessage = (type, text) => {
    setMessage({ type, text });
    setTimeout(() => setMessage({ type: "", text: "" }), 3000);
  };

  /* ================= LOAD DATA ================= */
  const loadData = async (showToast = false) => {
    try {
      setLoading(true);
      const res = await masterApi.getAll(config.api);
      const list = res.data || [];
      setData(list);
      setFilteredData(list);
      setLastRefreshed(new Date());
      if (showToast) showMessage("success", "Data refreshed");
    } catch {
      showMessage("error", "Failed to load data");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadData();
  }, [config.api]);


  const showSwal = (type, text) => {
  const isSuccess = type === "success";

  return Swal.fire({
    icon: isSuccess ? "success" : "error",
    title: isSuccess ? "Success" : "Error",
    text,
    position: "top-end",
    timer: 1200,
    showConfirmButton: false,
    width: 240,
    backdrop: false,

    customClass: {
      popup: "rounded-lg shadow-md p-3",
      title: `text-sm font-semibold ${isSuccess ? "text-green-700" : "text-red-700"}`,
      htmlContainer: "text-xs text-gray-600",
    },
  });
};

  /* ================= SEARCH ================= */
  useEffect(() => {
    const q = search.toLowerCase();
    const result = data.filter(d =>
      d.name.toLowerCase().includes(q)
    );
    setFilteredData(result);
    setPage(1);
  }, [search, data]);

  /* ================= SORT ================= */
  const sortedData = useMemo(() => {
    return [...filteredData].sort((a, b) => {
      if (sortOrder === "asc") {
        return a.name.localeCompare(b.name);
      }
      return b.name.localeCompare(a.name);
    });
  }, [filteredData, sortOrder]);

  /* ================= PAGINATION ================= */
  const totalPages = Math.ceil(sortedData.length / pageSize);
  const paginatedData = sortedData.slice(
    (page - 1) * pageSize,
    page * pageSize
  );

  /* ================= CREATE / UPDATE ================= */
  const handleSubmit = async (payload) => {
    try {
      setLoading(true);
      if (
        !selected &&
        data.some(d => d.name.toLowerCase() === payload.name.toLowerCase())
      ) {
        showMessage("error", "Record already exists");
        return;
      }

      if (selected) {
        await masterApi.update(config.api, selected.id, payload);

        await showSwal(
          "success",
          `${config.label} updated successfully`
        );

        setSelected(null);

      } else {
        await masterApi.create(config.api, payload);

        await showSwal(
          "success",
          `${config.label} created successfully`
        );
      }
      loadData();
    } catch {
      showMessage("error", "Save failed");
    } finally {
      setLoading(false);
    }
  };

  /* ================= DELETE ================= */
  const confirmDelete = async () => {
    try {
      setLoading(true);
      await masterApi.remove(config.api, deleteId);
      showMessage("success", "Deleted successfully");
      setDeleteId(null);
      loadData();
    } catch {
      showMessage("error", "Delete failed (record may be in use)");
    } finally {
      setLoading(false);
    }
  };

  /* ================= EXPORT CSV ================= */
  const exportCSV = () => {
    const rows = [["ID", "Name"]];
    sortedData.forEach(r => rows.push([r.id, r.name]));

    const csv = rows.map(r => r.join(",")).join("\n");
    const blob = new Blob([csv], { type: "text/csv" });
    const url = URL.createObjectURL(blob);

    const a = document.createElement("a");
    a.href = url;
    a.download = `${config.label}_Master.csv`;
    a.click();
    URL.revokeObjectURL(url);
  };

  /* ================= KEYBOARD ================= */
  useEffect(() => {
    const handler = (e) => {
      if (e.key === "Escape") setSelected(null);
      if (e.key === "/" && searchRef.current) {
        e.preventDefault();
        searchRef.current.focus();
      }
    };
    window.addEventListener("keydown", handler);
    return () => window.removeEventListener("keydown", handler);
  }, []);

  return (
    <div className="min-h-screen bg-gradient-to-br from-gray-50 to-gray-100 px-4 py-6">
      <div className="w-full space-y-6">
        <HRHeader onLogout={logout} />
        {/* MESSAGE */}
        {message.text && (
          <div className={`px-4 py-2 rounded-md text-sm border
            ${message.type === "success"
              ? "bg-green-50 border-green-200 text-green-700"
              : "bg-red-50 border-red-200 text-red-700"}`}>
            {message.text}
          </div>
        )}

        {/* HEADER */}
        <div className="bg-white rounded-2xl border shadow-sm p-6 flex flex-wrap justify-between gap-3">
          <div>
            <h1 className="text-2xl font-bold">{config.label} Master</h1>
            <p className="text-sm text-gray-500">
              Last refreshed: {lastRefreshed?.toLocaleTimeString() || "-"}
            </p>
          </div>

          <div className="flex flex-wrap gap-2">
            <input
              ref={searchRef}
              value={search}
              onChange={(e) => setSearch(e.target.value)}
              placeholder="Search ( / )"
              className="px-3 py-1 border rounded text-sm"
            />

            <button onClick={exportCSV} className="px-3 py-1 border rounded text-sm">
              Export CSV
            </button>

            <button
              onClick={() => setSortOrder(o => o === "asc" ? "desc" : "asc")}
              className="px-3 py-1 border rounded text-sm"
            >
              Sort {sortOrder === "asc" ? "A–Z" : "Z–A"}
            </button>

            <button
              onClick={() => loadData(true)}
              className="px-3 py-1 border rounded text-sm"
            >
              Refresh
            </button>
          </div>
        </div>

        {/* CONTENT */}
        <div className="grid grid-cols-1 lg:grid-cols-12 gap-6">

          <div className="lg:col-span-4">
            <div className="bg-white border rounded-2xl p-6 shadow-sm sticky top-24">
              <MasterForm onSubmit={handleSubmit} selected={selected} />
            </div>
          </div>

          <div className="lg:col-span-8">
            <div className="bg-white border rounded-2xl p-6 shadow-sm">

              <MasterTable
                data={paginatedData}
                page={page}
                pageSize={pageSize}
                selected={selected}
                onEdit={setSelected}
                onDelete={(id) => setDeleteId(id)}
              />

              {/* PAGINATION */}
              <div className="flex justify-between items-center mt-4 text-sm">
                <div>
                  Page {page} of {totalPages}
                </div>
                <div className="flex gap-2">
                  <button disabled={page === 1} onClick={() => setPage(p => p - 1)}>Prev</button>
                  <button disabled={page === totalPages} onClick={() => setPage(p => p + 1)}>Next</button>
                </div>
              </div>

            </div>
          </div>
        </div>

        {/* DELETE MODAL */}
        {(deleteId) && (
          <div className="fixed inset-0 bg-black/40 flex items-center justify-center">
            <div className="bg-white p-6 rounded-xl w-80">
              <h3 className="font-semibold mb-2">Confirm Delete</h3>
              <p className="text-sm text-gray-500 mb-4">
                This action cannot be undone.
              </p>
              <div className="flex justify-end gap-2">
                <button onClick={() => {
                  setDeleteId(null);
                }}>Cancel</button>
                <button
                  onClick={confirmDelete}
                  className="bg-red-600 text-white px-3 py-1 rounded">
                  Delete
                </button>
              </div>
            </div>
          </div>
        )}

      </div>
    </div>
  );
};

export default MasterPage;
