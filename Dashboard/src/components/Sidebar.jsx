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
import { useNavigate } from "react-router-dom";

const Sidebar = () => {
	const auth = useAuth();
	const navigate = useNavigate();

	const [open, setOpen] = useState(true);
	const [activeIcon, setActiveIcon] = useState("dashboard"); // ID de l'icône actuellement active

	const handleLogout = () => {
		auth.logout(() => navigate("/login"));
	};

	const handleCloseMenu = () => {
		setOpen(!open);
	};
	const handleNavigate = (path) => {
		setActiveIcon(path);
		navigate("/" + path);
	};

	useEffect(() => {}, [open, activeIcon]);

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
						<div
							className={`item ${activeIcon === "dashboard" ? "active" : ""}`}
							onClick={() => handleNavigate("dashboard")}
						>
							<BiHome className="icon" />
							Dashboard
						</div>
						<div
							className={`item ${activeIcon === "script" ? "active" : ""}`}
							onClick={() => handleNavigate("script")}
						>
							<BiTask className="icon" />
							My scripts
						</div>
						<div
							className={`item ${activeIcon === "contacts" ? "active" : ""}`}
							onClick={() => handleNavigate("contacts")}
						>
							<BiSolidReport className="icon" />
							My contacts
						</div>
						<div
							className={`item ${activeIcon === "exec" ? "active" : ""}`}
							onClick={() => handleNavigate("exec")}
						>
							<BiStats className="icon" />
							Execute my script
						</div>
						<div
							className={`item ${activeIcon === "share" ? "active" : ""}`}
							onClick={() => handleNavigate("share")}
						>
							<BiMessage className="icon" />
							Share your code
						</div>{" "}
						<div
							className={`item ${activeIcon === "edit" ? "active" : ""}`}
							onClick={() => handleNavigate("edit")}
						>
							<BiMessage className="icon" />
							Edit your code
						</div>
						<div className="item" onClick={() => handleLogout()}>
							<CgLogOut className="icon" />
							Logout
						</div>
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
