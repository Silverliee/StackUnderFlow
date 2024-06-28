import React, {useEffect, useState} from 'react';
import {useScripts} from "../hooks/ScriptsProvider.jsx";
import PostItem from "../components/Post/PostItem.jsx";
import Divider from '@mui/material/Divider';

const HomePage = () => {
    const [myPosts, setMyPosts] = useState([]);
    const scripts = useScripts();

    useEffect (() => {
        console.log(scripts.state.scriptsFound[0]);
        const posts = [...scripts.state.scriptsFound].reverse();
        setMyPosts(posts);
    },[scripts.state.scriptsFound]);

    return (
        <div>
            HomePage
            {myPosts?.length > 0 && (
                myPosts.map((post, index) => (
                    <PostItem key={index} post={post}/>
                )))
            }
        </div>
    )
};

export default HomePage;