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
                <b-row class="mb-2"><b-col align="center"><router-link to="/">Back to welcome page.</router-link></b-col></b-row>
                <b-row v-if="output()">
                    <b-alert variant="danger" show>{{output()}}</b-alert>
                </b-row>
            </b-container>
        </div>
        <router-view />
    </div>
</template>

<script>
    import { mapState } from 'vuex';
    export default {
        name: 'CreateAccount',
        data() {
            return {
                input: {
                    username: "",
                    password: "",
                    secondpassword: ""
                }
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
                            this.$store.dispatch('createaccount', { Username: this.input.username, PasswordHash: this.input.password })
                        } else {
                            this.$store.commit('setError', "Your password must be 8 or more characters.")
                        }
                    } else {
                        this.$store.commit('setError', "You must enter the same password into both boxes.")
                    }
                } else {
                    this.$store.commit('setError', "You must provide a username, and enter the same password in both fields.")
                }
            },
            output() {
                return !this.$store.getters.user.error_message ? false : this.$store.getters.user.error_message
            }
        },
        computed: mapState(['error_message']),
        created() {
            this.$store.commit('resetError')
        }

    }
</script>