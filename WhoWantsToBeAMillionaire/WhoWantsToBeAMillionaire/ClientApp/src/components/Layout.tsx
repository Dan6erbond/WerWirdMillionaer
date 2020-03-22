import * as React from 'react';
import NavMenu from './NavMenu';
import {Container} from "react-bootstrap";

export default (props: { children?: React.ReactNode }) => (
    <React.Fragment>
        <NavMenu/>
        <br/>
        <Container>
            {props.children}
        </Container>
        <br/>
    </React.Fragment>
);
