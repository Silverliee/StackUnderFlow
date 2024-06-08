import { Outlet } from "react-router-dom";
import Sidebar from "./components/Sidebar";
import "./styles/layout.css";
import ProfileBanner from "./components/ProfileBanner";
import { useAuth } from "./hooks/AuthProvider";

const Layout = () => {
	const { user } = useAuth();
	console.log("User in Layout:", user);
	return (
		<div className="container--layout">
			<div className="sidebar">
				<Sidebar />
			</div>
			<div
				className="content"
				style={{ display: "flex", flexDirection: "column" }}
			>
				<ProfileBanner user={user} />
				<Outlet />
			</div>
		</div>
	);
};

export default Layout;
