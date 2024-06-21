import React from "react";
import "./App.css";

import {
	BrowserRouter as Router,
	Route,
	Routes,
	Navigate,
} from "react-router-dom";
import Login from "./pages/Login.jsx";
import AuthProvider from "./hooks/AuthProvider";
import PrivateRoute from "./router/PrivateRoute";
import Profile from "./components/Profile";
import Register from "./components/Register.jsx";
import ExecutionPage from "./pages/ExecutionPage.jsx";
import ContactsPage from "./pages/ContactsPage";
import ScriptListPage from "./pages/ScriptListPage";
import ScriptDetails from "./pages/ScriptDetails";
import ScriptVersionPage from "./pages/ScriptVersionPage";
import Layout from "./Layout";
import HomePage from "./pages/HomePage";
import LocalEditor from "./pages/LocalEditor";
import ProfilePage from "./pages/ProfilePage.jsx";
import SharingPage from "./pages/SharingPage.jsx";
import MessagePage from "./pages/MessagePage";
import GroupDetails from "./pages/GroupDetails";
import RelationsProvider from "./hooks/RelationsProvider.jsx";
import ScriptsProvider from "./hooks/ScriptsProvider.jsx";

const App = () => {
	return (
		<div className="App">
			<Router>
				<AuthProvider>
					<RelationsProvider>
						<ScriptsProvider>
						<Routes>
						<Route exact path="/" element={<Login />} />
						<Route exact path="/login" element={<Login />} />
						<Route exact path="/register" element={<Register />} />
						<Route element={<Layout />}>
							<Route element={<PrivateRoute />}>
								<Route path="/home" element={<HomePage />} />
								<Route path="/exec" element={<ExecutionPage />} />
								<Route path="/contacts" element={<ContactsPage />} />
								<Route path="/profile" element={<Profile />} />
								<Route path="/share" element={<SharingPage />} />
								<Route exact path="/edit" element={<LocalEditor />} />
								<Route path="/editProfile" element={<ProfilePage />} />
								<Route path="/message" element={<MessagePage />} />
								<Route
									path="/script/:scriptId/version"
									element={<ScriptVersionPage />}
								/>
								<Route path="/script/:scriptId" element={<ScriptDetails />} />
								<Route path="/script" element={<ScriptListPage />} />
								<Route path="/group/:groupId" element={<GroupDetails />} />
							</Route>
						</Route>
						<Route path="*" element={<Navigate to="/" />} />
					</Routes>
						</ScriptsProvider>
					</RelationsProvider>
				</AuthProvider>
			</Router>
		</div>
	);
};

export default App;
