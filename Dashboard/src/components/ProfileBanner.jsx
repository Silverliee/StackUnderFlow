import React, { useEffect } from "react";
import "../styles/profileBanner.css";
import { Button } from "@mui/material";
import { useNavigate } from "react-router-dom";
import AccountCircleIcon from "@mui/icons-material/AccountCircle";
import MarkEmailReadIcon from "@mui/icons-material/MarkEmailRead";
import MarkEmailUnreadIcon from "@mui/icons-material/MarkEmailUnread";
import AxiosRq from "../Axios/AxiosRequester";

function ProfileBanner({ username }) {
	const [hasUnreadMessages, setHasUnreadMessages] = React.useState(true);
	const navigate = useNavigate();

	useEffect(() => {
		if (!username) {
			navigate("/login");
		} else {
			// check if user has unread messages
			getMessages();
			setHasUnreadMessages(true);
		}
	});

	const getMessages = async () => {
		const groupRequests = await AxiosRq.getInstance().getGroupRequests();
		const friendRequests = await AxiosRq.getInstance().getFriendRequests();

		if (groupRequests?.length === 0 && friendRequests?.length === 0) {
			setHasUnreadMessages(false);
		} else {
			setHasUnreadMessages(true);
		}
	};

	const handleNavigateMessage = () => {
		navigate("/message");
	};

	return (
		<>
			<div className="container--profile-banner">
				Welcome {username}
				{!hasUnreadMessages && (
					<MarkEmailReadIcon onClick={handleNavigateMessage} />
				)}
				{hasUnreadMessages && (
					<MarkEmailUnreadIcon
						onClick={handleNavigateMessage}
						sx={{ color: "red", cursor: "pointer" }}
					/>
				)}
				<Button onClick={() => navigate("/profile")}>
					<AccountCircleIcon />
				</Button>
			</div>
		</>
	);
}

export default ProfileBanner;
