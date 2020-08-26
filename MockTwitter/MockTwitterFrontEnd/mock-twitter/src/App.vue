<template>
    <div id="app">
        <p>{{counter}}</p>
    </div>
</template>

<script>
    export default {
        name: 'App',
        data() {
            return {
                counter: 0,
            }
        },
     // i have access to state.
        mounted() {
            console.log(this.$store.state.authenticated);
        }
    }
</script>