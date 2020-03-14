import {Action, Reducer} from 'redux';
import {AppThunkAction} from './';
import {ErrorResponse, KnownErrors, TokenResponse} from "./ApiResponse";

export interface User {
    userId: number;
    username: string;
    isAdmin: boolean;
}

// -----------------
// STATE - This defines the type of data maintained in the Redux store.

export interface UserState {
    token?: string;
    userCreated: boolean;
    apiError?: KnownErrors;
    userData?: User;
}

// -----------------
// ACTIONS - These are serializable (hence replayable) descriptions of state transitions.
// They do not themselves have any side-effects; they just describe something that is going to happen.

interface SetUserTokenAction {
    type: 'SET_USER_TOKEN';
    token: string;
}

interface SetUserDataAction {
    type: 'SET_USER_DATA';
    data: User;
}

interface SetUserCreatedAction {
    type: 'SET_USER_CREATED';
    created: boolean;
}

interface SetApiErrorAction {
    type: 'SET_API_ERROR';
    error: KnownErrors | undefined;
}

interface SignOutAction {
    type: 'SIGN_OUT';
}

// Declare a 'discriminated union' type. This guarantees that all references to 'type' properties contain one of the
// declared type strings (and not any other arbitrary string).

type KnownAction =
    SetUserTokenAction
    | SetUserDataAction
    | SetUserCreatedAction
    | SetApiErrorAction
    | SignOutAction;

// ----------------
// ACTION CREATORS - These are functions exposed to UI components that will trigger a state transition.
// They don't directly mutate state, but they can have external side-effects (such as loading data).

export const actionCreators = {
    requestUserData: (token: string): AppThunkAction<KnownAction> => (dispatch) => {
        fetch('api/users/data', {
            headers: {
                "Accept": "application/json, text/plain, */*",
                "Content-type": "application/json",
                "Authorization": `Bearer ${token}`
            },
        })
            .then(response => {
                if (!response.ok) {
                    return (response.json() as Promise<ErrorResponse>).then(error => {
                        throw new Error(error.title);
                    });
                }
                return response.json() as Promise<User>;
            })
            .then(data => {
                dispatch({type: 'SET_USER_DATA', data: data});
            })
            .catch(error => {
                console.error(error);
            });
    },
    createUser: (username: string, password: string): AppThunkAction<KnownAction> => (dispatch) => {
        let body = {
            Username: username,
            Password: password
        };

        fetch('api/users/create', {
            method: 'POST',
            headers: {
                "Accept": "application/json, text/plain, */*",
                "Content-type": "application/json"
            },
            body: JSON.stringify(body)
        })
            .then(response => {
                if (!response.ok) {
                    return (response.json() as Promise<ErrorResponse>).then(error => {
                        let type = error.type as KnownErrors;
                        switch(type) {
                            case "PASSWORD_NOT_CONTAINS_LETTERS":
                            case "PASSWORD_TOO_SHORT":
                            case "PASSWORD_NOT_CONTAINS_SPECIAL_CHARACTERS":
                            case "USER_ALREADY_EXISTS":
                                dispatch({type: 'SET_API_ERROR', error: type});
                                break;
                            default:
                                console.error(error);
                        }
                        throw new Error(error.title);
                    });
                }
                dispatch({type: 'SET_USER_CREATED', created: true});
            })
            .catch(error => {
                console.error(error.message);
            });
    },
    login: (username: string, password: string): AppThunkAction<KnownAction> => (dispatch) => {
        let body = {
            Username: username,
            Password: password
        };

        fetch('api/users/login', {
            method: 'POST',
            headers: {
                "Accept": "application/json, text/plain, */*",
                "Content-type": "application/json"
            },
            body: JSON.stringify(body)
        })
            .then(response => {
                if (!response.ok) {
                    return (response.json() as Promise<ErrorResponse>).then(error => {
                        let type = error.type as KnownErrors;
                        switch(type) {
                            case "USER_DOES_NOT_EXIST":
                            case "INCORRECT_PASSWORD":
                                dispatch({type: 'SET_API_ERROR', error: type});
                                break;
                            default:
                                console.error(error);
                        }
                        throw new Error(error.title);
                    });
                }
                return response.json() as Promise<TokenResponse>;
            })
            .then(d => {
                dispatch({type: 'SET_USER_TOKEN', token: d.token});
            })
            .catch(error => {
                console.error(error.message);
            });
    },
    signOut: (): AppThunkAction<KnownAction> => (dispatch) => {
        dispatch({type: 'SIGN_OUT'});
    }
};

// ----------------
// REDUCER - For a given state and action, returns the new state. To support time travel, this must not mutate the old state.

const unloadedState: UserState = {
    token: undefined,
    userData: undefined,
    userCreated: false,
    apiError: undefined
};

export const reducer: Reducer<UserState> = (state: UserState | undefined, incomingAction: Action): UserState => {
    if (state === undefined) {
        return unloadedState;
    }
    
    const retVal: UserState = Object.assign({}, state);

    const action = incomingAction as KnownAction;
    switch (action.type) {
        case 'SET_USER_TOKEN':
            retVal.token = action.token;
            break;
        case 'SET_USER_DATA':
            retVal.userData = action.data;
            break;
        case 'SET_USER_CREATED':
            retVal.userCreated = action.created;
            break;
        case 'SET_API_ERROR':
            retVal.apiError = action.error;
            break;
        case "SIGN_OUT":
            retVal.apiError = undefined;
            retVal.userCreated = false;
            retVal.userData = undefined;
            retVal.token = undefined;
            break;
    }
    
    return retVal;
};