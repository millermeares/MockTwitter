<template>
    <div>
        <b-container>
            <b-nav vertical>
                <b-nav-item><b-icon icon="bookshelf"></b-icon></b-nav-item>
                <b-nav-item><h3>Home</h3></b-nav-item>
                <b-nav-item><h3>About</h3></b-nav-item>
                <b-nav-item><h3>Settings</h3></b-nav-item>
                <b-nav-item><h3>Home</h3></b-nav-item>
                <b-nav-item><h3>About</h3></b-nav-item>
                <b-nav-item><h3>Settings</h3></b-nav-item>
                <b-nav-item><b-button pill size="lg" variant="primary">Tweet</b-button></b-nav-item>
                <b-nav-item v-on:click="logout()"><h3>Logout</h3></b-nav-item>
                <b-nav-item>Hello, {{msg}}</b-nav-item>
            </b-nav>
        </b-container>
    </div>
</template>
<script>
    export default {
        name: "Navbar",
        methods: {
            logout() {
                this.$store.dispatch('logout')
            },
            // on "tweet", need popup
        },
        props: {
            msg: String
        }
    }
</script>