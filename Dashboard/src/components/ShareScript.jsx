import React, { useState } from "react";
import { Modal, Box } from "@mui/material";
import Button from "@mui/material/Button";
import { styled } from "@mui/material/styles";
import CloudUploadIcon from "@mui/icons-material/CloudUpload";
import UnstyledTextareaIntroduction from "./UnstyledTextareaIntroduction";
import UnstyledInputIntroduction from "./UnstyledInputIntroduction";
import AxiosRq from "../Axios/AxiosRequester";
import UnstyledSelectIntroduction from "./UnstyledSelectIntroduction";

const ShareScript = ({ script }) => {
	const [open, setOpen] = useState(false);
	const [file, setFile] = useState(null);
	const [scriptName, setScriptName] = useState("");
	const [description, setDescription] = useState("");
	const [language, setLanguage] = useState("");
	const [inputType, setInputType] = useState("None");
	const [outputType, setOutputType] = useState("None");
	const [visibility, setVisibility] = useState("Public");

	const acceptedFiles = [".py", ".cs"];

	const style = {
		//transform: "translate(-50%, -50%)",
		width: 400,
		bgcolor: "background.paper",
		border: "2px solid #000",
		boxShadow: 24,
		pt: 2,
		px: 4,
		pb: 3,
	};

	const VisuallyHiddenInput = styled("input")({
		clip: "rect(0 0 0 0)",
		clipPath: "inset(50%)",
		height: 1,
		overflow: "hidden",
		position: "absolute",
		bottom: 0,
		left: 0,
		whiteSpace: "nowrap",
		width: 1,
	});

	const handleOpen = () => {
		setOpen(true);
	};
	const handleClose = () => {
		setOpen(false);
		setFile(null);
		setScriptName("");
		setDescription("");
		setLanguage("");
		setInputType("None");
		setOutputType("None");
		setVisibility("Public");
	};

	const handleSubmitEvent = async () => {
		console.log("submitting", {
			scriptName,
			description,
			language,
			inputType,
			outputType,
			visibility,
			file,
		});
		let result = await AxiosRq.getInstance().postScript({
			ScriptName: scriptName,
			Description: description,
			ProgrammingLanguage: language,
			InputScriptType: inputType,
			OutputScriptType: outputType,
			Visibility: visibility,
			SourceScriptBinary: file,
		});
		console.log(result);
		if (!result) {
			alert("Error uploading script");
		}
		alert("Script uploaded successfully");
		handleReset();
	};

	function handleReset() {
		setFile(null);
		setScriptName("");
		setDescription("");
		setLanguage("");
		setInputType("None");
		setOutputType("None");
		setVisibility("Public");
	}

	async function handleChange(event) {
		setFile(null);
		const selectedFile = event.target.files[0];
		if (selectedFile && selectedFile.type) {
			const fileExtension = selectedFile.name.split(".").pop();
			if (fileExtension === "py") {
				setLanguage("Python");
			} else if (fileExtension === "cs") {
				setLanguage("Csharp");
			}
			setFile(selectedFile);
		} else {
			alert("Invalid file type");
		}
	}

	return (
		<>
			<Box sx={{ ...style, width: 400 }}>
				<div
					style={{
						display: "flex",
						flexDirection: "column",
						gap: "5px",
						marginBottom: "20px",
					}}
				>
					<div style={{ display: "flex", alignItems: "flex-end" }}>
						<Button
							component="label"
							role={undefined}
							variant="contained"
							tabIndex={-1}
							startIcon={<CloudUploadIcon />}
						>
							Upload file
							<VisuallyHiddenInput
								type="file"
								accept={acceptedFiles}
								onChange={handleChange}
							/>
						</Button>
						<p>{file?.name}</p>
					</div>
					<div>
						<label>Name: </label>
						<UnstyledInputIntroduction
							id="scriptName"
							name="scriptName"
							value={scriptName}
							handleInput={(event) => setScriptName(event.target.value)}
						/>
					</div>
					<div>
						<label>Description: </label>
						<UnstyledTextareaIntroduction
							id="description"
							name="description"
							value={description}
							handleInput={(event) => setDescription(event.target.value)}
						/>
					</div>
					<div>
						<label>Language: </label>
						<UnstyledInputIntroduction
							id="version"
							name="version"
							value={language}
							readonly={true}
						/>
					</div>
					<div>
						<label>InputType: </label>
						<UnstyledInputIntroduction
							id="inputType"
							name="inputType"
							value={inputType}
							handleInput={(event) => setInputType(event.target.value)}
						/>
					</div>
					<div>
						<label>OutputType: </label>
						<UnstyledInputIntroduction
							id="outputType"
							name="outputType"
							value={outputType}
							handleInput={(event) => setOutputType(event.target.value)}
						/>
					</div>
					<div>
						<label>Visibility: </label>
						<br />
						<UnstyledSelectIntroduction
							id="visibility"
							name="visibility"
							options={["Public", "Friend", "Group", "Private"]}
							value={visibility}
							handleInput={(event) => setVisibility(event.target.value)}
							defaultValue={visibility}
						/>
					</div>
				</div>
				<Button
					component="label"
					role={undefined}
					variant="contained"
					tabIndex={-1}
					onClick={handleSubmitEvent}
					disabled={
						!description ||
						!scriptName ||
						!language ||
						!inputType ||
						!outputType ||
						!visibility ||
						!file
					}
				>
					Submit
				</Button>
			</Box>
		</>
	);
};

export default ShareScript;
