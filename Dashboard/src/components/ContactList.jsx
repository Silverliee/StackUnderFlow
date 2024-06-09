import React, { useState } from "react";
import Contact from "./Contact";
import { List } from "@mui/material";

function ContactList({ contacts, handleDelete, handleItemSelected }) {
	const [check, setCheck] = useState(false);

	return (
		<>
			<List id="contact-list">
				{contacts?.length > 0 &&
					contacts?.map((user) => (
						<Contact
							key={user.userId}
							user={user}
							check={check}
							handleDelete={handleDelete}
							handleItemSelected={handleItemSelected}
						/>
					))}
			</List>
		</>
	);
}

export default ContactList;
