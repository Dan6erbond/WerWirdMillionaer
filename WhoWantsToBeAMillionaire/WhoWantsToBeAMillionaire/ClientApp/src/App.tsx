import * as React from 'react';
import {Route} from 'react-router';
import Layout from './components/Layout';

import Home from './components/home/Home';
import SignUp from "./components/sign-up/SignUp";
import Quiz from "./components/quiz/Quiz";
import Leaderboard from "./components/leaderboard/Leaderboard";
import Games from "./components/games/Games";
import Admin from "./components/admin/Admin";

import { library } from '@fortawesome/fontawesome-svg-core'
import {faStopwatch, faCoins, faTrophy, faTrash, faPlus} from '@fortawesome/free-solid-svg-icons';

import 'bootstrap/dist/css/bootstrap.min.css';
import './custom.css';

library.add(faStopwatch, faCoins, faTrophy, faTrash, faPlus);

export default () => (
    <Layout>
        <Route exact path='/' component={Home}/>
        <Route path='/sign-up' component={SignUp}/>
        <Route path='/quiz' component={Quiz}/>
        <Route path='/games' component={Games}/>
        <Route path='/leaderboard' component={Leaderboard}/>
        <Route path='/admin' component={Admin}/>
    </Layout>
);
