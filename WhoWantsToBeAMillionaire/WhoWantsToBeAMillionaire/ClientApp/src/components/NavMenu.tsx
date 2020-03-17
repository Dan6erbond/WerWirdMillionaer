import * as React from 'react';
import {connect} from 'react-redux';
import {ApplicationState} from "../store";
import * as UserStateStore from "../store/Users";
import * as GameStateStore from "../store/Games";
import {RouteComponentProps} from "react-router";
import {Nav, Navbar} from "react-bootstrap";

import './NavMenu.css';
import {Link, NavLink} from "react-router-dom";
import {bindActionCreators} from "redux";

interface NavMenuProps {
    users: UserStateStore.UserState;
    userActions: typeof UserStateStore.actionCreators;
    games: GameStateStore.GameState;
    gameActions: typeof GameStateStore.actionCreators;
}

class NavMenu extends React.Component<NavMenuProps & RouteComponentProps> {
    constructor(props: NavMenuProps & RouteComponentProps) {
        super(props);

        this.signOut = this.signOut.bind(this);
    }

    public render() {
        return (
            <header>
                <Navbar collapseOnSelect expand="lg" bg="dark" variant="dark">
                    <Link to="/"><Navbar.Brand>Who wants to be a Millionaire?</Navbar.Brand></Link>
                    <Navbar.Toggle aria-controls="responsive-navbar-nav"/>
                    <Navbar.Collapse id="responsive-navbar-nav">
                        <Nav className="justify-content-end" activeKey="/home">
                            {!this.props.users.token ?
                                <Nav.Item>
                                    <NavLink to="/" className="nav-link">Home</NavLink>
                                </Nav.Item> : null}
                            {this.props.users.token ?
                                <Nav.Item>
                                    <NavLink to="/quiz" className="nav-link">Quiz</NavLink>
                                </Nav.Item> : null}
                            {this.props.users.token ?
                                <Nav.Item>
                                    <NavLink to="/games" className="nav-link">My Games</NavLink>
                                </Nav.Item> : null}
                            <Nav.Item>
                                <NavLink to="/leaderboard" className="nav-link">Leaderboard</NavLink>
                            </Nav.Item>
                            {this.props.users.token && this.props.users.userData && this.props.users.userData.isAdmin ?
                                <Nav.Item>
                                    <NavLink to="/admin" className="nav-link">Admin</NavLink>
                                </Nav.Item> : null}
                            {this.props.users.token ?
                                <Nav.Item>
                                    <Nav.Link onClick={this.signOut}>Sign out</Nav.Link>
                                </Nav.Item> : null}
                        </Nav>
                    </Navbar.Collapse>
                </Navbar>
            </header>
        );
    }

    private signOut() {
        this.props.userActions.signOut();
        this.props.gameActions.reset();
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
)(NavMenu as any);
