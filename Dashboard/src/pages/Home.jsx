import React from "react";
import { useNavigate } from "react-router-dom";

const Home = (props) => {
	const { loggedIn, email } = props;
	const navigate = useNavigate();

	const navigateToLogin = () => {
		if (loggedIn) {
			localStorage.removeItem("user");
			props.setLoggedIn(false);
		} else {
			navigate("/login");
		}
	};

	const navigateToRegister = () => {
		navigate("/register");
	};

	return (
		<div className="mainContainer">
			<div className={"titleContainer"}>
				<div>Welcome!</div>
			</div>
			<div>This is the home page.</div>
			<div className={"buttonContainer"}>
				<input
					className={"loginButton"}
					type="button"
					onClick={navigateToLogin}
					value={loggedIn ? "Log out" : "Log in"}
				/>
				<input
					className={"registerButton"}
					type="button"
					onClick={navigateToRegister}
					value={"Register"}
				/>
			</div>
		</div>
	);
};

export default Home;
