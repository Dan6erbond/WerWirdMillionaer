import * as React from 'react';
import NavMenu from './NavMenu';
import {Container} from "react-bootstrap";

export default (props: { children?: React.ReactNode }) => (
    <React.Fragment>
        <NavMenu/>
        <Container>
            {props.children}
        </Container>
    </React.Fragment>
);
