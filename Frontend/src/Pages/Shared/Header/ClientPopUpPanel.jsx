import {useNavigate} from "react-router-dom";

const ClientPopUpPanel = ({user}) => {
    const navigate = useNavigate()
    const navigateToPersonalAccount = (tabName) => {
        navigate("/PersonalAccount/" + tabName)
    }
    return(
        <div id="client-pop-up-panel">
            <label>{user.name}</label>
            <label className="separator"></label>
            <label onClick={() => {
                navigateToPersonalAccount("PersonalInfoTab")
            }}>Личные данные</label>
            <label onClick={() => {
                navigateToPersonalAccount("PersonalReviewsTab")
            }}>Рецензии</label>
            <label onClick={() => {
                navigateToPersonalAccount("FavouritesTab")
            }}>Избранное</label>
            <label onClick={() => {
                navigateToPersonalAccount("SubscriptionsTab")
            }}>Подписки</label>
            <label className="separator"></label>
            <input type="button" value="Выйти"></input>
        </div>
    )
}
export default ClientPopUpPanel