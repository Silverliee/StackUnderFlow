import {useEffect, useState} from "react";
import {useScripts} from "../hooks/ScriptsProvider.jsx";

const PipelinesPage = () => {
    const [pipelines, setPipelines] = useState(["292"]);

    const {state} = useScripts();

    useEffect(() => {
        console.log({
            state,
            pipelines,
            keys: Object.keys(state.pipelines)
        });
        setPipelines(Object.keys(state.pipelines));
    },[state.pipelines]);
    return (
        <>
            <div>PipelinePage</div>
            {pipelines.length > 0 && (
                pipelines.map((pipeline) => (
                    <div key={pipeline.id}>{pipeline.id}</div>
                ))
            )}
        </>
    )
}

export default PipelinesPage;