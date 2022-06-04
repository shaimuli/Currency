import Card from "@material-ui/core/Card";
import CardContent from "@material-ui/core/CardContent";
import Typography from "@material-ui/core/Typography";

function Currency(props) {
  const currency = props.currencypros.currency;
  const lastUppdate = props.currencypros.last_Update;
  return (
    <>
      <Typography>Currency Last Update: {lastUppdate}</Typography>
      {currency &&
        currency
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
