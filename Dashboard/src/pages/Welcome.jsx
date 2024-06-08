import React, { useEffect, useState } from "react";
import UnstyledInputIntroduction from "../components/UnstyledInputIntroduction";
import { searchScriptsByKeyWord } from "../Axios";
import { Button } from "@mui/material";

import { useAuth } from "../hooks/AuthProvider";

import ListSearchResults from "./ListSearchResults";

function Welcome() {
	const [search, setSearch] = React.useState("");
	const [display, setDisplay] = React.useState("none");
	const [scriptsFound, setScriptsFound] = React.useState([]);
	const [open, setOpen] = React.useState(false);
	const [selectedLanguage, setSelectedLanguage] = useState("Any language");
	const [scriptsFoundPaginated, setScriptsFoundPaginated] = useState([]);
	const [scriptsFoundFiltered, setScriptsFoundFiltered] = useState([]);
	const [page, setPage] = React.useState(0);
	const [rowsPerPage, setRowsPerPage] = React.useState(5);

	const handleChangePage = (event, newPage) => {
		setPage(newPage);
	};

	const handleChangeRowsPerPage = (event) => {
		setRowsPerPage(parseInt(event.target.value, 10));
		setPage(0);
	};
	const { userId } = useAuth();

	useEffect(() => {
		setScriptsFoundPaginated(
			scriptsFoundFiltered.slice(page * rowsPerPage, (page + 1) * rowsPerPage)
		);
	}, [rowsPerPage, page, scriptsFoundFiltered]);

	useEffect(() => {}, [scriptsFoundPaginated, scriptsFound]);

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
		setDisplay("none");
		setScriptsFound([]);
		setPage(0);
		setRowsPerPage(5);
	};

	const handleOpenAdvancedOptions = () => {
		setOpen(!open);
	};

	const handleSearch = async () => {
		if (search.length > 3) {
			const scriptsFound = await searchScriptsByKeyWord(search);
			console.log("Scripts found :", scriptsFound);
			setScriptsFound(scriptsFound);
			setScriptsFoundFiltered(
				scriptsFound.filter((script) => {
					if (selectedLanguage === "Any language") return true;
					return script.programmingLanguage === selectedLanguage;
				})
			);
			setDisplay("block");
		} else {
			alert("Please enter at least 4 characters");
		}
	};

	const handleClick = () => {};

	const handlePageChange = (event, newPage) => {
		setPage(newPage);
	};

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
			<ListSearchResults
				handleSelectChange={handleSelectChange}
				selectedLanguage={selectedLanguage}
				display={display}
				search={search}
				scriptsFoundFiltered={scriptsFoundFiltered}
				scriptsFoundPaginated={scriptsFoundPaginated}
				handleClick={handleClick}
				userId={userId}
				page={page}
				rowsPerPage={rowsPerPage}
				handleChangePage={handleChangePage}
				handleChangeRowsPerPage={handleChangeRowsPerPage}
				open={open}
			/>
		</>
	);
}

export default Welcome;
