import * as React from 'react';
import {connect} from 'react-redux';
import {ApplicationState} from "../../store";
import * as UserStateStore from "../../store/Users";
import * as GameStateStore from "../../store/Games";
import {RouteComponentProps} from "react-router";
import Question from "./Question";
import {bindActionCreators} from "redux";
import {Category, QuizQuestion} from "../../store/Games";
import CategorySelection from "./CategorySelection";
import {Alert, Button} from "react-bootstrap";
import {AnswerSpecification} from "../../store/Specification";

interface QuizProps {
    users: UserStateStore.UserState;
    userActions: typeof UserStateStore.actionCreators;
    games: GameStateStore.GameState;
    gameActions: typeof GameStateStore.actionCreators;
}

interface QuizState {
    secondsElapsed: number;
}

class Quiz extends React.Component<QuizProps & RouteComponentProps, QuizState> {
    constructor(props: QuizProps & RouteComponentProps) {
        super(props);
        this.state = {secondsElapsed: 0};

        this.selectCategories = this.selectCategories.bind(this);
        this.answerQuestion = this.answerQuestion.bind(this);
        this.addSeconds = this.addSeconds.bind(this);
        this.useJoker = this.useJoker.bind(this);
    }

    public componentDidMount() {
        if (!this.props.users.token) {
            this.props.history.push("/");
        }

        this.props.gameActions.reset();
        this.props.gameActions.fetchCategories(); // not checking if categories already fetched to call componentDidUpdate after
    }

    public componentDidUpdate(prevProps: Readonly<QuizProps>, prevState: Readonly<QuizState>, snapshot?: any) {
        const token = this.props.users.token!!;
        const answering = this.props.games.answering;
        const answerCorrect = this.props.games.answerCorrect;
        const loadingQuestion = this.props.games.loadingQuestion;
        const currentQuestion = this.props.games.currentQuestion;
        const gameStarted = this.props.games.gameStarted;

        if (!answering && !answerCorrect && currentQuestion) {
            // TODO: End game
            console.log("Lose!");
        } else if (!answering && answerCorrect && !loadingQuestion) {
            this.props.gameActions.fetchQuestion(token);
        } else if (!currentQuestion && gameStarted) {
            this.props.gameActions.fetchQuestion(token);
        }
    }

    private addSeconds() {
        this.setState({secondsElapsed: this.state.secondsElapsed + 1});
    }

    private useJoker() {
        if (!this.props.games.usedJoker) {
            const token = this.props.users.token!!;
            this.props.gameActions.useJoker(token);
        }
    }

    private selectCategories(categories: number[]) {
        const token = this.props.users.token!!;
        this.props.gameActions.startGame(token, {categories: categories});
        setInterval(this.addSeconds, 1000);
    }

    private answerQuestion(specification: AnswerSpecification) {
        const token = this.props.users.token!!;
        this.props.gameActions.answerQuestion(token, specification);
    }

    public render() {
        const loading = this.props.games.answering || this.props.games.loadingQuestion;
        const gameStarted = this.props.games.gameStarted;

        const categories = this.props.games.categories;
        const question = this.props.games.currentQuestion;
        const answerCorrect = this.props.games.answerCorrect;

        return (
            <div>
                {gameStarted ? <p>{this.state.secondsElapsed}</p> : null}
                <br/>
                {loading && gameStarted ? <p>Loading...</p> : gameStarted && question ?
                    <div>
                        <div style={{display: 'flex', flexDirection: 'row-reverse'}}>
                            <Button variant="dark" onClick={() => this.useJoker()} style={{float: 'right'}}
                                    disabled={this.props.games.usedJoker}>Use Joker</Button>
                        </div>
                        <br/>
                        <Question question={question} answerQuestion={this.answerQuestion}/>
                        <br/>
                        {answerCorrect ? <Alert variant='success'>Correct!</Alert> : answerCorrect === false ?
                            <Alert variant='danger'>Wrong!</Alert> : null}
                    </div> : categories ?
                        <CategorySelection categories={categories} selectCategories={this.selectCategories}/> : null}
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