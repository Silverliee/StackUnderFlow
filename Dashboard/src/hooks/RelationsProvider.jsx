import { createContext, useReducer, useEffect, useContext } from "react";
import AxiosRq from "../Axios/AxiosRequester.js";
import { useAuth } from "../hooks/AuthProvider.jsx";

const RelationsContext = createContext();

const friendsReducer = (state, action) => {
    switch (action.type) {
        case 'ADD_FRIEND':
            return [...state, action.payload];
        case 'REMOVE_FRIEND':
            return state.filter(friend => friend.userId !== action.payload.userId);
        case 'SET_FRIENDS':
            return action.payload;
        default:
            return state;
    }
};

const groupsReducer = (state, action) => {
    switch (action.type) {
        case 'ADD_GROUP':
            return [...state, action.payload];
        case 'REMOVE_GROUP':
            return state.filter(group => group.groupId !== action.payload.groupId);
        case 'SET_GROUPS':
            return action.payload;
        default:
            return state;
    }
};

const followingReducer = (state, action) => {
    switch (action.type) {
        case 'ADD_FOLLOW':
            return [...state, action.payload];
        case 'REMOVE_FOLLOW':
            return state.filter(follow => follow.id !== action.payload.id);
        case 'SET_FOLLOWS':
            return action.payload;
        default:
            return state;
    }
};

const RelationsProvider = ({ children }) => {
    const { authData } = useAuth();
    const initialFriendsState = [];
    const initialGroupsState = [];
    const initialFollowingState = [];

    const [myFriends, dispatchFriends] = useReducer(friendsReducer, initialFriendsState);
    const [myGroups, dispatchGroups] = useReducer(groupsReducer, initialGroupsState);
    const [myFollows, dispatchFollows] = useReducer(followingReducer, initialFollowingState);

    useEffect(() => {
        if (authData && authData.token) {
            const fetchFriendsGroupsFollows = async () => {
                try {
                    const [friends, groups, follows] = await Promise.all([
                        AxiosRq.getInstance().getFriends(), // Assurez-vous que cette méthode existe dans AxiosRq
                        AxiosRq.getInstance().getGroups(), // Assurez-vous que cette méthode existe dans AxiosRq
                        AxiosRq.getInstance().getFollows() // Assurez-vous que cette méthode existe dans AxiosRq
                    ]);
                    dispatchFriends({ type: 'SET_FRIENDS', payload: friends });
                    dispatchGroups({ type: 'SET_GROUPS', payload: groups });
                    dispatchFollows({ type: 'SET_FOLLOWS', payload: follows });
                } catch (error) {
                    console.error(error);
                }
            };
            fetchFriendsGroupsFollows();
        }
    }, [authData]);

    return (
        <RelationsContext.Provider
            value={{
                myFriends,
                dispatchFriends,
                myGroups,
                dispatchGroups,
                myFollows,
                dispatchFollows
            }}
        >
            {children}
        </RelationsContext.Provider>
    );
};

export default RelationsProvider;

export const useRelations = () => {
    return useContext(RelationsContext);
};
