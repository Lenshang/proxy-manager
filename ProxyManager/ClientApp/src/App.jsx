import './App.css';
import { DatePicker } from 'antd';
import { BrowserRouter, Switch, Route } from 'react-router-dom';
import Main from './pages/Main';
import Login from './pages/Login';
import Http from './utils/HttpClient';
import ExLoading from './components/ExLoading';
function App() {
    Http.onError = (err) => {
        console.log(err);
        if (err.response.status == 401) {
            window.location.href = "/login";
        }
        else {
            return err.response;
        }
    };
    Http.beforeRequest = (request) => {
        var bearerToken = localStorage.getItem("token");
        if (bearerToken) {
            if (!request.headers) {
                request.headers = {}
            }
            request.headers["Authorization"] = "Bearer " + bearerToken;
        }
        return request;
    };
    return (
        <BrowserRouter>
            <Switch>
                <Route key={0} path="/login" exact component={Login}></Route>
                <Route key={1} path="/" component={Main} />
            </Switch>
            <ExLoading />
        </BrowserRouter>
    );
}

export default App;
