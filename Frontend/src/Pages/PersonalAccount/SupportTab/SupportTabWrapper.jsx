import SupportTab from "./SupportTab.jsx";
import {useState} from "react";

const SupportTabWrapper = () => {
    const [selectedUserId, setSelectedUserId] = useState(null);
    const [messageInput, setMessageInput] = useState("")
    const [currentPage, setCurrentPage] = useState(0);
    const wrapObj = {
        selectedUserId: selectedUserId,
        setSelectedUserId: setSelectedUserId,
        messageInput: messageInput,
        setMessageInput: setMessageInput,
        currentPage: currentPage,
        setCurrentPage: setCurrentPage
    }
    return(
        <SupportTab wrapObj={wrapObj}/>
    )
}

export default SupportTabWrapper