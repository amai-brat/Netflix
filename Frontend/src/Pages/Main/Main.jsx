import { useState } from 'react'
import logo from '../../assets/logo.png';
import plus from '../../assets/plus.svg';
import {Link, useNavigate} from "react-router-dom";
import button from './styles/button.module.scss';
import sections from './styles/sections.module.scss';

const Main = () => {
    const navigate = useNavigate();
    function handleSignInButtonClick() {
        navigate("/signin");
    }

    function handleSignUpButtonClick() {
        navigate("/signup");
    }

    const faq = [
        {
            title: "Что такое Voltorka?",
            content: "Voltorka - это стриминговый сервис, предлагающий широкий выбор фильмов, сериалов, аниме, документальных фильмов и многого другого – на тысячах устройств, подключенных к Интернету. " +
                "Вы можете смотреть столько, сколько захотите, когда захотите, без единой рекламы – и все это по одной низкой ежемесячной цене. Всегда можно открыть для себя что-то новое!"
        },
        {
            title: "Что я могу смотреть в Voltorka?",
            content: "Voltorka располагает обширной библиотекой художественных и документальных фильмов, аниме и многого другого. Смотрите столько, сколько хотите, в любое удобное для вас время."
        }
    ];
    return (
        <>
            <div className={sections.top}>
                <header>
                    <Link to={"/MainContent"}>
                        <img src={logo} alt={"logo"} width={150} height={50}/>
                    </Link>
                    <button className={button.redButton} onClick={handleSignInButtonClick}>Войти</button>
                </header>
                <div className={sections.messageWrapper}>
                    <h1>
                        Неограниченное количество фильмов и сериалов<br/>
                        Смотрите в любом месте и в любое время
                    </h1>
                    <button className={button.redButton} onClick={handleSignUpButtonClick}>Начать</button>
                </div>
            </div>
            <div className={sections.bottom}>
                <div className={sections.faq}>
                    <h3 className={sections.faqTitle}>Часто задаваемые вопросы</h3>
                    <div className={sections.accordions}>
                        {faq.map((tuple) => 
                            <Accordion key={tuple.title} title={tuple.title} content={tuple.content}/> )}
                    </div>
                </div>
            </div>
            <footer className={sections.mainPageFooter}>
                <p><a href={"https://discord.gg/d9wRhhYNzG"}>Контакты</a></p>
                <p>© Copyright. Молокососы. 2024</p>
            </footer>
        </>
    )
}

export default Main

const Accordion = ({title, content}) => {
    const [isActive, setIsActive] = useState(false);

    return (
        <div className={sections.accordionItem}>
            <div className={sections.accordionTitle} onClick={() => setIsActive(!isActive)}>
                <p>{title}</p>
                <img src={plus} alt={"plus"} style={isActive ? {transform: "rotate(45deg)"} : {}}/>
            </div>
            {isActive && <div className={sections.accordionContent}>{content}</div>}
        </div>
    );
};
