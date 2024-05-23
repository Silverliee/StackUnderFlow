import React, { useEffect } from "react";
import { BiNotification, BiSearch } from "react-icons/bi";
import { CgProfile } from "react-icons/cg";
import { useNavigate } from "react-router-dom";

const ContentHeader = () => {
	const navigate = useNavigate();

	const handleClick = () => {
		navigate("/profile");
	};

	return (
		<div className="content--header">
			<h1 className="header--title">Dashboard</h1>
			<div className="header--activity">
				<div className="search-box">
					<input type="text" placeholder="Search ..." />
					<BiSearch className="icon" />
				</div>
				<div className="notify">
					<BiNotification className="icon" />
				</div>
				<div className="notify">
					<CgProfile className="icon" onClick={handleClick} />
				</div>
			</div>
		</div>
	);
};

export default ContentHeader;
