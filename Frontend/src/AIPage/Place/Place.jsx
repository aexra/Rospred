import "./Place.css";
import { useState } from "react";
import { Bar } from "react-chartjs-2";
import ButtonReg from "../../Choice/Button/ButtonReg";

export default function Place() {
  const [tableData, setTableData] = useState([
    { id: 1, value: "" },
    { id: 2, value: "" },
    { id: 3, value: "" },
  ]);
  const [confirmed, setConfirmed] = useState(false);

  const handleTableChange = (id, newValue) => {
    setTableData((prevData) =>
      prevData.map((item) =>
        item.id === id ? { ...item, value: newValue } : item
      )
    );
  };

  const handleConfirm = () => {
    setConfirmed(true);
  };

  return (
    <div className="place">
      {" "}
      <table>
        <thead>
          <tr>
            <th>Column 1</th>
            <th>Column 2</th>
            <th>Column 3</th>
            <th>Column 4</th>
            <th>Column 5</th>
          </tr>
        </thead>
        <tbody>
          {tableData.map((row) => (
            <tr key={row.id}>
              <td>
                <input
                  type="text"
                  value={row.value}
                  onChange={(e) => handleTableChange(row.id, e.target.value)}
                />
              </td>
              <td>
                <input
                  type="text"
                  value={tableData.find((item) => item.id === row.id).value}
                  onChange={(e) => handleTableChange(row.id, e.target.value)}
                />
              </td>
              <td>
                <input
                  type="text"
                  value={tableData.find((item) => item.id === row.id).value}
                  onChange={(e) => handleTableChange(row.id, e.target.value)}
                />
              </td>
              <td>
                <input
                  type="text"
                  value={tableData.find((item) => item.id === row.id).value}
                  onChange={(e) => handleTableChange(row.id, e.target.value)}
                />
              </td>
              <td>
                <input
                  type="text"
                  value={tableData.find((item) => item.id === row.id).value}
                  onChange={(e) => handleTableChange(row.id, e.target.value)}
                />
              </td>
            </tr>
          ))}
        </tbody>
      </table>
      <button className="transparent-button" onClick={handleConfirm}>
        Подтвердить
      </button>
      <div className={`chart ${confirmed ? "show" : ""}`}></div>
    </div>
  );
}
