import React, { useState } from "react";
import "../styles/scriptPage.css";
import { TiArrowBack } from "react-icons/ti";
import { useNavigate } from "react-router-dom";

function ScriptPage() {
	const [file, setFile] = useState(null);

	const navigate = useNavigate();
	const handleExecute = () => {
		console.log("Execute button clicked");
		console.log(file);
	};
	function handleChange(event) {
		setFile(event.target.files[0]);
	}
	return (
		<>
			<div className="script--header">
				<h1>Script Page Execution</h1>
				<TiArrowBack onClick={() => navigate("/dashboard")} />
			</div>
			<div className="script--content">
				<p>Here you can execute your scripts</p>
				<input type="file" onChange={handleChange}></input>
				<button className="executeButton" onClick={handleExecute}>
					Execute
				</button>
			</div>
		</>
	);
}

export default ScriptPage;
