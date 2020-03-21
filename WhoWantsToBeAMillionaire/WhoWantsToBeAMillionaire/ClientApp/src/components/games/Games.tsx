import * as React from 'react';
import {connect} from 'react-redux';
import {ApplicationState} from "../../store";
import * as UserStateStore from "../../store/Users";
import * as GameStateStore from "../../store/Games";
import {RouteComponentProps} from "react-router";
import {bindActionCreators} from "redux";
import {Accordion, Button, Card, ListGroup} from "react-bootstrap";
import {Game, Round} from "../../store/Games";
import GameRound from "./GameRound";

import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";

interface GamesProps {
    users: UserStateStore.UserState;
    userActions: typeof UserStateStore.actionCreators;
    games: GameStateStore.GameState;
    gameActions: typeof GameStateStore.actionCreators;
}

class Games extends React.Component<GamesProps & RouteComponentProps> {
    constructor(props: GamesProps & RouteComponentProps) {
        super(props);

        this.doForwards = this.doForwards.bind(this);
        this.ensureDataFetched = this.ensureDataFetched.bind(this);
    }

    public componentDidMount() {
        this.doForwards();
        this.ensureDataFetched();
    }

    public componentDidUpdate() {
        this.doForwards();
        this.ensureDataFetched();
    }

    private doForwards() {
        if (!this.props.users.token) {
            this.props.history.push("/");
        }
    }

    private ensureDataFetched() {
        const token = this.props.users.token!!;

        if (!this.props.games.userGames) {
            this.props.gameActions.fetchUserGames(token);
        }
    }

    public render() {
        return (
            <div>
                {this.props.games.userGames ? <Accordion>
                    {this.props.games.userGames.map((g: Game, i) =>
                        <Card key={i}>
                            <Card.Header>
                                <Accordion.Toggle as={Button} variant="link" eventKey={`${i}`}>
                                    {`Game played at ${new Date(Date.parse(g.start)).toLocaleString()}`}
                                </Accordion.Toggle>
                            </Card.Header>
                            <Accordion.Collapse eventKey={`${i}`}>
                                <Card.Body>
                                    <p>
                                        <b>Categories: </b> {this.props.games.categories!!.filter(c => g.categories.includes(c.categoryId!!)).map(c => c.name).join(", ")}
                                        <br/>
                                        <FontAwesomeIcon icon="stopwatch"/> <b>Game time:</b> {g.duration.toTimeString()}
                                        <br/>
                                        <FontAwesomeIcon icon="coins"/> <b>Points:</b> {g.points}
                                        <br/>
                                        <FontAwesomeIcon icon="trophy"/> <b>Rank:</b> {g.rank ? g.rank : <em>Game hidden from leaderboard.</em>}
                                    </p>
                                    <ListGroup>
                                        {g.rounds.map((r: Round, j) => <GameRound round={r} key={j}/>)}
                                    </ListGroup>
                                </Card.Body>
                            </Accordion.Collapse>
                        </Card>
                    )}
                </Accordion> : null}
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
)(Games as any);