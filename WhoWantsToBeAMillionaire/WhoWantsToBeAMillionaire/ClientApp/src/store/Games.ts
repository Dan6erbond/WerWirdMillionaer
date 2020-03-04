import {Action, Reducer} from 'redux';
import {AppThunkAction} from './';
import {ErrorResponse, KnownErrors, TokenResponse} from "./ApiResponse";

// -----------------
// STATE - This defines the type of data maintained in the Redux store.

export interface GameState {
    game?: Game;
}

interface Game {
    
}

interface QuizQuestion {
    
}

// -----------------
// ACTIONS - These are serializable (hence replayable) descriptions of state transitions.
// They do not themselves have any side-effects; they just describe something that is going to happen.

interface SetGameAction {
    type: 'SET_GAME';
    game: Game;
}

// Declare a 'discriminated union' type. This guarantees that all references to 'type' properties contain one of the
// declared type strings (and not any other arbitrary string).

type KnownAction = SetGameAction;

// ----------------
// ACTION CREATORS - These are functions exposed to UI components that will trigger a state transition.
// They don't directly mutate state, but they can have external side-effects (such as loading data).

export const actionCreators = {};

// ----------------
// REDUCER - For a given state and action, returns the new state. To support time travel, this must not mutate the old state.

const unloadedState: GameState = {
    game: undefined
};

export const reducer: Reducer<GameState> = (state: GameState | undefined, incomingAction: Action): GameState => {
    if (state === undefined) {
        return unloadedState;
    }

    const action = incomingAction as KnownAction;
    switch (action.type) {
        case 'SET_GAME':
            return {
                game: action.game
            };
        default:
            return state;
    }
};