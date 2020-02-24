﻿import * as React from 'react';
import {connect} from 'react-redux';
import {ApplicationState} from "../../store";
import * as UserStateStore from "../../store/Users";
import {RouteComponentProps} from "react-router";

import './SignUp.css';
import AuthenticationForm from "../forms/AuthenticationForm";

type SignUpProps =
    UserStateStore.UserState
    & typeof UserStateStore.actionCreators
    & RouteComponentProps;

interface SignUpState {
    username: string;
    password: string;
}

class SignUp extends React.Component<SignUpProps, SignUpState> {
    constructor(props: SignUpProps) {
        super(props);
        this.state = {username: "", password: ""};
        this.signUp = this.signUp.bind(this);
    }

    public componentDidUpdate(prevProps: Readonly<SignUpProps>, prevState: Readonly<{}>, snapshot?: any) {
        if (this.props.userCreated) {
            this.props.login(this.state.username, this.state.password);
        } else if (this.props.token) {
            this.props.requestUserData(this.props.token);
        }

        if (this.props.token && this.props.userData && !this.props.userData.IsAdmin) {
            this.props.history.push("quiz");
        } else if (this.props.token && this.props.userData && this.props.userData.IsAdmin) {
            this.props.history.push("admin");
        }
    }

    public render() {
        return (
            <div className="form-container">
                <h4 className="title">Log in</h4>
                <br/>
                <AuthenticationForm signUp={this.signUp}/>
            </div>
        );
    }

    private signUp(username: string, password: string) {
        this.setState({username: username, password: password});
        this.props.createUser(this.state.username, this.state.password);
    }
}

export default connect(
    (state: ApplicationState) => state.userState,
    UserStateStore.actionCreators
)(SignUp as any);