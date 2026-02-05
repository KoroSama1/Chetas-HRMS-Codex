const MasterTable = ({ data, onEdit, onDelete, page, pageSize }) => {
  return (
    <div className="overflow-x-auto">
      <table className="min-w-full border border-gray-200 rounded-lg overflow-hidden">
        <thead className="bg-gray-100 text-gray-700 text-sm">
          <tr>
            <th className="px-4 py-2 text-left">ID</th>
            <th className="px-4 py-2 text-left">Name</th>
            <th className="px-4 py-2 text-center">Actions</th>
          </tr>
        </thead>
        <tbody className="divide-y">
          {data.map((item, index) => (
            <tr key={item.id} className="hover:bg-gray-50">
              <td className="px-4 py-2">{(page - 1) * pageSize + index + 1}</td>
              <td className="px-4 py-2">{item.name}</td>
              <td className="px-4 py-2 text-center space-x-2">
                <button
                  onClick={() => onEdit(item)}
                  className="px-3 py-1 text-sm border border-blue-500 text-blue-600 rounded hover:bg-blue-50"
                >
                  Edit
                </button>
                <button
                  onClick={() => onDelete(item.id)}
                  className="px-3 py-1 text-sm border border-red-500 text-red-600 rounded hover:bg-red-50"
                >
                  Delete
                </button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default MasterTable;
