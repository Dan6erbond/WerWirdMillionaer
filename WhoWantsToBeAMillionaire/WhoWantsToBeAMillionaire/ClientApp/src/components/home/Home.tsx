import * as React from 'react';
import {connect} from 'react-redux';
import {ApplicationState} from "../../store";
import * as UserStateStore from "../../store/Users";
import {RouteComponentProps} from "react-router";

import './Home.css';
import AuthenticationForm from "../forms/AuthenticationForm";

type HomeProps =
    UserStateStore.UserState
    & typeof UserStateStore.actionCreators
    & RouteComponentProps;

interface HomeState {
    tries: number;
}

class Home extends React.Component<HomeProps, HomeState> {
    constructor(props: HomeProps) {
        super(props);
        this.state = {tries: 0};
        this.login = this.login.bind(this);
    }

    public componentDidMount() {
        this.doForwards();
    }

    public componentDidUpdate(prevProps: Readonly<HomeProps>, prevState: Readonly<{}>, snapshot?: any) {
        this.doForwards();
    }

    private doForwards() {
        if (this.props.token && !this.props.userData) {
            this.props.requestUserData(this.props.token);
        } else if (this.props.token && this.props.userData && !this.props.userData.isAdmin) {
            this.props.history.push("quiz");
        } else if (this.props.token && this.props.userData && this.props.userData.isAdmin) {
            this.props.history.push("admin");
        }
    }

    public render() {
        let usernameCorrect = true;
        let passwordCorrect = true;

        if (this.state.tries > 0 && this.props.apiError !== undefined) {
            usernameCorrect = this.props.apiError !== "USER_DOES_NOT_EXIST";
            passwordCorrect = this.props.apiError !== "INCORRECT_PASSWORD";
        }

        return (
            <div className="form-container">
                <h4 className="title">Log in</h4>
                <br/>
                <AuthenticationForm login={this.login} usernameCorrect={usernameCorrect}
                                    passwordCorrect={passwordCorrect}/>
            </div>
        );
    }

    private login(username: string, password: string) {
        this.setState({tries: this.state.tries + 1});
        this.props.login(username, password);
    }
}

export default connect(
    (state: ApplicationState) => state.userState,
    UserStateStore.actionCreators
)(Home as any);
