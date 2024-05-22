import React from "react";
import ProfileHeader from "./ProfileHeader";
import "../styles/profile.css";
import userImage from "../assets/image.png";
import { BiBook } from "react-icons/bi";

const courses = [
	{
		title: "English",
		duration: "2 hours",
		icon: <BiBook />,
	},
	{
		title: "JS",
		duration: "2 hours",
		icon: <BiBook />,
	},
	{
		title: "French",
		duration: "2 hours",
		icon: <BiBook />,
	},
];
const Profile = () => {
	return (
		<div className="profile">
			<ProfileHeader />
			<div className="user--profile">
				<div className="user--detail">
					<img src={userImage} alt="" />
					<h3 className="username">Dam Will</h3>
					<span className="profession">Teacher</span>
				</div>
				<div className="user-courses">
					{courses.map((course, index) => (
						<div key={index} className="course">
							<div className="course-detail">
								<div className="course-cover">{course.icon}</div>
								<div className="course-name">
									<h5 className="title">{course.title}</h5>
									<span>{course.duration}</span>
								</div>
							</div>
							<div className="action">:</div>
						</div>
					))}
				</div>
			</div>
		</div>
	);
};

export default Profile;
