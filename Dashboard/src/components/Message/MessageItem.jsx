import { ListItem, ListItemText, Paper, Typography } from "@mui/material";
import DoneIcon from "@mui/icons-material/Done";
import CloseIcon from "@mui/icons-material/Close";
import React, { useEffect } from "react";

function MessageItem({ title, message, id, handleAccept, handleDecline }) {
	useEffect(() => {});
	return (
		<ListItem
			secondaryAction={
				<div style={{ marginRight: "50px" }}>
					<DoneIcon
						id={id}
						style={{ color: "green", cursor: "pointer" }}
						onClick={() => handleAccept(id)}
					/>
					<CloseIcon
						id={id}
						style={{ color: "red", cursor: "pointer" }}
						onClick={() => handleDecline(id)}
					/>
				</div>
			}
		>
			<ListItemText
				primary={
					<Paper>
						<Typography variant="h7" component="h3">
							{title}
						</Typography>
						<Typography component="p">{message}</Typography>
					</Paper>
				}
				// secondary="Secondary Text"
			/>
		</ListItem>
	);
}

export default MessageItem;
