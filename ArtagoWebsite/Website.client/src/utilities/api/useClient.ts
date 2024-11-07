import { useEffect, useState } from "react";
import settings from "../../settings";
import { AuthorizedClient } from "./AuthorizedClient";
import { useSelector } from "react-redux";
import { RootState } from "../../reducer/store";

export default function useApiClient() {
    const token = useSelector((state: RootState) => state.authentication.token);
    const [client, setClient] = useState<AuthorizedClient | undefined>(() => {
        if (token) {
            return undefined;
        } else {
            return new AuthorizedClient(settings.baseUrl, token);
        }
    });

    useEffect(() => {
        const newClient = new AuthorizedClient(settings.baseUrl, token);
        setClient(newClient);
    }, [token]);

    return client;
}