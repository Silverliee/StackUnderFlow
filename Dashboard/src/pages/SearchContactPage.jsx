import React, { useEffect, useState } from "react";
import SearchBar from "../components/SearchBar";
import { useAuth } from "../hooks/AuthProvider";
import AxiosRq from "../Axios/AxiosRequester";
import SearchContactList from "../components/SearchContactList";
import { Modal, Box, Button } from "@mui/material";
import UnstyledTextareaIntroduction from "../components/UnstyledTextareaIntroduction";

function SearchContactPage({ friendsId }) {
	const [search, setSearch] = React.useState("");
	const [display, setDisplay] = React.useState("none");
	const [usersFound, setUsersFound] = React.useState([]);
	const [usersFoundPaginated, setUsersFoundPaginated] = useState([]);
	const [userToSendFriendRequestTo, setUserToSendFriendRequestTo] =
		useState(null);
	const [friendRequestMessage, setFriendRequestMessage] =
		useState("Let's be friends!");
	const [open, setOpen] = React.useState(false);
	const [page, setPage] = React.useState(0);
	const [rowsPerPage, setRowsPerPage] = React.useState(5);
	const userId = useAuth().authData?.userId;
	const handleChangePage = (event, newPage) => {
		setPage(newPage);
	};

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

	const handleChangeRowsPerPage = (event) => {
		setRowsPerPage(parseInt(event.target.value, 10));
		setPage(0);
	};

	useEffect(() => {
		setUsersFoundPaginated(
			usersFound.slice(page * rowsPerPage, (page + 1) * rowsPerPage)
		);
	}, [rowsPerPage, page, usersFound]);

	const handleKeyDown = async (event) => {
		if (event.key === "Enter") {
			handleSearch();
		}
	};

	const handleReset = () => {
		setSearch("");
		setDisplay("none");
		setUsersFound([]);
		setPage(0);
		setRowsPerPage(5);
	};

	const handleSearch = async () => {
		const result = await AxiosRq.getInstance().searchUsersByKeyword(search);
		setUsersFound(result);
		console.log({ result });
		setDisplay("block");
	};

	const handleCreateFriendRequest = async (userId, username) => {
		setUserToSendFriendRequestTo({ userId, username });
		setOpen(true);
	};

	const handleSubmitFriendRequest = async () => {
		const result = await AxiosRq.getInstance().createFriendRequest(
			userToSendFriendRequestTo.userId,
			friendRequestMessage
		);
		console.log({ result });
		setOpen(false);
		setFriendRequestMessage("Let's be friends!");
	};

	return (
		<>
			<SearchBar
				search={search}
				handleKeyDown={handleKeyDown}
				handleReset={handleReset}
				handleSearch={handleSearch}
				setSearch={setSearch}
				setDisplay={setDisplay}
			/>
			<SearchContactList
				contacts={usersFound}
				usersFoundPaginated={usersFoundPaginated}
				display={display}
				search={search}
				userId={userId}
				page={page}
				rowsPerPage={rowsPerPage}
				friendsId={friendsId}
				handleCreateFriendRequest={handleCreateFriendRequest}
				handleChangePage={handleChangePage}
				handleChangeRowsPerPage={handleChangeRowsPerPage}
			/>
			<Modal open={open}>
				<Box sx={{ ...style, width: 400 }}>
					<h2 id="parent-modal-title">
						Send friend request to {userToSendFriendRequestTo?.username}
					</h2>
					<div
						style={{
							display: "flex",
							flexDirection: "column",
							gap: "5px",
							marginBottom: "20px",
						}}
					>
						<div>
							<label>Message: </label>
							<UnstyledTextareaIntroduction
								value="Let's be friends!"
								id="friend-request-message"
								name="file-request-message"
								handleInput={(event) =>
									setFriendRequestMessage(event.target.value)
								}
							/>
						</div>
					</div>
					<Button
						component="label"
						role={undefined}
						variant="contained"
						tabIndex={-1}
						onClick={handleSubmitFriendRequest}
						disabled={!friendRequestMessage}
					>
						Submit
					</Button>
				</Box>
			</Modal>
		</>
	);
}

export default SearchContactPage;
