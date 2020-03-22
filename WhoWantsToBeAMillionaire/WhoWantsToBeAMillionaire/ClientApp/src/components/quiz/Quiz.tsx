import * as React from 'react';
import {connect} from 'react-redux';
import {ApplicationState} from "../../store";
import * as UserStateStore from "../../store/Users";
import * as GameStateStore from "../../store/Games";
import {RouteComponentProps} from "react-router";
import Question from "./Question";
import {bindActionCreators} from "redux";
import CategorySelection from "./CategorySelection";
import {Alert, Button} from "react-bootstrap";
import {AnswerSpecification} from "../../store/Specification";
import QuizEnd from "./QuizEnd";

interface QuizProps {
    users: UserStateStore.UserState;
    userActions: typeof UserStateStore.actionCreators;
    games: GameStateStore.GameState;
    gameActions: typeof GameStateStore.actionCreators;
}

interface QuizState {
    counterInterval?: ReturnType<typeof setInterval>;
    displayCorrect: boolean;
    displayedCorrect: boolean;
    answer?: string;
}

class Quiz extends React.Component<QuizProps & RouteComponentProps, QuizState> {
    constructor(props: QuizProps & RouteComponentProps) {
        super(props);
        this.state = {
            counterInterval: undefined,
            displayCorrect: false,
            displayedCorrect: false,
            answer: undefined
        };

        this.startGame = this.startGame.bind(this);
        this.answerQuestion = this.answerQuestion.bind(this);
        this.checkTime = this.checkTime.bind(this);
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

    public componentDidUpdate() {
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
        const ending = this.props.games.ending;

        if (!this.props.games.categories) {
            this.props.gameActions.fetchCategories();
        }

        const runningGame = this.props.games.runningGame;
        if (runningGame) {
            const quizResult = runningGame.result;
            const currentQuestion = runningGame.currentQuestion;
            const answerCorrect = runningGame.answerCorrect;

            if (quizResult && counterInterval) {
                if (counterInterval) clearTimeout(counterInterval);
                this.setState({counterInterval: undefined});
            } else if (runningGame.questionsOver && !ending && !quizResult) {
                this.props.gameActions.endGame(token);
            } else if (!quizResult && !loading && !ending && (!currentQuestion || answerCorrect)) {
                this.setState({displayedCorrect: false});
                this.props.gameActions.fetchQuestion(token);
            }

            if (answerCorrect && !this.state.displayedCorrect) {
                this.setState({displayedCorrect: true, displayCorrect: true});
                setTimeout(() => this.setState({displayCorrect: false}), 1000);
            }
        }
    }

    private checkTime() {
        const token = this.props.users.token!!;
        this.props.gameActions.checkTime(token);
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
        const interval: ReturnType<typeof setInterval> = setInterval(this.checkTime, 1000);
        this.setState({counterInterval: interval});
    }

    private playAgain() {
        this.props.gameActions.reset();
    }

    private answerQuestion(specification: AnswerSpecification, answer: string) {
        this.setState({answer: answer});
        const token = this.props.users.token!!;
        this.props.gameActions.answerQuestion(token, specification);
    }

    public render() {
        const loading = this.props.games.answering || this.props.games.loadingQuestion || !this.props.games.categories || this.props.games.ending;
        const runningGame = this.props.games.runningGame;

        const categories = this.props.games.categories;
        const question = runningGame ? runningGame.currentQuestion : undefined;
        const quizResult = runningGame ? runningGame.result : undefined;
        const usedJoker = runningGame ? runningGame.usedJoker : undefined;
        
        return (
            <div>
                {quizResult && runningGame ?
                    <div>
                        <QuizEnd result={quizResult} playAgain={this.playAgain}
                                 questionsOver={runningGame.questionsOver} answer={this.state.answer}/>
                    </div> : loading && runningGame ? <p>Loading...</p> : runningGame && question && categories ?
                        <div>
                            <p>
                                <b>Time elapsed:</b> {runningGame.quizTime.toTimeString()}
                                <br/>
                                <b>Points:</b> {runningGame.points}
                            </p>
                            <br/>
                            <div style={{display: 'flex', flexDirection: 'row-reverse'}}>
                                <Button variant="dark" onClick={() => this.useJoker()} style={{float: 'right'}}
                                        disabled={usedJoker}>Use Joker</Button>
                            </div>
                            <br/>
                            <Question question={question} answerQuestion={this.answerQuestion} categories={categories} questionTime={runningGame.questionTime}/>
                            {this.state.displayCorrect ? <div>
                                <br/>
                                <br/>
                                <Alert variant="success">
                                    Correct answer!
                                </Alert>
                            </div> : null}
                            <br/>
                            <br/>
                            <div style={{textAlign: 'center'}}><Button variant="outline-primary" size="lg" onClick={this.endGame}>End Game</Button></div>
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