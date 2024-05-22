import React from "react";
import {
	BiBuilding,
	BiLogoAndroid,
	BiLogoHtml5,
	BiLogoReact,
} from "react-icons/bi";

const courses = [
	{
		title: "HTML",
		icon: <BiLogoHtml5 />,
	},
	{
		title: "App Dev",
		icon: <BiLogoAndroid />,
		duration: "2 hours",
	},
	{
		title: "UX & UI",
		duration: "2 hours",
		icon: <BiBuilding />,
	},
];

const Card = () => {
	return (
		<div className="card--container">
			{courses.map((item) => (
				<div className="card">
					<div className="card--cover">{item.icon}</div>
					<div className="card--title">
						<h2>{item.title}</h2>
					</div>
				</div>
			))}
		</div>
	);
};

export default Card;
