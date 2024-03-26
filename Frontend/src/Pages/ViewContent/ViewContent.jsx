import { useState } from 'react'
import ContentInfo from "./ContentInfo.jsx";
import styles from './styles/ViewContent.module.css';
import ContentPlayer from "./contentPlayer.jsx";
import Reviews from "./Reviews.jsx";
const contentData =
    {
        id: 1,
        title: "Пираты Карибского моря: Проклятие Черной жемчужины (2003)",
        description: "Жизнь харизматичного авантюриста, капитана Джека Воробья, полная увлекательных приключений, резко меняется, когда его заклятый враг капитан Барбосса похищает корабль Джека Черную Жемчужину, а затем нападает на Порт Ройал и крадет прекрасную дочь губернатора Элизабет Свонн.\n" +
            "\n" +
            "Друг детства Элизабет Уилл Тернер вместе с Джеком возглавляет спасательную экспедицию на самом быстром корабле Британии, чтобы вызволить девушку и заодно отобрать у злодея Черную Жемчужину. Вслед за этой парочкой отправляется амбициозный коммодор Норрингтон, который к тому же числится женихом Элизабет.\n" +
            "\n" +
            "Однако Уилл не знает, что над Барбоссой висит вечное проклятие, при лунном свете превращающее его с командой в живых скелетов. Проклятье будет снято лишь тогда, когда украденное золото Ацтеков будет возвращено пиратами на старое место.",
        slogan: "«Over 3000 islands of paradise. For some it’s a blessing. For others… It’s A Curse»",
        country: "США",
        poster: "https://www.kinopoisk.ru/images/film_big/4374.jpg",
        releaseDate: 2003,
        movieLength: 144,
        contentType: "фильм",
        ageRating: {
            "age" : 12,
            "ageMpaa": "PG-13"
        },
        ratings:{
            "imdb": 8.0,
            "kinopoisk": 8.1,
            "local" : 8.2
        },
        trailerInfo:{
            name: "Проклятие Чёрной жемчужины - Трейлер",
            url: "https://www.youtube.com/embed/0xxLhqjbB2Y?modestbranding=1",
        },
        budget:{
            "budget": 140_000_000,
             currency: "USD"        
        },
        genres: [
            "боевик",
            "фэнтези",
            "приключения"
        ],
        personInContent: [
            {
                "id": 6245,
                "photo": "https://st.kp.yandex.net/images/actor_iphone/iphone360_6245.jpg",
                "name": "Джонни Депп",
                "enName": "Johnny Depp",
                "description": "Jack Sparrow",
                "profession": "актеры",
                "enProfession": "actor"
            },
            
            {
                "id": 24683,
                "photo": "https://st.kp.yandex.net/images/actor_iphone/iphone360_24683.jpg",
                "name": "Джеффри Раш",
                "enName": "Geoffrey Rush",
                "description": "Barbossa",
                "profession": "актеры",
                "enProfession": "actor"
            },
            {
                "id": 30875,
                "photo": "https://st.kp.yandex.net/images/actor_iphone/iphone360_30875.jpg",
                "name": "Орландо Блум",
                "enName": "Orlando Bloom",
                "description": "Will Turner",
                "profession": "актеры",
                "enProfession": "actor"
            },
            {
                "id": 24302,
                "photo": "https://st.kp.yandex.net/images/actor_iphone/iphone360_24302.jpg",
                "name": "Кира Найтли",
                "enName": "Keira Knightley",
                "description": "Elizabeth Swann",
                "profession": "актеры",
                "enProfession": "actor"
            },
            {
                "id": 3695,
                "photo": "https://st.kp.yandex.net/images/actor_iphone/iphone360_3695.jpg",
                "name": "Джек Девенпорт",
                "enName": "Jack Davenport",
                "description": null,
                "profession": "актеры",
                "enProfession": "actor"
            },
            {
                "id": 8630,
                "photo": "https://st.kp.yandex.net/images/actor_iphone/iphone360_8630.jpg",
                "name": "Кевин МакНэлли",
                "enName": "Kevin McNally",
                "description": "Joshamee Gibbs (в титрах: Kevin R. McNally)",
                "profession": "актеры",
                "enProfession": "actor"
            },
            {
                "id": 15074,
                "photo": "https://st.kp.yandex.net/images/actor_iphone/iphone360_15074.jpg",
                "name": "Джонатан Прайс",
                "enName": "Jonathan Pryce",
                "description": "Governor Weatherby Swann",
                "profession": "актеры",
                "enProfession": "actor"
            },
            {
                "id": 23731,
                "photo": "https://st.kp.yandex.net/images/actor_iphone/iphone360_23731.jpg",
                "name": "Ли Аренберг",
                "enName": "Lee Arenberg",
                "description": "Pintel",
                "profession": "актеры",
                "enProfession": "actor"
            },
            {
                "id": 30876,
                "photo": "https://st.kp.yandex.net/images/actor_iphone/iphone360_30876.jpg",
                "name": "Макензи Крук",
                "enName": "Mackenzie Crook",
                "description": "Ragetti",
                "profession": "актеры",
                "enProfession": "actor"
            },
            {
                "id": 10038,
                "photo": "https://st.kp.yandex.net/images/actor_iphone/iphone360_10038.jpg",
                "name": "Дэвид Бэйли",
                "enName": "David Bailie",
                "description": "Cotton",
                "profession": "актеры",
                "enProfession": "actor"
            },
            {
                "id": 10038,
                "photo": "https://st.kp.yandex.net/images/actor_iphone/iphone360_10038.jpg",
                "name": "Дэвид Бэйли",
                "enName": "David Bailie",
                "description": "Cotton",
                "profession": "актеры",
                "enProfession": "actor"
            },
            {
                "id": 608586,
                "photo": "https://st.kp.yandex.net/images/actor_iphone/iphone360_608586.jpg",
                "name": "Клаус Бадельт",
                "enName": "Klaus Badelt",
                "description": null,
                "profession": "композиторы",
                "enProfession": "composer"
            },
            {
                "id": 42549,
                "photo": "https://st.kp.yandex.net/images/actor_iphone/iphone360_42549.jpg",
                "name": "Дерек Р. Хилл",
                "enName": "Derek R. Hill",
                "description": null,
                "profession": "художники",
                "enProfession": "designer"
            },
            {
                "id": 2865971,
                "photo": "https://st.kp.yandex.net/images/actor_iphone/iphone360_2865971.jpg",
                "name": "Майкл Пауэлс",
                "enName": "Michael Powels",
                "description": null,
                "profession": "художники",
                "enProfession": "designer"
            },
            {
                "id": 2000232,
                "photo": "https://st.kp.yandex.net/images/actor_iphone/iphone360_2000232.jpg",
                "name": "Джеймс Е. Точчи",
                "enName": "James E. Tocci",
                "description": null,
                "profession": "художники",
                "enProfession": "designer"
            },
            {
                "id": 1997558,
                "photo": "https://st.kp.yandex.net/images/actor_iphone/iphone360_1997558.jpg",
                "name": "Дональд Б. Вудрафф",
                "enName": "Donald B. Woodruff",
                "description": null,
                "profession": "художники",
                "enProfession": "designer"
            },
            {
                "id": 2355007,
                "photo": "https://st.kp.yandex.net/images/actor_iphone/iphone360_2355007.jpg",
                "name": "Пенни Роуз",
                "enName": "Penny Rose",
                "description": null,
                "profession": "художники",
                "enProfession": "designer"
            },
            {
                "id": 2354628,
                "photo": "https://st.kp.yandex.net/images/actor_iphone/iphone360_2354628.jpg",
                "name": "Ларри Диас",
                "enName": "Larry Dias",
                "description": null,
                "profession": "художники",
                "enProfession": "designer"
            },
            {
                "id": 30870,
                "photo": "https://st.kp.yandex.net/images/actor_iphone/iphone360_30870.jpg",
                "name": "Гор Вербински",
                "enName": "Gore Verbinski",
                "description": null,
                "profession": "режиссеры",
                "enProfession": "director"
            },
            {
                "id": 611243,
                "photo": "https://st.kp.yandex.net/images/actor_iphone/iphone360_611243.jpg",
                "name": "Дариуш Вольски",
                "enName": "Dariusz Wolski",
                "description": null,
                "profession": "операторы",
                "enProfession": "operator"
            },
            {
                "id": 10207,
                "photo": "https://st.kp.yandex.net/images/actor_iphone/iphone360_10207.jpg",
                "name": "Джерри Брукхаймер",
                "enName": "Jerry Bruckheimer",
                "description": null,
                "profession": "продюсеры",
                "enProfession": "producer"
            },
            {
                "id": 23681,
                "photo": "https://st.kp.yandex.net/images/actor_iphone/iphone360_23681.jpg",
                "name": "Пол Дисон",
                "enName": "Paul Deason",
                "description": null,
                "profession": "продюсеры",
                "enProfession": "producer"
            },
            {
                "id": 30919,
                "photo": "https://st.kp.yandex.net/images/actor_iphone/iphone360_30919.jpg",
                "name": "Брюс Хендрикс",
                "enName": "Bruce Hendricks",
                "description": null,
                "profession": "продюсеры",
                "enProfession": "producer"
            },
            {
                "id": 10210,
                "photo": "https://st.kp.yandex.net/images/actor_iphone/iphone360_10210.jpg",
                "name": "Чад Оман",
                "enName": "Chad Oman",
                "description": null,
                "profession": "продюсеры",
                "enProfession": "producer"
            },
            {
                "id": 916397,
                "photo": "https://st.kp.yandex.net/images/actor_iphone/iphone360_916397.jpg",
                "name": "Александр Баргман",
                "enName": null,
                "description": null,
                "profession": "актеры дубляжа",
                "enProfession": "voice_actor"
            },
            {
                "id": 273210,
                "photo": "https://st.kp.yandex.net/images/actor_iphone/iphone360_273210.jpg",
                "name": "Виктор Костецкий",
                "enName": null,
                "description": null,
                "profession": "актеры дубляжа",
                "enProfession": "voice_actor"
            },
            {
                "id": 1188897,
                "photo": "https://st.kp.yandex.net/images/actor_iphone/iphone360_1188897.jpg",
                "name": "Андрей Зайцев",
                "enName": null,
                "description": null,
                "profession": "актеры дубляжа",
                "enProfession": "voice_actor"
            },
            {
                "id": 269814,
                "photo": "https://st.kp.yandex.net/images/actor_iphone/iphone360_269814.jpg",
                "name": "Евгения Игумнова",
                "enName": null,
                "description": null,
                "profession": "актеры дубляжа",
                "enProfession": "voice_actor"
            },
            {
                "id": 916982,
                "photo": "https://st.kp.yandex.net/images/actor_iphone/iphone360_916982.jpg",
                "name": "Евгений Дятлов",
                "enName": null,
                "description": null,
                "profession": "актеры дубляжа",
                "enProfession": "voice_actor"
            },
            {
                "id": 30871,
                "photo": "https://st.kp.yandex.net/images/actor_iphone/iphone360_30871.jpg",
                "name": "Тед Эллиот",
                "enName": "Ted Elliott",
                "description": null,
                "profession": "редакторы",
                "enProfession": "writer"
            },
            {
                "id": 30872,
                "photo": "https://st.kp.yandex.net/images/actor_iphone/iphone360_30872.jpg",
                "name": "Терри Россио",
                "enName": "Terry Rossio",
                "description": null,
                "profession": "редакторы",
                "enProfession": "writer"
            },
            {
                "id": 30873,
                "photo": "https://st.kp.yandex.net/images/actor_iphone/iphone360_30873.jpg",
                "name": "Стюарт Битти",
                "enName": "Stuart Beattie",
                "description": null,
                "profession": "редакторы",
                "enProfession": "writer"
            },
            {
                "id": 30874,
                "photo": "https://st.kp.yandex.net/images/actor_iphone/iphone360_30874.jpg",
                "name": "Джей Уолперт",
                "enName": "Jay Wolpert",
                "description": null,
                "profession": "редакторы",
                "enProfession": "writer"
            }
        ]
    }
    const reviews =  [
        {
            name: "User1",
            text: "Отличный фильм, смотрел несколько раз",
            score: 8,
            writtenAt: "2021-09-01",
            isPositive: true,
            likes: -100,
            comments: [

            ]
        }
    ]
const ViewContent = () => {
    return (
        <>
            <div className={styles.generalContainer}>
                <div className={styles.wholePageContainer}>
                    <ContentInfo contentData={contentData}/>
                    <ContentPlayer contentId={contentData.id}/>
                    <Reviews reviews={reviews}/>
                </div>
            </div>
        </>
    )
}

export default ViewContent