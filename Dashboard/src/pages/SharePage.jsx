import React, { useEffect, useState } from "react";
import { TiArrowBack } from "react-icons/ti";
import { useNavigate } from "react-router-dom";
import "../styles/sharePage.css";
import { postScript } from "../Axios";
import { useAuth } from "../hooks/AuthProvider";

function SharePage() {
	const [file, setFile] = useState(null);
	const [input, setInput] = useState({
		scriptName: "",
		description: "",
		language: "",
		inputType: "None",
		outputType: "None",
		visibility: "Publique",
	});
	const navigate = useNavigate();
	const auth = useAuth();

	async function handleChange(event) {
		setFile(null);
		const selectedFile = event.target.files[0];
		console.log(event);
		console.log(typeof selectedFile);
		if (selectedFile && selectedFile.type) {
			setFile(selectedFile);
			const fileExtension = selectedFile.name.split(".").pop();
			let language = "";
			if (fileExtension === "py") {
				language = "Python";
			} else if (fileExtension === "cs") {
				language = "Csharp";
			}
			setInput((prev) => ({
				...prev,
				language: language,
			}));
		} else {
			alert("Invalid file type");
		}
	}

	const handleInput = (e) => {
		const { name, value } = e.target;
		setInput((prev) => ({
			...prev,
			[name]: value,
		}));
	};

	async function handleSubmitEvent() {
		event.preventDefault();
		if (file === null) {
			alert("Please upload a file");
			return;
		}
		if (input.scriptName === "") {
			alert("Please provide a script name");
			return;
		}
		if (input.description === "") {
			alert("Please provide a description");
			return;
		}
		if (input.inputType === "") {
			alert("Please provide an input type");
			return;
		}
		if (input.outputType === "") {
			alert("Please provide an output type");
			return;
		}
		console.log("submitting", input, file);
		let result = await postScript({
			ScriptName: input.scriptName,
			Description: input.description,
			ProgrammingLanguage: input.language,
			InputScriptType: input.inputType,
			OutputScriptType: input.outputType,
			Visibility: input.visibility,
			SourceScriptBinary: file,
			UserId: auth.userId,
		});
		console.log(result);
		if (result) {
			alert("Script uploaded successfully");
		}
		alert("Error uploading script");
	}

	return (
		<>
			<div className="script--header">
				<h1>Script Page Sharing</h1>
				<TiArrowBack onClick={() => navigate("/dashboard")} />
			</div>
			<div className="script--content">
				<p>Upload your script</p>
				<input type="file" accept=".py,.cs" onChange={handleChange}></input>
			</div>
			<form onSubmit={handleSubmitEvent}>
				<div className="form_control">
					<label htmlFor="scriptName">ScriptName: </label>
					<input
						type="scriptName"
						id="scriptName"
						name="scriptName"
						placeholder="Scrip my doc"
						aria-describedby="scriptName"
						aria-invalid="false"
						onChange={handleInput}
					/>
				</div>
				<div className="form_description">
					<label htmlFor="description">Description: </label>
					<textarea
						id="description"
						name="description"
						placeholder="Describe your script"
						aria-describedby="description"
						aria-invalid="false"
						onChange={handleInput}
					/>
				</div>
				<div className="form_control">
					<label htmlFor="language">Language: </label>
					<input
						className="input-readonly"
						id="language"
						name="language"
						value={input.language}
						readOnly
						aria-describedby="user-password"
						aria-invalid="false"
					/>
				</div>
				<div className="form_control">
					<label htmlFor="inputType">InputType: </label>
					<input
						id="inputType"
						name="inputType"
						aria-describedby="Text"
						aria-invalid="false"
						onChange={handleInput}
						value={input.inputType}
					/>
				</div>
				<div className="form_control">
					<label htmlFor="outputType">OutputType: </label>
					<input
						id="outputType"
						name="outputType"
						aria-describedby="outputType"
						aria-invalid="false"
						onChange={handleInput}
						value={input.outputType}
					/>
				</div>
				<div className="form_control">
					<label htmlFor="visibility">Visibility:</label>
					<select
						id="visibility"
						name="visibility"
						onChange={handleInput}
						value={input.visibility}
					>
						<option value="publique">Publique</option>
						<option value="private">Private</option>
					</select>
				</div>
				<button className="btn-submit">Submit</button>
			</form>
		</>
	);
}

export default SharePage;
