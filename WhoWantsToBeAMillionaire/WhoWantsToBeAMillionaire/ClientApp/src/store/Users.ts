import {Action, Reducer} from 'redux';
import {AppThunkAction} from './';

// -----------------
// STATE - This defines the type of data maintained in the Redux store.

export interface UserState {
    token?: string;
    userCreated: boolean;
    usernameCorrect: boolean;
    passwordCorrect: boolean;
    userData?: User;
}

export interface TokenResponse {
    token: string;
    expiration: string;
}

export interface User {
    UserId: number;
    Username: string;
    IsAdmin: boolean;
    Salt: string;
    Password: string;
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

interface SetUsernameCorrectAction {
    type: 'SET_USERNAME_CORRECT';
    correct: boolean;
}

interface SetPasswordCorrectAction {
    type: 'SET_PASSWORD_CORRECT';
    correct: boolean;
}

// Declare a 'discriminated union' type. This guarantees that all references to 'type' properties contain one of the
// declared type strings (and not any other arbitrary string).

type KnownAction =
    SetUserTokenAction
    | SetUserDataAction
    | SetUserCreatedAction
    | SetUsernameCorrectAction
    | SetPasswordCorrectAction;

// ----------------
// ACTION CREATORS - These are functions exposed to UI components that will trigger a state transition.
// They don't directly mutate state, but they can have external side-effects (such as loading data).

export const actionCreators = {
    requestUserData: (token: string): AppThunkAction<KnownAction> => (dispatch, getState) => {
        fetch('api/users/data', {
            headers: {
                "Accept": "application/json, text/plain, */*",
                "Content-type": "application/json",
                "Authorization": `Bearer ${token}`
            },
        })
            .then(response => {
                if (!response.ok) {
                    throw new Error(response.statusText);
                }
                return response.json() as Promise<User>;
            })
            .then(data => {
                dispatch({type: 'SET_USER_DATA', data: data});
            })
            .catch((error: Error) => {
                console.error(error);
            });
    },
    createUser: (username: string, password: string): AppThunkAction<KnownAction> => (dispatch, getState) => {
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
                    throw new Error(response.statusText);
                }
                dispatch({type: 'SET_USER_CREATED', created: true});
                return response;
            })
            .catch((error: Error) => {
                console.error(error);
            });
    },
    login: (username: string, password: string): AppThunkAction<KnownAction> => (dispatch, getState) => {
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
                    throw new Error(response.statusText);
                }
                return response.json() as Promise<TokenResponse>;
            })
            .then(data => {
                dispatch({type: 'SET_USER_TOKEN', token: data.token});
            })
            .catch((error: Error) => {
                console.error(error);
            });
    }
};

// ----------------
// REDUCER - For a given state and action, returns the new state. To support time travel, this must not mutate the old state.

const unloadedState: UserState = {
    token: undefined,
    userData: undefined,
    userCreated: false,
    usernameCorrect: false,
    passwordCorrect: false
};

export const reducer: Reducer<UserState> = (state: UserState | undefined, incomingAction: Action): UserState => {
    if (state === undefined) {
        return unloadedState;
    }

    const action = incomingAction as KnownAction;
    switch (action.type) {
        case 'SET_USER_TOKEN':
            return {
                token: action.token,
                userData: state.userData,
                userCreated: state.userCreated,
                usernameCorrect: state.usernameCorrect,
                passwordCorrect: state.passwordCorrect
            };
        case 'SET_USER_DATA':
            return {
                token: state.token,
                userData: action.data,
                userCreated: state.userCreated,
                usernameCorrect: state.usernameCorrect,
                passwordCorrect: state.passwordCorrect
            };
        case 'SET_USER_CREATED':
            return {
                token: state.token,
                userData: state.userData,
                userCreated: action.created,
                usernameCorrect: state.usernameCorrect,
                passwordCorrect: state.passwordCorrect
            };
        case 'SET_USERNAME_CORRECT':
            return {
                token: state.token,
                userData: state.userData,
                userCreated: state.userCreated,
                usernameCorrect: action.correct,
                passwordCorrect: state.passwordCorrect
            };
        case 'SET_PASSWORD_CORRECT':
            return {
                token: state.token,
                userData: state.userData,
                userCreated: state.userCreated,
                usernameCorrect: state.usernameCorrect,
                passwordCorrect: action.correct
            };
        default:
            return state;
    }
};