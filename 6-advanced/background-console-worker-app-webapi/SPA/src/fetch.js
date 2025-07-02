import { msalInstance } from "./index";

export async function callEndpointWithToken(endpoint, tokenRequest = { scopes: [] },  accessToken = null) {
    if (!accessToken) {
        const account = msalInstance.getActiveAccount();
        
        if (!account) {
            throw Error("No active account! Verify a user has been signed in and setActiveAccount has been called.");
        }
    
        const response = await msalInstance.acquireTokenSilent({
            account: account,
            ...tokenRequest
        });

        accessToken = response.accessToken;
    }

    const headers = new Headers();
    const bearer = `Bearer ${accessToken}`;

    headers.append("Authorization", bearer);

    const options = {
        method: "GET",
        headers: headers
    };

    return fetch(endpoint, options)
        .then(response => response.json())
        .catch(error => console.log(error));
}