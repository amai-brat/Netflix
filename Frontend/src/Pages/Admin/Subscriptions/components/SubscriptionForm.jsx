import {useFormik} from "formik";
import formStyles from "../styles/form.module.scss";
import { DataGrid } from '@mui/x-data-grid';
import {createTheme, ThemeProvider} from "@mui/material";
import {useContext, useEffect, useState} from "react";
import {baseUrl} from "../../../Shared/HttpClient/baseUrl.js";
import {toast} from "react-toastify";
import {SubscriptionsContext} from "./SubscriptionsContext.js";
import {getSubscriptions} from "../httpClient.js";


export const SubscriptionForm = ({ subscription }) => {
    const [contents, setContents] = useState([]);
    const {setSubscriptions} = useContext(SubscriptionsContext);
    
    useEffect(() => {
        (async() => {
            try {
                const response = await fetch(baseUrl + "admin/subscription/contents", {
                    method: "GET",
                    headers: {
                        // TODO: auth token
                        // "Authorization": "Bearer [token]"
                    }
                });
                
                if (response.ok) {
                    const data = await response.json();
                    setContents(data);
                }
            } catch (e) {
                console.log(e);
            }
        })();
    }, []);
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
        setAccessibleContentIds(subscription ? subscription.accessibleContent?.map(x => x.id) : []);
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
            
            if (!subscription) {
                try {
                    const response = await fetch(baseUrl + "admin/subscription/add", {
                        method: "POST",
                        headers: {
                            "Content-Type": "application/json"
                        },
                        body: JSON.stringify(values)
                    })
                    
                    if (response.ok) {
                        toast.success("Успешно создана", {
                            position: "bottom-center"
                        });
                        setSubscriptions((await getSubscriptions()).data);
                    } else {
                        toast.error(await response.json(),{
                            position: "bottom-center"
                        });
                    }
                } catch (e) {
                    toast.error("Ошибка", {
                        position: "bottom-center"
                    })
                }
            } else {
                const dto = {
                    subscriptionId: subscription.id,
                    newName: values.name,
                    newDescription: values.description,
                    newMaxResolution: values.maxResolution,
                    newPrice: values.price,
                    accessibleContentIdsToAdd: values.accessibleContentIds.filter(x => !subscription.accessibleContent.map(y => y.id).includes(x.id)),
                    accessibleContentIdsToRemove: values.accessibleContentIds.filter(x => subscription.accessibleContent.map(y => y.id).includes(x.id)),
                };

                try {
                    const response = await fetch(baseUrl + "admin/subscription/edit", {
                        method: "PUT",
                        headers: {
                            "Content-Type": "application/json"
                        },
                        body: JSON.stringify(dto)
                    })

                    if (response.ok) {
                        toast.success("Успешно изменено", {
                            position: "bottom-center"
                        });
                        setSubscriptions((await getSubscriptions()).data);
                    } else {
                        toast.error(await response.json(),{
                            position: "bottom-center"
                        });
                    }
                } catch (e) {
                    toast.error("Ошибка", {
                        position: "bottom-center"
                    })
                }
            }
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
                    <DataGrid style={{color: "white"}} checkboxSelection columns={columns} rows={contents}
                              onRowSelectionModelChange={(model) => {setAccessibleContentIds(model)}}
                              rowSelectionModel={accessibleContentIds}
                    sx={dataGridStyles}></DataGrid>
                </ThemeProvider>
            </div>
            <button className={formStyles.submitButton} type={"submit"}>Отправить</button>
            <br/>
        </form>
    )
};

const dataGridStyles = {
    "& .MuiDataGrid-container--top [role=row]": {
        background: "#313131",
    }
}

const theme = createTheme({
    palette: {
        text: {
            primary: '#ffffff',
        },
    },
    components: {
        MuiPaper: {
            styleOverrides: {
                root: {
                    backgroundColor: "#313131",
                    color: "white"
                }
            }
        },
        MuiDataGrid: {
            styleOverrides: {
                root: {
                    width: "100%",
                    color: "white"
                }
            }
        },
        MuiTypography: {
            styleOverrides: {
                root: {
                    color: 'white'
                }
            }
        }
    }
});