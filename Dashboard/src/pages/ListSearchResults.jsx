import React, { useEffect } from "react";
import UnstyledPaginationIntroduction from "../components/UnstyledPaginationIntroduction";
import UnstyledSelectIntroduction from "../components/UnstyledSelectIntroduction";
import ScriptItem from "../components/ScriptItem";

function ListSearchResults({
	handleSelectChange,
	selectedLanguage,
	display,
	search,
	scriptsFoundFiltered,
	scriptsFoundPaginated,
	handleItemSelected,
	handleDelete,
	userId,
	page,
	rowsPerPage,
	handleChangePage,
	handleChangeRowsPerPage,
	open,
	selectedScripts,
}) {
	useEffect(() => {
		// console.log("selectedScripts", selectedScripts);
	});
	return (
		<>
			<div id="advanced-options" style={{ display: open ? "block" : "none" }}>
				<UnstyledSelectIntroduction
					options={["Python", "Csharp"]}
					handleSelectChange={handleSelectChange}
					selectedValue={selectedLanguage}
					label="Programming Language"
					defaultValue="Any"
				/>
			</div>
			<div id="search-results" style={{ display: display }}>
				Results for: {search} {scriptsFoundFiltered.length} result(s)
				<div>
					{scriptsFoundPaginated.map((script) => (
						<ScriptItem
							key={script.scriptId}
							script={script}
							handleItemSelected={handleItemSelected}
							handleDelete={handleDelete}
							userId={userId}
							check={
								selectedScripts
									? selectedScripts.includes(script.scriptId.toString())
									: false
							}
						/>
					))}
				</div>
				<UnstyledPaginationIntroduction
					numberOfResults={scriptsFoundFiltered.length}
					handleChangePage={handleChangePage}
					handleChangeRowsPerPage={handleChangeRowsPerPage}
					page={page}
					rowsPerPage={rowsPerPage}
				/>
			</div>
		</>
	);
}

export default ListSearchResults;
