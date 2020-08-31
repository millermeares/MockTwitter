<template>
    <div>
        <p>NAVBAR</p>
        <p v-on:click="logout">Logout</p>
    </div>
    
</template>
<script>
    export default {
        name: "Navbar",
        methods: {
            logout() {
                this.$store.dispatch('logout')
            }
        }
    }
</script>