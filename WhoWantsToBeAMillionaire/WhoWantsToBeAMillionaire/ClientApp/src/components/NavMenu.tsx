import * as React from 'react';
import {Nav, Navbar} from "react-bootstrap";

import './NavMenu.css';

export default class NavMenu extends React.Component<{}> {
    public render() {
        return (
            <header>
                <Navbar collapseOnSelect expand="lg" bg="dark" variant="dark">
                    <Navbar.Brand href="/">Who wants to be a Millionaire?</Navbar.Brand>
                    <Navbar.Toggle aria-controls="responsive-navbar-nav" />
                    <Navbar.Collapse id="responsive-navbar-nav">
                        <Nav className="justify-content-end" activeKey="/home">
                            <Nav.Item>
                                <Nav.Link href="/">Home</Nav.Link>
                            </Nav.Item>
                        </Nav>
                    </Navbar.Collapse>
                </Navbar>
            </header>
        );
    }
}
