<template>
  <div id="home">
      <NavBar></NavBar>
      <p>I should only be getting this if I'm authenticated.</p>
      <p> Hello {{username}}</p>
  </div>
</template>

<script>
    // @ is an alias to /src
    import Nav from '../components/Nav.vue'
    export default {
        computed: {
            username() {
                return !this.$store.getters.user ? false : this.$store.getters.user.username
            }
        },
        components: {
            NavBar: Nav
        }
    }
</script>
