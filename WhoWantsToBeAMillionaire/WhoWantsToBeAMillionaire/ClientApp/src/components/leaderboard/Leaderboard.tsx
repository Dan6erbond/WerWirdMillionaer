import * as React from 'react';
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
    gamesFetched: boolean;
}

class Leaderboard extends React.Component<LeaderboardProps & RouteComponentProps, LeaderboardState> {
    constructor(props: LeaderboardProps & RouteComponentProps) {
        super(props);
        this.state = {sort: LeaderboardSort.RankAscending, gamesFetched: false};

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
        this.ensureDataFetched();
    }

    private ensureDataFetched() {
        if (!this.props.games.categories) {
            this.props.gameActions.fetchCategories();
        }
        if (!this.state.gamesFetched) {
            this.setState({gamesFetched: true});
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

    private deleteGame(index: number) {
        const token = this.props.users.token;
        if (!token) return;
        this.props.gameActions.deleteGame(index, token);
    }

    public render() {
        const style = {verticalAlign: 'middle'};
        
        return (
            <div>
                <h4>Leaderboard</h4>
                <br/>
                {this.props.games.leaderboard && this.props.games.categories ?
                    <Table striped bordered hover responsive size="sm">
                        <thead>
                        <tr>
                            <th style={style}>
                                # <Button variant="light" onClick={this.toggleRankSort}>⇅</Button>
                            </th>
                            <th style={style}>
                                Username <Button variant="light" onClick={this.toggleUsernameSort}>⇅</Button>
                            </th>
                            <th style={style}>
                                Points <Button variant="light" onClick={this.togglePointsSort}>⇅</Button>
                            </th>
                            <th style={style}>
                                Weighted Points <Button variant="light"
                                                        onClick={this.toggleWeightedPointsSort}>⇅</Button>
                            </th>
                            <th style={style}>
                                Game Time <Button variant="light" onClick={this.toggleGameTimeSort}>⇅</Button>
                            </th>
                            <th style={style}>
                                Categories
                            </th>
                            <th style={style}>
                                Time of Game
                            </th>
                            {this.props.users.userData && this.props.users.userData.isAdmin ?
                                <th style={style}>Delete</th> : null}
                        </tr>
                        </thead>
                        <tbody>
                        {this.props.games.leaderboard.map((g, i) =>
                            <tr style={{verticalAlign: 'middle'}} key={g.gameId}>
                                <td style={style}>{g.rank}</td>
                                <td style={style}>{g.username}</td>
                                <td style={style}>{g.points}</td>
                                <td style={style}>{g.weightedPoints}</td>
                                <td style={style}>{g.duration > 60 ?
                                    <span>{Math.round(g.duration / 60)} minutes {g.duration % 60} seconds</span>
                                    : <span>{g.duration} seconds</span>}</td>
                                <td style={style}>{this.props.games.categories!!.filter(c => g.categories.includes(c.categoryId!!)).map(c => c.name).join(", ")}</td>
                                <td style={style}>{new Date(Date.parse(g.start)).toLocaleString()}</td>
                                {this.props.users.userData && this.props.users.userData.isAdmin ?
                                    <td style={style}>
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
