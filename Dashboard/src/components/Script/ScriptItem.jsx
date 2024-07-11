import React, { useEffect } from "react";
import {
	ListItem,
	ListItemIcon,
	Checkbox,
	ListItemText,
	Chip,
} from "@mui/material";
import DeleteIcon from "@mui/icons-material/Delete";
import DownloadIcon from "@mui/icons-material/Download";
import { Link, useLocation } from "react-router-dom";
import AxiosRq from "../../Axios/AxiosRequester.js";

export const ScriptItem = ({
	script,
	handleDelete,
	userId,
	handleItemSelected,
	check,
}) => {
	const location = useLocation();
	const pathSegments = location.pathname.split("/");
	const lastSegment = pathSegments[pathSegments.length - 1] || "/";

	const handleDownload = async () => {
		const data = await AxiosRq.getInstance().getScriptVersionBlob(
			script.scriptId
		);
		const element = document.createElement("a");
		const file = new Blob([data], { type: "text/plain" });
		element.href = URL.createObjectURL(file);
		element.download =
			script.scriptName +
			(script.programmingLanguage == "Python" ? ".py" : ".cs");
		document.body.appendChild(element); // Required for this to work in FireFox
		element.click();
		document.body.removeChild(element);
	};

	return (
		<ListItem key={script.scriptId} role={undefined} dense button>
			<div style={{ display: "flex", width: "100%" }}>
				<div style={{ flex: "1", display:"flex" }}>
					<ListItemIcon>
						{lastSegment === "script" && script.userId === userId && (
							<Checkbox
								checked={check}
								key={script.scriptId}
								edge="start"
								tabIndex={-1}
								disableRipple
								id={`${script.scriptId}`}
								onChange={handleItemSelected}
							/>
						)}
					</ListItemIcon>
					<ListItemText
						id={script.scriptId}
						primary={script.scriptName}
						secondary={script.programmingLanguage}
					/>
				</div>
				<div style={{ display: "flex", justifyContent: "space-between", flex: "1" }}>
					<ListItemText
						id={script.scriptId + "1"}
						primary={"By " + script.creatorName}
					/>
					<div style={{ display: "flex", justifyContent: "space-between" }}>
						<Link to={`/script/${script.scriptId}`}>See details</Link>
						<DownloadIcon onClick={handleDownload} style={{ marginLeft: "10px" }} />
						{lastSegment === "script" && script.userId === userId && (
							<DeleteIcon onClick={() => handleDelete(script.scriptId)} style={{ marginLeft: "10px" }} />
						)}
					</div>
				</div>
			</div>
		</ListItem>
	);


};

export default ScriptItem;
