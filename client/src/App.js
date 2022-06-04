import React, { useEffect, useState } from "react";
import Currency from "./components/Currency";
import "./App.css";

const baseUrl = "https://localhost:44359/api/Currency";

function App() {
  const [currency, setCurrency] = useState("");
  const [error, setError] = useState("");

  useEffect(() => {
    callApi().then((data) => {
      setCurrency(data.currencies);
    });
  }, []);

  const callApi = async () => {
    try {
      const res = await fetch(baseUrl);
      const resJson = await res.json();
      if (res.status === 200) {
        return resJson;
      } else {
        setError(resJson.message);
      }
    } catch (error) {
      console.log(error);
    }
  };

  return (
    <>
      {error && <div className="error-message">{error}</div>}
      <Currency currencypros={currency}></Currency>
    </>
  );
}
export default App;
