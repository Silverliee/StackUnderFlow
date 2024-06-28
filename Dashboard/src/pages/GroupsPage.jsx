import { useEffect, useState } from "react";
import AxiosRq from "../Axios/AxiosRequester";
import GroupList from "../components/Group/GroupList.jsx";
import AddCircleOutlineIcon from "@mui/icons-material/AddCircleOutline";
import { Modal, Box, Button } from "@mui/material";
import UnstyledInputIntroduction from "../components/Custom/UnstyledInputIntroduction.jsx";
import {useRelations} from "../hooks/RelationsProvider.jsx";

function GroupsPage() {
	const [groups, setGroups] = useState([]);
	const [groupsPaginated, setGroupsPaginated] = useState([]);
	const [page, setPage] = useState(0);
	const [rowsPerPage, setRowsPerPage] = useState(5);
	const [open, setOpen] = useState(false);
	const [groupName, setGroupName] = useState("");
	const [description, setDescription] = useState("");

	const { myGroups } = useRelations();

	useEffect( () => {
		setGroups(myGroups);
	})

	useEffect(() => {
		setGroupsPaginated(
			groups?.slice(page * rowsPerPage, (page + 1) * rowsPerPage)
		);
	}, [rowsPerPage, page, groups]);

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

	const handleDeleteGroup = (groupId) => {
		if (confirm("Are you sure you want to delete this group?")) {
			AxiosRq.getInstance().deleteGroup(groupId);
			setGroups(groups.filter((group) => group.groupId !== groupId));
		}
	};
	const handleItemSelected = (groupId) => {};

	const handleChangeRowsPerPage = (event) => {
		setRowsPerPage(parseInt(event.target.value, 10) ?? 5);
		setPage(0);
	};

	const handleChangePage = (event, newPage) => {
		setPage(newPage);
	};

	const handleCreateGroup = async (group) => {
		setOpen(true);
	};

	const handleClose = () => {
		setOpen(false);
		setGroupName("");
		setDescription("");
	};

	const handleSubmitRegisterEvent = async () => {
		const group = {
			GroupName: groupName,
			Description: description,
		};
		const result = await AxiosRq.getInstance().createGroup(group);
		if (result) {
			setGroups([...groups, result]);
			handleClose();
		}
	};

	return (
		<>
			<div style={{ display: "flex", marginRight: "10px" }}>
				<AddCircleOutlineIcon
					onClick={handleCreateGroup}
					style={{ marginRight: "10px", cursor: "pointer" }}
				/>
				<p>Create a Group</p>
			</div>
			<Modal open={open} onClose={handleClose}>
				<Box sx={{ ...style, width: 400 }}>
					<div>
						<label>Group name: </label>
						<UnstyledInputIntroduction
							id="username"
							name="username"
							handleInput={(event) => setGroupName(event.target.value)}
						/>
					</div>
					<div>
						<label>Description: </label>
						<UnstyledInputIntroduction
							id="email"
							name="email"
							handleInput={(event) => setDescription(event.target.value)}
						/>
					</div>
					<Button
						component="label"
						role={undefined}
						variant="contained"
						tabIndex={-1}
						onClick={handleSubmitRegisterEvent}
						disabled={!groupName || !description}
					>
						Submit
					</Button>
				</Box>
			</Modal>
			<GroupList
				groups={groups}
				groupsPaginated={groupsPaginated}
				handleDelete={handleDeleteGroup}
				handleItemSelected={handleItemSelected}
				page={page}
				rowsPerPage={rowsPerPage}
				handleChangePage={handleChangePage}
				handleChangeRowsPerPage={handleChangeRowsPerPage}
			/>
		</>
	);
}

export default GroupsPage;
