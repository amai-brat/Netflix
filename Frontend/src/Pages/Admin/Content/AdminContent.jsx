import styles from './css/AdminContent.module.css'
import {useState} from "react";
import AddSerialOptions from "./AddSerialOptions.jsx";
import {toast } from 'react-toastify';
import AddMovieOptions from "./AddMovieOptions.jsx";
import EditMovieOptions from "./EditMovieOptions.jsx";
import EditSerialOptions from "./EditSerialOptions.jsx";
import {adminContentService} from "../../../services/admin.content.service.js";
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
        const {response: resp, data: json} = await adminContentService.getEditMovieContent(editMovieId);
        if (resp.ok) {
            setEditMovieOptions(json)
            setEditMovieClicked(true)
        } else {
            setEditMovieClicked(false)
            toast.error(json.message, {
                position: "bottom-center"
            })
        }
        console.log(json)
    }
    const editSerialClickedAction = async () => {
        const {response: resp, data: json} = await adminContentService.getEditSerialContent(editSerialId);
        if (resp.ok) {
            setEditSerialClicked(true)
            setEditSerialOptions(json)
        } else {
            toast.error(json.message, {
                position: "bottom-center"
            })
        }
        console.log(json)
    }
    const deleteContent = async () => {
        if (idToDelete < 0) {
            toast.error("Введите id")
            return
        }
        const {response: resp, data: json} = await adminContentService.deleteContent(idToDelete);
        if (resp.status === 200) {
            toast.success("Успешно удалено", {
                position: "bottom-center"
            })
        } else {
            toast.error(json.message, {
                position: "bottom-center"
            })
        }
    }
    return (
        <div className={styles.main}>
            <div className={styles.delete}>
                <h2>Удалить контент по id</h2>
                <input type="number" placeholder="id" onChange={e => setIdToDelete(Number.parseInt(e.target.value))} defaultValue={0}/>
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