import * as React from 'react';
import {connect} from 'react-redux';
import {ApplicationState} from "../../store";
import * as UserStateStore from "../../store/Users";
import * as GameStateStore from "../../store/Games";
import {RouteComponentProps} from "react-router";
import {bindActionCreators} from "redux";
import {
    Button,
    ButtonGroup, Col,
    Dropdown,
    DropdownButton,
    Form, Overlay,
    Popover,
    Row,
    Spinner
} from "react-bootstrap";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {Category} from "../../store/Games";
import CategoryQuestions from "./CategoryQuestions";
import {ErrorResponse} from "../../store/ApiResponse";
import {createRef, ReactDOM} from "react";

interface AdminProps {
    users: UserStateStore.UserState;
    userActions: typeof UserStateStore.actionCreators;
    games: GameStateStore.GameState;
    gameActions: typeof GameStateStore.actionCreators;
}

interface AdminState {
    selectedCategory: number;
    adding: boolean;
    deleting: boolean;
    newCategoryName: string;
    showPopover: boolean;
}

class Admin extends React.Component<AdminProps & RouteComponentProps, AdminState> {
    private addButton = React.createRef<HTMLButtonElement & Button>();

    constructor(props: AdminProps & RouteComponentProps) {
        super(props);

        this.state = {
            selectedCategory: 0,
            adding: false,
            deleting: false,
            newCategoryName: "",
            showPopover: false
        };

        this.ensureDataFetched = this.ensureDataFetched.bind(this);
        this.doForwards = this.doForwards.bind(this);
        this.selectCategory = this.selectCategory.bind(this);
        this.deleteCategory = this.deleteCategory.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
    }

    public componentDidMount() {
        this.doForwards();
        this.ensureDataFetched();
    }

    public componentDidUpdate(prevProps: Readonly<AdminProps>) {
        this.doForwards();
        this.ensureDataFetched();
        
        if (this.props.games.categories && prevProps.games.categories) {
            if (this.props.games.categories.length > prevProps.games.categories.length) {
                this.setState({selectedCategory: this.props.games.categories.length-1, adding: false});
            }
        }
    }

    private doForwards() {
        if (!this.props.users.token || !this.props.users.userData || !this.props.users.userData.isAdmin) {
            this.props.history.push("/");
        }
    }

    private ensureDataFetched() {
        if (!this.props.games.categories) {
            this.props.gameActions.fetchCategories();
        }
    }

    public render() {
        const categories = this.props.games.categories;

        return (
            <div>
                <br/>

                {categories ? <div>
                    <div>
                        <ButtonGroup aria-label="Basic example">
                            <DropdownButton as={ButtonGroup} variant="secondary"
                                            title={`${categories[this.state.selectedCategory].name} `}
                                            id="test">
                                {categories.map((c: Category, i) =>
                                    <Dropdown.Item eventKey={`${i}`} onSelect={this.selectCategory}
                                                   key={c.categoryId!!}>{c.name}</Dropdown.Item>)}
                            </DropdownButton>

                            <Button variant="secondary" disabled={this.state.adding}
                                    onClick={() => this.setState({showPopover: !this.state.showPopover})}
                                    ref={this.addButton}>
                                {this.state.adding ? <Spinner animation="border" role="status" size="sm">
                                    <span className="sr-only">Loading...</span>
                                </Spinner> : <FontAwesomeIcon icon="plus"/>}
                            </Button>

                            {this.addButton.current ? <Overlay placement="right" show={this.state.showPopover}
                                                               target={this.addButton.current}>
                                <Popover id="popover-basic">
                                    <Popover.Title as="h3">Add category</Popover.Title>
                                    <Popover.Content>
                                        <Form onSubmit={this.handleSubmit}>
                                            <Form.Group as={Row} controlId="category">
                                                <Form.Label column sm="4">
                                                    Category name
                                                </Form.Label>
                                                <Col sm="8">
                                                    <Form.Control style={{width: '100%'}} placeholder="Category name"
                                                                  onChange={(event: React.FormEvent<HTMLInputElement>) => this.setState({newCategoryName: (event.currentTarget as any).value})}/>
                                                </Col>
                                            </Form.Group>

                                            <Button variant="primary" type="submit" disabled={this.state.adding}>
                                                {this.state.adding ? "Saving changes..." : "Add"}
                                            </Button>
                                        </Form>
                                    </Popover.Content>
                                </Popover>
                            </Overlay> : null}

                            <Button variant="secondary" disabled={this.state.deleting} onClick={this.deleteCategory}>
                                {this.state.deleting ? <Spinner animation="border" role="status" size="sm">
                                    <span className="sr-only">Loading...</span>
                                </Spinner> : <FontAwesomeIcon icon="trash"/>}
                            </Button>
                        </ButtonGroup>
                    </div>

                    <br/>

                    <CategoryQuestions category={categories[this.state.selectedCategory]}
                                       token={this.props.users.token}/>
                </div> : <p>Loading...</p>}
            </div>
        );
    }

    private selectCategory(eventKey: string, event: Object) {
        this.setState({selectedCategory: parseInt(eventKey)});
    }

    private deleteCategory() {
        if (!this.props.games.categories || !this.props.users.token) {
            return;
        }

        fetch(`api/admin/categories/delete/${this.props.games.categories[this.state.selectedCategory].categoryId!!}`, {
            headers: {
                "Accept": "application/json, text/plain, */*",
                "Content-type": "application/json",
                "Authorization": `Bearer ${this.props.users.token}`
            },
        })
            .then(response => {
                if (!response.ok) {
                    return (response.json() as Promise<ErrorResponse>).then(error => {
                        throw new Error(error.title);
                    });
                } else {
                    this.setState({selectedCategory: 0});
                    this.props.gameActions.fetchCategories();
                }
            })
            .catch(error => {
                console.error(error);
            });
    }

    private handleSubmit(e: React.FormEvent<HTMLFormElement>) {
        e.preventDefault();

        if (!this.props.users.token) {
            return;
        }

        this.setState({showPopover: false, adding: true});

        const category: Category = {categoryId: undefined, name: this.state.newCategoryName};

        fetch(`api/admin/categories/add`, {
            method: 'POST',
            body: JSON.stringify(category),
            headers: {
                "Accept": "application/json, text/plain, */*",
                "Content-type": "application/json",
                "Authorization": `Bearer ${this.props.users.token}`
            },
        })
            .then(response => {
                if (!response.ok) {
                    return (response.json() as Promise<ErrorResponse>).then(error => {
                        throw new Error(error.title);
                    });
                }
                return response.json() as Promise<Category>;
            })
            .then(data => {
                this.props.gameActions.fetchCategories();
            })
            .catch(error => {
                console.error(error);
            });
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
)(Admin as any);