import React, { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { TiArrowBack } from "react-icons/ti";
import AxiosRq from "../Axios/AxiosRequester";

import { List } from "@mui/material";
import ScriptVersionModal from "../components/ScriptVersion/ScriptVersionModal.jsx";
import EditorFrame from "../components/EditorFrame";
import Grid from "@mui/material/Grid";
import Typography from "@mui/material/Typography";
import { useAuth } from "../hooks/AuthProvider";
import ScriptVersionItem from "../components/ScriptVersion/ScriptVersionItem.jsx";
import "../styles/scriptDetails.css";
import { ThumbUpFilled as ThumbUpOffAltIcon } from "@mui/icons-material/ThumbUpOffAlt";
import { ThumbUp as ThumbUpAltIcon } from "@mui/icons-material/ThumbUpAlt";
import AddCircleOutlineIcon from "@mui/icons-material/AddCircleOutline";
import EditIcon from "@mui/icons-material/Edit";
import { Modal, Box } from "@mui/material";
import Button from "@mui/material/Button";
import UnstyledTextareaIntroduction from "../components/Custom/UnstyledTextareaIntroduction.jsx";
import UnstyledInputIntroduction from "../components/Custom/UnstyledInputIntroduction.jsx";
import UnstyledSelectIntroduction from "../components/Custom/UnstyledSelectIntroduction.jsx";
import ScriptVersionsList from "../components/ScriptVersion/ScriptVersionsList.jsx";

const ScriptDetails = () => {
	const { scriptId } = useParams();
	const [script, setScript] = useState({});
	const [scriptVersionIdEdited, setScriptVersionIdEdited] = useState("");
	const [scriptVersions, setScriptVersions] = useState([]);
	const [dense, setDense] = React.useState(false);
	const navigate = useNavigate();
	const [deleteVersion, setDeleteVersion] = useState(false);
	const [openEditor, setOpenEditor] = useState(false);
	const [openAddVersion, setOpenAddVersion] = useState(false);
	const [file, setFile] = useState(null);
	const [comment, setComment] = useState("");
	const [version, setVersion] = useState("");
	const [openEdit, setOpenEdit] = useState(false);
	const [newScriptName, setNewScriptName] = useState(script.scriptName);
	const [newDescription, setNewDescription] = useState(script.description);
	const [newInputType, setNewInputType] = useState(script.inputType);
	const [newOutputType, setNewOutputType] = useState(script.outputType);
	const [newVisibility, setNewVisibility] = useState(script.visibility);
	const [language, setLanguage] = useState(script.programmingLanguage);
	const [visibility, setVisibility] = useState(script.visibility);
	const [updated, setUpdated] = useState(false);
	const userId = useAuth().authData?.userId;
	useEffect(() => {
		AxiosRq.getInstance()
			.getScriptById(scriptId)
			.then((script) => {
				setScript(script);
				console.log(script);
				setNewDescription(script.description);
				setNewScriptName(script.scriptName);
				setNewInputType(script.inputScriptType);
				setNewOutputType(script.outputScriptType);
				setNewVisibility(script.visibility);
				setLanguage(script.programmingLanguage);
				setVisibility(script.visibility);
			});
		AxiosRq.getInstance()
			.getScriptVersions(scriptId)
			.then((data) => {
				setScriptVersions(data);
			});
	}, [scriptId, updated]);

	useEffect(() => {
		const deleteVersionTmp = scriptVersions.length == 1;
		setDeleteVersion(deleteVersionTmp);
	}, [scriptVersions]);

	const style = {
		position: "absolute",
		top: "50%",
		left: "50%",
		transform: "translate(-50%, -50%)",
		width: 400,
		bgcolor: "background.paper",
		border: "2px solid #000",
		boxShadow: 24,
		pt: 2,
		px: 4,
		pb: 3,
	};

	const handleDelete = async (scriptVersionId) => {
		if (
			confirm(
				`Are you sure you want to delete this version? ${
					scriptVersions.length == 1
						? "The script will be deleted as well as it is the only version"
						: ""
				}`
			)
		) {
			if (deleteVersion) {
				//remove last script version and script from DB
				AxiosRq.getInstance().deleteScript(scriptId);
				navigate("/script");
			} else {
				AxiosRq.getInstance().deleteScriptVersion(
					scriptVersionId,
					deleteVersion
				);
			}
			var scriptVersionsFiltered = scriptVersions?.filter(
				(scriptVersion) => scriptVersion.scriptVersionId !== scriptVersionId
			);
			setScriptVersions(scriptVersionsFiltered);
		}
	};

	const handleEditOnline = (scriptVersionId) => {
		console.log("edit on line", scriptVersionId);
		setOpenEditor(true);
		setScriptVersionIdEdited(scriptVersionId);
	};

	const handleSubmitEvent = async () => {
		console.log("submitting", comment, version, file);
		let result = await AxiosRq.getInstance().postScriptVersion({
			ScriptId: script.scriptId,
			VersionNumber: version,
			SourceScriptBinary: file,
			Comment: comment,
		});
		console.log(result);
		if (result) {
			alert("Script version uploaded successfully");
			setScriptVersions([...scriptVersions, result]);
		} else {
			alert("Error uploading script version");
		}
		setFile(null);
		setComment("");
		setVersion("");
		handleCloseAddVersion();
	};
	const handleSubmitUpdateEvent = async () => {
		const result = await AxiosRq.getInstance().updateScript({
			ScriptId: script.scriptId,
			ScriptName: newScriptName,
			Description: newDescription,
			InputScriptType: newInputType,
			OutputScriptType: newOutputType,
			ProgrammingLanguage: language,
			Visibility: newVisibility,
		});
		if (!result) {
			alert("Error updating script");
			return;
		}
		setUpdated(!updated);
		handleCloseEdit();
	};

	async function handleChange(event) {
		setFile(null);
		const selectedFile = event.target.files[0];
		if (!selectedFile || !selectedFile.type) {
			alert("Invalid file type");
			return;
		}
		setFile(selectedFile);
		console.log(selectedFile);
	}

	const handleCloseAndSaveAndAddVersionFromEditor = async (data) => {
		let result = await AxiosRq.getInstance().postScriptVersion(data);
		console.log(result);
		if (result) {
			alert("Script version uploaded successfully");
			setScriptVersions([...scriptVersions, result]);
		} else {
			alert("Error uploading script version");
		}
	};

	const handleDownload = async (scriptVersion) => {
		const data = await AxiosRq.getInstance().getScriptVersionBlob(
			scriptVersion.scriptVersionId
		);
		console.log(scriptVersion);
		const element = document.createElement("a");
		const file = new Blob([data], { type: "text/plain" });
		element.href = URL.createObjectURL(file);
		element.download =
			script.scriptName +
			`_v${scriptVersion.versionNumber}_` +
			scriptVersion.creationDate.split("T")[0] +
			(script.programmingLanguage == "Python" ? ".py" : ".cs");
		document.body.appendChild(element); // Required for this to work in FireFox
		element.click();
		document.body.removeChild(element);
	};

	const handleOpenAddVersion = () => {
		setOpenAddVersion(true);
	};

	const handleCloseAddVersion = () => {
		setOpenAddVersion(false);
	};

	const handleOpenEdit = () => {
		setOpenEdit(true);
	};

	const handleCloseEdit = () => {
		setOpenEdit(false);
		setNewDescription(script.description);
		setNewScriptName(script.scriptName);
		setNewInputType(script.inputScriptType);
		setNewOutputType(script.outputScriptType);
		setNewVisibility(script.visibility);
	};

	return (
		<>
			<div>ScriptDetails</div>
			<div className="contacts--header">
				<TiArrowBack onClick={() => navigate("/script")} />
			</div>
			<Grid item xs={12} md={6}>
				<div className="container--script-name">
					<Typography sx={{ mt: 4, mb: 2 }} variant="h6" component="div">
						Script Name: {script.scriptName}
					</Typography>
					<EditIcon onClick={handleOpenEdit}></EditIcon>
					<div title="Add version" onClick={handleOpenAddVersion}>
						<AddCircleOutlineIcon></AddCircleOutlineIcon>
					</div>
					<ScriptVersionModal
						handleClose={handleCloseAddVersion}
						handleSubmitEvent={handleSubmitEvent}
						handleChange={handleChange}
						setComment={setComment}
						setVersion={setVersion}
						open={openAddVersion}
						script={script}
						comment={comment}
						version={version}
						file={file}
					></ScriptVersionModal>
				</div>
				{!openEditor && (
					<div>
						<p>Author: {script.creatorName}</p>
						<p>Programming Language: {script.programmingLanguage}</p>
						<p>Description: {script.description}</p>
						<p>Visibility: {script.visibility}</p>

						<Typography sx={{ mt: 4, mb: 2 }} variant="h6" component="div">
							Script versions
						</Typography>
						<ScriptVersionsList
							dense={dense}
							userId={userId}
							handleDelete={handleDelete}
							handleEditOnline={handleEditOnline}
							handleDownload={handleDownload}
							scriptVersions={scriptVersions}
						/>
					</div>
				)}
			</Grid>
			{openEditor && (
				<EditorFrame
					scriptVersionIdEdited={scriptVersionIdEdited}
					scriptName={script.scriptName}
					scriptId={scriptId}
					setOpenEditor={setOpenEditor}
					handleCloseAndSaveAndAddVersionFromEditor={
						handleCloseAndSaveAndAddVersionFromEditor
					}
				/>
			)}
			<Modal
				open={openEdit}
				onClose={handleCloseEdit}
				aria-labelledby="parent-modal-title"
				aria-describedby="parent-modal-description"
			>
				<Box sx={{ ...style, width: 400 }}>
					<div
						style={{
							display: "flex",
							flexDirection: "column",
							gap: "5px",
							marginBottom: "20px",
						}}
					>
						<div>
							<label>Name: </label>
							<UnstyledInputIntroduction
								id="newScriptName"
								name="newScriptName"
								value={newScriptName}
								handleInput={(event) => setNewScriptName(event.target.value)}
							/>
						</div>
						<div>
							<label>Description: </label>
							<UnstyledTextareaIntroduction
								id="description"
								name="description"
								value={newDescription}
								handleInput={(event) => setNewDescription(event.target.value)}
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
								value={newInputType}
								handleInput={(event) => setNewInputType(event.target.value)}
							/>
						</div>
						<div>
							<label>OutputType: </label>
							<UnstyledInputIntroduction
								id="outputType"
								name="outputType"
								value={newOutputType}
								handleInput={(event) => setNewOutputType(event.target.value)}
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
								handleSelectChange={(event) =>
									setNewVisibility(event.target.innerHTML)
								}
								defaultValue={visibility}
							/>
						</div>
					</div>
					<Button
						component="label"
						role={undefined}
						variant="contained"
						tabIndex={-1}
						onClick={handleSubmitUpdateEvent}
						disabled={
							!newDescription ||
							!newScriptName ||
							!newInputType ||
							!newOutputType ||
							!newVisibility
						}
					>
						Submit
					</Button>
				</Box>
			</Modal>
		</>
	);
};

export default ScriptDetails;
