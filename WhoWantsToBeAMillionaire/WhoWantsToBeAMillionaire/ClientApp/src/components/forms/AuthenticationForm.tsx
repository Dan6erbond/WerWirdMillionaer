import * as React from 'react';

import {Button, Form} from "react-bootstrap";
import {Link} from "react-router-dom";

import './AuthenticationForm.css';

interface AuthenticationFormProps {
    login?: ((username: string, password: string) => void);
    signUp?: ((username: string, password: string) => void);
    usernameCorrect?: boolean;
    passwordCorrect?: boolean;
    usernameTaken?: boolean;
}

interface AuthenticationFormState {
    username: string;
    password: string;
    passwordConfirmation: string;
    passwordMatches: boolean;
}

export default class AuthenticationForm extends React.Component<AuthenticationFormProps, AuthenticationFormState> {
    public constructor(props: AuthenticationFormProps) {
        super(props);
        this.state = {username: "", password: "", passwordConfirmation: "", passwordMatches: true};

        this.handleSubmit = this.handleSubmit.bind(this);
        this.setPassword = this.setPassword.bind(this);
        this.setPasswordConfirmation = this.setPasswordConfirmation.bind(this);
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
                    {this.props.usernameCorrect === false || this.props.usernameTaken ?
                        <Form.Text className="text-danger">
                            {this.props.login ? "Please check your username." : "This username is already taken."}
                        </Form.Text> : null}
                </Form.Group>

                <Form.Group controlId="formBasicPassword">
                    <Form.Label>Password</Form.Label>
                    <Form.Control
                        onChange={this.setPassword}
                        type="password"
                        required={true}
                        placeholder="Password"/>
                    {this.props.passwordCorrect === false ?
                        <Form.Text className="text-danger">
                            {this.props.login ? "Please check your password." :
                                "Your password must contain at least one letter, one special character or number and be at least 6 characters long."}
                        </Form.Text> : null}
                </Form.Group>

                {
                    this.props.signUp ?
                        <Form.Group controlId="formBasicPasswordConfirmation">
                            <Form.Label>Password</Form.Label>
                            <Form.Control
                                onChange={this.setPasswordConfirmation}
                                type="password"
                                required={true}
                                placeholder="Confirm password"/>
                            {this.props.signUp && !this.state.passwordMatches ?
                                <Form.Text className="text-danger">
                                    Please re-enter your password correctly.
                                </Form.Text> : null}
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

    private setPassword(e: React.FormEvent<HTMLInputElement>) {
        let password = (e.currentTarget as any).value;
        let passwordMatches = password === this.state.passwordConfirmation;

        this.setState({password: password, passwordMatches: passwordMatches})
    }

    private setPasswordConfirmation(e: React.FormEvent<HTMLInputElement>) {
        let passwordConfirmation = (e.currentTarget as any).value;
        let passwordMatches = this.state.password === passwordConfirmation;

        this.setState({passwordConfirmation: passwordConfirmation, passwordMatches: passwordMatches})
    }

    private handleSubmit(e: React.FormEvent<HTMLFormElement>) {
        e.preventDefault();

        if (this.props.login) {
            this.props.login(this.state.username, this.state.password);
        } else if (this.props.signUp) {
            if (this.state.passwordMatches) {
                this.props.signUp(this.state.username, this.state.password);
            }
        }
    }
}