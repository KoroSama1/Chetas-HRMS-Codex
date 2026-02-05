import Chart from "react-apexcharts";

const HRStatsChart = () => {
  const series = [45, 20]; 

  const options = {
    chart: {
      type: "donut",
      toolbar: { show: false },
    },
    labels: ["Present", "Absent"],
    colors: ["#22c55e", "#ef4444"],
    legend: {
      show: false,
    },
    dataLabels: {
      enabled: false,
    },
    tooltip: {
      x: {
        show: false,
      },
    },
  };

  return (
    <div className="bg-white rounded-xl shadow p-4 mb-8">
      <h3 className="text-lg font-semibold mb-1 text-gray-700">
        Today’s Attendance
      </h3>

      <Chart
        options={options}
        series={series}
        type="donut"
        height={220}
      />

      {/* NEW STATS ROW */}
      <div className="mt-4 grid grid-cols-3 gap-3 text-center">
        <div className="rounded-lg bg-green-50 p-3">
          <p className="text-sm text-gray-500">Present</p>
          <p className="text-xl font-bold text-green-600">45</p>
        </div>

        <div className="rounded-lg bg-red-50 p-3">
          <p className="text-sm text-gray-500">Absent</p>
          <p className="text-xl font-bold text-red-500">20</p>
        </div>

        <div className="rounded-lg bg-indigo-50 p-3">
          <p className="text-sm text-gray-500">Total</p>
          <p className="text-xl font-bold text-indigo-600">65</p>
        </div>
      </div>
    </div>
  );
};

export default HRStatsChart;