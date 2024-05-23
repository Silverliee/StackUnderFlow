import React from "react";

import Dashboard from "./pages/Dashboard";
import { useEffect, useState } from "react";

import "./App.css";

import { BrowserRouter as Router, Route, Routes } from "react-router-dom";
import Login from "./pages/Login";
import Home from "./pages/Home";
import AuthProvider from "./hooks/AuthProvider";
import PrivateRoute from "./router/PrivateRoute";
import Profile from "./components/Profile";
import Register from "./pages/Register";
import ScriptPage from "./pages/ScriptPage";
import Contacts from "./pages/Contacts";

const App = () => {
	const [loggedIn, setLoggedIn] = useState(false);
	const [email, setEmail] = useState("");

	useEffect(() => {
		// Fetch the user email and token from local storage
		const user = JSON.parse(localStorage.getItem("user"));

		// If the token/email does not exist, mark the user as logged out
		if (!user || !user.token) {
			setLoggedIn(false);
			return;
		}

		// If the token exists, verify it with the auth server to see if it is valid
		fetch("http://localhost:3080/verify", {
			method: "POST",
			headers: {
				"jwt-token": user.token,
			},
		})
			.then((r) => r.json())
			.then((r) => {
				setLoggedIn("success" === r.message);
				setEmail(user.email || "");
			});
	}, []);
	return (
		<div className="App">
			<Router>
				<AuthProvider>
					<Routes>
						<Route
							exact
							path="/"
							element={
								<Home
									email={email}
									loggedIn={loggedIn}
									setLoggedIn={setLoggedIn}
								/>
							}
						/>
						<Route
							path="/login"
							element={<Login setLoggedIn={setLoggedIn} setEmail={setEmail} />}
						/>
						<Route
							path="/register"
							element={
								<Register setLoggedIn={setLoggedIn} setEmail={setEmail} />
							}
						/>
						<Route element={<PrivateRoute />}>
							<Route path="/dashboard" element={<Dashboard />} />
							<Route path="/exec" element={<ScriptPage />} />
							<Route path="/contacts" element={<Contacts />} />
							<Route path="/profile" element={<Profile />} />
						</Route>
						{/* <Route exact path="*" component={NotFound} /> */}
					</Routes>
				</AuthProvider>
			</Router>
		</div>
	);
};

export default App;
