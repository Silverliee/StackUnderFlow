import React, { useEffect, useState } from "react";
import ListSearchResults from "../pages/ListSearchResults";

function MyScriptsList({
	myScripts,
	scriptsFoundFiltered,
	display,
	search,
	handleDelete,
	selectedScripts,
	setSelectedScripts,
	userId,
}) {
	const [page, setPage] = React.useState(0);
	const [rowsPerPage, setRowsPerPage] = React.useState(5);
	const [scriptsFoundPaginated, setScriptsFoundPaginated] = useState([]);
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

	const handleItemSelected = (event) => {
		if (event.target.checked) {
			setSelectedScripts([...selectedScripts, event.target.id]);
		} else {
			setSelectedScripts(
				selectedScripts?.filter((script) => script !== event.target.id)
			);
		}
	};
	return (
		<>
			<ListSearchResults
				myScripts={myScripts}
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
				selectedScripts={selectedScripts}
			/>
		</>
	);
}

export default MyScriptsList;
