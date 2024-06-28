import * as React from 'react';
import Card from '@mui/material/Card';
import CardActions from '@mui/material/CardActions';
import CardContent from '@mui/material/CardContent';
import CardMedia from '@mui/material/CardMedia';
import Button from '@mui/material/Button';
import Typography from '@mui/material/Typography';
import { PiFileCSharpDuotone } from "react-icons/pi";
import { SiPython } from "react-icons/si";
import ThumbUpOffAltIcon from '@mui/icons-material/ThumbUpOffAlt';
import ThumbUpAltIcon from '@mui/icons-material/ThumbUpAlt';
import CommentContainer from "./CommentContainer.jsx";
import AxiosRq from "../../Axios/AxiosRequester.js";
import {useEffect} from "react";

export default function PostItem({post}) {
    const [comments, setComments] = React.useState([{text:"coucou"},{text:"coucou2"},{text:"coucou3"}]);
    const [open, setOpen] = React.useState(false);
    const [editMode, setEditMode] = React.useState(false);
    const [numberOfLikes, setNumberOfLikes] = React.useState(post?.numberOfLikes);
    const [isLiked, setIsLiked] = React.useState(post?.isLiked);

    const handleOpenComments = () => {
        setOpen(!open);
    }

    useEffect(() => {
        fetchComments()
    },[post.scriptId]);

    const fetchComments = async () => {
        AxiosRq.getInstance().getComments(post.scriptId).then((res) => {
            if(res) {
                console.log(res);
                setComments(res)
            }
        })
    }

    const handleComment = async (newComment) => {
        //post newComment
        console.log(newComment);
        await AxiosRq.getInstance().postComment(post.scriptId, newComment);
        //add it to comments List
        fetchComments();
    }

    const handleDelete = async (commentId) => {
        const result = await AxiosRq.getInstance().deleteComment(commentId);
        if (result === "success") {
            setComments(comments.filter((comment) => comment.commentId !== commentId));
        }
    }

    const handleEdit = async (commentId, newText) => {
        console.log("Edit comment " + commentId + " with text " + newText);
        const newComment = await AxiosRq.getInstance().updateComment(commentId,newText);
        fetchComments();
    }

    const handleDownload = async () => {
        const data = await AxiosRq.getInstance().getScriptVersionBlob(
            post.scriptId
        );
        const element = document.createElement("a");
        const file = new Blob([data], { type: "text/plain" });
        element.href = URL.createObjectURL(file);
        element.download =
            post.scriptName +
            (post.programmingLanguage == "Python" ? ".py" : ".cs");
        document.body.appendChild(element); // Required for this to work in FireFox
        element.click();
        document.body.removeChild(element);
    };

    const handleUnlike = async (scriptId) => {
        console.log('unlike')
        const result = await AxiosRq.getInstance().unlike(scriptId);
        if (result === "success") {
            setNumberOfLikes(numberOfLikes-1);
            setIsLiked(false);
        }
    }
    const handleLike = async (scriptId) => {
        console.log('like')
        const result = await AxiosRq.getInstance().like(scriptId);
        if (result === "success") {
            setNumberOfLikes(numberOfLikes+1);
            setIsLiked(true);
        }
    }

    return (
        <>
            <Card sx={{ maxWidth: 600, marginBottom: 4 }}>
                <CardMedia
                    component="img"
                    alt="random_picture"
                    height="140"
                    image={post.source ? post.source : "https://www.echosciences-grenoble.fr/uploads/article/image/attachment/1005418938/xl_lens-1209823_1920.jpg"}
                />
                <CardContent>
                    <Typography gutterBottom variant="h5" component="div">
                        {post.scriptName}
                    </Typography>
                    <Typography variant="body2" color="text.secondary">
                        {post.description}
                    </Typography>
                    <br/>
                    <Button onClick={handleDownload}>
                        {post.programmingLanguage == "Csharp" ? <PiFileCSharpDuotone style={{marginRight: 4}}/> :
                            <SiPython style={{marginRight: 4}}/>
                        } Try me !
                    </Button>
                </CardContent>
                <CardActions style={{display:"flex", justifyContent: 'space-between'}}>
                    <Button size="small" onClick={handleOpenComments}>Comments</Button>
                    <div style={{alignContent:"center"}}>{isLiked ?
                        <ThumbUpAltIcon style={{cursor:"pointer"}} onClick={() => handleUnlike(post.scriptId)}/>
                        :
                        <ThumbUpOffAltIcon style={{cursor:"pointer"}} onClick={() => handleLike(post.scriptId)}/>
                        }
                        {numberOfLikes}
                    </div>
                </CardActions>
                {open && (
                    <CommentContainer key={post.scriptId} comments={comments} handleComment={handleComment} handleDelete={handleDelete} handleEdit={handleEdit} editMode={editMode} setEditMode={setEditMode}/>
                )}
            </Card>
        </>
    );
}
