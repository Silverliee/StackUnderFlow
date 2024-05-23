import { useContext, createContext, useState } from "react";
import { useNavigate } from "react-router-dom";
const AuthContext = createContext();

const AuthProvider = ({ children }) => {
	const [user, setUser] = useState(null);
	const [token, setToken] = useState(localStorage.getItem("site") || "");
	const navigate = useNavigate();
	const loginAction = async (data) => {
		//See what data sent with token from backend
		console.log("login Action");
		// try {
		// 	const response = await fetch("your-api-endpoint/auth/login", {
		// 		method: "POST",
		// 		headers: {
		// 			"Content-Type": "application/json",
		// 		},
		// 		body: JSON.stringify(data),
		// 	});
		// 	const res = await response.json();
		// 	if (res.data) {
		// 		setUser(res.data.user);
		// 		setToken(res.token);
		// 		localStorage.setItem("site", res.token);
		// 		navigate("/dashboard");
		// 		return;
		// 	}
		// 	throw new Error(res.message);
		// } catch (err) {
		// 	console.error(err);
		// }
		navigate("/dashboard");
	};

	const register = async (data) => {
		console.log("register Action");
		// try {
		// 	const response = await fetch("your-api-endpoint/auth/login", {
		// 		method: "POST",
		// 		headers: {
		// 			"Content-Type": "application/json",
		// 		},
		// 		body: JSON.stringify(data),
		// 	});
		// 	const res = await response.json();
		// 	if (res.data) {
		// 		alert("User registered successfully");
		// 	}
		// 	throw new Error(res.message);
		// } catch (err) {
		// 	console.error(err);
		// }
		navigate("/login");
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
