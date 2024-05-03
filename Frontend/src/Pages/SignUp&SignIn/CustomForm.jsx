import React, { useState } from 'react';
import { Formik, Form, Field, ErrorMessage } from 'formik';
import { Alert } from '@mui/material';
import {Link, useNavigate} from 'react-router-dom';
import {authenticationService} from "../../services/authentication.service.js";

export const CustomForm = ({formType}) => {
    const [twoFactorEnabled, setTwoFactorEnabled] = useState(false);
    const [rememberMe, setRememberme] = useState(true);
    const [response, setResponse] = useState(null);
    const navigate = useNavigate();

    const validateSignin = (values) => {
        const errors = {};

        if (!values.email) {
            errors.email = 'Обязательное поле';
        }

        if (!values.password) {
            errors.password = "Обязательное поле";
        }

        return errors;
    }

    const validateSignup = (values) => {
        const errors = {};
        
        if (!values.login){
            errors.login = 'Обязательное поле';
        } else if (values.login.length < 4){
            errors.login = 'Минимальная длина логина - 4 символов';
        } else if (values.login.length > 25){
            errors.login = 'Максимальная длина логина - 25 символов'
        } else if (!/^[a-zA-Z0-9_]+$/.test(values.login)){
            errors.login = "Запрещенные символы";
        }

        if (!values.email) {
            errors.email = 'Обязательное поле';
        } else if (!/^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}$/i.test(values.email)) {
            errors.email = 'Неправильный адрес почты';
        }

        if (!values.password) {
            errors.password = "Обязательное поле";
        } else if(values.password.length < 8) {
            errors.password = "Минимальная длина пароля - 8 символов";
        } else if (values.password.length > 30) {
            errors.password = "Максимальная длина пароля - 30 символов"
        } else if (!/^(?=.*[a-zA-Z])(?=.*\d)(?=.*[@;.,$!%*?&]).+$/.test(values.password)) {
            errors.password = "Пароль должен содержать хотя бы одну букву, цифру и спецсимвол";
        }
      
        return errors;
    };
    
    const validateTwoFactor = (values) => {
        const errors = {};

        if (!values.twoFactorToken){
            errors.twoFactorToken = 'Обязательное поле';
        }

        return errors;
    };

    const initialSignupValues = {
        login: '',
        email: '',
        password: '',
    };
    
    const initialSigninValues = {
        email: '',
        password: '',
        rememberMe: false
    }

    const handleSubmit = async (values) => {    
        try {
            let resp;
            if (formType === "signin") {
                setRememberme(values.rememberMe);
                resp = await authenticationService.signin(values);
            } else {
                resp = await authenticationService.signup(values);
            }

            if (resp.ok){
                if (resp.data === "2FA"){
                    setTwoFactorEnabled(true);
                    return;
                }
                let message = formType === "signin" ? "Успешный вход" : "Успешная регистрация";
                setResponse({Success: true, Message: message});
                await new Promise((resolve => setTimeout(resolve, 1000)));
                navigate(formType === "signin" ? "/MainContent" : "/signin");
            }
            else {
                let errorText = resp.data.message.match(/^[^(]*/)[0];
                setResponse({Success: false, Message: errorText})
            }
        }
        catch (error) {
            setResponse({Success: false, Message: "Произошла ошибка при отправке запроса"})
        }
    };

    const handleTwoFactorTokenSubmit = async (values) => {
        const resp2fa = await authenticationService.sendTwoFactorToken(values.twoFactorToken, rememberMe);
        if (!resp2fa.ok) {
            let errorText = resp2fa.data.message.match(/^[^(]*/)[0];
            setResponse({Success: false, Message: errorText})
        }
        let message = "Успешный вход";
        setResponse({Success: true, Message: message});
        await new Promise((resolve => setTimeout(resolve, 1000)));
        navigate("/MainContent");
    }

    return (
    <>
        <Formik 
            initialValues={formType === "signup" ? initialSignupValues : initialSigninValues} 
            onSubmit={handleSubmit} 
            validate={formType === "signup"? validateSignup: validateSignin}
        >
            <Form id="form">
                { formType === 'signup' ? (
                  <div className="inputWrapper">
                      <Field type="text" name="login" placeholder="Логин"/>
                      <ErrorMessage name="login" component="span"/>
                  </div>
                ) : null}
                <div className="inputWrapper">
                    <Field type="email" name="email" placeholder="Почта" />
                    <ErrorMessage name="email" component="span"/>
                </div>

                <div className="inputWrapper">
                    <Field type="password" name="password" placeholder="Пароль"/>
                    <ErrorMessage name="password" component="span"/>
                </div>
                
                <div className="inputWrapper">
                    <button type="submit" className="submitButton">{formType === 'signin' ? 'Войти' : 'Зарегистрироваться'}</button>
                </div>
                {formType === 'signin' ? (
                    <div className="rememberCheckbox">
                        <Field type="checkbox" name="rememberMe" />
                        <label>Запомнить меня</label>
                    </div>
                ) : null}

                <div className="redirect">
                    <p>{formType === "signin" ? "Новенький в Netflix?" : "Уже есть аккаунт?"}</p>
                    <Link to={`/${formType === "signin" ? "signup" : "signin"}`}>{formType ==="signin" ? "Зарегистрироваться" : "Войти"}</Link>
                </div>

                <Alert severity={response != null && response.Success ? "success" : "error"} 
                    variant="filled"
                    onClose = {() => setResponse(null)}
                    icon={false}
                    sx={{
                        marginTop: "3%", 
                        display: response != null ? "flex" : "none",
                        paddingTop: "0%",
                        paddingBottom: "0%"
                    }}>
                    {response != null ? response.Message : ""}
                </Alert>
            </Form>
        </Formik>
        {twoFactorEnabled && 
            <Formik 
                initialValues={{
                    twoFactorToken: ''
                }}
                validate={validateTwoFactor}
                onSubmit={handleTwoFactorTokenSubmit}>
                <Form>
                    <div className="inputWrapper">
                        <Field type="text" name="twoFactorToken" placeholder="Токен двухфакторной аутентификации"/>
                        <ErrorMessage name="twoFactorToken" component="span"/>
                    </div>
                    <div className="inputWrapper">
                        <button type="submit" className="submitButton">Войти</button>
                    </div>
                </Form>
            </Formik>}
    </>
    );
};