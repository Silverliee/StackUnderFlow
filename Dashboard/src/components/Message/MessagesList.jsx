import MessageItem from "./MessageItem.jsx";
import {useEffect} from "react";

function MessagesList({ messageList, handleAccept, handleDecline, group }) {

	useEffect(() => {
		console.log({group, messageList});
	}, []);
	return (
		<>
			{messageList?.length > 0 &&
				messageList?.map((request, index) => (
					<MessageItem
						key={index}
						title={request.friendName ? request.friendName : request.groupName}
						message={request.message}
						id={group ? request.groupId : request.userId}
						handleDecline={() => handleDecline(group ? request.groupId : request.userId)}
						handleAccept={() => handleAccept(group ? request.groupId : request.userId)}
					/>
				))}
		</>
	);
}

export default MessagesList;
