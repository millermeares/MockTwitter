<template>
    <div id="home">
        <div class="min-vh-100 min-vw-100">
            <b-container fluid style="background-color:aliceblue">
                <b-row class="min-vh-100 min-vw-100">
                    <b-col cols="3" class="border-right border-secondary">
                        <NavBar :msg="username"></NavBar>
                    </b-col>
                    <b-col cols="6">
                        <!--might add router-view here. this would make home the layout more than just 'post tweets'-->
                        <b-row class="mb-2 border-bottom p-2 sticky-top bg-light">
                            <b-col><h2>Home</h2></b-col>
                            <!--add small icon here-->
                            <b-col class="text-right">
                                <h2><b-icon icon="bookshelf"></b-icon></h2>
                            </b-col>
                        </b-row>
                        <PostTweet></PostTweet>
                        <b-row class="bg-info mt-2 mb-2"><p></p></b-row>
                        
                    </b-col>
                    <b-col class="border-left border-secondary">
                        <b-row class="p-2">
                            <b-form-input v-model="text" placeholder="Search"></b-form-input>
                        </b-row>
                    </b-col>
                </b-row>
            </b-container>
        </div>
    </div>
</template>

<script>
    // @ is an alias to /src
    import Nav from '../components/Nav.vue'
    import PostTweet from '../components/PostTweet.vue'
    export default {
        computed: {
            username() {
                return !this.$store.getters.user ? false : this.$store.getters.user.username
            }
        },
        components: {
            NavBar: Nav,
            PostTweet: PostTweet
        },
        data() {
            return {
                text: null
            }
        }
        //created() {
        //    this.$store.dispatch('fetchUser')
        //}
    }
</script>