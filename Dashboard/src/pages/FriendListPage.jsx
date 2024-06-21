import React, { useEffect, useState } from "react";
import ContactList from "../components/ContactList";
import AxiosRq from "../Axios/AxiosRequester";
import {useRelations} from "../hooks/RelationsProvider.jsx";

function FriendListPage() {
	const [friendsList, setFriendsList] = useState([]);
	const [friendsPaginated, setFriendsPaginated] = useState([]);
	const [page, setPage] = React.useState(0);
	const [rowsPerPage, setRowsPerPage] = React.useState(5);

	const { myFriends, myGroups, myFollows, dispatchFriends } = useRelations();

	useEffect(() => {
		console.log('friendsPage');
		console.log(myFriends);
		setFriendsList(myFriends);
	}, [myFriends]);

	useEffect(() => {
		setFriendsPaginated(
			friendsList.slice(page * rowsPerPage, (page + 1) * rowsPerPage)
		);
	}, [rowsPerPage, page, friendsList]);

	const handleDeleteFriend = (userId) => {
		if (confirm("Are you sure you want to delete this friend?")) {
			AxiosRq.getInstance().deleteFriend(userId);
			const friend = friendsList.filter((friend) => friend.userId == userId)[0];
			dispatchFriends({type: "REMOVE_FRIEND", payload: friend});
			setFriendsList(friendsList.filter((friend) => friend.userId !== userId));
		}
	};
	const handleItemSelected = (userId) => {};

	const handleChangeRowsPerPage = (event) => {
		setRowsPerPage(parseInt(event.target.value, 10) ?? 5);
		setPage(0);
	};

	const handleChangePage = (event, newPage) => {
		setPage(newPage);
	};

	return (
		<ContactList
			contacts={friendsList}
			contactsPaginated={friendsPaginated}
			handleDelete={handleDeleteFriend}
			handleItemSelected={handleItemSelected}
			page={page}
			rowsPerPage={rowsPerPage}
			handleChangePage={handleChangePage}
			handleChangeRowsPerPage={handleChangeRowsPerPage}
		/>
	);
}

export default FriendListPage;
