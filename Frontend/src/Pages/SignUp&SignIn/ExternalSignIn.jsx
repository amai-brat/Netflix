import React, {useEffect, useState} from "react";
import googleLogo from "../../assets/GoogleLogo.svg";
import vkLogo from "../../assets/VkLogo.svg";
import {baseUrl} from "../../httpClient/baseUrl.js";
import {useLocation, useNavigate, useSearchParams} from "react-router-dom";
import {authenticationService} from "../../services/authentication.service.js";
import {Alert} from "@mui/material";

const ExternalSignIn = () => {
    const [response, setResponse] = useState(null);
    const [query] = useSearchParams()
    const location = useLocation()
    const provider = location.pathname.split('/').at(-1)
    const code = query.get("code")
    const navigate = useNavigate()
    
    
    const authAsync = async () => {
        const resp = await authenticationService.externalSignIn(provider, code)
        if(resp.ok){
            setResponse({Success: true, Message: "Успешный вход"});
            await new Promise((resolve => setTimeout(resolve, 1000)));
            navigate("/MainContent");
        }else{
            if(response === null){
                const errorText = resp.data.message.match(/^[^(]*/)[0];
                setResponse({Success: false, Message: errorText})   
            }
        }
    }

    useEffect(() => {
        if(code !== null){
            authAsync()
        }
    }, []);
    
    return (
        <div>
            <Alert severity={response != null && response.Success ? "success" : "error"}
                   variant="filled"
                   onClose={() => setResponse(null)}
                   icon={false}
                   sx={{
                       marginTop: "3%",
                       display: response != null ? "flex" : "none",
                       paddingTop: "0%",
                       paddingBottom: "0%"
                   }}>
                {response != null ? response.Message : ""}
            </Alert>
            <div className="servicesContainer">
                <form id="sign-in-google" action={baseUrl + "auth/external/google"}>
                    <img
                        src={googleLogo}
                        alt="googleLogo"
                        width={50}
                        height={50}
                        onClick={() => {
                            document.getElementById("sign-in-google").submit()
                        }}
                    />
                </form>
                <form id="sign-in-vk" action={baseUrl + "auth/external/vk"}>
                    <img
                        src={vkLogo}
                        alt="vkLogo"
                        width={50}
                        height={50}
                        onClick={() => {
                            document.getElementById("sign-in-vk").submit()
                        }}
                    />
                </form>
            </div>
        </div>
    )
}

export default ExternalSignIn