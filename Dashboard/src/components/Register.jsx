import { useState } from "react";
import { useAuth } from "../hooks/AuthProvider.jsx";
import { TiArrowBack } from "react-icons/ti";
import { useNavigate } from "react-router-dom";

const Register = () => {
	const [input, setInput] = useState({
		username: "",
		email: "",
		password: "",
		password2: "",
	});

	const navigate = useNavigate();

	const auth = useAuth();
	const handleSubmitEvent = (e) => {
		e.preventDefault();
		if (input.username !== "" && input.password !== "" && input.email !== "") {
			if (input.password === input.password2) {
				auth.register(
					{
						username: input.username,
						email: input.email,
						password: input.password,
					},
					() => navigate("/login")
				);
				return;
			}
			alert("passwords do not match");
		} else {
			alert("please provide a valid input");
		}
	};

	const handleInput = (e) => {
		const { name, value } = e.target;
		setInput((prev) => ({
			...prev,
			[name]: value,
		}));
	};

	const handleClick = () => {
		navigate("/");
	};

	return (
		<>
			<div className="login--header">
				<h1>Register Page</h1>
				<TiArrowBack onClick={handleClick} />
			</div>
			<form onSubmit={handleSubmitEvent}>
				<div className="form_control">
					<label htmlFor="user-email">Username: </label>
					<input
						type="username"
						id="user-username"
						name="username"
						placeholder="Jericho777"
						aria-describedby="user-username"
						aria-invalid="false"
						onChange={handleInput}
					/>
				</div>
				<div className="form_control">
					<label htmlFor="user-email">Email: </label>
					<input
						type="email"
						id="user-email"
						name="email"
						placeholder="example@yahoo.com"
						aria-describedby="user-email"
						aria-invalid="false"
						onChange={handleInput}
					/>
				</div>
				<div className="form_control">
					<label htmlFor="password">Password: </label>
					<input
						type="password"
						id="password"
						name="password"
						aria-describedby="user-password"
						aria-invalid="false"
						onChange={handleInput}
					/>
				</div>
				<div className="form_control">
					<label htmlFor="password">Confirm Password: </label>
					<input
						type="password"
						id="password2"
						name="password2"
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

export default Register;
