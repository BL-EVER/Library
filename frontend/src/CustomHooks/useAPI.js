import { useState, useEffect } from 'react';
import { useOidcAccessToken } from '@axa-fr/react-oidc';

//Usage example: const libraryAPI = useAPI(LibraryAPI);
const useAPI = (APIClass) => {
    const [apiInstance, setApiInstance] = useState(null);
    const { accessToken } = useOidcAccessToken();

    useEffect(() => {
            const instance = new APIClass(accessToken);
            setApiInstance(instance);
    }, [APIClass, accessToken]);

    return apiInstance;

};

export default useAPI;