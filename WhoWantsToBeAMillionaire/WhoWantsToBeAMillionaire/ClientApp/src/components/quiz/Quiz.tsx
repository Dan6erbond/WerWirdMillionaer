import * as React from 'react';
import {connect} from 'react-redux';
import {ApplicationState} from "../../store";
import * as UserStateStore from "../../store/Users";
import {RouteComponentProps} from "react-router";

type QuizProps =
    UserStateStore.UserState
    & typeof UserStateStore.actionCreators
    & RouteComponentProps; 

class Quiz extends React.Component<QuizProps> {
    componentDidMount() {
        if (!this.props.token) {
            this.props.history.push("/");
        }        
    }
}

export default connect(
    (state: ApplicationState) => state.userState,
    UserStateStore.actionCreators
)(Quiz as any);