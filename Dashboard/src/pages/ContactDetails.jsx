import React, {useEffect, useState} from 'react';
import {useParams} from "react-router-dom";
import AxiosRequester from "../Axios/AxiosRequester.js";
import {getRandomInt} from "../utils/utils.js";
import {Image} from "@mui/icons-material";
import Card from "@mui/material/Card";
import {TextareaAutosize} from "@mui/material";
import Profile from "../components/Profile/Profile.jsx";
import PostItem from "../components/Post/PostItem.jsx";
import {TiArrowBack} from "react-icons/ti";
import { useNavigate} from "react-router-dom";

const ContactDetails = () => {
    const [userDetails, setUserDetails] = useState({});
    const [loading, setLoading] = useState(true);
    const [userScripts, setUserScripts] = useState([]);
    const { friendId } = useParams();
    const randomInt = getRandomInt(friendId);
    const landscapeInt = friendId % 3 + 1;
    const navigate = useNavigate();

    useEffect(() => {
        getUserInfo();
        getUserScripts();
    }, [friendId]);

    const getUserInfo = async () => {
        const user = await AxiosRequester.getInstance().getUserById(friendId);
        console.log({ user, friendId });
        if (user !== null) {
            setUserDetails(user);
        }
    }

    const getUserScripts = async () => {
        const result = await AxiosRequester.getInstance().getScriptByUserIdAndVisiblity(friendId,"Friend");
        if (result !== null) {
            setUserScripts(result);
        }
    }

    return (
        <div>
            <div style={{ display: "flex", justifyContent: "flex-end", marginBottom:"2px", cursor:"pointer" }}>
                <TiArrowBack onClick={() => navigate("/contacts")} />
            </div>
            <div
                style={{
                    display: "flex",
                    justifyContent: "space-between",
                    alignItems: "center",
                    backgroundImage: `url(/assets/Landscape${landscapeInt}.jpg)`,
                    backgroundSize: 'cover',
                    backgroundPosition: 'center',
                    position: 'relative',
                    height: '40vh', // Adjust as needed
                    width: '100%', // Adjust as needed
                    opacity: 1, // Adjust as needed
                    marginBottom: '10px',
                }}
            >
                <Profile randomInt={randomInt}
                         user={userDetails}
                         />
            </div>
            {userScripts?.length > 0 ? (
                    userScripts.map((post, index) => (
                        <PostItem key={index} post={post}/>
                    ))
                ) :
                <div>No Scripts Shared</div>
            }
        </div>
    );
};

export default ContactDetails;