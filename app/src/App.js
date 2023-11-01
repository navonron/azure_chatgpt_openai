import "./App.css";
import { Route, Switch, withRouter } from "react-router-dom";
import "bootstrap/dist/css/bootstrap.min.css";
import Csso from "./componenets/Csso";
import { AzureAD } from "react-aad-msal";
import { authProvider } from "./authProvider";

function App() {
  return (
    <div className="App">
      <AzureAD provider={authProvider} forceLogin={true}>
        <Switch>
          <Route path="/" component={Csso} provider={authProvider} exact />
        </Switch>
      </AzureAD>
    </div>
  );
}

export default withRouter(App);
