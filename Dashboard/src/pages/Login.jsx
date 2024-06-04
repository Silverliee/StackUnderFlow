import { useState } from "react";
import { useAuth } from "../hooks/AuthProvider";
import { TiArrowBack } from "react-icons/ti";
import "../styles/login.css";
import { useNavigate } from "react-router-dom";
import { loginRequest } from "../Axios/index.js";

const Login = () => {
	const [input, setInput] = useState({
		email: "",
		password: "",
	});
	const navigate = useNavigate();

	const auth = useAuth();
	const handleSubmitEvent = async (e) => {
		console.log(input);
		e.preventDefault();
		if (input.email !== "" && input.password !== "") {
			auth.loginAction(input);
			return;
		}
		alert("please provide a valid input");
	};

	const handleInput = (e) => {
		const { name, value } = e.target;
		setInput((prev) => ({
			...prev,
			[name]: value,
		}));
	};

	const handleClick = async () => {
		console.log("clicked");
		navigate("/");
	};

	return (
		<>
			<div className="login--header">
				<h1>Login Page</h1>
				<TiArrowBack onClick={handleClick} />
			</div>
			<form onSubmit={handleSubmitEvent}>
				<div className="form_control">
					<label htmlFor="user-email">Email:</label>
					<input
						type="email"
						id="user-email"
						name="email"
						placeholder="example@yahoo.com"
						aria-describedby="user-email"
						aria-invalid="false"
						onChange={handleInput}
					/>
					<div id="user-email" className="sr-only">
						Please enter a valid username. It must contain at least 6
						characters.
					</div>
				</div>
				<div className="form_control">
					<label htmlFor="password">Password:</label>
					<input
						type="password"
						id="password"
						name="password"
						aria-describedby="user-password"
						aria-invalid="false"
						onChange={handleInput}
					/>
					<div id="user-password" className="sr-only">
						your password should be more than 6 character
					</div>
				</div>
				<button className="btn-submit">Submit</button>
			</form>
		</>
	);
};

export default Login;
