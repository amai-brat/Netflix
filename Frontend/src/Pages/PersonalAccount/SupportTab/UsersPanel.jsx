import "/src/Pages/PersonalAccount/SupportTab/Styles/SupportChatPanel.css"
import forward from "/src/assets/Forward.svg"
import UsersPanelUserCard from "./UsersPanelUserCard.jsx";

const UsersPanel = ({usersMessages, wrapObj}) => {
    const usersPerPage = 10;
    const currentPage = wrapObj.currentPage
    const setCurrentPage = wrapObj.setCurrentPage
    const selectedUserId = wrapObj.selectedUserId
    const setSelectedUserId = wrapObj.setSelectedUserId

    const handleUserClick = (userId) => {
        setSelectedUserId(userId);
    };
    
    const handleNextPage = () => {
        setCurrentPage(prevPage => prevPage + 1);
    };
    
    const  handleSelectedPage = (page) => {
        setCurrentPage(page);
    };

    const handlePrevPage = () => {
        setCurrentPage(prevPage => Math.max(prevPage - 1, 0));
    };

    const getPaginationButtons = () => {
        const buttons = [];
        
        buttons.push(
            <button key={0} className="support-tab-users-panel-pag-btn"
                    onClick={() => handleSelectedPage(0)}
                    disabled={currentPage === 0}>
                1
            </button>
        );
        
        let leftCount = 3;
        let rightCount = 3;
        
        if (currentPage < 4) {
            leftCount = currentPage; 
            rightCount = 7 - leftCount; 
        } else if (currentPage > totalPages - 5) {
            rightCount = totalPages - 1 - currentPage; 
            leftCount = 7 - rightCount; 
        }

        for (let i = Math.max(1, currentPage - leftCount); i <= Math.min(totalPages - 2, currentPage + rightCount); i++) {
            buttons.push(
                <button key={i} className="support-tab-users-panel-pag-btn"
                        onClick={() => handleSelectedPage(i)}
                        disabled={currentPage === i}>
                    {i + 1}
                </button>
            );
        }
        
        if (totalPages > 1) {
            buttons.push(
                <button key={totalPages - 1} className="support-tab-users-panel-pag-btn"
                        onClick={() => handleSelectedPage(totalPages - 1)}
                        disabled={currentPage === totalPages - 1}>
                    {totalPages}
                </button>
            );
        }

        return buttons;
    };

    const paginatedUsers = usersMessages.slice(currentPage * usersPerPage, (currentPage + 1) * usersPerPage);
    const totalPages = Math.floor(usersMessages.length / usersPerPage) + Number(usersMessages.length % usersPerPage !== 0 ? 1 : 0)
    
    return (
        <div id="support-tab-users-panel">
            <div id="support-tab-users-panel-header">
                <label>Пользователи</label>
            </div>
            <div id="support-tab-users-panel-aside">
                {paginatedUsers.map(user => (
                    <div key={user.id} onClick={() => handleUserClick(user.id)}>
                        <UsersPanelUserCard user={user} selectedUserId={selectedUserId}/>
                    </div>
                ))}
            </div>
            {totalPages > 1 && <div id="support-tab-users-panel-paginate">
                <button className="support-tab-users-panel-pag-btn" onClick={handlePrevPage}
                        disabled={currentPage === 0}>
                    <img className="support-tab-users-panel-pag-btn-icon" src={forward} alt="Back"
                         style={{transform: "scale(-1, 1)"}}/>
                </button>
                {getPaginationButtons()}
                <button className="support-tab-users-panel-pag-btn" onClick={handleNextPage}
                        disabled={(currentPage + 1) * usersPerPage >= usersMessages.length}>
                    <img className="support-tab-users-panel-pag-btn-icon" src={forward} alt="Forward"/>
                </button>
            </div>}
        </div>
    )
}

export default UsersPanel