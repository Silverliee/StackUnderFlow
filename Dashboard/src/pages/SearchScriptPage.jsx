import React, { useEffect, useState } from "react";
import UnstyledInputIntroduction from "../components/Custom/UnstyledInputIntroduction.jsx";
import AxiosRq from "../Axios/AxiosRequester";
import { Button } from "@mui/material";

import { useAuth } from "../hooks/AuthProvider";

import NewSearchScripts from "../components/Search/NewSearchScripts.jsx";

function SearchScriptPage() {
	const [search, setSearch] = React.useState("");
	const [display, setDisplay] = React.useState("none");
	const [scriptsFound, setScriptsFound] = React.useState([]);
	const [open, setOpen] = React.useState(false);
	const [selectedLanguage, setSelectedLanguage] = useState("Any language");
	const [scriptsFoundPaginated, setScriptsFoundPaginated] = useState([]);
	const [scriptsFoundFiltered, setScriptsFoundFiltered] = useState([]);
	const [page, setPage] = React.useState(0);
	const [rowsPerPage, setRowsPerPage] = React.useState(5);
	const [numberOfScripts, setNumberOfScripts] = React.useState(0);
	const userId = useAuth().authData?.userId;

	const handleChangePage = (event, newPage) => {
		setPage(newPage);
	};

	const handleChangeRowsPerPage = (event) => {
		setRowsPerPage(parseInt(event.target.value, 10));
		setPage(0);
	};

	useEffect(() => {
		fetchScripts();
	}, [rowsPerPage, page]);

	const fetchScripts = async () => {
		if (search.length > 3) {
			const result = await AxiosRq.getInstance().searchScriptsByKeyWord(
				search, {offset: page * rowsPerPage, records: rowsPerPage, visibility: "Public"}
			);
			setScriptsFound(result.scripts);
			setNumberOfScripts(result.totalCount);
		}
	}
	useEffect(() => {}, [scriptsFoundPaginated, scriptsFound]);

	const handleSelectChange = (event) => {
		const value = event?.target?.innerHTML; // Get the selected value
		setSelectedLanguage(value);
		setScriptsFoundFiltered(
			scriptsFound?.filter((script) => {
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
		setDisplay("none");
		setScriptsFound([]);
		setPage(0);
		setRowsPerPage(5);
	};

	const handleOpenAdvancedOptions = () => {
		setOpen(!open);
	};

	const handleItemSelected = () => {}

	const handleSearch = async () => {
		if (search.length > 3) {
			fetchScripts();
			setDisplay("block");
		} else {
			alert("Please enter at least 4 characters");
		}
	};

	const handleClick = () => {};

	return (
		<>
			<div className="container--search-bar" style={{ display: "flex" }}>
				<UnstyledInputIntroduction
					value={search}
					id="search"
					name="search"
					handleInput={(event) => {
						setSearch(event.target.value);
						setDisplay("none");
					}}
					handleKeyDown={handleKeyDown}
					placeholder={"Search in all our public database..."}
				/>
				<Button onClick={handleSearch}>Search</Button>
				<Button onClick={handleReset}>Reset</Button>
			</div>
			<div>
				<Button onClick={handleOpenAdvancedOptions}>Advanced Options</Button>
			</div>
			<NewSearchScripts
					handleSelectChange={handleSelectChange}
					selectedLanguage={selectedLanguage}
					display={display}
					search={search}
					scriptsFound={scriptsFound}
					handleItemSelected={handleItemSelected}
					handleClick={handleClick}
					userId={userId}
					page={page}
					rowsPerPage={rowsPerPage}
					handleChangePage={handleChangePage}
					handleChangeRowsPerPage={handleChangeRowsPerPage}
					numberOfScripts={numberOfScripts}
			/>
		</>
	);
}

export default SearchScriptPage;
