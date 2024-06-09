import React, { useEffect } from "react";
import "../styles/profileBanner.css";
import { Button } from "@mui/material";
import { useNavigate } from "react-router-dom";
import AccountCircleIcon from "@mui/icons-material/AccountCircle";
import MarkEmailReadIcon from "@mui/icons-material/MarkEmailRead";
import MarkEmailUnreadIcon from "@mui/icons-material/MarkEmailUnread";

function ProfileBanner({ username }) {
	const [hasUnreadMessages, setHasUnreadMessages] = React.useState(true);
	const navigate = useNavigate();

	useEffect(() => {
		if (!username) {
			navigate("/login");
		}
	});

	return (
		<>
			<div className="container--profile-banner">
				Welcome {username}
				{!hasUnreadMessages && <MarkEmailReadIcon />}
				{hasUnreadMessages && (
					<MarkEmailUnreadIcon sx={{ color: "red", cursor: "pointer" }} />
				)}
				<Button onClick={() => navigate("/profile")}>
					<AccountCircleIcon />
				</Button>
			</div>
		</>
	);
}

export default ProfileBanner;
