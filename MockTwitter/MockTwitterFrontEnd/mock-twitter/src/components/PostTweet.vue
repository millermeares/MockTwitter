<template>
    <div>
        <b-row align-v="center">
            <b-col cols="2" align="center"><b-img v-bind="imageProps" rounded="circle" alt="rounded image"></b-img></b-col>
            <b-col cols="10" class="text-break mt-2">
                <b-form-textarea v-model="text" placeholder="What's Happening?" class="tweetarea" size="lg"></b-form-textarea>
            </b-col>
        </b-row>
        <!--world icon here-->
        <b-row class="pl-2">
            <b-col></b-col>
            <b-col cols="10" class="p-2">
                <div class="border-bottom border-secondary text-primary">
                    <b-icon icon="cup"></b-icon> Everyone can reply
                </div>
            </b-col>
        </b-row>
        <b-row>
            <b-col></b-col>
            <b-col></b-col>
            <b-col cols="2"><b-button pill variant="primary" v-on:click="sendTweet">Tweet</b-button></b-col>
        </b-row>
    </div>
</template>

<script>
    import axios from 'axios'
    export default {
        name: "PostTweet",
        data() {
            return {
                text: null,
                imageProps: { blank: true, blankColor: '#7777', height: 60, width: 60 }
            }
        },
        methods: {
            sendTweet: function () {
                console.log(this.$store.getters.user)
                axios.post("https://localhost:5001/api/Tweets", {
                    UserId: this.$store.getters.user.userId,
                    TextContent: this.text,
                    // add idToken for validation.
                }, {
                    headers: {
                        'Authorization': 'Bearer ' + this.$store.getters.user.idToken
                    }
                }).then(res => {
                    console.log(res)
                    this.postSuccess()
                }).catch(error => {
                    console.log(error)
                })
            },
            postSuccess: function () {
                this.text = null;
            },
            postFail: function () {

            }
        },
        props: {
            privacy: Boolean
        }
    }
</script>
<style>
    .tweetarea {
        border: none;
        background-color: transparent;
        resize: none;
        outline: none;
        padding-left: 0;
    }

        .tweetarea:focus {
            border: none;
            background-color: transparent;
            resize: none;
            outline: none;
            padding-left: 0;
            outline: 0px !important;
            -webkit-appearance: none;
            box-shadow: none !important;
        }
</style>