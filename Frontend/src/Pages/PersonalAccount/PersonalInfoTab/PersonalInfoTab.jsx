import {useEffect, useRef, useState} from 'react';
import DefaultUserIcon from "../../../assets/DefaultUserIcon.svg";
import "./styles/personalInfo.css";
import {UserAvatar} from './components/UserAvatar';
import {DataField} from './components/DataField';
import {Divider, Typography, Button } from '@mui/material';
import {PasswordField} from './components/PasswordField';
import {userService} from "../../../services/user.service.js";
import { authenticationService } from '../../../services/authentication.service.js';

const PersonalInfoTab = () => {
    const inputFile = useRef(null)
    const[user, setUser] = useState({});
    const [twoFactorEnabled, setTwoFactorEnabled] = useState(false);

    useEffect(() => {
        const fetchUserData = async () => {
            try {
                const {response, data} = await userService.getPersonalInfo();
                if (response.ok){
                    setUser(data);
                }
                else{
                    setUser(null);
                }
                
            } catch (error) {
                console.log(error);
            }
        };

        fetchUserData();
    }, 
    []);

    useEffect(() => {
        (async() => {
            const {response, data} = await authenticationService.getWhetherTwoFactorEnabled();
            if (response.ok) {
                setTwoFactorEnabled(data);
            }
        })()
    }, []);

    const handleAvatarClick = () => {
        inputFile.current.click();
    };

    const handleAvatarChange = async () => {
        const formData = new FormData(document.getElementById("avatar-change-form"));
        try {
            const {response, data} = await userService.changeProfilePicture(formData);
            if (response.ok) {
                setUser(data)
            } else {
                alert("Ошибка при изменении фотографии");
            }
        }
        catch (error) {
            alert("Ошибка при изменении фотографии");
        }
    }

    const handleDataChange = async (setResponse, label, data) => {
        const urlLabel = label === "Почта" ? "email" : "birthDay";
        
        try {
            let response, responseData;
            if (urlLabel === "email") {
                const res = await userService.changeEmail(data);
                response = res.response;
                responseData = res.data;
            } else {
                const res = await userService.changeBirthDay(data);
                response = res.response;
                responseData = res.data;
            }

            if (response.ok){
                setUser(responseData);
                setResponse({Success: true, Message: "Данные успешно обновлены"});
            }
            else {
                setResponse({Success: false, Message: responseData.message})
            }
        }
        catch (error) {
            setResponse({Success: false, Message: "Произошла ошибка при изменении информации пользователя"})
        }
    }

    const handleTwoFactorActivateClick = async () => {
        const {response, data} = await authenticationService.enableTwoFactorAuth();
        if (response.ok) {
            setTwoFactorEnabled(true);
        }
    }

    return (
        <>
            <div className = {"profileDataBlock"}>
                <form id={"avatar-change-form"} encType={"multipart/form-data"}>
                    <input type='file' id='file' name={"image"} ref={inputFile} accept="image/png, image/webp, image/jpeg"
                           onChange={handleAvatarChange} style={{display: 'none'}}/>
                </form>

                <UserAvatar pictureUrl={user.profilePictureUrl ?? DefaultUserIcon} onAvatarClick = {handleAvatarClick}/>

                <div className = {"userForm"}>
                    <DataField data={user.nickname} label={"Никнейм"}></DataField>
                    <Divider sx={{ borderBottomWidth: "4px", background: "black"}}></Divider>

                    <DataField data={user.email} label={"Почта"} handleDataChange={handleDataChange}></DataField>
                    <Divider sx={{ borderBottomWidth: "4px", background: "black"}}></Divider>

                    <DataField data={user.birthDay} label={"Дата рождения"} handleDataChange={handleDataChange}></DataField>
                    <Divider sx={{ borderBottomWidth: "4px", background: "black"}}></Divider>

                    <div className = "dataField">
                        <div style={{ flex: 1 }}>
                            <Typography variant="h6" gutterBottom sx={{
                                color: "white",
                                fontWeight: "800"
                            }}>2FA</Typography>

                            <Typography sx ={{
                                color: "#B3B3B3"
                            }}>{twoFactorEnabled ? "Активирована" : "Не активирована"}</Typography>
                        </div>
                        
                        {!twoFactorEnabled && 
                            <Button variant="outlined" onClick={handleTwoFactorActivateClick}>Активировать</Button>
                        }
                    </div>
                    <Divider sx={{ borderBottomWidth: "4px", background: "black"}}></Divider>

                    <PasswordField></PasswordField>
                </div>
            </div>
        </>
    )
}

export default PersonalInfoTab