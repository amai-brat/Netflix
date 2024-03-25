import { Typography, Modal, TextField, Button, Alert } from "@mui/material";
import { useState } from "react";

export const DataField = ({label, data, handleDataChange}) => {
    const [open, setOpen] = useState(false);
    const [value, setValue] = useState(null);
    const [response, setResponse] = useState(null);

    const handleInputChange = (e) => {
        const { value } = e.target;
        setValue(value);
    }
    
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
                
                {label != "Никнейм" && 
                    <Button variant="outlined" onClick={() => setOpen(true)}>Изменить</Button>
                }
            </div>
            
            {label != "Никнейм" && 
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
                        value={value}
                        margin="normal"
                        sx={{
                            color: "white",
                            backgroundColor: "gray",
                            borderRadius: "10px",
                            marginBottom: "3%"
                        }}
                        onChange = {handleInputChange}
                    />
                    <Button variant="contained" onClick={() => handleDataChange(setResponse, label, value)}>Сохранить</Button>

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
            }
        </>
    );
}