import React from "react";
import { authProvider } from "../authProvider";
import { AzureAD, AuthenticationState } from "react-aad-msal";
import Cchat from "./Cchat";
import logout_logo from "../assets/logout.png";
import "../css/sso.css";
import txt from "../elements/Etext";

const Csso = () => {
  return (
    <div>
      <AzureAD provider={authProvider} forceLogin={true}>
        {({ login, logout, authenticationState, error, accountInfo }) => {
          switch (authenticationState) {
            case AuthenticationState.Authenticated:
              return (
                <div>
                  <button onClick={logout} className="message" xs={12}>
                    <img src={logout_logo} className="send_img" xs={12} />
                    Logout
                  </button>
                  <br />
                  <br />
                  <Cchat useremail={accountInfo.account.userName} />
                </div>
              );
            case AuthenticationState.Unauthenticated:
              return (
                <div>
                  {error && (
                    <p>
                      <span>{txt.authError}</span>
                    </p>
                  )}
                  <p>
                    <span>{txt.authError}</span>
                    <button onClick={login}>Login</button>
                  </p>
                </div>
              );
            case AuthenticationState.InProgress:
              return <p>Authenticating...</p>;
            case AuthenticationState.ClientAuthError:
              return <p>error</p>;
          }
        }}
      </AzureAD>
    </div>
  );
};

export default Csso;
