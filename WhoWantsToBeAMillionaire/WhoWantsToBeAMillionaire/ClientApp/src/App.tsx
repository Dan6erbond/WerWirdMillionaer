import * as React from 'react';
import { Route } from 'react-router';
import Layout from './components/Layout';

import Home from './components/home/Home';
import SignUp from "./components/sign-up/SignUp";
import Quiz from "./components/quiz/Quiz";

import 'bootstrap/dist/css/bootstrap.min.css';
import './custom.css';
import Leaderboard from "./components/leaderboard/Leaderboard";
import Games from "./components/games/Games";

export default () => (
    <Layout>
        <Route exact path='/' component={Home} />
        <Route path='/sign-up' component={SignUp} />
        <Route path='/quiz' component={Quiz} />
        <Route path='/games' component={Games} />
        <Route path='/leaderboard' component={Leaderboard} />
    </Layout>
);
