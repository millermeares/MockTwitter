<template>
    <div id="login">
        <br />
        <div class="w-25 mx-auto">
            <b-container>
                <b-row class="mb-2 text-center"><h2 class="text-primary">Login</h2></b-row>
                <b-row class="mb-2"><b-form-input type="text" v-model="input.username" placeholder="Enter your username."></b-form-input></b-row>
                <b-row class="mb-2"><b-form-input type="password" v-model="input.password" placeholder="Enter your password."></b-form-input></b-row>
                <b-row class="mb-2">
                    <b-col><b-button variant="outline-primary" v-on:click="login">Login</b-button></b-col>
                    <b-col><b-button variant="outline-primary" to="/createaccount">New? Create An Account!</b-button></b-col>
                </b-row>
                <b-row><b-alert variant="danger" v-model="badInput" class="mb-2">{{message}}</b-alert></b-row>
            </b-container>
        </div>
        <router-view />
    </div>
</template>

<script>
    import axios from 'axios'
    export default {
        mounted() {
            this.$emit("authenticated", false);
        },
        name: 'Login',
        data() {
            return {
                input: {
                    username: "",
                    password: ""
                },
                message: "",
                badInput: false
            }
        },
        methods: {
            login() {
                if (this.input.username != "" && this.input.password != "") {
                    // need to make a request to server to get the userId
                    // if valid, return user id, else return invalid username/pw.

                    var data = JSON.stringify({ "Username": this.input.username, "Password_hash": this.input.password });
                    var config = {
                        method: 'post',
                        url: 'https://localhost:5001/api/Users/login',
                        headers: {
                            'Content-type': 'application/json',
                        },
                        data: data
                    };

                    axios(config)
                        .then(response => {
                            // need some way to store the userId and :)
                            console.log(response);
                            // want to replace "emit" with vuex
                            this.$emit("authenticated", true);
                            this.$router.replace({ name: "Home" });
                        })
                        .catch(error => {
                            if (error.response != null) {
                                if (error.response.status == 401) {
                                    this.message = "Username or password are incorrect";
                                    this.badInput = true;
                                } else if (error.response.status == 303) {
                                    this.message = "User does not exist. Try a different username or create an account.";
                                    this.badInput = true;
                                } else {
                                    this.message = "Something went wrong. Please try again.";
                                    this.badInput = true;
                                }
                            } else {
                                if (error.request != null) {
                                    this.message = "Something is wrong with the database or your internet connection. Please try again.";
                                    this.badInput = true;
                                }
                            }
                            // want to replace emit with vuex
                            this.$emit("authenticated", false);
                            console.log(error.response);
                        });
                    
                } else {
                    this.badInput = true;
                    this.message = "You must provide both a username and a password.";
                }
            }
        }
    }
</script>