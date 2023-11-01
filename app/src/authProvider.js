import { MsalAuthProvider, LoginType } from "react-aad-msal";
import txt from "../src/elements/Etext.json"

// Msal Configurations
const config = {
  auth: {
    authority:
      "https://login.microsoftonline.com/"+ txt.tenantId,
    clientId: txt.clientId,
    redirectUri: txt.redirectUri,
  },
  cache: {
    cacheLocation: "localStorage",
    storeAuthStateInCookie: false,
  },
};


// Options
const options = {
  loginType: LoginType.Redirect,
  tokenRefreshUri: window.location.origin + "/auth.html",
};

export const authProvider = new MsalAuthProvider(
  config,
  options
);
