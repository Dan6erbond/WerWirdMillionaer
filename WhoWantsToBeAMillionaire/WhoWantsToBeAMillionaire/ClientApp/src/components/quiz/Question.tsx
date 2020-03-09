import * as React from 'react';
import {QuizAnswer, QuizQuestion} from "../../store/Games";
import {Button, Col, Container, Row} from "react-bootstrap";
import {AnswerSpecification} from "../../store/Specification";

interface QuestionProps {
    question: QuizQuestion;
    answerQuestion: (specification: AnswerSpecification) => void;
}

export default class Question extends React.Component<QuestionProps> {
    constructor(props: QuestionProps) {
        super(props);

        this.answerQuestion = this.answerQuestion.bind(this);
        this.answerQuestionZero = this.answerQuestionZero.bind(this);
        this.answerQuestionOne = this.answerQuestionOne.bind(this);
        this.answerQuestionTwo = this.answerQuestionTwo.bind(this);
        this.answerQuestionThree = this.answerQuestionThree.bind(this);
    }

    private answerQuestion(answer: QuizAnswer) {
        this.props.answerQuestion({
            questionId: this.props.question.questionId,
            answerId: answer.answerId
        });
    }

    private answerQuestionZero() {
        this.answerQuestion(this.props.question.answers[0]);
    }

    private answerQuestionOne() {
        this.answerQuestion(this.props.question.answers[1]);
    }

    private answerQuestionTwo() {
        this.answerQuestion(this.props.question.answers[2]);
    }

    private answerQuestionThree() {
        this.answerQuestion(this.props.question.answers[3]);
    }

    public render() {
        return (
            <div>
                <h1 className="display-4">{this.props.question.question}</h1>
                <Container>
                    <Row>
                        <Col>
                            <Button variant="primary" size="lg" block
                                    onClick={this.answerQuestionZero}>
                                {this.props.question.answers[0].answer}
                            </Button>
                        </Col>
                        <Col>
                            <Button variant="primary" size="lg" block
                                    onClick={this.answerQuestionOne}>
                                {this.props.question.answers[1].answer}
                            </Button>
                        </Col>
                    </Row>
                    <br/>
                    <Row>
                        <Col>
                            <Button variant="primary" size="lg" block
                                    onClick={this.answerQuestionTwo}>
                                {this.props.question.answers[2].answer}
                            </Button>
                        </Col>
                        <Col>
                            <Button variant="primary" size="lg" block
                                    onClick={this.answerQuestionThree}>
                                {this.props.question.answers[3].answer}
                            </Button>
                        </Col>
                    </Row>
                </Container>
            </div>
        );
    }
}