import React, { useEffect, useState } from "react";
import { useAuth } from "../hooks/AuthProvider";
import MessageItem from "../components/MessageItem";
import MessagesList from "../components/MessagesList";
import {
	Accordion,
	AccordionDetails,
	AccordionSummary,
	Typography,
} from "@mui/material";
import ExpandMoreIcon from "@mui/icons-material/ExpandMore";
import AxiosRq from "../Axios/AxiosRequester";

function MessagePage() {
	const [friendRequests, setFriendRequests] = useState([]);
	const [groupRequests, setGroupRequests] = useState([]);
	const [messages, setMessages] = useState([]);

	const { userId } = useAuth();

	const fakeFriendRequests = [
		{
			userId: 3,
			friendId: 4,
			friendName: "John Doe",
			status: "Pending",
			message: "Hey, I would like to be your friend",
		},
		{
			userId: 3,
			friendId: 5,
			friendName: "Jane Doe",
			status: "Pending",
			message: "Hey, I would like to be your friend",
		},
	];

	const fakeGroupRequests = [
		{
			UserId: 3,
			GroupId: 999,
			GroupName: "Group 1",
			Message: "Join my group",
		},
		{
			UserId: 3,
			GroupId: 5,
			GroupName: "Group 2",
			Status: "Pending",
			Message: "No, join my group instead",
		},
	];

	useEffect(() => {
		getFriendRequests();
		getGroupRequests();
		console.log("Main render");
		//setFriendRequests(fakeFriendRequests);
		//setGroupRequests(fakeGroupRequests);
	}, [userId]);

	useEffect(() => {
		console.log("Render");
	}, [friendRequests, groupRequests]);

	const getFriendRequests = async () => {
		const friendRequests = await AxiosRq.getInstance().getFriendRequests(
			userId
		);
		console.log("friendRequests", friendRequests);
		setFriendRequests(friendRequests);
	};
	const getGroupRequests = async () => {
		const groupRequests = await AxiosRq.getInstance().getGroupRequests(userId);
		setGroupRequests(groupRequests);
	};

	const handleDeclineFriendRequest = (friendId) => {
		console.log("Decline friend request", friendId);
		AxiosRq.getInstance()
			.declineFriendRequest(friendId)
			.then(() => {
				const friendRequestsUpdated = friendRequests.filter(
					(request) => request.userId != friendId
				);
				setFriendRequests(friendRequestsUpdated);
			});
	};

	const handleAcceptFriendRequest = (friendId) => {
		console.log("Accept friend request", friendId);
		AxiosRq.getInstance()
			.acceptFriendRequest(friendId)
			.then(() => {
				const friendRequestsUpdated = friendRequests.filter(
					(request) => request.userId != friendId
				);
				setFriendRequests(friendRequestsUpdated);
			});
	};

	const handleDeclineGroupRequest = (groupId) => {
		console.log("Decline group request", groupId);
		AxiosRq.getInstance()
			.declineGroupInvitation(groupId)
			.then(() => {
				const groupRequestsUpdated = groupRequests.filter(
					(request) => request.groupId != groupId
				);
				setGroupRequests(groupRequestsUpdated);
			});
	};

	const handleAcceptGroupRequest = (groupId) => {
		console.log("Accept group request", groupId);
		AxiosRq.getInstance()
			.acceptGroupInvitation(groupId)
			.then(() => {
				const groupRequestsUpdated = groupRequests.filter(
					(request) => request.groupId != groupId
				);
				setGroupRequests(groupRequestsUpdated);
			});
	};

	return (
		<>
			<div>MessagePage</div>
			<div>
				{friendRequests?.length > 0 && (
					<Accordion defaultExpanded>
						<AccordionSummary
							expandIcon={<ExpandMoreIcon />}
							aria-controls="panel1-content"
							id="panel1-header"
						>
							<Typography>Friend Requests</Typography>
						</AccordionSummary>
						<AccordionDetails>
							<MessagesList
								handleAccept={handleAcceptFriendRequest}
								handleDecline={handleDeclineFriendRequest}
								messageList={friendRequests}
							/>
						</AccordionDetails>
					</Accordion>
				)}
				{groupRequests?.length > 0 && (
					<Accordion defaultExpanded>
						<AccordionSummary
							expandIcon={<ExpandMoreIcon />}
							aria-controls="panel2-content"
							id="panel2-header"
						>
							<Typography>Group Requests</Typography>
						</AccordionSummary>
						<AccordionDetails>
							<MessagesList
								group={true}
								handleAccept={handleAcceptGroupRequest}
								handleDecline={handleDeclineGroupRequest}
								messageList={groupRequests}
							/>
						</AccordionDetails>
					</Accordion>
				)}
			</div>
		</>
	);
}

export default MessagePage;
