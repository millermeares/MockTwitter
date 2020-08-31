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
        name: 'Login',
        data() {
            return {
                input: {
                    username: "",
                    password: ""
                },
            }
        },
        methods: { // can send requests with vuex
            login() {
                if (this.input.username != "" && this.input.password != "") {
                    this.$store.dispatch('login', { Username: this.input.username, PasswordHash: this.input.password });
                } else {
                    this.$store.commit('setError', "You must provide both a username and a password.")
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