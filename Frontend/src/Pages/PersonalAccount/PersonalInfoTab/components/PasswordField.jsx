import { Typography, Button, Modal, TextField, Alert } from "@mui/material";
import { useState } from "react";

export const PasswordField = () => {
    const [open, setOpen] = useState(false);
    const [response, setResponse] = useState(null);
    
    const handlePasswordChange = () => {
        //TODO валидация двух новых паролей на совпадение и соответствие (спец. символы, заглавная буква и т.д.)
        setResponse({Success: false, Message: "Произошла ошибка при сохранении"})
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
                    }}>{"Изменить информацию"}</Typography>
                    
                    <PasswordTextField label="Пароль" marginBottom={"2%"}></PasswordTextField>
                    
                    <PasswordTextField label="Новый пароль" marginBottom={"0%"}></PasswordTextField>
                    <PasswordTextField label="Повторите новый пароль" marginBottom={"3%"}></PasswordTextField>
                    
                    <Button variant="contained" onClick={handlePasswordChange}>Сохранить</Button>

                    <Alert severity={response != null && response.Success ? "success" : "error"} 
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

const PasswordTextField = ({label, marginBottom}) => {
    return (
        <TextField
            variant="filled"
            fullWidth
            type="password"
            label={label}
            margin="normal"
            sx={{
                color: "white",
                backgroundColor: "gray",
                borderRadius: "10px",
                marginBottom: {marginBottom}
            }}
        />
    );
};