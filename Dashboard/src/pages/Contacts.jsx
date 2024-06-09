import React, { useEffect, useState } from "react";
import {
	BiBuilding,
	BiLogoAndroid,
	BiLogoReact,
	BiSearch,
} from "react-icons/bi";
import { FaRegTrashAlt } from "react-icons/fa";

import Image1 from "../assets/tree.jpg";
import { TiArrowBack } from "react-icons/ti";
import { useNavigate } from "react-router-dom";

const Contacts = () => {
	const [contactSelected, setContactSelected] = useState(null);
	const [contactListSearched, setContactListSearched] = useState([
		{ name: "test", photo: Image1 },
	]);
	const contactList = [
		{
			name: "Joe",
			photo: Image1,
		},
		{
			name: "Nouredine",
			photo: Image1,
		},
		{
			name: "Mohamed",
			photo: Image1,
		},
	];

	const navigate = useNavigate();

	const handleDelete = (index) => {
		setContactSelected(contactList[index]);
		console.log(index);
		console.log(contactList[index].name);
		const userConfirm = confirm(
			"are you sure you want to delete this contact?"
		);
		if (userConfirm) {
			//requestDeleteContact(contactSelected);
			console.log("User Deleted");
		} else {
			setContactSelected(null);
		}
	};

	const handleSearch = () => {
		console.log("Search");
	};

	const handleFriendRequest = (index) => {
		console.log("Friend Request");
		console.log(contactListSearched[index].name);
	};

	useEffect(() => {}, [contactList]);

	return (
		<>
			<div className="contacts--header">
				<TiArrowBack onClick={() => navigate("/home")} />
			</div>
			<div className="contacts--main-container">
				<div className="contacts--left-container">
					<h1>Your contact list</h1>
					{contactList?.map((item, index) => (
						<div key={index} className="contact">
							<img className="contact--photo" src={item.photo} alt="" />
							<div className="contact--name">
								<h2>{item.name}</h2>
							</div>
							<FaRegTrashAlt
								className="delete-icon"
								onClick={() => handleDelete(index)}
							/>
						</div>
					))}
				</div>
				<div className="contacts--right-container">
					<div className="search-box">
						<input type="text" placeholder="Search a contact on the site" />
						<BiSearch className="icon" onClick={() => handleSearch()} />
					</div>
					{contactListSearched?.map((item, index) => (
						<div key={index} className="contact">
							<img className="contact--photo" src={item.photo} alt="" />
							<div className="contact--name">
								<h2>{item.name}</h2>
							</div>
							<p
								className="friend-request-button"
								onClick={() => handleFriendRequest(index)}
							>
								Friend request
							</p>
						</div>
					))}
				</div>
			</div>
		</>
	);
};

export default Contacts;
