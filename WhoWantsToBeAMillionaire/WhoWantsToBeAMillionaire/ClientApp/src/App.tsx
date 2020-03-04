import * as React from 'react';
import { Route } from 'react-router';
import Layout from './components/Layout';

import Home from './components/home/Home';
import SignUp from "./components/sign-up/SignUp";
import Quiz from "./components/quiz/Quiz";

import 'bootstrap/dist/css/bootstrap.min.css';
import './custom.css';

export default () => (
    <Layout>
        <Route exact path='/' component={Home} />
        <Route path='/sign-up' component={SignUp} />
        <Route path='/quiz' component={Quiz} />
    </Layout>
);
