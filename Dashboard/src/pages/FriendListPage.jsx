import React, { useEffect, useState } from "react";
import ContactList from "../components/ContactList";
import AxiosRq from "../Axios/AxiosRequester";

function FriendListPage() {
	const [friends, setFriends] = useState([]);
	const [friendsPaginated, setFriendsPaginated] = useState([]);
	const [page, setPage] = React.useState(0);
	const [rowsPerPage, setRowsPerPage] = React.useState(5);

	useEffect(() => {
		fetchFriends();
	}, []);

	const fetchFriends = async () => {
		const result = await AxiosRq.getInstance().getFriends();
		setFriends(result);
	};

	useEffect(() => {
		setFriendsPaginated(
			friends.slice(page * rowsPerPage, (page + 1) * rowsPerPage)
		);
	}, [rowsPerPage, page, friends]);

	const handleDeleteFriend = (userId) => {
		if (confirm("Are you sure you want to delete this friend?")) {
			AxiosRq.getInstance().deleteFriend(userId);
			setFriends(friends.filter((friend) => friend.userId !== userId));
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
			contacts={friends}
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
