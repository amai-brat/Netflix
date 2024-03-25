import { Avatar } from '@mui/material';
import FlipCameraIosRoundedIcon from '@mui/icons-material/FlipCameraIosRounded';
import { useState } from 'react';

export const UserAvatar = ({pictureUrl, onAvatarClick}) => {
    const [isHovered, setIsHovered] = useState(false);

    return (
        <div className="avatarContainer">
            <Avatar
                onMouseEnter={() => setIsHovered(true)}
                onMouseLeave={() => setIsHovered(false)}
                src = { pictureUrl } 
                onClick = { onAvatarClick }
                alt = {"User icon"}
                sx = {{
                    width: "20%",
                    height: "20%",
                    "&:hover": {
                        cursor: 'pointer',
                        filter: 'brightness(60%)'
                    }
                }}
            />
            <FlipCameraIosRoundedIcon 
                onClick = {onAvatarClick}
                sx = {{
                    width: "30%",
                    height: "30%",
                    color: "#FFFFFF",
                    opacity: "0.8",
                    position: "absolute",
                    pointerEvents: "none",
                    display: isHovered? "block" : "none"
                }}
            />
        </div>
    );
    
}