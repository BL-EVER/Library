import React from 'react';
import { Navigate } from 'react-router-dom';
import { useOidc, useOidcAccessToken } from "@axa-fr/react-oidc";

const ProtectedPageWrapper = ({ authorizationRoles = [], children }) => {
    const { login, isAuthenticated } = useOidc();
    const { accessTokenPayload } = useOidcAccessToken();

    if (!isAuthenticated) {
        login(window.location.pathname);
        return null;
    }

    if (
        authorizationRoles.length === 0 ||
        accessTokenPayload?.realm_access?.roles.some(r => authorizationRoles.includes(r))
    ) {
        return children;
    }

    return <Navigate to="/" />;
};

export default ProtectedPageWrapper;