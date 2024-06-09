import { Outlet } from "react-router-dom";
import Sidebar from "./components/Sidebar";
import "./styles/layout.css";
import ProfileBanner from "./components/ProfileBanner";
import { useAuth } from "./hooks/AuthProvider";

const Layout = () => {
	const username = useAuth().authData?.username;

	return (
		<div className="container--layout">
			<div className="sidebar">
				<Sidebar />
			</div>
			<div
				className="content"
				style={{ display: "flex", flexDirection: "column" }}
			>
				<ProfileBanner username={username} />
				<Outlet />
			</div>
		</div>
	);
};

export default Layout;
