import React from "react";
import "./App.css";

import {
	BrowserRouter as Router,
	Route,
	Routes,
	Navigate,
} from "react-router-dom";
import WelcomePage from "./pages/WelcomePage";
import AuthProvider from "./hooks/AuthProvider";
import PrivateRoute from "./router/PrivateRoute";
import Profile from "./components/Profile";
import Register from "./pages/Register";
import ScriptExecutionPage from "./pages/ScriptExecutionPage";
import ContactsPage from "./pages/ContactsPage";
import ScriptListPage from "./pages/ScriptListPage";
import ScriptDetails from "./pages/ScriptDetails";
import ScriptVersionPage from "./pages/ScriptVersionPage";
import Layout from "./Layout";
import HomePage from "./pages/HomePage";
import LocalEditor from "./pages/LocalEditor";
import EditProfile from "./pages/EditProfile";
import ShareScriptPage from "./pages/ShareScriptPage";
import MessagePage from "./pages/MessagePage";
import GroupDetails from "./pages/GroupDetails";

const App = () => {
	return (
		<div className="App">
			<Router>
				<AuthProvider>
					<Routes>
						<Route exact path="/" element={<WelcomePage />} />
						<Route exact path="/login" element={<WelcomePage />} />
						<Route exact path="/register" element={<Register />} />
						<Route element={<Layout />}>
							<Route element={<PrivateRoute />}>
								<Route path="/home" element={<HomePage />} />
								<Route path="/exec" element={<ScriptExecutionPage />} />
								<Route path="/contacts" element={<ContactsPage />} />
								<Route path="/profile" element={<Profile />} />
								<Route path="/share" element={<ShareScriptPage />} />
								<Route exact path="/edit" element={<LocalEditor />} />
								<Route path="/editProfile" element={<EditProfile />} />
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
				</AuthProvider>
			</Router>
		</div>
	);
};

export default App;
