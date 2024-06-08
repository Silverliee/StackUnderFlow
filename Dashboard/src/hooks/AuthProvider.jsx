import { useContext, createContext, useState } from "react";
import { loginRequest, registerRequest } from "../Axios/index.js";

const AuthContext = createContext();

const AuthProvider = ({ children }) => {
	const [user, setUser] = useState(null);
	const [userId, setUserId] = useState(null);
	const [token, setToken] = useState("");
	const loginAction = async (data, callback) => {
		console.log("login Action");
		try {
			const response = await loginRequest(data);
			console.log({ response });
			if (response && response.token) {
				console.log(response.user);
				setUser(response.user);
				setToken(response.token);
				setUserId(response.user.userId);
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
			const response = await registerRequest(data);
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
		setUser(null);
		setUserId(null);
		setToken("");
		callback();
	};

	return (
		<AuthContext.Provider
			value={{ token, user, userId, loginAction, logout, register }}
		>
			{children}
		</AuthContext.Provider>
	);
};

export default AuthProvider;

export const useAuth = () => {
	return useContext(AuthContext);
};
