import Vue from 'vue'
import Vuex from 'vuex'
import axios from 'axios'
Vue.use(Vuex);
import router from '../router/index.js'

export const store = new Vuex.Store({
    state: {
        idToken: null,
        userId: null,
        expiresIn: null,
        username: null,
        expirationDate: null,
        error_message: null
    },
    mutations: {
        authUser(state, userSecTokenData) {
            state.idToken = userSecTokenData.idToken
            state.userId = userSecTokenData.userId
            state.expiresIn = userSecTokenData.expiresIn
            state.expirationDate = userSecTokenData.expirationDate // calcualte this.
            state.username = userSecTokenData.username
            state.error_message = null
        },
        storeUser(state, userInfo) {
            state.username = userInfo.username
        },
        clearAuthData(state) {
            state.idToken = null
            state.userId = null
            state.expiresIn = null
            state.expirationDate = null
            state.username = null
        },
        setError(state, error_message) {
            state.error_message = error_message
        },
        clearError(state) {
            state.error_message = null
        }
    },
    actions: {
        setLogoutTimer({ commit }, expirationTime) {
            setTimeout(() => {
                commit('clearAuthData')
            }, expirationTime * 1000)
        },
        createaccount({ commit, dispatch }, authData) {
            // create account request to DB.
            // if success, commit. then set local storage. call storeUser and setLogoutTimer
            // if not success, don't determine why and set error_message to that thing.
            const username = authData.Username
            axios.post("https://localhost:5001/api/Users/createaccount", {
                Username: authData.Username,
                PasswordHash: authData.PasswordHash
            })
                .then(res => {
                    console.log(res)
                    const now = new Date()
                    // make expiration date for auto logout.
                    const expirationDate = new Date(now.getTime() + res.data.expiresIn * 60000)
                    localStorage.setItem('idToken', res.data.idToken)
                    localStorage.setItem('userId', res.data.userId)
                    localStorage.setItem('expirationDate', expirationDate)
                    localStorage.setItem('username', username)
                    commit('authUser', {
                        idToken: res.data.idToken,
                        userId: res.data.userId,
                        expiresIn: res.data.expiresIn,
                        expirationDate: expirationDate,
                        username: username
                    })
                    commit('clearError')
                    dispatch('setLogoutTimer', res.data.expiresIn * 60)
                    router.replace('/home')
                })
                .catch(error => {
                    if (error.response != null) {
                        if (error.response.status == 303) {
                            commit('setError', "That username is already taken. Please try a different username.")
                        } else {
                            commit('setError', "Something went wrong. Please try again.")
                        }
                    } else {
                        if (error.request != null) {
                            commit('setError', "Something is wrong with the database or your internet connection. Please try again.")
                        }
                    }
                    //console.log(error)
                });
        },
        login({ commit, dispatch }, authData) {
            // login request to DB.
            // same pattern as createAccount
            const username = authData.Username
            axios.post("https://localhost:5001/api/Users/login/", {
                Username: authData.Username,
                PasswordHash: authData.PasswordHash
            })
                .then(res => {
                    console.log(res)
                    const now = new Date()
                    // make expiration date for auto logout.
                    const expirationDate = new Date(now.getTime() + res.data.expiresIn * 60000)
                    localStorage.setItem('idToken', res.data.idToken)
                    localStorage.setItem('userId', res.data.userId)
                    localStorage.setItem('expirationDate', expirationDate)
                    localStorage.setItem('username', username)
                    commit('authUser', {
                        idToken: res.data.idToken,
                        userId: res.data.userId,
                        expiresIn: res.data.expiresIn,
                        expirationDate: expirationDate,
                        username: username
                    })
                    commit('clearError')
                    dispatch('setLogoutTimer', res.data.expiresIn * 60)
                    router.replace('/home')
                })
                .catch(error => {
                    if (error.response != null) {
                        if (error.response.status == 401) {
                            this.message = "Username or password are incorrect";
                            commit('setError', 'Username or password are incorrect')
                        } else if (error.response.status == 303) {
                            commit('setError', "User does not exist. Try a different username or create an account.")
                        } else {
                            commit('setError', "Something went wrong. Please try again.")
                        }
                    } else {
                        if (error.request != null) {
                            commit('setError', "Something is wrong with the database or your internet connection. Please try again.")
                        }
                    }
                    //console.log(error)
                });

        },
        tryAutoLogin({ commit }) {
            // try to get id token from local storage. if it's not there, return.
            // if it is there, check if it's expired. if it is, return. 
            const token = localStorage.getItem('idToken')
            if (!token) {
                return
            }
            const expirationDate = localStorage.getItem('expirationDate')
            const now = new Date()
            if (now >= expirationDate) {
                return
            }
            commit('authUser', {
                idToken: localStorage.getItem('idToken'),
                userId: localStorage.getItem('userId'),
                expirationDate: localStorage.getItem('expirationDate'),
                username: localStorage.getItem('username')
            })
            router.replace('/home')
        },
        logout({ commit }) {
            commit('clearAuthData')
            localStorage.removeItem('expirationDate')
            localStorage.removeItem('idToken')
            localStorage.removeItem('userId')
            localStorage.removeItem('username')
            router.replace('/login')
        },
    },
    getters: {
        user(state) {
            return state
        },
        isAuthenticated(state) {
            return state.idToken !== null
        },
    }
});