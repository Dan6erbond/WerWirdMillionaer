import * as React from 'react';
import {Alert, Badge, ListGroup} from "react-bootstrap";
import {QuizAnswer, QuizQuestion, Round} from "../../store/Games";
import {ErrorResponse} from "../../store/ApiResponse";

interface GameRoundProps {
    round: Round;
}

interface GameRoundState {
    question?: QuizQuestion;
}

export default class GameRound extends React.Component<GameRoundProps, GameRoundState> {
    constructor(props: GameRoundProps) {
        super(props);
        this.state = {question: undefined};
        this.ensureDataFetched = this.ensureDataFetched.bind(this);
    }

    public componentDidMount() {
        this.ensureDataFetched();
    }

    public componentDidUpdate() {
        this.ensureDataFetched();
    }

    private ensureDataFetched() {
        if (!this.state.question) {
            fetch(`api/games/questions/${this.props.round.questionId}`, {
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
                    return response.json() as Promise<QuizQuestion>;
                })
                .then(data => {
                    this.setState({question: data});
                })
                .catch(error => {
                    console.error(error);
                });
        }
    }

    public render() {
        const round = this.props.round;
        const question = this.state.question;

        if (question) {
            const answeredAnswer = round.answerId ? question.answers.filter((a: QuizAnswer) => a.answerId === round.answerId)[0] : null;
            const correctAnswer = question.answers.filter((a: QuizAnswer) => a.correct)[0];

            return (
                <ListGroup.Item variant={round.answerId === correctAnswer.answerId ? "success" : "danger"}>
                    <h6>
                        {question.question} {round.usedJoker ?
                        <Badge variant="secondary">Joker Used</Badge> : null}
                        {round.duration > 120 ? <div>
                            <br/>
                            <Alert variant="warning">
                                Took too long to answer.
                            </Alert>
                        </div> : null}
                    </h6>
                    {answeredAnswer ?
                        <span>
                            <b>Answered:</b> {answeredAnswer.answer}
                            {answeredAnswer.answerId !== correctAnswer.answerId ? <span>
                                <br/>
                                <b>Correct answer:</b> {correctAnswer.answer}
                            </span> : null}
                        </span> : null}
                </ListGroup.Item>
            );
        } else {
            return (
                <ListGroup.Item variant="light">Loading...</ListGroup.Item>
            );
        }
    }
}