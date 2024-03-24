import { Typography, Modal, TextField, Button, Alert } from "@mui/material";
import { useState } from "react";

export const DataField = ({label, data, handleDataChange}) => {
    const [open, setOpen] = useState(false);
    const [response, setResponse] = useState(null);
    
    return (
        <>
            <div className = "dataField">
                <div style={{ flex: 1 }}>
                    <Typography variant="h6" gutterBottom sx={{
                        color: "white",
                        fontWeight: "800"
                    }}>{label}</Typography>

                    <Typography sx ={{
                        color: "#B3B3B3"
                    }}>{data}</Typography>
                </div>
                <Button variant="outlined" onClick={() => setOpen(true)}>Изменить</Button>
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
                    
                    <TextField
                        InputLabelProps={{ shrink: true }}
                        type = {label == "Дата рождения" ? "date" : "text"}
                        variant="filled"
                        fullWidth
                        label={label}
                        margin="normal"
                        sx={{
                            color: "white",
                            backgroundColor: "gray",
                            borderRadius: "10px",
                            marginBottom: "3%"
                        }}
                    />
                    <Button variant="contained" onClick={() => handleDataChange(setResponse, label)}>Сохранить</Button>

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