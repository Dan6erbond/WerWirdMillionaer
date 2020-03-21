import * as React from 'react';
import {Category, QuizQuestion} from "../../store/Games";
import {Badge, Button, Col, Container, Row} from "react-bootstrap";
import {AnswerSpecification} from "../../store/Specification";

interface QuestionProps {
    question: QuizQuestion;
    categories: Category[];
    answerQuestion: (specification: AnswerSpecification, answer: string) => void;
}

interface QuestionState {
    secondsElapsed: number;
    counterInterval?: ReturnType<typeof setInterval>;
}

export default class Question extends React.Component<QuestionProps, QuestionState> {
    constructor(props: QuestionProps) {
        super(props);
        this.state = {secondsElapsed: 0, counterInterval: undefined};

        this.answerQuestion = this.answerQuestion.bind(this);
        this.addSeconds = this.addSeconds.bind(this);
    }

    public componentDidUpdate(prevProps: Readonly<QuestionProps>) {
        if (prevProps.question != this.props.question) {
            this.setState({secondsElapsed: 0});
        }
        if (!this.state.counterInterval) {
            const interval: ReturnType<typeof setInterval> = setInterval(this.addSeconds, 1000);
            this.setState({counterInterval: interval});
        }
    }

    private answerQuestion(answerNr: number) {
        const answer = this.props.question.answers[answerNr];

        this.props.answerQuestion({
            questionId: this.props.question.questionId!!,
            answerId: answer.answerId!!
        }, answer.answer);
    }

    private addSeconds() {
        this.setState({secondsElapsed: this.state.secondsElapsed + 1});
    }

    public render() {
        const categoryName = this.props.categories.filter(c => c.categoryId === this.props.question.categoryId)[0].name;

        return (
            <div>
                <div>
                    <h1 className="display-4">
                        {this.props.question.question} <Badge variant="secondary"
                                                              style={{fontSize: '35%'}}>{categoryName}</Badge>
                    </h1>
                    <br/>
                    <p style={{textAlign: 'center'}}>{this.props.question.timesAsked === 0 ? "Never asked" :
                        `${Math.round(this.props.question.correctlyAnswered * 100 / this.props.question.timesAsked)}% correctly answered`}</p>
                    <p style={{textAlign: 'center', fontWeight: 'bold'}}
                        className={this.state.secondsElapsed > 120 ? "text-danger" : this.state.secondsElapsed > 90 ? "text-warning" : ""}>
                        {this.state.secondsElapsed.toTimeString()} elapsed for this question
                    </p>
                </div>
                <br/>
                <Container>
                    <Row>
                        <Col>
                            <Button variant="primary" size="lg" block
                                    onClick={() => this.answerQuestion(0)}
                                    disabled={this.props.question.answers[0].correct === false}>
                                {this.props.question.answers[0].answer}
                            </Button>
                        </Col>
                        <Col>
                            <Button variant="primary" size="lg" block
                                    onClick={() => this.answerQuestion(1)}
                                    disabled={this.props.question.answers[1].correct === false}>
                                {this.props.question.answers[1].answer}
                            </Button>
                        </Col>
                    </Row>
                    <br/>
                    <Row>
                        <Col>
                            <Button variant="primary" size="lg" block
                                    onClick={() => this.answerQuestion(2)}
                                    disabled={this.props.question.answers[2].correct === false}>
                                {this.props.question.answers[2].answer}
                            </Button>
                        </Col>
                        <Col>
                            <Button variant="primary" size="lg" block
                                    onClick={() => this.answerQuestion(3)}
                                    disabled={this.props.question.answers[3].correct === false}>
                                {this.props.question.answers[3].answer}
                            </Button>
                        </Col>
                    </Row>
                </Container>
            </div>
        );
    }
}