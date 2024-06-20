import React, { useState, useEffect } from "react";
import PropTypes from "prop-types";
import Tabs from "@mui/material/Tabs";
import Tab from "@mui/material/Tab";
import Box from "@mui/material/Box";
import SearchContactPage from "../pages/SearchContactPage";
import FriendListPage from "../pages/FriendListPage";
import AxiosRq from "../Axios/AxiosRequester";
import GroupListPage from "../pages/GroupListPage";

function ContactTabPanel(props) {
	const { children, value, index, ...other } = props;

	return (
		<div
			role="tabpanel"
			hidden={value !== index}
			id={`simple-tabpanel-${index}`}
			aria-labelledby={`simple-tab-${index}`}
			{...other}
		>
			{value === index && <Box sx={{ p: 3 }}>{children}</Box>}
		</div>
	);
}

ContactTabPanel.propTypes = {
	children: PropTypes.node,
	index: PropTypes.number.isRequired,
	value: PropTypes.number.isRequired,
};

function a11yProps(index) {
	return {
		id: `simple-tab-${index}`,
		"aria-controls": `simple-tabpanel-${index}`,
	};
}

export default function BasicTabs() {
	const [value, setValue] = useState(0);
	const [friendsId, setFriendsId] = useState([]);

	useEffect(() => {
		fetchFriends();
	}, []);

	const fetchFriends = async () => {
		const result = await AxiosRq.getInstance().getFriends();
		setFriendsId(result.map((friend) => friend.userId));
	};

	const handleChange = (event, newValue) => {
		setValue(newValue);
	};

	return (
		<Box sx={{ width: "100%" }}>
			<Box sx={{ borderBottom: 1, borderColor: "divider" }}>
				<Tabs
					value={value}
					onChange={handleChange}
					aria-label="basic tabs example"
				>
					<Tab label="Search" {...a11yProps(0)} />
					<Tab label="Friends" {...a11yProps(1)} />
					<Tab label="Groups" {...a11yProps(2)} />
					<Tab label="Follows" {...a11yProps(3)} />
				</Tabs>
			</Box>
			<ContactTabPanel value={value} index={0}>
				<SearchContactPage friendsId={friendsId} />
			</ContactTabPanel>
			<ContactTabPanel value={value} index={1}>
				<FriendListPage />
			</ContactTabPanel>
			<ContactTabPanel value={value} index={2}>
				<GroupListPage />
			</ContactTabPanel>
			<ContactTabPanel value={value} index={3}>
				List of Follows
			</ContactTabPanel>
		</Box>
	);
}
