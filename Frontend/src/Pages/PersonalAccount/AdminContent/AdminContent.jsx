import styles from './css/AdminContent.module.css'
import {useState} from "react";
import AddSerialOptions from "./AddSerialOptions.jsx";
import { ToastContainer, toast } from 'react-toastify';
import AddMovieOptions from "./AddMovieOptions.jsx";
import EditMovieOptions from "./EditMovieOptions.jsx";
import EditSerialOptions from "./EditSerialOptions.jsx";
const AdminContent = () => {
    const [AddMovieClicked, setAddMovieClicked] = useState(false)
    const [AddSerialClicked, setAddSerialClicked] = useState(false)
    const [editMovieClicked, setEditMovieClicked] = useState(false)
    const [editMovieId, setEditMovieId] = useState(0)
    const [editSerialClicked, setEditSerialClicked] = useState(false)
    const [editSerialId, setEditSerialId] = useState(0)
    const [editMovieOptions, setEditMovieOptions] = useState([])
    const [editSerialOptions, setEditSerialOptions] = useState([])
    const [idToDelete, setIdToDelete] = useState(0)
    const stopEditMovie = () => {
        setEditMovieClicked(false)
    }
    const stopEditSerial = () => {
        setEditSerialClicked(false)
    }
    const editMovieClickedAction = async () => {
        const resp = await fetch(`http://localhost:5114/content/admin/movie/${editMovieId}`, {
            method: "GET",
            headers: {
                "Content-Type": "application/json"
            }
        })
        const json = await resp.json()
        if (resp.ok) {
            setEditMovieOptions(json)
            setEditMovieClicked(true)
        } else {
            setEditMovieClicked(false)
            toast.error(json.message, {
                position: "bottom-center"
            })
        }
    }
    const editSerialClickedAction = async () => {
        const resp = await fetch(`http://localhost:5114/content/admin/serial/${editSerialId}`, {
            method: "GET",
            headers: {
                "Content-Type": "application/json"
            }
        })
        const json = await resp.json()
        if (resp.ok) {
            setEditSerialClicked(true)
            setEditSerialOptions(json)
        } else {
            toast.error(json.message, {
                position: "bottom-center"
            })
        }
    }
    const deleteContent = async () => {
        if (idToDelete <= 0) {
            toast.error("Введите id")
            return
        }
        const resp = await fetch(`http://localhost:5114/content/delete/${idToDelete}`, {
            method: "GET",
            headers: {
                "Content-Type": "application/json"
            }
        })
        if (resp.status === 200) {
            toast.success("Успешно удалено", {
                position: "bottom-center"
            })
        } else {
            const json = await resp.json()
            toast.error(json.message, {
                position: "bottom-center"
            })
        }
    }
    return (
        <div className={styles.main}>
            <div className={styles.delete}>
                <h2>Удалить контент по id</h2>
                <input type="number" placeholder="id" onChange={e => setIdToDelete(Number.parseInt(e.target.value))}/>
                <button onClick={deleteContent}>Удалить</button>
            </div>
            <div className={styles.separator}></div>
            <div className={styles.add}>
                <div className={styles.addMovie}>
                    <h2>Добавить фильм</h2>
                    <div style={AddMovieClicked ? {transform: "rotate(180deg)"} : {}} className={styles.arrow}
                         onClick={() => setAddMovieClicked(!AddMovieClicked)}></div>
                    {AddMovieClicked && <div className={styles.addMovieOptions}>
                        <AddMovieOptions></AddMovieOptions>
                    </div>}
                </div>
                <div className={styles.separator}></div>
                <div className={styles.addSerial}>
                    <h2>Добавить сериал</h2>
                    <div style={AddSerialClicked ? {transform: "rotate(180deg)"} : {}} className={styles.arrow}
                         onClick={() => setAddSerialClicked(!AddSerialClicked)}></div>
                    {AddSerialClicked && <div className={styles.addSerialOptions}>
                        <AddSerialOptions/>
                    </div>}
                </div>
            </div>
            <div className={styles.separator}></div>
            <div className={styles.edit}>
                <div className={styles.editMovie}>
                    <h2>Редактировать фильм</h2>
                    <input type="number" className={styles.editInput} placeholder="id"
                           onChange={e => setEditMovieId(Number.parseInt(e.target.value))}/>
                    <button className={styles.editButton} onClick={editMovieClickedAction}>Редактировать</button>
                    {editMovieClicked && <button onClick={stopEditMovie}>Прекратить</button>}
                    {editMovieClicked && <div className={styles.editMovieOptions}>
                        <EditMovieOptions movieOptions={editMovieOptions}></EditMovieOptions>
                    </div>}
                </div>
                <div className={styles.separator}></div>
                <div className={styles.editSerial}>
                    <h2>Редактировать сериал</h2>
                    <input type="number" className={styles.editInput} placeholder="id"
                           onChange={e => setEditSerialId(Number.parseInt(e.target.value))}/>
                    <button className={styles.editButton} onClick={editSerialClickedAction}>Редактировать</button>
                    {editSerialClicked && <button onClick={stopEditSerial}>Прекратить</button>}
                    {editSerialClicked && <div className={styles.editSerialOptions}>
                        <EditSerialOptions serialOptions={editSerialOptions}></EditSerialOptions>
                    </div>
                    }
                </div>
            </div>
        </div>
    )
}
export default AdminContent