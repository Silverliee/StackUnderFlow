
const MessageItem = ({ message, onClick }) => {

    return (
        <>
            <div style={{padding:'5px 10px 0px 10px'}}>{message.date + ' - ' + message.text}</div>
        </>
    )
}

export default MessageItem