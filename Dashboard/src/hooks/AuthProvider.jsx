import { useContext, useEffect, createContext, useState } from "react";
import AxiosRq from "../Axios/AxiosRequester.js";

const AuthContext = createContext();

const AuthProvider = ({ children }) => {
	// const [authData, setAuthData] = useState(() => {
	// 	const savedAuthData = localStorage.getItem("authData");
	// 	return savedAuthData ? JSON.parse(savedAuthData) : null;
	// });
	const [authData, setAuthData] = useState(null);

	const [isLoggedIn, setIsLoggedIn] = useState(!!authData);

	useEffect(() => {
		setIsLoggedIn(!!authData);
	}, [authData]);

	useEffect(() => {
		if (authData && authData.token) {
			AxiosRq.getInstance().setToken(authData.token);
		}
	}, [authData]);

	const loginAction = async (data, callback) => {
		try {
			const response = await AxiosRq.getInstance().loginRequest(data);
			if (response && response.token) {
				AxiosRq.getInstance().setToken(response.token);
				const user = await AxiosRq.getInstance().getUserByToken();
				setAuthData({
					token: response.token,
					username: user.username,
					userId: user.userId,
				});
				setIsLoggedIn(true);
				callback();
			} else {
				alert("Invalid login credentials");
			}
		} catch (err) {
			console.error(err);
		}
	};

	const register = async (data, callback) => {
		console.log(`register Action ${data}`);
		try {
			const response = await AxiosRq.getInstance().registerRequest(data);
			if (response) {
				alert("Registration successful");
				callback();
				return;
			}
			alert("Invalid registration credentials");
		} catch (err) {
			console.error(err);
		}
	};

	const logout = (callback) => {
		setAuthData(null);
		//localStorage.removeItem("authData");
		setIsLoggedIn(false);
		callback();
	};

	return (
		<AuthContext.Provider
			value={{
				authData,
				isLoggedIn,
				loginAction,
				logout,
				register,
			}}
		>
			{children}
		</AuthContext.Provider>
	);
};

export default AuthProvider;

export const useAuth = () => {
	return useContext(AuthContext);
};
