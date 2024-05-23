import React, { useEffect, useState } from "react";
import {
	BiBookAlt,
	BiHome,
	BiMessage,
	BiSolidReport,
	BiStats,
	BiTask,
} from "react-icons/bi";
import { CgLogOut } from "react-icons/cg";
import "../styles/sidebar.css";
import { useAuth } from "../hooks/AuthProvider";

const Sidebar = () => {
	const auth = useAuth();

	const [open, setOpen] = useState(true);

	const handleLogout = () => {
		auth.logout();
	};

	const handleCloseMenu = () => {
		console.log("clicked");
		setOpen(!open);
	};

	useEffect(() => {
		console.log("useEffect");
	}, [open]);

	return (
		<>
			{open && (
				<div className="menu">
					<div className="logo">
						<BiBookAlt
							className="logo-icon"
							onClick={() => handleCloseMenu()}
						/>
						<h2>StackUnderFlow</h2>
					</div>

					<div className="menu--list">
						<a href="#" className="item active">
							<BiHome className="icon" />
							Dashboard
						</a>
						<a href="#" className="item">
							<BiTask className="icon" />
							My scripts
						</a>
						<a href="contacts" className="item">
							<BiSolidReport className="icon" />
							My contacts
						</a>
						<a href="exec" className="item">
							<BiStats className="icon" />
							Execute my script
						</a>
						<a href="#" className="item">
							<BiMessage className="icon" />
							Share your code
						</a>
						<a href="/" className="item" onClick={() => handleLogout()}>
							<CgLogOut className="icon" />
							Logout
						</a>
					</div>
				</div>
			)}
			{!open && (
				<div className="menu--close">
					<BiBookAlt className="logo-icon" onClick={() => handleCloseMenu()} />
				</div>
			)}
		</>
	);
};

export default Sidebar;
