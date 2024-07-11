import React from 'react';
import {Box, Modal} from "@mui/material";
import UnstyledInputIntroduction from "../Custom/UnstyledInputIntroduction.jsx";
import Button from "@mui/material/Button";

const ModalAddGroupMember = ({open,
                               handleClose,
                               handleSubmitRegisterEvent
                           }) =>
{
    const [username, setUsername] = React.useState("");
    const style = {
            position: "absolute",
            top: "50%",
            left: "50%",
            transform: "translate(-50%, -50%)",
            width: 400,
            bgcolor: "background.paper",
            border: "2px solid #000",
            boxShadow: 24,
            pt: 2,
            px: 4,
            pb: 3,
        };

        return (
            <Modal
                open={open}
                onClose={handleClose}
                aria-labelledby="parent-modal-title"
                aria-describedby="parent-modal-description"
            >
                <Box sx={{ ...style, width: 400 }}>
                        <div>
                            <label>Username: </label>
                            <UnstyledInputIntroduction
                                id="username"
                                name="username"
                                handleInput={(event) => setUsername(event.target.value)}
                            />
                        </div>



                    <Button
                        component="label"
                        role={undefined}
                        variant="contained"
                        tabIndex={-1}
                        onClick={handleSubmitRegisterEvent}
                        disabled={!username}
                    >
                        Submit
                    </Button>
                </Box>
            </Modal>
        );
}


export default ModalAddGroupMember;