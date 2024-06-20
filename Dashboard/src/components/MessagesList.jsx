import React from "react";
import MessageItem from "../components/MessageItem";

function MessagesList({ messageList, handleAccept, handleDecline, group }) {
	return (
		<>
			{messageList?.length > 0 &&
				messageList?.map((request, index) => (
					<MessageItem
						key={index}
						title={request.friendName ? request.friendName : request.groupName}
						message={request.message}
						id={group ? request.groupId : request.userId}
						handleDecline={handleDecline}
						handleAccept={handleAccept}
					/>
				))}
		</>
	);
}

export default MessagesList;
