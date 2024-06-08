import React, { useEffect, useState } from "react";
import { getScripts } from "../Axios";
import { useAuth } from "../hooks/AuthProvider";
import ScriptsList from "../components/ScriptsList";
import { Container } from "@mui/material";
import { deleteScript, getScriptBlob } from "../Axios";
import { TiArrowBack } from "react-icons/ti";
import { useNavigate } from "react-router-dom";
import ListSearchResults from "./ListSearchResults";
import { searchScriptsByKeyWord } from "../Axios";
import UnstyledInputIntroduction from "../components/UnstyledInputIntroduction";
import { Button } from "@mui/material";

function ScriptListPage() {
	const [search, setSearch] = React.useState("");
	const [display, setDisplay] = React.useState("none");
	const [scriptsFound, setScriptsFound] = React.useState([]);
	const [open, setOpen] = React.useState(false);
	const [selectedLanguage, setSelectedLanguage] = useState("Any language");
	const [selectedScripts, setSelectedScripts] = useState([]);
	const [scriptsFoundPaginated, setScriptsFoundPaginated] = useState([]);
	const [scriptsFoundFiltered, setScriptsFoundFiltered] = useState([]);
	const [page, setPage] = React.useState(0);
	const [rowsPerPage, setRowsPerPage] = React.useState(5);
	const { userId } = useAuth();

	useEffect(() => {
		const fetchScripts = async (userId) => {
			const scriptsLoaded = await getScripts(userId);
			setScriptsFound(scriptsLoaded);
			setScriptsFoundFiltered(
				scriptsFound.filter((script) => {
					if (selectedLanguage === "Any language") return true;
					return script.programmingLanguage === selectedLanguage;
				})
			);
			setDisplay("block");
		};
		fetchScripts(userId);
	}, [userId]);
	useEffect(() => {
		setScriptsFoundFiltered(
			scriptsFound.filter((script) => {
				if (selectedLanguage === "Any language") return true;
				return script.programmingLanguage === selectedLanguage;
			})
		);
	}, [scriptsFound, selectedLanguage]);
	useEffect(() => {
		setScriptsFoundPaginated(
			scriptsFoundFiltered.slice(page * rowsPerPage, (page + 1) * rowsPerPage)
		);
	}, [rowsPerPage, page, scriptsFoundFiltered]);

	const handleChangePage = (event, newPage) => {
		setPage(newPage);
	};

	const handleChangeRowsPerPage = (event) => {
		setRowsPerPage(parseInt(event.target.value, 10));
		setPage(0);
	};
	const handleDelete = async (scriptId) => {
		if (
			confirm(
				"Are you sure you want to delete this script? (All version will be removed)"
			)
		) {
			deleteScript(scriptId);
			var scriptsFiltered = scriptsFound.filter(
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
				deleteScript(scriptId);
				scriptsWithoutDeletedScripts = scriptsWithoutDeletedScripts.filter(
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
		setScriptsFoundFiltered(
			scriptsFound.filter((script) => {
				if (value === "Any language") return true;
				return script.programmingLanguage === value;
			})
		);
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

	//to rework
	const handleSearch = async () => {
		setScriptsFoundFiltered(
			scriptsFound
				.filter((script) => {
					if (selectedLanguage === "Any language") return true;
					return script.programmingLanguage === selectedLanguage;
				})
				.filter((script) => {
					return script.scriptName.toLowerCase().includes(search.toLowerCase());
				})
		);
		setDisplay("block");
	};

	const handleItemSelected = (event) => {
		if (event.target.checked) {
			setSelectedScripts([...selectedScripts, event.target.id]);
		} else {
			setSelectedScripts(
				selectedScripts.filter((script) => script !== event.target.id)
			);
		}
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
			<ListSearchResults
				handleSelectChange={handleSelectChange}
				selectedLanguage={selectedLanguage}
				display={display}
				search={search}
				scriptsFoundFiltered={scriptsFoundFiltered}
				scriptsFoundPaginated={scriptsFoundPaginated}
				handleItemSelected={handleItemSelected}
				handleDelete={handleDelete}
				userId={userId}
				page={page}
				rowsPerPage={rowsPerPage}
				handleChangePage={handleChangePage}
				handleChangeRowsPerPage={handleChangeRowsPerPage}
				open={open}
				selectedScripts={selectedScripts}
			/>
		</>
	);
}

// <Container>
// 	<ScriptsList scripts={scripts} handleDelete={handleDelete} />
// </Container>
export default ScriptListPage;
