import React from "react";
import Sidebar from "../components/Sidebar";
import Content from "../components/Content";

function Dashboard() {
	return (
		<div className="dashboard">
			<Sidebar />
			<div className="dashboard--content">{/* <Content /> */}</div>
		</div>
	);
}

export default Dashboard;
