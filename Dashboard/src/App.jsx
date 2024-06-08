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
import ScriptExecutionPage from "./pages/ScriptExecutionPage";
import Contacts from "./pages/Contacts";
import SharePage from "./pages/SharePage";
import ScriptListPage from "./pages/ScriptListPage";
import ScriptDetails from "./pages/ScriptDetails";
import ScriptVersionPage from "./pages/ScriptVersionPage";
import Layout from "./Layout";
import Welcome from "./pages/Welcome";
import EditorTest from "./pages/EditorTest";
import EditProfile from "./pages/EditProfile";
import ScriptModal from "./components/ScriptModal";

const App = () => {
	const [loggedIn, setLoggedIn] = useState(false);
	const [email, setEmail] = useState("");

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
						<Route exact path="/login" element={<Home />} />
						<Route
							exact
							path="/register"
							element={
								<Register setLoggedIn={setLoggedIn} setEmail={setEmail} />
							}
						/>
						<Route element={<Layout />}>
							<Route element={<PrivateRoute />}>
								<Route path="/dashboard" element={<Welcome />} />
								<Route path="/exec" element={<ScriptExecutionPage />} />
								<Route path="/contacts" element={<Contacts />} />
								<Route path="/profile" element={<Profile />} />
								{/* <Route path="/share" element={<SharePage />} /> */}
								<Route path="/share" element={<ScriptModal />} />
								<Route path="/edit" element={<EditorTest />} />
								<Route
									path="/script/:scriptId/version"
									element={<ScriptVersionPage />}
								/>
								<Route path="/script/:scriptId" element={<ScriptDetails />} />
								<Route path="/script" element={<ScriptListPage />} />
							</Route>
						</Route>
						{/* <Route exact path="*" component={NotFound} /> */}
					</Routes>
				</AuthProvider>
			</Router>
		</div>
	);
};

export default App;
