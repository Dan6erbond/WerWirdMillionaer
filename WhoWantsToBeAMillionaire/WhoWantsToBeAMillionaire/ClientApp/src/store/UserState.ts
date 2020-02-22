﻿import { Action, Reducer } from 'redux';
import { AppThunkAction } from './';

// -----------------
// STATE - This defines the type of data maintained in the Redux store.

export interface UserState {
    token?: string;
}

export interface TokenResponse {
    token: string;
    expiration: string;
}

// -----------------
// ACTIONS - These are serializable (hence replayable) descriptions of state transitions.
// They do not themselves have any side-effects; they just describe something that is going to happen.

interface RequestUserTokenAction {
    type: 'SET_USER_TOKEN';
    token: string;
}

// Declare a 'discriminated union' type. This guarantees that all references to 'type' properties contain one of the
// declared type strings (and not any other arbitrary string).

type KnownAction = RequestUserTokenAction;

// ----------------
// ACTION CREATORS - These are functions exposed to UI components that will trigger a state transition.
// They don't directly mutate state, but they can have external side-effects (such as loading data).

export const actionCreators = {
    login: (username: string, password: string): AppThunkAction<KnownAction> => (dispatch, getState) => {
        const appState = getState();
        if (appState && appState.userState) {
            let body = {
                username: username,
                password: password
            };
            fetch('api/users/login', {
                body: JSON.stringify(body)
            })
                .then(response => {
                    if (!response.ok) {
                        throw new Error(response.statusText);
                    }
                    return response.json() as Promise<TokenResponse>
                })
                .then(data => {
                    dispatch({type: "SET_USER_TOKEN", token: data.token});
                });
        }
    }
}

// ----------------
// REDUCER - For a given state and action, returns the new state. To support time travel, this must not mutate the old state.

const unloadedState: UserState = { token: undefined };

export const reducer: Reducer<UserState> = (state: UserState | undefined, incomingAction: Action) : UserState => {
    if (state === undefined) {
        return unloadedState;
    }
    
    const action = incomingAction as KnownAction;
    switch (action.type) {
        case 'SET_USER_TOKEN':
            return {
                token: action.token
            };
            break;
    }
    
    return state;
};