import React from "react";
import "../styles/profileBanner.css";
import { Button } from "@mui/material";
import { useNavigate } from "react-router-dom";
import AccountCircleIcon from "@mui/icons-material/AccountCircle";

function ProfileBanner({ user }) {
	const navigate = useNavigate();

	return (
		<>
			<div className="container--profile-banner">
				Welcome {user.username}
				<Button onClick={() => navigate("/profile")}>
					<AccountCircleIcon />
				</Button>
			</div>
		</>
	);
}

export default ProfileBanner;
