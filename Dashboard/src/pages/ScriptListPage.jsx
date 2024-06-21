import { useEffect, useState } from "react";
import AxiosRq from "../Axios/AxiosRequester";
import { useAuth } from "../hooks/AuthProvider";
import { useRelations } from "../hooks/RelationsProvider.jsx";
import { useScripts} from "../hooks/ScriptsProvider.jsx";
import UnstyledInputIntroduction from "../components/UnstyledInputIntroduction";
import UnstyledSelectIntroduction from "../components/UnstyledSelectIntroduction";

import { Button } from "@mui/material";
import { Typography } from "@mui/material";
import { Accordion, AccordionDetails, AccordionSummary } from "@mui/material";
import ExpandMoreIcon from "@mui/icons-material/ExpandMore";
import MyScriptsList from "../components/MyScriptsList";

function ScriptListPage() {
	const [search, setSearch] = useState("");
	const [selectedLanguage, setSelectedLanguage] = useState("");
	const [display, setDisplay] = useState("none");
	const [scriptsFound, setScriptsFound] = useState([]);
	const [myFriends, setMyFriends] = useState([]);
	const [myFriendsScripts, setMyFriendsScripts] = useState([]);
	const [myGroups, setMyGroups] = useState([]);
	const [myGroupsScripts, setMyGroupsScripts] = useState([]);
	const [groupMembers, setGroupMembers] = useState([]);
	const [myFollows, setMyFollows] = useState([]);
	const [myFollowingScripts, setMyFollowingScripts] = useState([]);

	const [selectedScripts, setSelectedScripts] = useState([]);

	const [scriptsFoundFiltered, setScriptsFoundFiltered] = useState([]);
	const [friendsScriptsFiltered, setFriendsScriptsFiltered] = useState([]);
	const [groupsScriptsFiltered, setGroupsScriptsFiltered] = useState([]);
	const [followingScriptsFiltered, setFollowingScriptsFiltered] = useState([]);

	const [open, setOpen] = useState(false);
	const userId = useAuth().authData?.userId;

	const { myFriends:friends, myGroups:groups, myFollows:follows } = useRelations();
	const { state, dispatch } = useScripts();
	useEffect(() => {
		setScriptsFound(state.scriptsFound);
		setMyFriends(friends);
		setMyFriendsScripts(state.myFriendsScripts);
		setMyGroups(groups);
		setGroupMembers(state.groupMembers);
		const uniqueScripts = state.myGroupsScripts.filter((script, index, self) =>
				index === self.findIndex((s) => (
					s.scriptId === script.scriptId
				))
		);
		setMyGroupsScripts(uniqueScripts);
		setMyFollows(follows);
		setMyFollowingScripts(state.myFollowingScripts);
		setSelectedLanguage("Any language");
		setDisplay("block");
	}, [userId]);

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
			dispatch({ type: 'SET_SCRIPTS_FOUND', payload: scriptsFiltered });
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
			dispatch({ type: 'SET_SCRIPTS_FOUND', payload: scriptsWithoutDeletedScripts });

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
						item={"my"}
						myScripts={true}
						display={display}
						search={search}
						scriptsFoundFiltered={scriptsFoundFiltered}
						handleDelete={handleDelete}
						userId={userId}
						selectedScripts={selectedScripts}
						setSelectedScripts={setSelectedScripts}
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
						item={"friend"}
						myScripts={false}
						display={display}
						search={search}
						scriptsFoundFiltered={friendsScriptsFiltered}
						handleDelete={handleDelete}
						userId={userId}
						selectedScripts={selectedScripts}
						setSelectedScripts={setSelectedScripts}
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
						item={"group"}
						myScripts={false}
						display={display}
						search={search}
						scriptsFoundFiltered={groupsScriptsFiltered}
						handleDelete={handleDelete}
						userId={userId}
						selectedScripts={selectedScripts}
						setSelectedScripts={setSelectedScripts}
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
						item={"follow"}
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
