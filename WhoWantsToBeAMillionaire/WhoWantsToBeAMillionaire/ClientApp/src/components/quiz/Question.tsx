import * as React from 'react';
import {Category, QuizQuestion} from "../../store/Games";
import {Badge, Button, Col, Container, Row} from "react-bootstrap";
import {AnswerSpecification} from "../../store/Specification";

interface QuestionProps {
    question: QuizQuestion;
    questionTime: number;
    categories: Category[];
    answerQuestion: (specification: AnswerSpecification, answer: string) => void;
}

export default class Question extends React.Component<QuestionProps> {
    constructor(props: QuestionProps) {
        super(props);
        this.answerQuestion = this.answerQuestion.bind(this);
    }

    private answerQuestion(answerNr: number) {
        const answer = this.props.question.answers[answerNr];

        this.props.answerQuestion({
            questionId: this.props.question.questionId!!,
            answerId: answer.answerId!!
        }, answer.answer);
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
                       className={this.props.questionTime > 120 ? "text-danger" : this.props.questionTime > 90 ? "text-warning" : ""}>
                        {(120 - this.props.questionTime).toTimeString()} remaining
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