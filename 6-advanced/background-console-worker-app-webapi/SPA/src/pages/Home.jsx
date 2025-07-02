import 'bootstrap/dist/css/bootstrap.min.css';  
import { Button } from 'react-bootstrap'  

import { useEffect, useState } from "react";
import { MsalAuthenticationTemplate } from "@azure/msal-react";
import { loginRequest, protectedResources } from "../authConfig";
import { InteractionType } from "@azure/msal-browser";
import { callEndpointWithToken } from "../fetch";

import { ProfileData } from "../components/DataDisplay";

const ProfileContent = () => {
    /**
     * useMsal is hook that returns the PublicClientApplication instance,
     * an array of all accounts currently signed in and an inProgress value
     * that tells you what msal is currently doing. For more, visit:
     * https://github.com/AzureAD/microsoft-authentication-library-for-js/blob/dev/lib/msal-react/docs/hooks.md
     */
    const [profileData, setProfileData] = useState(null);

    useEffect(() => {
        if (!profileData) {
            callEndpointWithToken(protectedResources.profileApi.endpoint, loginRequest)
                .then(response => setProfileData(response));
        }
    }, [profileData]);

    return (
        <>
            { profileData ? <ProfileData  profileData={profileData} /> : <p className='data-area-div'>Retrieving user data from server...</p> }
        </>
    );
};

/***
 * Component to detail ID token claims with a description for each claim. For more details on ID token claims, please check the following links:
 * ID token Claims: https://learn.microsoft.com/en-us/entra/identity-platform/id-tokens#claims-in-an-id-token
 * Optional Claims:  https://learn.microsoft.com/en-us/entra/identity-platform/active-directory-optional-claims#v10-and-v20-optional-claims-set
 */
export const Home = () => {
    const authRequest = {
        ...loginRequest
    };


    return (
        <>
            <MsalAuthenticationTemplate
                interactionType={InteractionType.Popup}
                authenticationRequest={authRequest}
            >
                <ProfileContent />
            </MsalAuthenticationTemplate>
        </>
    );
}
