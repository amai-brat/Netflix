import { useState } from 'react'
import logo from './logo.svg';
import plus from '../../assets/plus.svg';
import {Link, useNavigate} from "react-router-dom";
// import './Main.scss';

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
            title: "Что такое Netflix?",
            content: "Netflix - это стриминговый сервис, предлагающий широкий выбор фильмов, сериалов, аниме, документальных фильмов и многого другого – на тысячах устройств, подключенных к Интернету. " +
                "Вы можете смотреть столько, сколько захотите, когда захотите, без единой рекламы – и все это по одной низкой ежемесячной цене. Всегда можно открыть для себя что-то новое!"
        },
        {
            title: "Что я могу смотреть в Netflix?",
            content: "Netflix располагает обширной библиотекой художественных и документальных фильмов, аниме и многого другого. Смотрите столько, сколько хотите, в любое удобное для вас время."
        }
    ];
    return (
        <>
            <div id={"top"}>
                <header>
                    <Link to={"/MainContent"}>
                        <img src={logo} alt={"logo"} width={150} height={50}/>
                    </Link>
                    <button onClick={handleSignInButtonClick}>Войти</button>
                </header>
                <div className={"message-wrapper"}>
                    <h1>
                        Неограниченное количество фильмов и сериалов<br/>
                        Смотрите в любом месте и в любое время
                    </h1>
                    <button onClick={handleSignUpButtonClick}>Начать</button>
                </div>
            </div>
            <div id={"bottom"}>
                <div id={"faq"}>
                    <h3>Часто задаваемые вопросы</h3>
                    <div className={"accordions"}>
                        {faq.map((tuple) => 
                            <Accordion key={tuple.title} title={tuple.title} content={tuple.content}/> )}
                    </div>
                </div>
            </div>
            <footer>
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
        <div className="accordion-item">
            <div className="accordion-title" onClick={() => setIsActive(!isActive)}>
                <p>{title}</p>
                <img src={plus} alt={"plus"} style={isActive ? {transform: "rotate(45deg)"} : {}}/>
            </div>
            {isActive && <div className="accordion-content">{content}</div>}
        </div>
    );
};
