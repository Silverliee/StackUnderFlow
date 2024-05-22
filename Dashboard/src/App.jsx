import React from "react";

import Dashboard from "./Dashboard";

import "./App.css";

import { BrowserRouter as Router, Route, Link, Routes } from "react-router-dom";
import Login from "./Login";

const App = () => {
	return (
		<Router>
			<div>
				<header>
					<section>
						<h1> react-starter </h1>
						<h2> (React version 18.0.2) </h2>
					</section>
					<nav>
						<ul>
							<li>
								<Link to="/">Login</Link>
							</li>
							<li>
								<Link to="/dashboard">Dashboard</Link>
							</li>
						</ul>
					</nav>
				</header>
				<main>
					<Routes>
						<Route exact path="/" element={<Login />} />
						<Route path="/dashboard" element={<Dashboard />} />
						{/* <Route exact path="*" component={NotFound} /> */}
					</Routes>
				</main>
			</div>
		</Router>
	);
};

export default App;
