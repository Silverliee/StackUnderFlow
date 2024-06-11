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
import AxiosRq from "../Axios/AxiosRequester";

export const Contact = ({ user, check, handleItemSelected, handleDelete }) => {
	return (
		<ListItem key={user.userId} role={undefined} dense button>
			<ListItemIcon>
				<Checkbox
					checked={check}
					key={user.userId}
					edge="start"
					tabIndex={-1}
					disableRipple
					id={`${user.userId}`}
					onChange={handleItemSelected}
				/>
			</ListItemIcon>
			<ListItemText
				id={user.userId}
				primary={user.username}
				// secondary={script.programmingLanguage}
			/>
			{/* <ListItemText
				id={user.userId + "1"}
				primary={"By " + script.creatorName}
			/> */}
			<Link to={""}>See Scripts</Link>
			<DeleteIcon onClick={() => handleDelete(user.userId)}></DeleteIcon>
		</ListItem>
	);
};

export default Contact;
