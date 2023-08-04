import config from 'config';
import { authHeader } from '../_helpers';
import axios from 'axios';

export const userService = {
    login,
    logout,
    register,
    getAll,
    getById,
    update,
    delete: _delete
};

function login(email, password) {
    const request = { email: email, password: password };
    return axios.post(`${config.apiUrl}/Token/Get`, request)
        .then(response => {
            if (response != null) {
                // store user details and jwt token in local storage to keep user logged in between page refreshes
                localStorage.setItem('userToken', JSON.stringify(response.data));
            }
            return response;
        }).catch(error => {
            console.error("There was an error!", error);
        });
}

function logout() {
    // remove user from local storage to log user out
    localStorage.removeItem('user');
}

function register(user) {
    return axios.post(`${config.apiUrl}/TestUser/Insert`, user)
        .then(response => {           
            return response;
        }).catch(error => {
            console.error("There was an error!", error);
        });
}

function getAll() {
    return axios.get(`${config.apiUrl}/TestUser/Get`)
        .then(response => {
            debugger;
            return response;
        }).catch(error => {
            console.error("There was an error!", error);
        });
}


function getById(id) {
    const requestOptions = {
        method: 'GET',
        headers: authHeader()
    };

    return fetch(`${config.apiUrl}/users/${id}`, requestOptions).then(handleResponse);
}

function update(user) {
    const requestOptions = {
        method: 'PUT',
        headers: { ...authHeader(), 'Content-Type': 'application/json' },
        body: JSON.stringify(user)
    };

    return fetch(`${config.apiUrl}/users/${user.id}`, requestOptions).then(handleResponse);
}

// prefixed function name with underscore because delete is a reserved word in javascript
function _delete(id) {
    const requestOptions = {
        method: 'DELETE',
        headers: authHeader()
    };

    return fetch(`${config.apiUrl}/users/${id}`, requestOptions).then(handleResponse);
}

function handleResponse(response) {
    return response.text().then(text => {
        const data = text && JSON.parse(text);
        if (!response.ok) {
            if (response.status === 401) {
                // auto logout if 401 response returned from api
                logout();
                location.reload(true);
            }

            const error = (data && data.message) || response.statusText;
            return Promise.reject(error);
        }

        return data;
    });
}