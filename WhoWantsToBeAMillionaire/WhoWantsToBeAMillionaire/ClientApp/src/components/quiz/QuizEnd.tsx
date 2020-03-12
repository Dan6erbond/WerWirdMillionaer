import * as React from "react";
import {QuizResult} from "../../store/ApiResponse";
import {Button, ButtonGroup, ButtonToolbar, Jumbotron} from "react-bootstrap";
import {Link} from "react-router-dom";

interface QuizEndProps {
    result: QuizResult;
}

export default class QuizEnd extends React.Component<QuizEndProps> {
    constructor(props: QuizEndProps) {
        super(props);
    }

    public render() {
        return (
            <Jumbotron>
                <h1>Game over!</h1>
                <p>{this.props.result.won ? "You won." : "You lost."}</p>
                <p>
                    Game time:
                    {this.props.result.timeElapsed > 60 ?
                        <span>{Math.round(this.props.result.timeElapsed / 60)} minutes {this.props.result.timeElapsed % 60} seconds</span>
                        : <span>{this.props.result.timeElapsed} seconds</span>}
                </p>
                <p>Points: {this.props.result.points}</p>
                <ButtonToolbar aria-label="Toolbar with button groups">
                    <ButtonGroup className="mr-2" aria-label="First group">
                        <Button variant="primary">Play again</Button>
                    </ButtonGroup>
                    <ButtonGroup className="mr-2" aria-label="Second group">
                        <Button variant="primary">
                            <Link to="/leaderboard" style={{textDecoration: 'none', color: 'white'}}>Leaderboard</Link>
                        </Button>
                    </ButtonGroup>
                </ButtonToolbar>
            </Jumbotron>
        );
    }
}