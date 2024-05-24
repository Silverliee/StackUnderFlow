import React from "react";
import { Navigate, Outlet } from "react-router-dom";
import { useAuth } from "../hooks/AuthProvider";

const PrivateRoute = () => {
	const user = useAuth();
	//TO DO
	console.log(`current token: ${user.token}`);
	//if (!user.token)
	//return <Navigate to="/login" />;
	return <Outlet />;
};

export default PrivateRoute;
