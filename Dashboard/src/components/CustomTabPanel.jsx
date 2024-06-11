import React, { useState, useEffect } from "react";
import PropTypes from "prop-types";
import Tabs from "@mui/material/Tabs";
import Tab from "@mui/material/Tab";
import Box from "@mui/material/Box";
import ContactList from "./ContactList";
import AxiosRq from "../Axios/AxiosRequester";

function CustomTabPanel(props) {
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

CustomTabPanel.propTypes = {
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
	const [friends, setFriends] = useState([]);

	useEffect(() => {
		fetchFriends();
	}, []);

	const fetchFriends = async () => {
		const result = await AxiosRq.getInstance().getFriends();
		setFriends(result);
	};

	const handleChange = (event, newValue) => {
		setValue(newValue);
	};
	const handleDeleteFriend = (userId) => {
		if (confirm("Are you sure you want to delete this friend?")) {
			AxiosRq.getInstance().deleteFriend(userId);
			setFriends(friends.filter((friend) => friend.id !== userId));
		}
	};
	const handleItemSelected = (userId) => {};

	return (
		<Box sx={{ width: "100%" }}>
			<Box sx={{ borderBottom: 1, borderColor: "divider" }}>
				<Tabs
					value={value}
					onChange={handleChange}
					aria-label="basic tabs example"
				>
					<Tab label="Friends" {...a11yProps(0)} />
					<Tab label="Groups" {...a11yProps(1)} />
					<Tab label="Follows" {...a11yProps(2)} />
				</Tabs>
			</Box>
			<CustomTabPanel value={value} index={0}>
				<ContactList
					contacts={friends}
					handleDelete={handleDeleteFriend}
					handleItemSelected={handleItemSelected}
				/>
			</CustomTabPanel>
			<CustomTabPanel value={value} index={1}>
				List of Groups
			</CustomTabPanel>
			<CustomTabPanel value={value} index={2}>
				List of Follows
			</CustomTabPanel>
		</Box>
	);
}
