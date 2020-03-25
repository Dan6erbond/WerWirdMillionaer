import * as React from 'react';
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
        this.doForwards = this.doForwards.bind(this);
    }

    public componentDidMount() {
        this.doForwards();
    }

    public componentDidUpdate() {
        this.doForwards();

        if (this.props.userCreated && !this.props.token) {
            this.props.login(this.state.username, this.state.password);
        } else if (this.props.token && !this.props.userData) {
            this.props.requestUserData(this.props.token);
        }
    }

    private doForwards() {
        const userData = this.props.userData;
        console.log(userData)

        if (userData && !userData.isAdmin) {
            this.props.history.push("quiz");
        } else if (userData && userData.isAdmin) {
            this.props.history.push("admin");
        }
    }

    public render() {
        let usernameTaken = this.props.apiError === "USER_ALREADY_EXISTS";

        return (
            <div className="form-container">
                <h4 className="title">Sign up</h4>
                <br/>
                <AuthenticationForm signUp={this.signUp} usernameTaken={usernameTaken}/>
            </div>
        );
    }

    private signUp(username: string, password: string) {
        this.setState({
            username: username,
            password: password
        }, () => this.props.createUser(this.state.username, this.state.password));
    }
}

export default connect(
    (state: ApplicationState) => state.userState,
    UserStateStore.actionCreators
)(SignUp as any);