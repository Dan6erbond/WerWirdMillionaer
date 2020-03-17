import * as React from "react";
import {Category, QuizQuestion} from "../../store/Games";
import {Accordion, Button} from "react-bootstrap";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {ErrorResponse} from "../../store/ApiResponse";
import CategoryQuestion from "./CategoryQuestion";

interface CategoryQuestionsProps {
    category: Category;
    token: string | undefined;
}

interface CategoryQuestionsState {
    questions?: QuizQuestion[];
    questionActiveKey: number;
}

export default class CategoryQuestions extends React.Component<CategoryQuestionsProps, CategoryQuestionsState> {
    constructor(props: CategoryQuestionsProps) {
        super(props);
        this.state = {questions: undefined, questionActiveKey: 0};

        this.ensureDataFetched = this.ensureDataFetched.bind(this);
        this.editQuestion = this.editQuestion.bind(this);
        this.deleteQuestion = this.deleteQuestion.bind(this);
        this.addQuestion = this.addQuestion.bind(this);
        this.addQuestionUi = this.addQuestionUi.bind(this);
        this.setQuestionActiveKey = this.setQuestionActiveKey.bind(this);
    }

    public componentDidMount() {
        this.ensureDataFetched();
    }

    public componentDidUpdate(prevProps: Readonly<CategoryQuestionsProps>) {
        if (prevProps.category !== this.props.category) {
            this.setState({questions: undefined});
        }

        this.ensureDataFetched();
    }

    private ensureDataFetched() {
        if (!this.state.questions) {
            fetch(`api/games/category/${this.props.category.categoryId!!}`, {
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
                    return response.json() as Promise<QuizQuestion[]>;
                })
                .then(data => {
                    this.setState({questions: data});
                })
                .catch(error => {
                    console.error(error);
                });
        }
    }

    public render() {
        return (
            <div>
                <Accordion activeKey={`${this.state.questionActiveKey}`}>
                    {this.state.questions ? this.state.questions.map((q, i) =>
                        <CategoryQuestion question={q} index={i} key={i} editQuestion={this.editQuestion}
                                          addQuestion={this.addQuestion} deleteQuestion={this.deleteQuestion}
                                          setQuestionActiveKey={this.setQuestionActiveKey}/>) : null}
                </Accordion>

                <br/>

                <Button variant="secondary" style={{float: 'right'}} onClick={this.addQuestionUi}>
                    <FontAwesomeIcon icon="plus"/> Add Question
                </Button>
            </div>
        )
    }

    private setQuestionActiveKey(key: number) {
        this.setState({questionActiveKey: key === this.state.questionActiveKey ? -1 : key});
    }

    private addQuestionUi() {
        if (!this.state.questions) {
            return;
        }

        const questions = this.state.questions;
        questions.push({
            questionId: undefined,
            categoryId: this.props.category.categoryId!!,
            question: "",
            timesAsked: 0,
            correctlyAnswered: 0,
            uses: 0,
            answers: [{
                answerId: undefined,
                answer: "",
                correct: true
            }, {
                answerId: undefined,
                answer: "",
                correct: false
            }, {
                answerId: undefined,
                answer: "",
                correct: false
            }, {
                answerId: undefined,
                answer: "",
                correct: false
            }]
        });

        this.setState({questions: questions, questionActiveKey: questions.length - 1});
    }

    private deleteQuestion(index: number) {
        if (!this.state.questions) {
            return;
        }

        const questions = this.state.questions;

        if (this.props.token) {
            fetch(`api/admin/questions/delete/${questions[index].questionId}`, {
                headers: {
                    "Accept": "application/json, text/plain, */*",
                    "Content-type": "application/json",
                    "Authorization": `Bearer ${this.props.token}`
                },
            })
                .then(response => {
                    if (!response.ok) {
                        return (response.json() as Promise<ErrorResponse>).then(error => {
                            throw new Error(error.title);
                        });
                    } else {
                        questions.splice(index, 1);
                        this.setState({questions: questions});
                    }
                })
                .catch(error => {
                    console.error(error);
                });
        }
    }

    private editQuestion(index: number, question: QuizQuestion, setSavingDone: () => void) {
        if (!this.state.questions) {
            return;
        }

        const questions = this.state.questions;

        if (this.props.token) {
            fetch(`api/admin/questions/edit`, {
                method: 'POST',
                body: JSON.stringify(question),
                headers: {
                    "Accept": "application/json, text/plain, */*",
                    "Content-type": "application/json",
                    "Authorization": `Bearer ${this.props.token}`
                },
            })
                .then(response => {
                    if (!response.ok) {
                        return (response.json() as Promise<ErrorResponse>).then(error => {
                            throw new Error(error.title);
                        });
                    } else {
                        questions[index] = question;
                        this.setState({questions: questions});
                        setSavingDone();
                    }
                })
                .catch(error => {
                    console.error(error);
                });
        }
    }

    private addQuestion(index: number, question: QuizQuestion, setSavingDone: () => void) {
        if (!this.state.questions) {
            return;
        }

        const questions = this.state.questions;

        if (this.props.token) {
            fetch(`api/admin/questions/add`, {
                method: 'POST',
                body: JSON.stringify(question),
                headers: {
                    "Accept": "application/json, text/plain, */*",
                    "Content-type": "application/json",
                    "Authorization": `Bearer ${this.props.token}`
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
                    questions[index] = data;
                    this.setState({questions: questions});
                    setSavingDone();
                })
                .catch(error => {
                    console.error(error);
                });
        }
    }
}