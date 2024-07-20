import {useEffect, useState} from 'react';
import PipelineItem from "./PipelineItem.jsx";
import SocketManager from "../../Socket/SocketManager";
import LogDisplayer from "./LogDisplayer.jsx";
import {useParams} from "react-router-dom";
import {useScripts} from "../../hooks/ScriptsProvider.jsx";

const PipelineDetails = () => {
    const [scriptListLoaded, setScriptListLoaded] = useState([]);
    const [messages, setMessages] = useState([]);
    const [scriptList, setScriptList] = useState([]);

    const {pipelineId} = useParams();
    const {state} = useScripts();

    const fakeData = [
        {
            "scriptId": 7,
            "scriptName": "test",
            "description": "ezaeaze",
            "inputScriptType": "None",
            "outputScriptType": "None",
            "programmingLanguage": "Csharp",
            "visibility": "Public",
            "userId": 4,
            "creatorName": "User2",
            "numberOfLikes": 1,
            "isLiked": false,
            "isFavorite": true,
            "creationDate": "17/07/2024 17:16:30",
            "status": "Pending",
            "result": "Info"
        },
        {
            "scriptId": 7,
            "scriptName": "test",
            "description": "ezaeaze",
            "inputScriptType": "None",
            "outputScriptType": "None",
            "programmingLanguage": "Csharp",
            "visibility": "Public",
            "userId": 4,
            "creatorName": "User2",
            "numberOfLikes": 1,
            "isLiked": false,
            "isFavorite": true,
            "creationDate": "17/07/2024 17:16:30",
            "status": "Waiting",
            "result": "success"
        },
        {
            "scriptId": 5,
            "scriptName": "HelloScript2",
            "description": "A better script than HelloScript",
            "inputScriptType": "None",
            "outputScriptType": "None",
            "programmingLanguage": "Csharp",
            "visibility": "Public",
            "userId": 2,
            "creatorName": "Damien",
            "numberOfLikes": 1,
            "isLiked": true,
            "isFavorite": false,
            "creationDate": "15/07/2024 18:31:35",
            "status": "Done",
            "result": "error"
        }
    ]
    const fakeMessages = [
        {text:"a lot of text to see how the next line looks like",date:"19/07/2024 17:31:30",},
        {text:"fake",date:"19/07/2024 17:31:31",},
        {text:"fake",date:"19/07/2024 17:31:32",},
        {text:"fake",date:"19/07/2024 17:31:33",},
        {text:"fake",date:"19/07/2024 17:31:33",},
        {text:"fake",date:"19/07/2024 17:31:33",},
        {text:"fake",date:"19/07/2024 17:31:33",},
        {text:"fake",date:"19/07/2024 17:31:33",},
        {text:"fake",date:"19/07/2024 17:31:33",},
        {text:"fake",date:"19/07/2024 17:31:33",},
        {text:"fake",date:"19/07/2024 17:31:33",},
        {text:"fake",date:"19/07/2024 17:31:33",},
    ];

    useEffect(() => {
    //TODO checker que le formattage des scripts fonctionne

    // const scriptListFormatted = scriptList.map((item,index) => {
    //     if (index === 0) {
    //         return {...item,status:"Pending",result:"info"}
    //     }
    //     return {...item,status:"Waiting",result:"info"}
    // });
    // setScriptListLoaded(scriptListFormatted);
        setScriptListLoaded(fakeData);
        setMessages(fakeMessages);
    }, []);

    useEffect(() => {
        setScriptList(state.pipelines[pipelineId]);
    },[state.pipelines[pipelineId]]);


    useEffect(() => {
        //TODO checker si la connexion fonctionne
        const pipelineId = crypto.randomUUID();
        SocketManager.connectWebSocketPipeline(pipelineId, handleWebSocketMessage);
    }, []);

    useEffect(() => {
        console.log(scriptListLoaded);
    }, [scriptListLoaded]);

    const handleWebSocketMessage = (message) => {
        //TODO :
        // analyser le contenu du message pour updater les scripts (status/result) et la couleur du message (pour l'affichage dans LogDisplayer
        // récuperer le lien ou le binary pour permettre le download du fichier intermédiaire et/ou final
        setMessages(prevMessages => [...prevMessages, message]);
    };

    //TODO:
    // Peut-être lié l'index ici avec un tableau setté dans handleWebSocketMessage lorsque l'on reçoit le lien ou le binary
    const handleDownload = (index) => {
        console.log(index);
    }

    return (
        <>
            <div>Pipeline n°{pipelineId}</div>
            <br/>
            <div className={"mainContainer"} style={{display:"flex", justifyContent:"space-between"}}>
                <div className={"leftContainer"} style={{
                    // border: "1px solid black",
                    height:"80vh", width:"49%",
                    display:'flex',
                    flexDirection:'column',
                    borderRadius: '10px',
                    overflow: 'auto'}}>
                    {scriptListLoaded.map((item, index) => (
                        <PipelineItem key={index} script={item} index={index} handleDownload={handleDownload}/>
                    ))}
                </div>
                <div className={"rightContainer"} style={{
                    height:"60vh",
                    width:"49%"
                   }}>
                    <LogDisplayer messages={messages}/>
                </div>
            </div>
        </>
    );
}

export default PipelineDetails;
