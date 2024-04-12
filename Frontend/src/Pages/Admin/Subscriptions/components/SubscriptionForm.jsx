import {useFormik} from "formik";
import formStyles from "../styles/form.module.scss";
import { DataGrid } from '@mui/x-data-grid';
import {createTheme, ThemeProvider} from "@mui/material";
import {useEffect, useState} from "react";


export const SubscriptionForm = ({ subscription }) => {
    const contents = [
        {
            id: 1,
            name: "BLYYYYA"
        },
        {
            id: 2,
            name: "FAPAHHH"
        },
        {
            id: 3,
            name: "BLYYYYA"
        },
        {
            id: 4,
            name: "FAPAHHH"
        }
    ];
    
    const columns = [
        {
            field: 'id', headerName: 'Id', width: 120
        },
        {
            field: 'name', headerName: 'Name', width: 120
        }
    ];
    
    const validate = (values) => {
        const errors = {};
        
        if (!values.name) {
            errors.name = "Обязательное поле";
        } else if (values.name.replace(/\s/g, '').length === 0) {
            errors.name = "Название не может содержать только пробелы"
        } else if (!/^[А-Яа-яA-Za-z0-9-_ ]+$/.test(values.name)) {
            errors.name = "Может содержать только буквы, цифры, дефис, подчёркивание, пробел"
        }
        
        if (!values.description) {
            errors.description = "Обязательное поле"
        } else if (values.description.replace(/\s/g, '').length === 0) {
            errors.description = "Описание не может содержать только пробелы"
        }
        
        if (values.maxResolution && values.maxResolution <= 0) {
            errors.maxResolution = "Разрешение должен быть больше нуля"
        } 
        
        if (!values.price) {
            errors.price = "Обязательное поле"
        } else if (!values.price < 0) {
            errors.price = "Цена не может быть меньше нуля"
        }
        
        return errors;
    }
    
    useEffect(() => {
        setAccessibleContentIds(subscription ? subscription.accessibleContents?.map(x => x.id) : []);
    }, [subscription]);
    
    const [accessibleContentIds, setAccessibleContentIds] = useState();
    const formik= useFormik({
        initialValues: {
            name: subscription?.name ?? '',
            description: subscription?.description ?? '',
            maxResolution: subscription?.maxResolution ?? '',
            price: subscription?.price ?? '',
        },
        validate,
        onSubmit: async (values) => {
            values.accessibleContentIds = accessibleContentIds;
            console.log(values)
        },
        enableReinitialize: true
    });
    
    return (
        <form className={formStyles.form} onSubmit={formik.handleSubmit}>
            <div className={formStyles.inputWrapper}>
                <label htmlFor={"name"}>Название</label>
                <input name={"name"}
                       className={formStyles.allInputs + ' ' + formStyles.input}
                       onChange={formik.handleChange}
                       value={formik.values.name}/>
                {formik.errors.name ? <span>{formik.errors.name}</span> : null}
            </div>
            <div className={formStyles.inputWrapper}>
                <label htmlFor={"description"}>Описание</label>
                <textarea name={"description"}
                          rows={5} cols={10}
                          className={formStyles.allInputs}
                          onChange={formik.handleChange}
                          value={formik.values.description}/>
                {formik.errors.description ? <span>{formik.errors.description}</span> : null}
            </div>
            <div className={formStyles.inputWrapper}>
                <label htmlFor={"maxResolution"}>Максимальное разрешение</label>
                <input name={"maxResolution"}
                       type={"number"}
                       className={formStyles.allInputs}
                       onChange={formik.handleChange}
                       value={formik.values.maxResolution}
                />
                {formik.errors.maxResolution ? <span>{formik.errors.maxResolution}</span> : null}
            </div>
            <div className={formStyles.inputWrapper}>
                <label htmlFor={"price"}>Цена</label>
                <input name={"price"}
                       type={"number"}
                       className={formStyles.allInputs}
                       onChange={formik.handleChange}
                       value={formik.values.price}
                />
                {formik.errors.price ? <span>{formik.errors.price}</span> : null}
            </div>
            <div className={formStyles.inputWrapper}>
                <label>Доступные по подписке произведения</label>
                <ThemeProvider theme={theme}>
                    <DataGrid checkboxSelection columns={columns} rows={contents}
                              onRowSelectionModelChange={(model) => {setAccessibleContentIds(model)}}
                              rowSelectionModel={accessibleContentIds}
                    sx={dataGridStyles}></DataGrid>
                </ThemeProvider>
            </div>
            <button className={formStyles.submitButton} type={"submit"}>Отправить</button>
            <br/>
            <span id={"serverMessage"}></span>
        </form>
    )
};

const dataGridStyles = {
    "& .MuiDataGrid-container--top [role=row]": {
        background: "#313131",
    }
}

const theme = createTheme({
    components: {
        MuiPaper: {
            styleOverrides: {
                root: {
                    backgroundColor: "#313131",
                }
            }
        },
        MuiDataGrid: {
            styleOverrides: {
                root: {
                    width: "100%",
                }
            }
        }
    }
});