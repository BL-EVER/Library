import {useOidc, useOidcAccessToken} from "@axa-fr/react-oidc";
import React from "react";

const ProtectedComponentWrapper = ({ authorizationRoles = [], children }) => {
    const { isAuthenticated } = useOidc();
    const { accessTokenPayload } = useOidcAccessToken();

    if (!isAuthenticated) {
        return <></>;
    }

    if (
        authorizationRoles.length === 0 ||
        accessTokenPayload?.realm_access?.roles.some(r => authorizationRoles.includes(r))
    ) {
        return children;
    }

    return <></>;
};

export default ProtectedComponentWrapper;