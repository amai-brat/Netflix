import styles from './css/AdminContent.module.css'
import {useState} from "react";
import AddSerialOptions from "./AddSerialOptions.jsx";
const AdminContent = () => {
    const [AddMovieClicked, setAddMovieClicked] = useState(false)
    const [AddSerialClicked, setAddSerialClicked] = useState(false)
    const [editMovieClicked, setEditMovieClicked] = useState(false)
    const [editSerialClicked, setEditSerialClicked] = useState(false)
    
    return (
        <div className={styles.main}>
            <div className={styles.delete}>
                <h2>Удалить контент по id</h2>
                <input type="text" placeholder="id"/>
                <button>Удалить</button>
            </div>
            <div className={styles.separator}></div>
            <div className={styles.add}>
                <div className={styles.addMovie}>
                    <h2>Добавить фильм</h2>
                    <div style={AddMovieClicked? {transform: "rotate(180deg)"}: {}} className={styles.arrow} onClick={() => setAddMovieClicked(!AddMovieClicked)}></div>
                </div>
                <div className={styles.addSerial}>
                    <h2>Добавить сериал</h2>
                    <div style={AddSerialClicked? {transform: "rotate(180deg)"}: {}} className={styles.arrow} onClick={() => setAddSerialClicked(!AddSerialClicked)}></div>
                </div>
                {AddSerialClicked && <div className={styles.addSerialOptions}>
                    <AddSerialOptions/>
                </div>}
                {AddMovieClicked && <div className={styles.addMovieOptions}></div>}
            </div>
            <div className={styles.separator}></div>
            <div className={styles.edit}>
                <div className={styles.editMovie}>
                    <h2>Редактировать фильм</h2>
                    <input type="text" placeholder="id"/>
                    <button onClick={() => setEditMovieClicked(!editMovieClicked)}>Редактировать</button>
                </div>
                <div className={styles.editSerial}>
                    <h2>Редактировать сериал</h2>
                    <input type="text" placeholder="id"/>
                    <button onClick={() => setEditSerialClicked(!editSerialClicked)}>Редактировать</button>
                </div>
                {editMovieClicked && <div className={styles.editMovieOptions}></div>}
                {editSerialClicked && <div className={styles.editSerialOptions}></div>}
            </div>
        </div>
    )
}
export default AdminContent