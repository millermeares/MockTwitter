<template>
    <!--<b-col></b-col>-->
    <div id="createaccount">
        <br />
        <div class="w-25 mx-auto">
            <b-container>
                <b-row class="mb-2 text-center"><h2 class="text-primary">Create Account</h2></b-row>
                <b-row class="mb-2"><b-form-input type="text" v-model="input.username" placeholder="Enter your desired username."></b-form-input></b-row>
                <b-row class="mb-2"><b-form-input type="password" v-model="input.password" placeholder="Enter your password."></b-form-input></b-row>
                <b-row class="mb-2"><b-form-input type="password" v-model="input.secondpassword" placeholder="Verify your password."></b-form-input></b-row>
                <b-row><br /></b-row>
                <b-row class="text-center mb-2">
                    <b-col><b-button variant="outline-primary" v-on:click="create">Create Account</b-button></b-col>
                    <b-col><b-button variant="outline-primary" to="/login">Back to Login</b-button></b-col>
                </b-row>
                <b-row><br /></b-row>
                <b-alert variant="danger" v-model="badInput">{{message}}</b-alert>
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
        name: 'CreateAccount',
        data() {
            return {
                input: {
                    username: "",
                    password: "",
                    secondpassword: ""
                },
                message: "",
                badInput: false,
            }
        },
        methods: {
            create() {
                if (this.input.username != "" && this.input.password != "") {
                    // need to make a request to server to get the userId
                    // if valid, return user id, else return invalid username/pw.
                    if (this.input.password == this.input.secondpassword) {
                        // store this in the database.
                        if (this.input.password.length > 7) {
                            // verify that the username is not already taken. if it is, say so. 

                            // if it's not, post this to DB.
                            var data = JSON.stringify({ "Username": this.input.username, "Password_hash": this.input.password });
                            var config = {
                                method: 'post',
                                url: 'https://localhost:5001/api/Users',
                                headers: {
                                    'Content-type': 'application/json',
                                },
                                data: data
                            };

                            axios(config)
                                .then(response => {
                                    console.log(response);
                                    this.$router.replace({ name: "Login" });
                                    
                                })
                                .catch(error => {
                                    if (error.response != null) {
                                        if (error.response.status == 303) {
                                            this.message = "That username is already taken. Please try a different username."
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
                                    console.log(error);
                                });

                        } else {
                            this.message = "Your password must be 8 or more characters.";
                            this.badInput = true;
                        }
                    } else {
                        this.message = "You must enter the same password into both boxes";
                        this.badInput = true;
                    }
                } else {
                    this.message = "You must provide a username, and enter the same password in both fields.";
                    this.badInput = true;
                }
            }
        }
    }
</script>