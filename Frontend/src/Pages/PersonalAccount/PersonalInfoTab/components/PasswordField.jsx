import { Typography, Button, Modal, TextField, Alert } from "@mui/material";
import { useState } from "react";
import {userService} from "../../../../services/user.service.js";


export const PasswordField = () => {
    const [open, setOpen] = useState(false);
    const [response, setResponse] = useState(null);

    const [previousPassword, setPreviousPassword] = useState('');
    const [newPassword, setNewPassword] = useState('');
    const [newRepeatPassword, setNewRepeatPassword] = useState('');

    const handlePasswordChange = async () => {
        if (!newPassword.match(/^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$ %^&*-]).{8,}$/))
        {
            setResponse({Success: false, Message: "Минимум 8 символов, хотя бы одна заглавная латинская буква, одна строчная латинская буква, одна цифра и спец. символ"});
            return;
        }

        if (newPassword !== newRepeatPassword)
        {
            setResponse({Success: false, Message: "Пароли не совпадают"});
            return;
        }

        try {
            const {response, data} = await userService.changePassword(previousPassword, newPassword);
            
            if (response.ok){
                setResponse({Success: true, Message: "Пароль поменялся"});
            }
            else {
                setResponse({Success: false, Message: data.message.match(/^[^(]*/)[0]})
            }

        } catch (error) {
            console.log(error);
            setResponse({Success: false, Message: "Произошла ошибка при сохранении"});
        }
    };

    return(
        <>
            <div style={{ display: 'flex', alignItems: 'center', marginBottom: "8px", marginTop: "10px" }}>
                <div style={{ flex: 1 }}>
                    <Typography variant="h6" gutterBottom sx={{
                        color: "white",
                        fontWeight: "800"
                    }}>Пароль</Typography>

                    <Typography sx ={{
                        color: "#B3B3B3"
                    }}>Для смены пароля нажмите на кнопку</Typography>
                </div>
                <Button variant="outlined" onClick = {() => setOpen(true)}>Изменить</Button>
            </div>

            <Modal open={open} onClose={() => setOpen(false)}  sx={{
                display: "flex",
                justifyContent: "center",
                alignItems: "center"
            }}>
                <div className = "modalContainer">
                    <Typography variant="h6" gutterBottom sx={{
                        color: "white"
                    }}>{"Смена пароля"}</Typography>
                    
                    <PasswordTextField label="Пароль" marginBottom={"2%"}
                                       value={previousPassword} setValue={setPreviousPassword}/>
                    
                    <PasswordTextField label="Новый пароль" marginBottom={"0%"}
                                       value={newPassword} setValue={setNewPassword}/>
                    <PasswordTextField label="Повторите новый пароль" marginBottom={"3%"}
                                       value={newRepeatPassword} setValue={setNewRepeatPassword}/>
                    
                    <Button variant="contained" onClick={handlePasswordChange}>Сохранить</Button>

                    <Alert severity={response != null && response.Success ? "success" : "error"} 
                           variant="filled"
                        onClose = {() => setResponse(null)}
                        icon={false}
                        sx={{
                            marginTop: "4%", 
                            display: response != null ? "flex" : "none"
                        }}>
                        {response != null ? response.Message : ""}
                    </Alert>
                </div>
            </Modal>
        </>
    );
}

const PasswordTextField = ({label, marginBottom, value, setValue}) => {
    return (
        <TextField
            variant="filled"
            fullWidth
            type="password"
            label={label}
            margin="normal"
            value={value}
            onChange={x => setValue(x.target.value)}
            sx={{
                color: "white",
                backgroundColor: "gray",
                borderRadius: "10px",
                marginBottom: {marginBottom}
            }}
        />
    );
};