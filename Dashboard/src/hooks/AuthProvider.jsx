import { useContext, createContext, useState } from "react";
import { useNavigate } from "react-router-dom";
import { loginRequest, registerRequest } from "../Axios/index.js";

const AuthContext = createContext();

const AuthProvider = ({ children }) => {
	const [user, setUser] = useState(null);
	const [token, setToken] = useState(localStorage.getItem("site") || "");
	const navigate = useNavigate();
	const loginAction = async (data) => {
		console.log("login Action");
		try {
			const response = await loginRequest(data);

			if (response.token) {
				setUser(response.username);
				setToken(response.token);
				localStorage.setItem("site", response.token);
				navigate("/dashboard");
				return;
			}
			alert("Invalid login credentials");
		} catch (err) {
			console.error(err);
		}
		navigate("/login");
	};

	const register = async (data) => {
		console.log(`register Action ${data}`);
		try {
			const response = await registerRequest(data);
			if (response) {
				alert("Registration successful");
				navigate("/login");
				return;
			}
			alert("Invalid registration credentials");
		} catch (err) {
			console.error(err);
		}
	};

	const logOut = () => {
		setUser(null);
		setToken("");
		localStorage.removeItem("site");
		navigate("/login");
	};

	return (
		<AuthContext.Provider
			value={{ token, user, loginAction, logOut, register }}
		>
			{children}
		</AuthContext.Provider>
	);
};

export default AuthProvider;

export const useAuth = () => {
	return useContext(AuthContext);
};
