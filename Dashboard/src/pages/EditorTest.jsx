import React, { useState, useEffect } from "react";
import Editor from "@monaco-editor/react";
import { Button, Input } from "@mui/material";

function EditorTest() {
	const [fileName, setFileName] = useState("default.txt");
	const [file, setFile] = useState(null);
	const [fileValue, setFileValue] = useState("");
	const [newFileValue, setNewFileValue] = useState("");
	const [reset, setReset] = useState(false);
	const [backup, setBackup] = useState("");

	useEffect(() => {
		if (reset) {
			setFileValue(backup);
			setNewFileValue(backup);
			setReset(false);
		}
	}, [reset, backup]);

	async function handleChange(event) {
		setFile(null);
		const selectedFile = event.target.files[0];
		console.log(selectedFile);
		if (selectedFile && selectedFile.type) {
			setFile(selectedFile);
			const reader = new FileReader();
			reader.onload = function (e) {
				const text = e.target.result;
				setFileValue(text);
				setNewFileValue(text);
				setBackup(text);
				setFileName(selectedFile.name);
			};
			reader.readAsText(selectedFile);
		} else {
			console.log("No file or invalid file selected");
		}
	}

	function handleEditorChange(value, event) {
		setNewFileValue(value);
	}

	function handleReset() {
		if (confirm("Are you sure you want to discard your changes?")) {
			setFileValue(newFileValue);
			setReset(true);
		}
	}

	function handleSaveAndDownload() {
		const element = document.createElement("a");
		console.log("newFileValue", newFileValue);
		const file = new Blob([newFileValue], { type: "text/plain" });
		element.href = URL.createObjectURL(file);
		element.download = fileName;
		document.body.appendChild(element); // Required for this to work in FireFox
		element.click();
		document.body.removeChild(element); // Clean up after the download
		setFileValue(newFileValue);
	}

	return (
		<>
			<div className="script--content">
				<p>Upload your script</p>
				<Input type="file" accept=".py,.cs" onChange={handleChange}></Input>
				<Input
					type="text"
					value={fileName}
					onChange={(e) => setFileName(e.target.value)}
					placeholder="Enter file name to be saved as..."
				/>
				<div style={{ display: "flex" }}>
					<Button onClick={handleSaveAndDownload}>Save & Download</Button>
					{/* <Button onClick={handleSaveAndDownload}>Save & Add Version</Button> */}
					<Button onClick={handleReset}>Reset</Button>
				</div>
			</div>
			<Editor
				height="80vh"
				theme="vs-dark"
				path={file?.name}
				language={file?.name?.endsWith(".py") ? "python" : "csharp"}
				value={fileValue}
				onChange={handleEditorChange}
			/>
		</>
	);
}

export default EditorTest;
