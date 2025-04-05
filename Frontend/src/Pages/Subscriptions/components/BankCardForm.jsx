import formStyles from '../styles/cardForm.module.scss';
import { useFormik } from "formik";
import {useNavigate} from "react-router-dom";
import {subscriptionService} from "../../../services/subscription.service.js";
import {authenticationService} from "../../../services/authentication.service.js";
import _ from "lodash";

export const BankCardForm = ({ subscriptionId }) => {
    const validate = values => {
        const errors = {};
        if (!values.cardNumber) {
            errors.cardNumber = 'Обязательное поле';
        } else if (values.cardNumber.length !== 16) {
            errors.cardNumber = 'Номер карты состоит из 16 цифр';
        }
        
        if (!values.cardOwner) {
            errors.cardOwner = 'Обязательное поле';
        }
        
        if (!values.validThru) {
            errors.validThru = 'Обязательное поле';
        } else if (!/^(0[1-9]|1[0-2])\/[0-9]{2}$/.test(values.validThru)) {
            errors.validThru = 'Дата в формате: ММ/ГГ';
        }

        if (!values.cvc) {
            errors.cvc = 'Обязательное поле';
        } else if (!/^[0-9]{3}$/.test(values.cvc)) {
            errors.cvc = '3 цифры на задней стороне карты';
        }
        
        return errors;
    };
    
    const navigate = useNavigate();
    const formik = useFormik({
        initialValues: {
            cardNumber: '',
            cardOwner: '',
            validThru: '',
            cvc: '',
            subscriptionId: subscriptionId
        },
        validate,
        onSubmit: async (values) => {
            const v = {card: {..._.omit(values, ['subscriptionId'])}, subscriptionId: values.subscriptionId}
            let {response} = await subscriptionService.buySubscription(v);
            
            if (response.ok)
            {
                await authenticationService.refreshToken();
                document.getElementById("serverMessage").textContent = 
                    "Успешная покупка. Перенаправление на главную страницу";
                setTimeout(() => navigate("/mainContent"), 2000);
            }
            else {
                document.getElementById("serverMessage").textContent = "Ошибка";
            }
        },
    });
    return (
        <form className={formStyles.bankCardForm} onSubmit={formik.handleSubmit}>
            <div className={formStyles.inputWrapper}>
                <label htmlFor={"cardNumber"}>Номер карты</label>
                <input id={"cardNumber"}
                       name={"cardNumber"}
                       onChange={formik.handleChange}
                       value={formik.values.cardNumber}/>
                {formik.errors.cardNumber ? <span>{formik.errors.cardNumber}</span> : null}
            </div>
            <div className={formStyles.inputWrapper}>
                <label htmlFor={"cardOwner"}>ФИО обладателя карты</label>
                <input id={"cardOwner"}
                       name={"cardOwner"}
                       onChange={formik.handleChange}
                       value={formik.values.cardOwner}/>
                {formik.errors.cardOwner ? <span>{formik.errors.cardOwner}</span> : null}
            </div>
            <div className={formStyles.expiresAndCvcWrapper}>
                <div className={formStyles.inputWrapper}>
                    <label htmlFor={"validThru"}>Срок</label>
                    <input id={"validThru"}
                           name={"validThru"}
                           onChange={formik.handleChange}
                           value={formik.values.validThru}/>
                    {formik.errors.validThru ? <span>{formik.errors.validThru}</span> : null}
                </div>
                <div className={formStyles.inputWrapper}>
                    <label htmlFor={"cvc"}>CVC/CVV</label>
                    <input id={"cvc"}
                           name={"cvc"}
                           onChange={formik.handleChange}
                           value={formik.values.cvc}/>
                    {formik.errors.cvc ? <span>{formik.errors.cvc}</span> : null}
                </div>
            </div>
            <button className={formStyles.submitButton} type={"submit"}>Отправить</button>
            <br/>
            <span id={"serverMessage"}></span>
        </form>
    );
};