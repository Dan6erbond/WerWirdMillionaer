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
    fetched: boolean;
}

class Leaderboard extends React.Component<LeaderboardProps & RouteComponentProps, LeaderboardState> {
    constructor(props: LeaderboardProps & RouteComponentProps) {
        super(props);
        this.state = {sort: LeaderboardSort.RankAscending, fetched: false};

        this.ensureDataFetched = this.ensureDataFetched.bind(this);
        this.toggleRankSort = this.toggleRankSort.bind(this);
        this.toggleUsernameSort = this.toggleUsernameSort.bind(this);
        this.togglePointsSort = this.togglePointsSort.bind(this);
        this.toggleWeightedPointsSort = this.toggleWeightedPointsSort.bind(this);
        this.toggleGameTimeSort = this.toggleGameTimeSort.bind(this);
        this.deleteGame = this.deleteGame.bind(this);
    }

    public componentDidMount() {
        this.ensureDataFetched();
    }

    public componentDidUpdate(prevProps: Readonly<LeaderboardProps>, prevState: Readonly<LeaderboardState>, snapshot?: any) {
        if (!this.state.fetched) {
            this.ensureDataFetched();
        }
    }

    private ensureDataFetched() {
        this.setState({fetched: true});
        this.props.gameActions.fetchLeaderboard();
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

    private deleteGame(index: number) {
        const token = this.props.users.token;
        if (!token) return;
        this.props.gameActions.deleteGame(index, token);
    }

    public render() {
        return (
            <div>
                <h4>Leaderboard</h4>
                <br/>
                {this.props.games.leaderboard ?
                    <Table striped bordered hover>
                        <thead>
                        <tr>
                            <th style={{verticalAlign: 'middle'}}>
                                # <Button variant="light" onClick={this.toggleRankSort}>⇅</Button>
                            </th>
                            <th style={{verticalAlign: 'middle'}}>
                                Username <Button variant="light" onClick={this.toggleUsernameSort}>⇅</Button>
                            </th>
                            <th style={{verticalAlign: 'middle'}}>
                                Points <Button variant="light" onClick={this.togglePointsSort}>⇅</Button>
                            </th>
                            <th style={{verticalAlign: 'middle'}}>
                                Weighted Points <Button variant="light"
                                                        onClick={this.toggleWeightedPointsSort}>⇅</Button>
                            </th>
                            <th style={{verticalAlign: 'middle'}}>
                                Game Time <Button variant="light" onClick={this.toggleGameTimeSort}>⇅</Button>
                            </th>
                            {this.props.users.userData && this.props.users.userData.isAdmin ?
                                <th style={{verticalAlign: 'middle'}}>Delete</th> : null}
                        </tr>
                        </thead>
                        <tbody>
                        {this.props.games.leaderboard.map((g, i) =>
                            <tr key={g.gameId}>
                                <td style={{verticalAlign: 'middle'}}>{g.rank}</td>
                                <td style={{verticalAlign: 'middle'}}>{g.username}</td>
                                <td style={{verticalAlign: 'middle'}}>{g.points}</td>
                                <td style={{verticalAlign: 'middle'}}>{g.weightedPoints}</td>
                                <td style={{verticalAlign: 'middle'}}>{g.duration > 60 ?
                                    <span>{Math.round(g.duration / 60)} minutes {g.duration % 60} seconds</span>
                                    : <span>{g.duration} seconds</span>}</td>
                                {this.props.users.userData && this.props.users.userData.isAdmin ?
                                    <td style={{verticalAlign: 'middle'}}>
                                        <Button variant="primary" onClick={() => this.deleteGame(i)}>Delete</Button>
                                    </td> : null}
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
