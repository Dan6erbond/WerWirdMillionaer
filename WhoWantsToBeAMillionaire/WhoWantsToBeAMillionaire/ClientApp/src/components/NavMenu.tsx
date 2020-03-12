import * as React from 'react';
import {connect} from 'react-redux';
import {ApplicationState} from "../store";
import * as UserStateStore from "../store/Users";
import {RouteComponentProps} from "react-router";
import {Nav, Navbar} from "react-bootstrap";

import './NavMenu.css';
import {SyntheticEvent} from "react";
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
                            <Nav.Item>
                                <NavLink to="/" className="nav-link">Home</NavLink>
                            </Nav.Item>
                            {this.props.token ?
                                <Nav.Item>
                                    <Nav.Link onSelect={this.signOut}>Sign out</Nav.Link>
                                </Nav.Item> : null}
                        </Nav>
                    </Navbar.Collapse>
                </Navbar>
            </header>
        );
    }

    private signOut() {
        console.log("click");
    }
}

export default connect(
    (state: ApplicationState) => state.userState,
    UserStateStore.actionCreators
)(NavMenu as any);
