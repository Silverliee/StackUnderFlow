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
import { useAuth } from "../../hooks/AuthProvider.jsx";

export const Group = ({ group, check, handleItemSelected, handleDelete }) => {
	const { authData } = useAuth();

	return (
		<ListItem key={group.groupId} role={undefined} dense button>
			<ListItemIcon>
				<Checkbox
					checked={check}
					key={group.groupId}
					edge="start"
					tabIndex={-1}
					disableRipple
					id={`${group.groupId}`}
					onChange={handleItemSelected}
				/>
			</ListItemIcon>
			<ListItemText
				id={group.groupId}
				primary={group.groupName}
				// secondary={script.programmingLanguage}
			/>
			{/* <ListItemText
				id={user.userId + "1"}
				primary={"By " + script.creatorName}
			/> */}
			<Link to={`/group/${group.groupId}`}>See Group details</Link>
			{group.creatorUserID == authData.userId && (
				<DeleteIcon onClick={() => handleDelete(group.groupId)}></DeleteIcon>
			)}
		</ListItem>
	);
};

export default Group;
