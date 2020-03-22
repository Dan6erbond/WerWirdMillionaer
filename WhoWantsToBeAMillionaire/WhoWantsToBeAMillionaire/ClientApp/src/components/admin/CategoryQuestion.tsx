import * as React from "react";
import {QuizQuestion} from "../../store/Games";
import {Accordion, Button, Card, Col, Form, Row, Spinner, Table} from "react-bootstrap";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";

interface CategoryQuestionProps {
    question: QuizQuestion;
    index: number;
    setQuestion: (index: number, question: QuizQuestion) => void;
    editQuestion: (index: number, question: QuizQuestion, setSavingDone: () => void) => void;
    addQuestion: (index: number, question: QuizQuestion, setSavingDone: () => void) => void;
    deleteQuestion: (index: number) => void;
    setQuestionActiveKey: (index: number) => void;
}

interface CategoryQuestionState {
    saving: boolean;
    deleting: boolean;
}

export default class CategoryQuestion extends React.Component<CategoryQuestionProps, CategoryQuestionState> {
    constructor(props: CategoryQuestionProps) {
        super(props);
        this.state = {saving: false, deleting: false};

        this.selectCorrect = this.selectCorrect.bind(this);
        this.setAnswer = this.setAnswer.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
        this.handleDelete = this.handleDelete.bind(this);
        this.setQuestion = this.setQuestion.bind(this);
        this.setSavingDone = this.setSavingDone.bind(this);
    }

    public render() {
        const question = this.props.question;
        
        return (
            <Card>
                <Card.Header>
                    <Accordion.Toggle as={Button} variant="link" eventKey={`${this.props.index}`}
                                      onClick={() => this.props.setQuestionActiveKey(this.props.index)}>
                        {question.question}
                    </Accordion.Toggle>
                    <Button variant="link" style={{float: 'right'}} onClick={() => this.handleDelete()} disabled={this.state.deleting}>
                        {this.state.deleting ? <Spinner animation="border" role="status" size="sm">
                            <span className="sr-only">Loading...</span>
                        </Spinner> : <FontAwesomeIcon icon="trash"/>}
                    </Button>
                </Card.Header>
                <Accordion.Collapse eventKey={`${this.props.index}`}>
                    <Card.Body>
                        <Form style={{width: '100%'}} onSubmit={this.handleSubmit}>
                            <Form.Group as={Row} controlId="question">
                                <Form.Label column sm="2">
                                    Question
                                </Form.Label>
                                <Col sm="10">
                                    <Form.Control as="textarea" rows="2" defaultValue={question.question}
                                                  onChange={(event: React.FormEvent<HTMLInputElement>) => this.setQuestion((event.currentTarget as any).value)}/>
                                </Col>
                            </Form.Group>

                            <Form.Group as={Row} controlId="answers" style={{padding: '15px'}}>
                                <Form.Label>
                                    Answers
                                </Form.Label>
                                <Table bordered>
                                    <thead>
                                    <tr>
                                        <th>#</th>
                                        <th>Answer</th>
                                        <th>Correct</th>
                                    </tr>
                                    </thead>
                                    <tbody>
                                    {question.answers.map((a, i) => <tr key={i}>
                                        <td>{i + 1}</td>
                                        <td>
                                            <Form.Control as="textarea" rows="2" defaultValue={a.answer}
                                                          onChange={(event: React.FormEvent<HTMLInputElement>) => this.setAnswer(i, (event.currentTarget as any).value)}/>
                                        </td>
                                        <td>
                                            <Form.Check custom type="radio" label="" checked={a.correct}
                                                        id={`radio-${i}`} onChange={() => this.selectCorrect(i)}/>
                                        </td>
                                    </tr>)}
                                    </tbody>
                                </Table>
                            </Form.Group>

                            <Button variant="primary" type="submit" disabled={this.state.saving}>
                                {this.state.saving ? "Saving changes..." : question.questionId ? "Save" : "Add"}
                            </Button>
                        </Form>
                    </Card.Body>
                </Accordion.Collapse>
            </Card>
        )
    }

    private handleDelete() {
        this.setState({deleting: true});
        this.props.deleteQuestion(this.props.index);
    }

    private setSavingDone() {
        this.setState({saving: false});
    }

    private handleSubmit(e: React.FormEvent<HTMLFormElement>) {
        e.preventDefault();

        this.setState({saving: true});
        
        const question = this.props.question;

        if (question.questionId) {
            this.props.editQuestion(this.props.index, question, this.setSavingDone);
        } else {
            this.props.addQuestion(this.props.index, question, this.setSavingDone);
        }
    }

    private selectCorrect(index: number) {
        const question = this.props.question;

        if (!question.answers[index].correct) {
            question.answers[index].correct = true;

            for (let i = 0; i < question.answers.length; i++) {
                if (i === index) {
                    continue;
                }
                question.answers[i].correct = false;
            }
        }

        this.props.setQuestion(this.props.index, question);
    }

    private setQuestion(q: string) {
        const question = this.props.question;
        question.question = q;
        this.props.setQuestion(this.props.index, question);
    }

    private setAnswer(index: number, answer: string) {
        const question = this.props.question;
        question.answers[index].answer = answer;
        this.props.setQuestion(this.props.index, question);
    }
}