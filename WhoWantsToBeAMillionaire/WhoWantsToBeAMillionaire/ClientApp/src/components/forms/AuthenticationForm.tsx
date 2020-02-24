import * as React from 'react';

import {Button, Form} from "react-bootstrap";
import {Link} from "react-router-dom";

import './AuthenticationForm.css';

interface AuthenticationFormProps {
    login?: ((username: string, password: string) => void);
    signUp?: ((username: string, password: string) => void);
}

interface AuthenticationFormState {
    username: string;
    password: string;
}

export default class AuthenticationForm extends React.Component<AuthenticationFormProps, AuthenticationFormState> {
    public constructor(props: AuthenticationFormProps) {
        super(props);
        this.state = {username: "", password: ""};
        this.handleSubmit = this.handleSubmit.bind(this);
    }

    public render() {
        return (
            <Form onSubmit={this.handleSubmit}>
                <Form.Group controlId="formBasicUsername">
                    <Form.Label>Username</Form.Label>
                    <Form.Control
                        onChange={(event: React.FormEvent<HTMLInputElement>) => this.setState({username: (event.currentTarget as any).value})}
                        type="text"
                        required={true}
                        placeholder="Username"/>
                </Form.Group>

                <Form.Group controlId="formBasicPassword">
                    <Form.Label>Password</Form.Label>
                    <Form.Control
                        onChange={(event: React.FormEvent<HTMLInputElement>) => this.setState({password: (event.currentTarget as any).value})}
                        type="password"
                        required={true}
                        placeholder="Password"/>
                </Form.Group>

                {
                    this.props.signUp ?
                        <Form.Group controlId="formBasicPasswordConfirmation">
                            <Form.Label>Password</Form.Label>
                            <Form.Control
                                onChange={(event: React.FormEvent<HTMLInputElement>) => this.setState({password: (event.currentTarget as any).value})}
                                type="password"
                                required={true}
                                placeholder="Confirm password"/>
                        </Form.Group> : null
                }

                <Form.Group>
                    {
                        !this.props.signUp ?
                            <Form.Label><Link to="/sign-up">Register now!</Link></Form.Label> : null
                    }
                    <Button variant="primary" type="submit">
                        {this.props.signUp ? "Sign up" : "Log in"}
                    </Button>
                </Form.Group>
            </Form>
        );
    }

    private handleSubmit(e: React.FormEvent<HTMLFormElement>) {
        e.preventDefault();
        
        if (this.props.login) {
            this.props.login(this.state.username, this.state.password);
        } else if (this.props.signUp) {
            // TODO: Check if confirmation password matches
            this.props.signUp(this.state.username, this.state.password);
        }
    }
}