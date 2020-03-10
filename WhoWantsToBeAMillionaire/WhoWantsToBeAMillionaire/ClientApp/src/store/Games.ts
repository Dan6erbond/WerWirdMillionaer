import {Action, Reducer} from 'redux';
import {AppThunkAction} from './';
import {AnswerResponse, ErrorResponse} from "./ApiResponse";
import CategorySelection from "../components/quiz/CategorySelection";
import {AnswerSpecification, GameSpecification} from "./Specification";

export interface Category {
    categoryId: number;
    name: string;
}

export interface QuizQuestion {
    questionId: number;
    categoryId: number;
    question: string;
    answers: QuizAnswer[];
}

export interface QuizAnswer {
    answerId: number;
    answer: string;
    correct: boolean | undefined;
}

// -----------------
// STATE - This defines the type of data maintained in the Redux store.

export interface GameState {
    gameStarted: boolean;
    currentQuestion?: QuizQuestion;
    categories?: Category[];
    answerCorrect?: boolean;
    answering: boolean;
    loadingQuestion: boolean;
    usedJoker: boolean;
}

// -----------------
// ACTIONS - These are serializable (hence replayable) descriptions of state transitions.
// They do not themselves have any side-effects; they just describe something that is going to happen.

interface SetGameStartedAction {
    type: 'SET_GAME_STARTED';
    started: boolean;
}

interface SetCurrentQuestionAction {
    type: 'SET_CURRENT_QUESTION';
    question: QuizQuestion;
}

interface SetLoadingQuestionAction {
    type: 'SET_LOADING_QUESTION';
    loading: boolean;
}

interface SetCategoriesAction {
    type: 'SET_CATEGORIES';
    categories: Category[];
}

interface SetAnswerCorrectAction {
    type: 'SET_ANSWER_CORRECT';
    correct: boolean | undefined;
}

interface SetAnsweringAction {
    type: 'SET_ANSWERING';
    answering: boolean;
}

interface ResetAction {
    type: 'RESET';
}

interface SetUsedJokerAction {
    type: 'SET_USED_JOKER';
    usedJoker: boolean;
}

// Declare a 'discriminated union' type. This guarantees that all references to 'type' properties contain one of the
// declared type strings (and not any other arbitrary string).

type KnownAction =
    SetGameStartedAction
    | SetCurrentQuestionAction
    | SetLoadingQuestionAction
    | SetCategoriesAction
    | SetAnswerCorrectAction
    | SetAnsweringAction
    | ResetAction
    | SetUsedJokerAction;

// ----------------
// ACTION CREATORS - These are functions exposed to UI components that will trigger a state transition.
// They don't directly mutate state, but they can have external side-effects (such as loading data).

export const actionCreators = {
    startGame: (token: string, specification: GameSpecification): AppThunkAction<KnownAction> => (dispatch) => {
        fetch('api/games/start', {
            method: 'POST',
            headers: {
                "Accept": "application/json, text/plain, */*",
                "Content-type": "application/json",
                "Authorization": `Bearer ${token}`
            },
            body: JSON.stringify(specification)
        })
            .then(response => {
                if (!response.ok) {
                    return response.text().then(text => {
                        console.error(text);
                        return (response.json() as Promise<ErrorResponse>).then(error => {
                            throw new Error(error.title);
                        });
                    });
                }
                dispatch({type: 'SET_GAME_STARTED', started: true});
            })
            .catch(error => {
                console.error(error);
            });
    },
    fetchQuestion: (token: string): AppThunkAction<KnownAction> => (dispatch) => {
        fetch('api/games/question', {
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
                return response.json() as Promise<QuizQuestion>;
            })
            .then(data => {
                console.log(data);
                dispatch({type: 'SET_CURRENT_QUESTION', question: data});
            })
            .catch(error => {
                console.error(error);
            });
    },
    useJoker: (token: string): AppThunkAction<KnownAction> => (dispatch) => {
        fetch('api/games/joker', {
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
                return response.json() as Promise<QuizQuestion>;
            })
            .then(data => {
                dispatch({type: 'SET_CURRENT_QUESTION', question: data});
                dispatch({type: 'SET_USED_JOKER', usedJoker: true});
            })
            .catch(error => {
                console.error(error);
            });
    },
    fetchCategories: (): AppThunkAction<KnownAction> => (dispatch) => {
        dispatch({type: 'SET_LOADING_QUESTION', loading: true});

        fetch('api/games/categories', {
            headers: {
                "Accept": "application/json, text/plain, */*",
                "Content-type": "application/json",
            },
        })
            .then(response => {
                if (!response.ok) {
                    return (response.json() as Promise<ErrorResponse>).then(error => {
                        throw new Error(error.title);
                    });
                }
                return response.json() as Promise<Category[]>;
            })
            .then(data => {
                console.log(data);
                dispatch({type: 'SET_CATEGORIES', categories: data});
            })
            .catch(error => {
                console.error(error);
            });
    },
    answerQuestion: (token: string, specification: AnswerSpecification): AppThunkAction<KnownAction> => (dispatch) => {
        dispatch({type: 'SET_ANSWERING', answering: true});

        fetch('api/games/answer', {
            method: 'POST',
            headers: {
                "Accept": "application/json, text/plain, */*",
                "Content-type": "application/json",
                "Authorization": `Bearer ${token}`
            },
            body: JSON.stringify(specification)
        })
            .then(response => {
                if (!response.ok) {
                    dispatch({type: 'SET_ANSWER_CORRECT', correct: false});
                    return response.text().then(text => {
                        console.error(text);
                        return (response.json() as Promise<ErrorResponse>).then(error => {
                            throw new Error(error.title);
                        });
                    });
                }
                return response.json() as Promise<AnswerResponse>;
            })
            .then(data => {
                dispatch({type: 'SET_ANSWER_CORRECT', correct: data.correct});
            })
            .catch(error => {
                console.error(error);
            });
    },
    reset: (): AppThunkAction<KnownAction> => (dispatch) => {
        dispatch({type: 'RESET'});
    }
};

// ----------------
// REDUCER - For a given state and action, returns the new state. To support time travel, this must not mutate the old state.

const unloadedState: GameState = {
    gameStarted: false,
    currentQuestion: undefined,
    categories: undefined,
    answerCorrect: undefined,
    answering: false,
    loadingQuestion: false,
    usedJoker: false
};

export const reducer: Reducer<GameState> = (state: GameState | undefined, incomingAction: Action): GameState => {
    if (state === undefined) {
        return unloadedState;
    }

    const action = incomingAction as KnownAction;
    switch (action.type) {
        case "SET_GAME_STARTED":
            return {
                gameStarted: action.started,
                currentQuestion: state.currentQuestion,
                categories: state.categories,
                answerCorrect: state.answerCorrect,
                answering: state.answering,
                loadingQuestion: state.loadingQuestion,
                usedJoker: state.usedJoker
            };
        case "SET_CURRENT_QUESTION":
            return {
                gameStarted: state.gameStarted,
                currentQuestion: action.question,
                categories: state.categories,
                answerCorrect: undefined,
                answering: state.answering,
                loadingQuestion: false,
                usedJoker: state.usedJoker
            };
        case "SET_CATEGORIES":
            return {
                gameStarted: state.gameStarted,
                currentQuestion: state.currentQuestion,
                categories: action.categories,
                answerCorrect: state.answerCorrect,
                answering: state.answering,
                loadingQuestion: state.loadingQuestion,
                usedJoker: state.usedJoker
            };
        case "SET_ANSWER_CORRECT":
            return {
                gameStarted: state.gameStarted,
                currentQuestion: state.currentQuestion,
                categories: state.categories,
                answerCorrect: action.correct,
                answering: false,
                loadingQuestion: state.loadingQuestion,
                usedJoker: state.usedJoker
            };
        case "SET_ANSWERING":
            return {
                gameStarted: state.gameStarted,
                currentQuestion: state.currentQuestion,
                categories: state.categories,
                answerCorrect: state.answerCorrect,
                answering: action.answering,
                loadingQuestion: state.loadingQuestion,
                usedJoker: state.usedJoker
            };
        case "SET_LOADING_QUESTION":
            return {
                gameStarted: state.gameStarted,
                currentQuestion: state.currentQuestion,
                categories: state.categories,
                answerCorrect: state.answerCorrect,
                answering: state.answering,
                loadingQuestion: action.loading,
                usedJoker: state.usedJoker
            };
        case "SET_USED_JOKER":
            return {
                gameStarted: state.gameStarted,
                currentQuestion: state.currentQuestion,
                categories: state.categories,
                answerCorrect: state.answerCorrect,
                answering: state.answering,
                loadingQuestion: state.loadingQuestion,
                usedJoker: action.usedJoker
            };
        case "RESET":
            return unloadedState;
        default:
            return state;
    }
};