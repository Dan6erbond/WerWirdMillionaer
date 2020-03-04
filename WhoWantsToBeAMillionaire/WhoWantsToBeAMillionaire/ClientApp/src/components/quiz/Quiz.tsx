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
    constructor(props: QuizProps) {
        super(props);
    }
    
    public componentDidMount() {
        if (!this.props.token) {
            this.props.history.push("/");
        }
    }
    
    public render() {
        return (
            <div>Test</div>
        );
    }
}

export default connect(
    (state: ApplicationState) => state.userState,
    UserStateStore.actionCreators
)(Quiz as any);