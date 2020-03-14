import * as React from 'react';
import {connect} from 'react-redux';
import {ApplicationState} from "../store";
import * as UserStateStore from "../store/Users";
import {RouteComponentProps} from "react-router";
import {Nav, Navbar} from "react-bootstrap";

import './NavMenu.css';
import {Link, NavLink} from "react-router-dom";

type NavMenuProps =
    UserStateStore.UserState
    & typeof UserStateStore.actionCreators
    & RouteComponentProps;

class NavMenu extends React.Component<NavMenuProps> {
    constructor(props: NavMenuProps) {
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
                            {!this.props.token ?
                                <Nav.Item>
                                    <NavLink to="/" className="nav-link">Home</NavLink>
                                </Nav.Item> : null}
                            {this.props.token ?
                                <Nav.Item>
                                    <NavLink to="/quiz" className="nav-link">Quiz</NavLink>
                                </Nav.Item> : null}
                            {this.props.token ?
                                <Nav.Item>
                                    <NavLink to="/games" className="nav-link">My Games</NavLink>
                                </Nav.Item> : null}
                            <Nav.Item>
                                <NavLink to="/leaderboard" className="nav-link">Leaderboard</NavLink>
                            </Nav.Item>
                            {this.props.token ?
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
        this.props.signOut();
    }
}

export default connect(
    (state: ApplicationState) => state.userState,
    UserStateStore.actionCreators
)(NavMenu as any);
