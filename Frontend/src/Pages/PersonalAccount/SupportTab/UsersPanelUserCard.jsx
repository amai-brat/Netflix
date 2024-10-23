import "/src/Pages/PersonalAccount/SupportTab/Styles/UsersPanelUserCard.css"
const UsersPanelUserCard = ({user, selectedUserId}) => {
    return(
        <div className={"support-tab-users-panel-user-card" + (selectedUserId === user.id ? " support-selected" : "")}>
            <label className="support-tab-users-panel-user-card-name">{user.name}</label>
            <label className="support-tab-users-panel-user-card-id">ID: {user.id}</label>
        </div>
    )
}

export default UsersPanelUserCard