import * as React from 'react';
import {connect} from 'react-redux';
import {ApplicationState} from "../../store";
import * as UserStateStore from "../../store/Users";
import * as GameStateStore from "../../store/Games";
import {RouteComponentProps} from "react-router";
import {bindActionCreators} from "redux";
import {Button, ButtonGroup, Dropdown, DropdownButton, Spinner} from "react-bootstrap";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {Category, QuizQuestion} from "../../store/Games";
import CategoryQuestions from "./CategoryQuestions";

interface AdminProps {
    users: UserStateStore.UserState;
    userActions: typeof UserStateStore.actionCreators;
    games: GameStateStore.GameState;
    gameActions: typeof GameStateStore.actionCreators;
}

interface AdminState {
    selectedCategory: number;
}

class Admin extends React.Component<AdminProps & RouteComponentProps, AdminState> {
    constructor(props: AdminProps & RouteComponentProps) {
        super(props);

        this.state = {selectedCategory: 0};

        this.ensureDataFetched = this.ensureDataFetched.bind(this);
        this.doForwards = this.doForwards.bind(this);
        this.selectCategory = this.selectCategory.bind(this);
    }

    public componentDidMount() {
        this.doForwards();
        this.ensureDataFetched();
    }

    public componentDidUpdate(prevProps: Readonly<AdminProps>) {
        this.doForwards();
        this.ensureDataFetched();
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
                                                   key={c.categoryId}>{c.name}</Dropdown.Item>)}
                            </DropdownButton>

                            <Button variant="secondary"><FontAwesomeIcon icon="plus"/></Button>
                            <Button variant="secondary">
                                <FontAwesomeIcon icon="trash"/>
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