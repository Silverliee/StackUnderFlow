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

	return (
		<ListItem key={script.scriptId} role={undefined} dense button>
			<ListItemIcon>
				{lastSegment == "script" && script.userId === userId && (
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
			<ListItemText
				id={script.scriptId + "1"}
				primary={"By " + script.creatorName}
			/>{" "}
			<Link to={`/script/${script.scriptId}`}>See details</Link>
			<a
				href={`http://localhost:5008/Script/${userId}/${script.scriptId}/blob`}
			>
				<DownloadIcon></DownloadIcon>
			</a>
			{lastSegment == "script" && script.userId === userId && (
				<DeleteIcon onClick={() => handleDelete(script.scriptId)}></DeleteIcon>
			)}
		</ListItem>
	);
};

export default ScriptItem;
