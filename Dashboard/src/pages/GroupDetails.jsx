import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import AxiosRq from "../Axios/AxiosRequester";
import { TiArrowBack } from "react-icons/ti";
import { useNavigate } from "react-router-dom";
import { Grid, Typography } from "@mui/material";
import ContactList from "../components/ContactList";

function GroupDetails() {
	const { groupId } = useParams();
	const [group, setGroup] = useState({});
	const [members, setMembers] = useState([]);
	const [creator, setCreator] = useState({});
	const [membersPaginated, setMembersPaginated] = useState([]);
	const [page, setPage] = useState(0);
	const [rowsPerPage, setRowsPerPage] = useState(5);

	const navigate = useNavigate();

	useEffect(() => {
		AxiosRq.getInstance()
			.getGroupByGroupId(groupId)
			.then((res) => {
				var group = res;
				setGroup(group);
				AxiosRq.getInstance()
					.getGroupMembers(groupId)
					.then((response) => {
						var members = response;
						setMembers(members);
						setMembersPaginated(
							members.slice(page * rowsPerPage, (page + 1) * rowsPerPage)
						);
						var creator = members?.find(
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

	return (
		<>
			<div>Group Details</div>
			<div className="contacts--header">
				<TiArrowBack onClick={() => navigate("/contacts")} />
			</div>
			<Grid item xs={12} md={6}>
				<div className="container--group-name">
					<Typography sx={{ mt: 4, mb: 2 }} variant="h6" component="div">
						Group Name: {group?.groupName}
					</Typography>
					{/* <EditIcon onClick={handleOpenEdit}></EditIcon>
					<div title="Add version" onClick={handleOpenAddVersion}>
						<AddCircleOutlineIcon></AddCircleOutlineIcon> 
					</div>*/}
				</div>
				<div>
					<p>Description: {group?.description}</p>
					<p>Creator: {creator?.username}</p>

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
			</Grid>
		</>
	);
}

export default GroupDetails;
