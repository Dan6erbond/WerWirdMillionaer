﻿import * as React from 'react';
import {connect} from 'react-redux';
import {ApplicationState} from "../../store";
import * as UserStateStore from "../../store/Users";
import {RouteComponentProps} from "react-router";
import * as GameStateStore from "../../store/Games";
import {bindActionCreators} from "redux";
import {Button, Table} from "react-bootstrap";

export enum LeaderboardSort {
    RankAscending,
    RankDescending,
    UsernameAscending,
    UsernameDescending,
    PointsAscending,
    PointsDescending,
    WeightedPointsAscending,
    WeightedPointsDescending,
    GameTimeSortAscending,
    GameTimeSortDescending
}

interface LeaderboardProps {
    users: UserStateStore.UserState;
    userActions: typeof UserStateStore.actionCreators;
    games: GameStateStore.GameState;
    gameActions: typeof GameStateStore.actionCreators;
}

interface LeaderboardState {
    sort: LeaderboardSort;
}

class Leaderboard extends React.Component<LeaderboardProps & RouteComponentProps, LeaderboardState> {
    constructor(props: LeaderboardProps & RouteComponentProps) {
        super(props);
        this.state = {sort: LeaderboardSort.RankAscending};
        
        this.ensureDataFetched = this.ensureDataFetched.bind(this);
        this.toggleRankSort = this.toggleRankSort.bind(this);
        this.toggleUsernameSort = this.toggleUsernameSort.bind(this);
        this.togglePointsSort = this.togglePointsSort.bind(this);
        this.toggleWeightedPointsSort = this.toggleWeightedPointsSort.bind(this);
        this.toggleGameTimeSort = this.toggleGameTimeSort.bind(this);
    }

    public componentDidMount() {
        this.ensureDataFetched();
    }

    public componentDidUpdate(prevProps: Readonly<LeaderboardProps>, prevState: Readonly<{}>, snapshot?: any) {
        this.ensureDataFetched();
    }

    private ensureDataFetched() {
        if (!this.props.games.leaderboard) {
            this.props.gameActions.fetchLeaderboard();
        }
    }

    private toggleRankSort() {
        let sort = this.state.sort === LeaderboardSort.RankAscending ? LeaderboardSort.RankDescending : LeaderboardSort.RankAscending;
        this.setState({sort: sort});
        this.props.gameActions.sortLeaderboard(sort);
    }
    
    private toggleUsernameSort() {
        let sort = this.state.sort === LeaderboardSort.UsernameAscending ? LeaderboardSort.UsernameDescending : LeaderboardSort.UsernameAscending;
        this.setState({sort: sort});
        this.props.gameActions.sortLeaderboard(sort);
    }
    
    private togglePointsSort() {
        let sort = this.state.sort === LeaderboardSort.PointsAscending ? LeaderboardSort.PointsDescending : LeaderboardSort.PointsAscending;
        this.setState({sort: sort});
        this.props.gameActions.sortLeaderboard(sort);
    }
    
    private toggleWeightedPointsSort() {
        let sort = this.state.sort === LeaderboardSort.WeightedPointsAscending ? LeaderboardSort.WeightedPointsDescending : LeaderboardSort.WeightedPointsAscending;
        this.setState({sort: sort});
        this.props.gameActions.sortLeaderboard(sort);
    }

    private toggleGameTimeSort() {
        let sort = this.state.sort === LeaderboardSort.GameTimeSortAscending ? LeaderboardSort.GameTimeSortDescending : LeaderboardSort.GameTimeSortAscending;
        this.setState({sort: sort});
        this.props.gameActions.sortLeaderboard(sort);
    }

    public render() {
        return (
            <div>
                <br/>
                <h4>Leaderboard</h4>
                <br/>
                {this.props.games.leaderboard ?
                    <Table striped bordered hover>
                        <thead>
                        <tr>
                            <th style={{verticalAlign: 'middle'}}># <Button variant="light" onClick={this.toggleRankSort}>⇅</Button></th>
                            <th style={{verticalAlign: 'middle'}}>Username <Button variant="light" onClick={this.toggleUsernameSort}>⇅</Button></th>
                            <th style={{verticalAlign: 'middle'}}>Points <Button variant="light" onClick={this.togglePointsSort}>⇅</Button></th>
                            <th style={{verticalAlign: 'middle'}}>Weighted Points <Button variant="light" onClick={this.toggleWeightedPointsSort}>⇅</Button></th>
                            <th style={{verticalAlign: 'middle'}}>Game Time <Button variant="light" onClick={this.toggleGameTimeSort}>⇅</Button></th>
                        </tr>
                        </thead>
                        <tbody>
                        {this.props.games.leaderboard.map((g) =>
                            <tr key={g.gameId}>
                                <td>{g.rank}</td>
                                <td>{g.username}</td>
                                <td>{g.points}</td>
                                <td>{g.weightedPoints}</td>
                                <td>{g.duration > 60 ?
                                    <span>{Math.round(g.duration / 60)} minutes {g.duration % 60} seconds</span>
                                    : <span>{g.duration} seconds</span>}</td>
                            </tr>)}
                        </tbody>
                    </Table> : <p>Loading...</p>}
            </div>
        );
    }
}

export default connect(
    (state: ApplicationState) => ({
        users: state.userState,
        games: state.gameState,
    }),
    (dispatch) => {
        return {
            userActions: bindActionCreators(UserStateStore.actionCreators, dispatch),
            gameActions: bindActionCreators(GameStateStore.actionCreators, dispatch)
        };
    }
)(Leaderboard as any);
