import { useEffect, useState, useRef } from 'react';
import DefaultUserIcon from "../../../assets/DefaultUserIcon.svg";
import "./styles/personalInfo.css";
import { UserAvatar } from './components/UserAvatar';
import { DataField } from './components/DataField';
import { Divider } from '@mui/material';
import { PasswordField } from './components/PasswordField';
import {baseUrl} from "../../Shared/HttpClient/baseUrl.js";

const PersonalInfoTab = () => {
    const inputFile = useRef(null)
    
    const testUser = {
        nickname: "TestGuy",
        profilePictureUrl: "https://distribution.faceit-cdn.net/images/02947bb3-c786-41df-a6a3-a4a9e6ed4d2f.jpeg",
        email: "testguy2024@mail.com",
        birthDay: "22.04.1987"
    }
    const[user, setUser] = useState(testUser);

    useEffect(() => {
        const fetchUserData = async () => {
            try {
                const response = await fetch(baseUrl + "user/get-personal-info");
                if (response.ok){
                    const user = await response.json();
                    setUser(user);
                }
                else{
                    setUser(null);
                }
                
            } catch (error) {
                console.log(error);
                setUser(testUser);
            }
        };

        fetchUserData();
    }, 
    []);

    const handleAvatarClick = () => {
        inputFile.current.click();
    };

    const handleAvatarChange = async () => {
        const formData = new FormData(document.getElementById("avatar-change-form"));
        try {
            const response = await fetch(baseUrl + "user/change-profile-picture", {
                method: "PATCH",
                body: formData
            });

            if (response.ok) {
                setUser(await response.json())
            } else {
                alert("Ошибка при изменении фотографии");
            }
        }
        catch (error) {
            alert("Ошибка при изменении фотографии");
        }
    }

    const handleDataChange = async (setResponse, label, data) => {
        const urlLabel = label == "Почта" ? "email" : "birthDay";
        
        try {
            const response = await fetch(`${baseUrl}user/change-${urlLabel}`, {
                method: "patch",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(data)
            });

            if (response.ok){
                let updatedUser = await response.json();
                setUser(updatedUser);
                setResponse({Success: true, Message: "Данные успешно обновлены"});
            }
            else {
                let errorText = await response.text();
                setResponse({Success: false, Message: errorText})
            }
        }
        catch (error) {
            setResponse({Success: false, Message: "Произошла ошибка при изменении информации пользователя"})
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

                    <PasswordField></PasswordField>
                </div>
            </div>
        </>
    )
}

export default PersonalInfoTab