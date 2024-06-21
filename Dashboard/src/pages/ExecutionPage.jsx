import React, { useState } from "react";
import "../styles/scriptPage.css";
import { TiArrowBack } from "react-icons/ti";
import { useNavigate } from "react-router-dom";
import AxiosRq from "../Axios/AxiosRequester";
import { styled } from "@mui/material/styles";
import CloudUploadIcon from "@mui/icons-material/CloudUpload";
import Button from "@mui/material/Button";

function ExecutionPage() {
	const [file, setFile] = useState(null);

	const acceptedFiles = [".py", ".cs"];

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
	const navigate = useNavigate();
	const handleExecute = () => {
		console.log("Execute button clicked");
		if (file === null) {
			alert("Please select a file");
			return;
		}
		const formData = new FormData();
		formData.append("script", file);
		AxiosRq.getInstance().executeScript(formData);
	};
	function handleChange(event) {
		setFile(null);
		const selectedFile = event.target.files[0];
		console.log(event);
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
				<div
					style={{
						display: "flex",
						alignItems: "flex-end",
					}}
				>
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
					<p style={{ marginLeft: "10px" }}>{file?.name}</p>
				</div>
				<Button
					component="label"
					role={undefined}
					variant="contained"
					tabIndex={-1}
					onClick={handleExecute}
					disabled={!file}
				>
					Execute
				</Button>
			</div>
		</>
	);
}

export default ExecutionPage;
