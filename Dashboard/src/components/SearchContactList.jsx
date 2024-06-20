import React from "react";
import List from "@mui/material/List";
import ListItem from "@mui/material/ListItem";
import ListItemIcon from "@mui/material/ListItemIcon";
import ListItemText from "@mui/material/ListItemText";
import AddReactionIcon from "@mui/icons-material/AddReaction";
import UnstyledPaginationIntroduction from "../components/UnstyledPaginationIntroduction";

function SearchContactList({
	contacts,
	usersFoundPaginated,
	handleCreateFriendRequest,
	handleChangePage,
	handleChangeRowsPerPage,
	page,
	rowsPerPage,
	friendsId,
}) {
	return (
		<>
			<List id="contact-list">
				{contacts?.length > 0 &&
					usersFoundPaginated?.map((user) => (
						<ListItem key={user.userId} role={undefined} dense button>
							<ListItemIcon></ListItemIcon>
							<ListItemText
								id={user.userId}
								primary={user.username}
								// secondary={script.programmingLanguage}
							/>
							{!friendsId.includes(user.userId) && (
								<AddReactionIcon
									onClick={() =>
										handleCreateFriendRequest(user.userId, user.username)
									}
								></AddReactionIcon>
							)}
						</ListItem>
					))}
			</List>
			{contacts.length > 0 && (
				<UnstyledPaginationIntroduction
					numberOfResults={contacts.length}
					handleChangePage={handleChangePage}
					handleChangeRowsPerPage={handleChangeRowsPerPage}
					page={page}
					rowsPerPage={rowsPerPage}
				/>
			)}
		</>
	);
}

export default SearchContactList;
