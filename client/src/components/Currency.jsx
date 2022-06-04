import React, { useEffect , useState } from "react";
import Card from "@material-ui/core/Card";
import CardContent from "@material-ui/core/CardContent";
import Typography from "@material-ui/core/Typography";

function Currency() {
  const baseUrl = "https://localhost:44359/api/Currency";
 
  const [currency, setCurrency] = useState("");
  const [error, setError] = useState("");

  useEffect(() => {
    getCurrencyApi().then((data) => {
      setCurrency(data.currencies);
    });
  }, []);

  const getCurrencyApi = async () => {
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
      <Typography>Currency Last Update: {currency.last_Update}</Typography>
      {currency.currency &&
        currency.currency
          .filter((currency) => currency.change < 0)
          .map((currency, index) => (
            <Card key={currency.currencycode + index}>
              <CardContent>
                <Typography color="textPrimary">
                  {currency.currencyCode}
                </Typography>
                <Typography variant="body2" color="textSecondary">
                  {currency.change}
                </Typography>
              </CardContent>
            </Card>
          ))}
    </>
  );
}
export default Currency;
