import React from 'react';
import ReactDOM from 'react-dom/client';
import App from './App';
import {OidcProvider} from "@axa-fr/react-oidc";
import { Global } from '@emotion/react'

import '@fontsource/roboto/300.css';
import '@fontsource/roboto/400.css';
import '@fontsource/roboto/500.css';
import '@fontsource/roboto/700.css';

const globalStyles = `
  body {
    margin: 0;
  }
`;

const configuration = {
    authority: "http://localhost:8080/realms/app",
    client_id: "frontend",
    logout_redirect_uri: window.location.origin,
    monitor_session: true,
    redirect_uri: window.location.origin + "/authentication/callback",
    scope: "openid profile email offline_access", // offline_access scope allow your client to retrieve the refresh_token
    //service_worker_relative_url: "/OidcServiceWorker.js",
    service_worker_only: false,
};

const root = ReactDOM.createRoot(document.getElementById('root'));
root.render(
  <React.StrictMode>
      <Global styles={globalStyles} />
      <OidcProvider configuration={configuration}>
        <App />
      </OidcProvider>
  </React.StrictMode>
);

