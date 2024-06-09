import React, { useState } from "react";
import "../styles/scriptPage.css";
import { TiArrowBack } from "react-icons/ti";
import { useNavigate } from "react-router-dom";

function ScriptExecutionPage() {
	const [file, setFile] = useState(null);

	const navigate = useNavigate();
	const handleExecute = () => {
		console.log("Execute button clicked");
		console.log(file);
	};
	function handleChange(event) {
		setFile(null);
		const selectedFile = event.target.files[0];
		if (selectedFile && selectedFile.type) {
			setFile(selectedFile);
		} else {
			alert("Invalid file type");
		}
	}
	return (
		<>
			<div className="script--header">
				<h1>Script Page Execution</h1>
				<TiArrowBack onClick={() => navigate("/home")} />
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

export default ScriptExecutionPage;
