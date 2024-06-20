import React, { useEffect, useState } from "react";
import AxiosRq from "../Axios/AxiosRequester";
import { useAuth } from "../hooks/AuthProvider";
import ScriptsList from "../components/ScriptsList";
import { Container } from "@mui/material";
import { TiArrowBack } from "react-icons/ti";
import { useNavigate } from "react-router-dom";
import ListSearchResults from "./ListSearchResults";
import UnstyledInputIntroduction from "../components/UnstyledInputIntroduction";
import UnstyledSelectIntroduction from "../components/UnstyledSelectIntroduction";

import { Button } from "@mui/material";
import { Typography } from "@mui/material";
import { Accordion, AccordionDetails, AccordionSummary } from "@mui/material";
import ExpandMoreIcon from "@mui/icons-material/ExpandMore";
import MyScriptsList from "../components/MyScriptsList";

function ScriptListPage() {
	const [search, setSearch] = React.useState("");
	const [selectedLanguage, setSelectedLanguage] = useState("");
	const [display, setDisplay] = React.useState("none");
	const [scriptsFound, setScriptsFound] = React.useState([]);
	const [myFriends, setMyFriends] = React.useState([]);
	const [myFriendsScripts, setMyFriendsScripts] = React.useState([]);
	const [myGroups, setMyGroups] = React.useState([]);
	const [myGroupsScripts, setMyGroupsScripts] = React.useState([]);
	const [groupMembers, setGroupMembers] = useState([]);
	const [following, setFollowing] = React.useState([]);
	const [myFollowingScripts, setMyFollowingScripts] = React.useState([]);

	const [selectedScripts, setSelectedScripts] = useState([]);

	const [scriptsFoundFiltered, setScriptsFoundFiltered] = useState([]);
	const [friendsScriptsFiltered, setFriendsScriptsFiltered] = useState([]);
	const [groupsScriptsFiltered, setGroupsScriptsFiltered] = useState([]);
	const [followingScriptsFiltered, setFollowingScriptsFiltered] = useState([]);

	const [open, setOpen] = React.useState(false);
	const userId = useAuth().authData?.userId;
	useEffect(() => {
		const fetchScripts = async () => {
			//Get my scripts
			const scriptsLoaded = await AxiosRq.getInstance().getScripts();
			setScriptsFound(scriptsLoaded);

			//Get my friends
			const friends = await AxiosRq.getInstance().getFriends(userId);
			setMyFriends(friends);

			//Get my friends scripts
			friends.forEach(async (friend) => {
				const friendScripts =
					await AxiosRq.getInstance().getScriptByUserIdAndVisiblity(
						friend.userId,
						"Friend"
					);
				setMyFriendsScripts((oldScripts) => [...oldScripts, ...friendScripts]);
			});

			//Get my groups
			const groups = await AxiosRq.getInstance().getGroups();
			setMyGroups(groups);
			//Get my groups and its members
			const groupMembersByGroupId = await fetchGroupsAndMembers(groups);

			//Get my groups scripts
			groups.forEach(async (group) => {
				const groupMembers = groupMembersByGroupId[group.groupId];
				groupMembers.forEach(async (member) => {
					const scripts =
						await AxiosRq.getInstance().getScriptByUserIdAndVisiblity(
							member.userId,
							"Group",
							group.groupId
						);
					setMyGroupsScripts((oldScripts) => [...oldScripts, ...scripts]);
				});
			});

			//Get people I follow
			const following = await AxiosRq.getInstance().getFollows(userId);
			setFollowing(following);

			//Get people I follow scripts
			following.forEach(async (follow) => {
				const scripts =
					await AxiosRq.getInstance().getScriptByUserIdAndVisiblity(
						follow.userId,
						"Follow"
					);
				setMyFollowingScripts((oldScripts) => [...oldScripts, ...scripts]);
			});

			setSelectedLanguage("Any language");
			setDisplay("block");
		};
		fetchScripts();
	}, [userId]);

	const fetchGroupsAndMembers = async (groups) => {
		try {
			// Fetch members for each group
			const memberPromises = groups.map(async (group) => {
				const members = await AxiosRq.getInstance().getGroupMembers(
					group.groupId
				);
				return {
					groupId: group.groupId,
					members: members.map((m) => ({
						userId: m.userId,
						username: m.username,
					})),
				};
			});

			const membersResults = await Promise.all(memberPromises);
			setGroupMembers(membersResults);
			return membersResults;
		} catch (error) {
			console.error("Failed to fetch groups or group members", error);
		}
	};

	useEffect(() => {
		setScriptsFoundFiltered(
			scriptsFound?.filter((script) => {
				if (selectedLanguage === "Any language") return true;
				return script.programmingLanguage === selectedLanguage;
			})
		);
		setFriendsScriptsFiltered(
			myFriendsScripts?.filter((script) => {
				if (selectedLanguage === "Any language") return true;
				return script.programmingLanguage === selectedLanguage;
			})
		);
		setGroupsScriptsFiltered(
			myGroupsScripts?.filter((script) => {
				if (selectedLanguage === "Any language") return true;
				return script.programmingLanguage === selectedLanguage;
			})
		);
		setFollowingScriptsFiltered(
			myFollowingScripts?.filter((script) => {
				if (selectedLanguage === "Any language") return true;
				return script.programmingLanguage === selectedLanguage;
			})
		);
	}, [
		scriptsFound,
		myFriendsScripts,
		myGroupsScripts,
		myFollowingScripts,
		selectedLanguage,
	]);

	const handleDelete = async (scriptId) => {
		if (
			confirm(
				"Are you sure you want to delete this script? (All version will be removed)"
			)
		) {
			AxiosRq.getInstance().deleteScript(scriptId);
			var scriptsFiltered = scriptsFound?.filter(
				(script) => script.scriptId !== scriptId
			);
			setScriptsFound(scriptsFiltered);
		}
	};
	const handleDeleteSelection = async () => {
		if (
			confirm(
				"Are you sure you want to delete the selected scripts? (All version will be removed)"
			)
		) {
			var scriptsWithoutDeletedScripts = scriptsFound;
			selectedScripts.forEach(async (scriptId) => {
				AxiosRq.getInstance().deleteScript(scriptId);
				scriptsWithoutDeletedScripts = scriptsWithoutDeletedScripts?.filter(
					(script) => script.scriptId != scriptId
				);
			});
			console.log({ scriptsWithoutDeletedScripts, scriptsFound });
			setScriptsFound(scriptsWithoutDeletedScripts);
			setSelectedScripts([]);
		}
	};
	const handleSelectChange = (event) => {
		const value = event?.target?.innerHTML; // Get the selected value
		setSelectedLanguage(value);
	};
	const handleKeyDown = async (event) => {
		if (event.key === "Enter") {
			handleSearch();
		}
	};
	const handleReset = () => {
		setSearch("");
		setOpen(false);
		setScriptsFoundFiltered(scriptsFound);
		setScriptsFound(scriptsFound);
		setSelectedLanguage("Any language");
		setPage(0);
		setRowsPerPage(5);
		setSelectedScripts([]);
	};
	const handleOpenAdvancedOptions = () => {
		setOpen(!open);
	};
	useEffect(() => {
		handleSearch();
	}, [search, selectedLanguage]);
	const handleSearch = async () => {
		setScriptsFoundFiltered(
			scriptsFound
				?.filter((script) => {
					if (selectedLanguage === "Any language") return true;
					return script.programmingLanguage === selectedLanguage;
				})
				?.filter((script) => {
					return script.scriptName
						.toLowerCase()
						?.includes(search.toLowerCase());
				})
		);
		setFriendsScriptsFiltered(
			myFriendsScripts
				?.filter((script) => {
					if (selectedLanguage === "Any language") return true;
					return script.programmingLanguage === selectedLanguage;
				})
				?.filter((script) => {
					return script.scriptName
						.toLowerCase()
						?.includes(search.toLowerCase());
				})
		);
		setGroupsScriptsFiltered(
			myGroupsScripts
				?.filter((script) => {
					if (selectedLanguage === "Any language") return true;
					return script.programmingLanguage === selectedLanguage;
				})
				?.filter((script) => {
					return script.scriptName
						.toLowerCase()
						?.includes(search.toLowerCase());
				})
		);
		setFollowingScriptsFiltered(
			myFollowingScripts
				?.filter((script) => {
					if (selectedLanguage === "Any language") return true;
					return script.programmingLanguage === selectedLanguage;
				})
				?.filter((script) => {
					return script.scriptName
						.toLowerCase()
						?.includes(search.toLowerCase());
				})
		);
		setDisplay("block");
	};

	return (
		<>
			<div>ScriptListPage</div>
			<div className="container--search-bar" style={{ display: "flex" }}>
				<UnstyledInputIntroduction
					value={search}
					id="search"
					name="search"
					handleInput={(event) => {
						setSearch(event.target.value);
					}}
					handleKeyDown={handleKeyDown}
					placeholder={"Enter your research"}
				/>
				<Button onClick={handleSearch}>Search</Button>
				<Button onClick={handleReset}>Reset</Button>
				{selectedScripts.length > 0 && (
					<Button onClick={handleDeleteSelection} style={{ color: "red" }}>
						Delete Selected Scripts
					</Button>
				)}
			</div>
			<div>
				<Button onClick={handleOpenAdvancedOptions}>Advanced Options</Button>
			</div>
			<div id="advanced-options" style={{ display: open ? "block" : "none" }}>
				<UnstyledSelectIntroduction
					options={["Python", "Csharp"]}
					handleSelectChange={handleSelectChange}
					selectedValue={selectedLanguage}
					label="Programming Language"
					defaultValue="Any"
				/>
			</div>
			<Accordion defaultExpanded>
				<AccordionSummary
					expandIcon={<ExpandMoreIcon />}
					aria-controls="panel1-content"
					id="panel1-header"
				>
					<Typography>My Scripts</Typography>
				</AccordionSummary>
				<AccordionDetails>
					<MyScriptsList
						myScripts={true}
						display={display}
						search={search}
						scriptsFoundFiltered={scriptsFoundFiltered}
						handleDelete={handleDelete}
						userId={userId}
						selectedScripts={selectedScripts}
					/>
				</AccordionDetails>
			</Accordion>
			<Accordion>
				<AccordionSummary
					expandIcon={<ExpandMoreIcon />}
					aria-controls="panel1-content"
					id="panel1-header"
				>
					<Typography>My Friends script</Typography>
				</AccordionSummary>
				<AccordionDetails>
					<MyScriptsList
						myScripts={false}
						display={display}
						search={search}
						scriptsFoundFiltered={friendsScriptsFiltered}
						handleDelete={handleDelete}
						userId={userId}
						selectedScripts={selectedScripts}
					/>
				</AccordionDetails>
			</Accordion>
			<Accordion>
				<AccordionSummary
					expandIcon={<ExpandMoreIcon />}
					aria-controls="panel1-content"
					id="panel1-header"
				>
					<Typography>My Groups scripts</Typography>
				</AccordionSummary>
				<AccordionDetails>
					<MyScriptsList
						myScripts={false}
						display={display}
						search={search}
						scriptsFoundFiltered={groupsScriptsFiltered}
						handleDelete={handleDelete}
						userId={userId}
						selectedScripts={selectedScripts}
					/>
				</AccordionDetails>
			</Accordion>
			<Accordion>
				<AccordionSummary
					expandIcon={<ExpandMoreIcon />}
					aria-controls="panel1-content"
					id="panel1-header"
				>
					<Typography>Scripts from people I follow</Typography>
				</AccordionSummary>
				<AccordionDetails>
					<MyScriptsList
						myScripts={false}
						display={display}
						search={search}
						scriptsFoundFiltered={followingScriptsFiltered}
						handleDelete={handleDelete}
						userId={userId}
						selectedScripts={selectedScripts}
					/>
				</AccordionDetails>
			</Accordion>
		</>
	);
}

export default ScriptListPage;
