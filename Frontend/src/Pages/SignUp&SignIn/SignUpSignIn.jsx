import React, { useEffect } from 'react';
import { CustomForm } from './CustomForm';
import { Link, useNavigate } from 'react-router-dom';
import netflixLogo from '../../assets/NetflixLogo.svg';
import googleLogo from '../../assets/GoogleLogo.svg';
import vkLogo from '../../assets/VkLogo.svg'
import "./Styles/pagestyle.css";
import { userService } from '../../services/user.service';

const SignUpSignIn = ({ formType }) => {
    const navigate = useNavigate();

    useEffect(() => {
        const getCurrentUserDataToCheckAuthenticationAsync = async () => {
            try{
                const {response, data} = await userService.getPersonalInfo();
                if(response.ok){
                    navigate("/MainContent")
                }
            }
            catch (error){
                console.log(error);
            }
        }

        getCurrentUserDataToCheckAuthenticationAsync();
    }, [])

    return (
        <>
            <div className="page">
                <div className="logoContainer">
                    <Link to={"/MainContent"}>
                        <img src={netflixLogo} alt={"logo"} width={150} height={50}/>
                    </Link>
                </div>
                <div className="formContainer">
                    <div className="formHeader">
                        {formType === "signup" ? "Зарегистрироваться" : "Войти"}
                    </div>

                    <div className="servicesContainer">
                        <Link to={"google"}>
                            <img src={googleLogo} alt="googleLogo" width={50} height={50}/>
                        </Link>
                        <Link to={"vk"}>
                            <img src={vkLogo} alt="vkLogo" width={50} height={50}/>
                        </Link>
                    </div>

                    <div className="divider">
                        или
                    </div>
                    
                    <div>
                        <CustomForm formType={formType}/>
                    </div>
                </div>
            </div>
        </>
    );
};

export default SignUpSignIn;