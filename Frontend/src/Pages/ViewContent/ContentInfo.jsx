import styles from './styles/ContentInfo.module.css';
import {toast} from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import {useEffect, useState} from "react";

function ContentInfo({contentData}){
    const groupedByRole = contentData.personInContent.reduce((acc, person) => {
        if (acc[person.profession]) {
            acc[person.profession].push(person);
        } else {
            acc[person.profession] = [person];
        }
        return acc;
    }, {});
    // должно прийти 11 профессий по каждой роли. если их меньше, то последний не получает , ...
    // если их пришло 11, то последний получает , ...
    // это не гарантия что их больше 11, но хоть что-то
    function printAllPersonsByRole(profession){
        return groupedByRole[profession].map((person, index) => {
            if (groupedByRole[profession].length < 11){
                if (index === (groupedByRole[profession].length - 1)) {
                    return person.name + "";
                } else {
                    return person.name + ", ";
                }
            }
            else{
                if (index === (groupedByRole[profession].length - 1)) {
                    return person.name + ", ...";
                } else {
                    return person.name + ", ";
                }
            }
        });
    }
    function getNoun(number, one, two, five) {
        let n = Math.abs(number);
        n %= 100;
        if (n >= 5 && n <= 20) {
            return five;
        }
        n %= 10;
        if (n === 1) {
            return one;
        }
        if (n >= 2 && n <= 4) {
            return two;
        }
        return five;
    }
    function capitalizeFirstLetter(string) {
        return string.charAt(0).toUpperCase() + string.slice(1);
    }
    async function addToFavourites() {
        try{
            const resp = await fetch("http://localhost:5000/api/favourites/add", {
                method: "POST",
                //TODO: добавить токен
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({contentId: contentData.id})
            });
            const body = await resp.json();
            if (resp.ok) {
                notifyAboutResult(true, body.message);
            } else {
                notifyAboutResult(false, body.message);
            }
        } catch (e) {
            notifyAboutResult(false, "Произошла ошибка при добавлении в избранное");
        }
    }
    function notifyAboutResult(isSuccess, message){
        if (isSuccess) {
            toast.success(message, {
                position: "bottom-center"
            });
        } else {
            toast.error(message, {
                position: "bottom-center"
            });
        }
    }
    return (
        <>
            <div className={styles.container}>
                <div className={styles.posterTrailer}>
                    <div className={styles.poster}>
                        <div className={styles.favourite} title={"В избранное"} onClick={addToFavourites}></div>
                        <img src={contentData.poster} alt="poster" className={styles.poster}/>
                    </div>
                    {contentData.trailerInfo &&
                        <div className={styles.trailer}>
                            <h3>{contentData.trailerInfo.name}</h3>
                            <iframe className={styles.trailerEmbed}
                                    allowFullScreen={true}
                                    src={contentData.trailerInfo.url}>
                            </iframe>
                        </div>
                    }
                </div>
                <div className={styles.contentInfo}>
                    <span className={styles.title}>{contentData.title}</span>
                    <span className={styles.description}>{contentData.description}</span>
                    <h2>Детали</h2>
                    {contentData.country &&
                        <span><strong>Страна:</strong> {contentData.country}</span>
                    }
                    {contentData.slogan && 
                        <span><strong>Слоган:</strong> {contentData.slogan}</span>
                    }
                    {contentData.genres && 
                        <span><strong>Жанры:</strong> {contentData.genres.join(", ")}</span>
                    }
                    {contentData.releaseYears && 
                        <span><strong>Годы выхода:</strong> {contentData.releaseYears.start + " - " + contentData.releaseYears.end}</span>
                    }
                    {contentData.seasonInfos && 
                        <span><strong>Количество сезонов: </strong> {contentData.seasonInfos.length}</span>}
                    {contentData.contentType && 
                        <span><strong>Тип контента:</strong> {contentData.contentType}</span>
                    }
                    {contentData.releaseDate && 
                        <span><strong>Дата выхода:</strong> {contentData.releaseDate}</span>
                    }
                    {contentData.movieLength &&
                        <span><strong>Продолжительность:</strong> {contentData.movieLength} {getNoun(contentData.movieLength, "минута", "минуты", "минут")}</span>
                    }
                    {contentData.budget &&
                        <span><strong>Бюджет:</strong> {contentData.budget.budget} {contentData.budget.currency}</span>
                    }
                    {contentData.ageRating && 
                        <span className={styles.ageRating}><strong>Возрастной рейтинг: </strong>
                            <span>
                                {contentData.ageRating.age && <span>{contentData.ageRating.age}+</span>}
                                {contentData.ageRating.ageMpaa && <span> ({contentData.ageRating.ageMpaa})</span>}
                            </span>
                        </span>
                    }
                    <span className={styles.ratings}>
                        <strong>Рейтинги:</strong>
                        {contentData.ratings.imdb && <span className={styles.ratingImdb}>IMDb: {contentData.ratings.imdb}</span>}
                        {contentData.ratings.kinopoisk && <span className={styles.ratingKinopoisk}>Кинопоиск: {contentData.ratings.kinopoisk}</span>}
                        <span className={styles.ratingLocal}>Локальный: {contentData.ratings.local == null? "недостаточно оценок": contentData.ratings.local}</span></span>
                    <h2>В главных ролях:</h2>
                    <span>{printAllPersonsByRole("актеры")}</span>
                    <h2>Также работали</h2>
                    {/*перепиши код ниже чтобы там был keys*/}
                    {Object.keys(groupedByRole).map((role) => {
                        if (role === "актеры") {
                            return;
                        }
                        return <span key={role}><strong>{capitalizeFirstLetter(role) + ": "}</strong> {printAllPersonsByRole(role)}</span>

                    })}
                </div>

            </div>
        </>


    );
}
export default ContentInfo