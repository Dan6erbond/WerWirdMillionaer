import * as React from 'react';
import {connect} from 'react-redux';
import {ApplicationState} from "../../store";
import * as UserStateStore from "../../store/Users";
import * as GameStateStore from "../../store/Games";
import {RouteComponentProps} from "react-router";
import Question from "./Question";
import {bindActionCreators} from "redux";
import CategorySelection from "./CategorySelection";
import {Button} from "react-bootstrap";
import {AnswerSpecification} from "../../store/Specification";
import QuizEnd from "./QuizEnd";

interface QuizProps {
    users: UserStateStore.UserState;
    userActions: typeof UserStateStore.actionCreators;
    games: GameStateStore.GameState;
    gameActions: typeof GameStateStore.actionCreators;
}

interface QuizState {
    secondsElapsed: number;
    counterInterval?: ReturnType<typeof setInterval>;
}

class Quiz extends React.Component<QuizProps & RouteComponentProps, QuizState> {
    constructor(props: QuizProps & RouteComponentProps) {
        super(props);
        this.state = {secondsElapsed: 0, counterInterval: undefined};

        this.startGame = this.startGame.bind(this);
        this.answerQuestion = this.answerQuestion.bind(this);
        this.addSeconds = this.addSeconds.bind(this);
        this.useJoker = this.useJoker.bind(this);
        this.ensureDataFetched = this.ensureDataFetched.bind(this);
        this.playAgain = this.playAgain.bind(this);
        this.doForwards = this.doForwards.bind(this);
        this.endGame = this.endGame.bind(this);
    }

    public componentDidMount() {
        this.doForwards();
        this.ensureDataFetched();
    }

    public componentDidUpdate(prevProps: Readonly<QuizProps>, prevState: Readonly<QuizState>, snapshot?: any) {
        this.doForwards();
        this.ensureDataFetched();
    }

    private doForwards() {
        if (!this.props.users.token) {
            this.props.history.push("/");
        }
    }
    
    private endGame() {
        const token = this.props.users.token!!;
        this.props.gameActions.endGame(token);
    }

    private ensureDataFetched() {
        const counterInterval = this.state.counterInterval;
        const token = this.props.users.token!!;
        const loading = this.props.games.answering || this.props.games.loadingQuestion;

        if (!this.props.games.categories) {
            this.props.gameActions.fetchCategories();
        }

        const runningGame = this.props.games.runningGame;
        if (runningGame) {
            const answerCorrect = runningGame.answerCorrect;
            const quizResult = runningGame.result;
            const currentQuestion = runningGame.currentQuestion;

            if (quizResult && counterInterval) {
                if (counterInterval) clearTimeout(counterInterval);
                this.setState({secondsElapsed: 0, counterInterval: undefined});
            } else if (!loading && (!currentQuestion || answerCorrect)) {
                this.props.gameActions.fetchQuestion(token);
            }
        }
    }

    private addSeconds() {
        this.setState({secondsElapsed: this.state.secondsElapsed + 1});
    }

    private useJoker() {
        if (this.props.games.runningGame && !this.props.games.runningGame.usedJoker) {
            const token = this.props.users.token!!;
            this.props.gameActions.useJoker(token);
        }
    }

    private startGame(categories: number[]) {
        const token = this.props.users.token!!;
        this.props.gameActions.startGame(token, {categories: categories});
        const interval: ReturnType<typeof setInterval> = setInterval(this.addSeconds, 1000);
        this.setState({counterInterval: interval});
    }

    private playAgain() {
        this.props.gameActions.reset();
    }

    private answerQuestion(specification: AnswerSpecification) {
        const token = this.props.users.token!!;
        this.props.gameActions.answerQuestion(token, specification);
    }

    public render() {
        const loading = this.props.games.answering || this.props.games.loadingQuestion || !this.props.games.categories;
        const runningGame = this.props.games.runningGame;

        const categories = this.props.games.categories;
        const question = runningGame ? runningGame.currentQuestion : undefined;
        const quizResult = runningGame ? runningGame.result : undefined;
        if (quizResult) console.log(quizResult);
        const usedJoker = runningGame ? runningGame.usedJoker : undefined;

        return (
            <div>
                {quizResult ?
                    <div>
                        <br/>
                        <QuizEnd result={quizResult} playAgain={this.playAgain}/>
                    </div> : loading && runningGame ? <p>Loading...</p> : runningGame && question ?
                        <div>
                            {runningGame ? <p>{this.state.secondsElapsed}</p> : null}
                            <br/>
                            <div style={{display: 'flex', flexDirection: 'row-reverse'}}>
                                <Button variant="dark" onClick={() => this.useJoker()} style={{float: 'right'}}
                                        disabled={usedJoker}>Use Joker</Button>
                            </div>
                            <br/>
                            <Question question={question} answerQuestion={this.answerQuestion}/>
                            <br/>
                            <Button variant="outline-primary" onClick={this.endGame}>End Game</Button>
                        </div> : categories ?
                            <CategorySelection categories={categories}
                                               play={this.startGame}/> : null}
            </div>
        );
    }
}

export default connect(
    (state: ApplicationState) => ({
        users: state.userState,
        games: state.gameState,
    }),
    (dispatch) => {
        return {
            userActions: bindActionCreators(UserStateStore.actionCreators, dispatch),
            gameActions: bindActionCreators(GameStateStore.actionCreators, dispatch)
        };
    }
)(Quiz as any);