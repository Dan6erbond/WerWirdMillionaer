﻿import {Action, Reducer} from 'redux';
import {AppThunkAction} from './';
import {AnswerResult, ErrorResponse, QuizResult} from "./ApiResponse";
import {AnswerSpecification, GameSpecification} from "./Specification";
import {LeaderboardSort} from "../components/leaderboard/Leaderboard";

declare global {
    interface Array<T> {
        sortBy(property: string, ascending: boolean): Array<T>;
    }
}

Array.prototype.sortBy = function (property: string, ascending: boolean) {
    return this.sort(function (a, b) {
        return a[property] == b[property] ? 0 : (a[property] > b[property]) && ascending ? 1 : -1;
    });
};

export interface Category {
    categoryId: number;
    name: string;
}

export interface QuizQuestion {
    questionId: number;
    categoryId: number;
    question: string;
    timesAsked: number;
    correctlyAnswered: number;
    answers: QuizAnswer[];
    uses: number;
}

export interface QuizAnswer {
    answerId: number;
    answer: string;
    correct: boolean | undefined;
}

interface RunningGame {
    askedQuestions: QuizQuestion[];
    currentQuestion?: QuizQuestion;
    answerCorrect?: boolean;
    result?: QuizResult;
    usedJoker: boolean;
}

interface Game {
    gameId: number;
    username: string;
    start: string;
    rounds: Round[];
    points: number;
    duration: number;
    rank: number;
    weightedPoints: number;
}

interface Round {
    roundId: number;
    questionId: number;
    answerId?: number;
    duration: number;
    usedJoker: boolean;
}

// -----------------
// STATE - This defines the type of data maintained in the Redux store.

export interface GameState {
    runningGame?: RunningGame;
    categories?: Category[];
    answering: boolean;
    loadingQuestion: boolean;
    leaderboard?: Game[];
}

// -----------------
// ACTIONS - These are serializable (hence replayable) descriptions of state transitions.
// They do not themselves have any side-effects; they just describe something that is going to happen.

interface SetRunningGameAction {
    type: 'SET_RUNNING_GAME';
    game: RunningGame | undefined;
}

interface SetLoadingQuestionAction {
    type: 'SET_LOADING_QUESTION';
    loading: boolean;
}

interface SetCategoriesAction {
    type: 'SET_CATEGORIES';
    categories: Category[];
}

interface SetAnsweringAction {
    type: 'SET_ANSWERING';
    answering: boolean;
}

interface ResetAction {
    type: 'RESET';
}

interface SetLeaderboardAction {
    type: 'SET_LEADERBOARD';
    leaderboard: Game[];
}

// Declare a 'discriminated union' type. This guarantees that all references to 'type' properties contain one of the
// declared type strings (and not any other arbitrary string).

type KnownAction =
    SetRunningGameAction
    | SetLoadingQuestionAction
    | SetCategoriesAction
    | SetAnsweringAction
    | ResetAction
    | SetLeaderboardAction;

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
                dispatch({
                    type: 'SET_RUNNING_GAME',
                    game: {
                        answerCorrect: undefined,
                        askedQuestions: [],
                        currentQuestion: undefined,
                        result: undefined,
                        usedJoker: false
                    }
                });
            })
            .catch(error => {
                console.error(error);
            });
    },
    fetchQuestion: (token: string): AppThunkAction<KnownAction> => (dispatch, getState) => {
        dispatch({type: 'SET_LOADING_QUESTION', loading: true});
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
                const runningGame = getState().gameState.runningGame!!;
                if (runningGame.currentQuestion) runningGame.askedQuestions.push(runningGame.currentQuestion);
                runningGame.currentQuestion = data;
                runningGame.answerCorrect = false;
                dispatch({type: 'SET_RUNNING_GAME', game: runningGame});
            })
            .catch(error => {
                console.error(error);
            });
    },
    useJoker: (token: string): AppThunkAction<KnownAction> => (dispatch, getState) => {
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
                const runningGame = getState().gameState.runningGame!!;
                runningGame.currentQuestion = data;
                runningGame.usedJoker = true;
                dispatch({type: 'SET_RUNNING_GAME', game: runningGame});
            })
            .catch(error => {
                console.error(error);
            });
    },
    fetchCategories: (): AppThunkAction<KnownAction> => (dispatch) => {
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
                dispatch({type: 'SET_CATEGORIES', categories: data});
            })
            .catch(error => {
                console.error(error);
            });
    },
    answerQuestion: (token: string, specification: AnswerSpecification): AppThunkAction<KnownAction> => (dispatch, getState) => {
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
                    const runningGame = getState().gameState.runningGame!!;
                    runningGame.answerCorrect = false;
                    dispatch({type: 'SET_RUNNING_GAME', game: runningGame});

                    return response.text().then(text => {
                        console.error(text);
                        return (response.json() as Promise<ErrorResponse>).then(error => {
                            throw new Error(error.title);
                        });
                    });
                }
                return response.json() as Promise<AnswerResult | QuizResult>;
            })
            .then(data => {
                const runningGame = getState().gameState.runningGame!!;
                switch (data.type) {
                    case "ANSWER_RESULT":
                        runningGame.answerCorrect = data.correct;
                        break;
                    case "QUIZ_RESULT":
                        runningGame.result = data;
                        break;
                }
                dispatch({type: 'SET_RUNNING_GAME', game: runningGame});
            })
            .catch(error => {
                console.error(error);
            });
    },
    reset: (): AppThunkAction<KnownAction> => (dispatch) => {
        dispatch({type: 'RESET'});
    },
    fetchLeaderboard: (): AppThunkAction<KnownAction> => (dispatch) => {
        fetch('api/games/leaderboard', {
            headers: {
                "Accept": "application/json, text/plain, */*",
                "Content-type": "application/json"
            },
        })
            .then(response => {
                if (!response.ok) {
                    return (response.json() as Promise<ErrorResponse>).then(error => {
                        throw new Error(error.title);
                    });
                }
                return response.json() as Promise<Game[]>;
            })
            .then(data => {
                dispatch({type: 'SET_LEADERBOARD', leaderboard: data});
            })
            .catch(error => {
                console.error(error);
            });
    },
    sortLeaderboard: (sort: LeaderboardSort): AppThunkAction<KnownAction> => (dispatch, getState) => {
        let leaderboard = Object.assign([], getState().gameState.leaderboard);

        switch (sort) {
            case LeaderboardSort.PointsAscending:
                leaderboard.sortBy('points', true);
                break;
            case LeaderboardSort.PointsDescending:
                leaderboard.sortBy('points', false);
                break;
            case LeaderboardSort.RankAscending:
                leaderboard.sortBy('rank', true);
                break;
            case LeaderboardSort.RankDescending:
                leaderboard.sortBy('rank', false);
                break;
            case LeaderboardSort.UsernameAscending:
                leaderboard.sortBy('username', true);
                break;
            case LeaderboardSort.UsernameDescending:
                leaderboard.sortBy('username', false);
                break;
            case LeaderboardSort.WeightedPointsAscending:
                leaderboard.sortBy('weightedPoints', true);
                break;
            case LeaderboardSort.WeightedPointsDescending:
                leaderboard.sortBy('weightedPoints', false);
                break;
            case LeaderboardSort.GameTimeSortAscending:
                leaderboard.sortBy('duration', true);
                break;
            case LeaderboardSort.GameTimeSortDescending:
                leaderboard.sortBy('duration', false);
                break;
        }

        dispatch({type: 'SET_LEADERBOARD', leaderboard: leaderboard});
    }
};

// ----------------
// REDUCER - For a given state and action, returns the new state. To support time travel, this must not mutate the old state.

const unloadedState: GameState = {
    runningGame: undefined,
    categories: undefined,
    answering: false,
    loadingQuestion: false,
    leaderboard: undefined
};

export const reducer: Reducer<GameState> = (state: GameState | undefined, incomingAction: Action): GameState => {
    if (state === undefined) {
        return unloadedState;
    }

    const retVal: GameState = Object.assign({}, state);

    const action = incomingAction as KnownAction;
    switch (action.type) {
        case "SET_RUNNING_GAME":
            retVal.runningGame = action.game;
            retVal.loadingQuestion = false;
            retVal.answering = false;
            break;
        case "SET_CATEGORIES":
            retVal.categories = action.categories;
            break;
        case "SET_ANSWERING":
            retVal.answering = action.answering;
            break;
        case "SET_LOADING_QUESTION":
            retVal.loadingQuestion = action.loading;
            break;
        case "RESET":
            return unloadedState;
        case "SET_LEADERBOARD":
            retVal.leaderboard = action.leaderboard;
            break;
    }
    return retVal;
};