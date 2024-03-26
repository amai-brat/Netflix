import React, { useState } from 'react';
import { Formik, Form, Field, ErrorMessage } from 'formik';
import { Alert } from '@mui/material';
import { Link } from 'react-router-dom';

export const CustomForm = ({formType}) => {
    const [response, setResponse] = useState(null);

    const validate = (values) => {
        const errors = {};
        
        if (!values.login){
            errors.login = 'Обязательное поле';
        } else if (values.login.length < 4 && values.login.length > 32){
            errors.login = 'Минимальная длина логина - 4, максимальная - 32';
        } else if (!/^[a-zA-Z0-9_]$/.test(values.login)){
            errors.login = "Запрещенные символы";
        }

        if (!values.email) {
            errors.email = 'Обязательное поле';
        } else if (!/^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}$/i.test(values.email)) {
            errors.email = 'Неправильный адрес почты';
        }

        if (!values.password) {
            errors.password = "Обязательное поле";
        } else if (/^(?=.*[a-zA-Z])(?=.*\d)(?=.*[@;.,$!%*?&])$/.test(values.password)) {
            errors.password = "Пароль должен содержать букву, цифру и спецсимвол";
        }
      
        return errors;
    };
    
    const initialSignupValues = {
        login: '',
        email: '',
        password: '',
    };
    
    const initialSigninValues = {
        login: '',
        password: '',
        rememberMe: false,
    }

    const handleSubmit = async (values) => {    
        try {
            const response = await fetch(`https://localhost:5000/${formType}`, {
                method: "post",
                headers:{
                    "Content-Type": 'application/json'
                },
                body: values
            });

            if (response.ok){
                //Какая-то логика
                let message = formType == "signin" ? "Успешный вход" : "Успешная регистрация";
                setResponse({Success: true, Message: message});
            }
            else {
                let errorText = await response.text();
                setResponse({Success: false, Message: errorText})
            }
        }
        catch (error) {
            setResponse({Success: false, Message: "Произошла ошибка при отправке запроса"})
        }

        const formResults = JSON.stringify(values);
        console.log(values.login.length);
        console.log(formResults);
    };

    return (
        <Formik 
            initialValues={formType == "signup" ? initialSignupValues : initialSigninValues} 
            onSubmit={handleSubmit} 
            validate={formType=="signup"? validate : null}
        >
            <Form id="form">
                <div className="inputWrapper">
                    <Field type="text" name="login" placeholder="Логин"/>
                    <ErrorMessage name="login" component="span"/>
                </div>

                { formType == 'signup' ? (
                    <div className="inputWrapper">
                        <Field type="email" name="email" placeholder="Почта" />
                        <ErrorMessage name="email" component="span"/>
                    </div>
                    ) : null }

                <div className="inputWrapper">
                    <Field type="password" name="password" placeholder="Пароль"/>
                    <ErrorMessage name="password" component="span"/>
                </div>
                
                <div className="inputWrapper">
                    <button type="submit" className="submitButton">{formType === 'signin' ? 'Войти' : 'Зарегистрироваться'}</button>
                </div>

                {formType == 'signin' ? (
                    <div className="rememberCheckbox">
                        <Field type="checkbox" name="rememberMe" />
                        <label>Запомнить меня</label>
                    </div>
                ) : null}

                <div className="redirect">
                    <p>{formType=="signin" ? "Новенький в Netflix?" : "Уже есть аккаунт?"}</p>
                    <Link to={`/${formType=="signin" ? "signup" : "signin"}`}>{formType=="signin" ? "Зарегистрироваться" : "Войти"}</Link>
                </div>

                <Alert severity={response != null && response.Success ? "success" : "error"} 
                    variant="filled"
                    onClose = {() => setResponse(null)}
                    icon={false}
                    sx={{
                        marginTop: "2%", 
                        display: response != null ? "flex" : "none",
                        paddingTop: "0%",
                        paddingBottom: "0%"
                    }}>
                    {response != null ? response.Message : ""}
                </Alert>
            </Form>
        </Formik>
    );
};