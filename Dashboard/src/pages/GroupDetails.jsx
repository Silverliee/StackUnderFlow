import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import AxiosRq from "../Axios/AxiosRequester";
import { TiArrowBack } from "react-icons/ti";
import { useNavigate } from "react-router-dom";
import { Grid, Typography } from "@mui/material";
import ContactList from "../components/Contact/ContactList.jsx";
import Button from "@mui/material/Button";
import ModalAddGroupMember from "../components/Group/ModalAddGroupMember.jsx";
import { useAuth} from "../hooks/AuthProvider.jsx";

function GroupDetails() {
	const [group, setGroup] = useState({});
	const [members, setMembers] = useState([]);
	const [creator, setCreator] = useState({});
	const [membersPaginated, setMembersPaginated] = useState([]);
	const [page, setPage] = useState(0);
	const [rowsPerPage, setRowsPerPage] = useState(5);
	const [open, setOpen] = useState(false);

	const { groupId } = useParams();
	const { authData } = useAuth();
	const navigate = useNavigate();

	useEffect(() => {
		AxiosRq.getInstance()
			.getGroupByGroupId(groupId)
			.then((res) => {
				let group = res;
				setGroup(group);
				AxiosRq.getInstance()
					.getGroupMembers(groupId)
					.then((response) => {
						let members = response;
						setMembers(members);
						setMembersPaginated(
							members.slice(page * rowsPerPage, (page + 1) * rowsPerPage)
						);
						let creator = members?.find(
							(member) => member.userId === group.creatorUserID
						);
						setCreator(creator);
					});
			});
	}, [groupId]);

	const handRemoveMember = (userId) => {};

	const handleItemSelected = (userId) => {};

	const handleChangeRowsPerPage = (event) => {
		setRowsPerPage(parseInt(event.target.value, 10) ?? 5);
		setPage(0);
	};

	const handleChangePage = (event, newPage) => {
		setPage(newPage);
	};

	const handleOpen = () => {
		setOpen(true);
	}

	const handleClose = () => {
		setOpen(false);
	};

	const handleSubmitRegisterEvent = () => {

	}

	return (
		<>
			<div>Group Details</div>
			<div className="contacts--header" style={{display: "flex", justifyContent: "flex-end"}}>
				<TiArrowBack onClick={() => navigate("/contacts")} />
			</div>
			<Grid item xs={12} md={6}>
				<div style={{display: "flex", alignItems:'center', justifyContent:'space-around'}}>
					<div style={{flexGrow: 1}}>
						<img
							src={`/assets/Group${group?.groupId % 3 + 1}.jpg`}
							alt="Profile"
							style={{
								width: '80px',
								height: '80px',
								borderRadius: '50%',
								objectFit: 'cover',
								marginRight: '15px'
							}}
						/>
					</div>
					<div className="container--group-name"
						 style={{display: "flex", flexDirection: "column", flexGrow: 3}}>
						<p>{group?.groupName}</p>
						<p>Description: {group?.description}</p>
						<p>Creator: {creator?.username}</p>
					</div>
					{ authData.userId === group.creatorUserID && (<div style={{flexGrow: 1}}>
						<Button onClick={handleOpen}>Add Members</Button>
					</div>)}
				</div>
				<div>
					<Typography sx={{ mt: 4, mb: 2 }} variant="h6" component="div">
						Members
					</Typography>
					<ContactList
						contacts={members}
						contactsPaginated={membersPaginated}
						handleDelete={handRemoveMember}
						handleItemSelected={handleItemSelected}
						page={page}
						rowsPerPage={rowsPerPage}
						handleChangePage={handleChangePage}
						handleChangeRowsPerPage={handleChangeRowsPerPage}
					/>
				</div>
				<ModalAddGroupMember open={open}
									 handleClose={handleClose}
									 handleSubmitRegisterEvent={handleSubmitRegisterEvent}/>
			</Grid>
		</>
	);
}

export default GroupDetails;
