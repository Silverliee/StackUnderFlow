import { useEffect, useState } from "react";
import AxiosRq from "../Axios/AxiosRequester";
import { useAuth } from "../hooks/AuthProvider";
import { useRelations } from "../hooks/RelationsProvider.jsx";
import { useScripts} from "../hooks/ScriptsProvider.jsx";
import UnstyledInputIntroduction from "../components/Custom/UnstyledInputIntroduction.jsx";
import UnstyledSelectIntroduction from "../components/Custom/UnstyledSelectIntroduction.jsx";

import { Button } from "@mui/material";
import { Typography } from "@mui/material";
import { Accordion, AccordionDetails, AccordionSummary } from "@mui/material";
import ExpandMoreIcon from "@mui/icons-material/ExpandMore";
import MyScriptsList from "../components/Script/MyScriptsList.jsx";
import MyNewScriptsList from "../components/Script/MyNewScriptList.jsx";

function ScriptListPage() {
    const [search, setSearch] = useState("");
    const [selectedLanguage, setSelectedLanguage] = useState("");
    const [display, setDisplay] = useState("none");
    const [scriptsFound, setScriptsFound] = useState([]);
    const [selectedScripts, setSelectedScripts] = useState([]);
    const [scriptsFoundFiltered, setScriptsFoundFiltered] = useState([]);
    const [page, setPage] = useState(0);
    const [rowsPerPage, setRowsPerPage] = useState(5);
    const [scriptsFoundPaginated, setScriptsFoundPaginated] = useState([]);
    const [open, setOpen] = useState(false);
    const userId = useAuth().authData?.userId;

    useEffect(() => {
        fetchScripts();
    }, [userId]);

    const fetchScripts = async () => {
        const scriptsLoaded = await AxiosRq.getInstance().getScripts();
        setScriptsFound(scriptsLoaded);
        setSelectedLanguage("Any language");
        setDisplay("block");
    }

    useEffect(() => {
        setScriptsFoundFiltered(
            scriptsFound?.filter((script) => {
                if (selectedLanguage === "Any language") return true;
                return script.programmingLanguage === selectedLanguage;
            })
        );
    }, [
        scriptsFound,
        selectedLanguage,
    ]);

    useEffect(() => {
        setScriptsFoundPaginated(
            scriptsFoundFiltered.slice(page * rowsPerPage, (page + 1) * rowsPerPage)
        );
    }, [rowsPerPage, page, scriptsFoundFiltered]);

    useEffect(() => {
        handleSearch();
    }, [search, selectedLanguage]);

    const handleChangePage = (event, newPage) => {
        setPage(newPage);
    };

    const handleChangeRowsPerPage = (event) => {
        setRowsPerPage(parseInt(event.target.value, 10));
        setPage(0);
    };

    const handleItemSelected = (event) => {
        console.log({event})
        if (event.target.checked) {
            setSelectedScripts([...selectedScripts, event.target.id]);
        } else {
            setSelectedScripts(
                selectedScripts?.filter((script) => script !== event.target.id)
            );
        }
    };

    const handleDelete = async (scriptId) => {
        if (
            confirm(
                "Are you sure you want to delete this script? (All version will be removed)"
            )
        ) {
            AxiosRq.getInstance().deleteScript(scriptId);
            const scriptsFiltered = scriptsFound?.filter(
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
            let scriptsWithoutDeletedScripts = scriptsFound;
            for (const scriptId of selectedScripts) {
                AxiosRq.getInstance().deleteScript(scriptId);
                scriptsWithoutDeletedScripts = scriptsWithoutDeletedScripts?.filter(
                    (script) => script.scriptId != scriptId
                );
            }
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
                    <MyNewScriptsList
                        item={"my"}
                        myScripts={true}
                        display={display}
                        search={search}
                        scriptsFoundFiltered={scriptsFoundFiltered}
                        handleDelete={handleDelete}
                        userId={userId}
                        selectedScripts={selectedScripts}
                        page={page}
                        rowsPerPage={rowsPerPage}
                        scriptsFoundPaginated={scriptsFoundPaginated}
                        handleChangePage={handleChangePage}
                        handleChangeRowsPerPage={handleChangeRowsPerPage}
                        handleItemSelected={handleItemSelected}
                    />
                </AccordionDetails>
            </Accordion>
        </>
    );
}

export default ScriptListPage;
