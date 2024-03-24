import { useEffect, useState, useRef } from 'react';
import DefaultUserIcon from "../../../assets/DefaultUserIcon.svg";
import "./styles/personalInfo.css";
import { UserAvatar } from './components/UserAvatar';
import { DataField } from './components/DataField';
import { Divider } from '@mui/material';
import { PasswordField } from './components/PasswordField';

const PersonalInfoTab = () => {
    const[user, setUser] = useState({});
    const inputFile = useRef(null)
    
    const testUser = {
        Nickname: "TestGuy",
        ProfilePictureUrl: "https://distribution.faceit-cdn.net/images/02947bb3-c786-41df-a6a3-a4a9e6ed4d2f.jpeg",
        Email: "testguy2024@mail.com",
        BirthDay: "22.04.1987"
    }
    
    useEffect(() => {
        const fetchUserData = async () => {
            try {
                //TODO Подставить корректный url
                const response = await fetch("http://localhost:5000/getUserData");
                
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

    const handleAvatarChange = () => {
        //TODO сделать запрос к серверу на изменение фотографии
        alert("фотография изменена");
    }

    const handleDataChange = (setResponse, label) => {
        //Fetch, в зависимости от label - свой endpoint. Затем изменить User для отображения. 
        //Можно в ответе с сервера отправлять json юзера и соответственно тут установить setUser
        setResponse({Success: true, Message: "Все хорошо"})
    }

    return (
        <>
            <div className = {"profileDataBlock"}>
                <input type='file' id='file' ref={inputFile} accept="image/png, image/webp, image/jpeg"  
                    onChange={handleAvatarChange} style={{display: 'none'}}/>
                
                <UserAvatar pictureUrl={user.ProfilePictureUrl ?? DefaultUserIcon} onAvatarClick = {handleAvatarClick}/>

                <div className = {"userForm"}>
                    <DataField data={user.Nickname} label={"Никнейм"}></DataField>
                    <Divider sx={{ borderBottomWidth: "4px", background: "black"}}></Divider>

                    <DataField data={user.Email} label={"Почта"} handleDataChange={handleDataChange}></DataField>
                    <Divider sx={{ borderBottomWidth: "4px", background: "black"}}></Divider>

                    <DataField data={user.BirthDay} label={"Дата рождения"} handleDataChange={handleDataChange}></DataField>
                    <Divider sx={{ borderBottomWidth: "4px", background: "black"}}></Divider>

                    <PasswordField></PasswordField>
                </div>
            </div>
        </>
    )
}

export default PersonalInfoTab