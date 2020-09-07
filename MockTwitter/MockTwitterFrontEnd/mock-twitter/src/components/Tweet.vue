<template>
    <div class="border-bottom border-secondary">
        <b-row>
            <b-col cols="2" class="pb-2" align="center"><b-img v-bind="imageProps" rounded="circle" alt="rounded image"></b-img></b-col>
            <b-col>
                <b-row class="font-weight-bold">{{tweetData.username}}</b-row>
                <b-row class="font-weight-normal">{{tweetData.textContent}}</b-row>
                <b-row><b-col align="right">{{tweetData.createdAt}}</b-col></b-row>
            </b-col>
        </b-row>
    </div>
    <!--<b-img v-bind="imageProps" rounded="circle" alt="rounded image"></b-img>-->
</template>
<script>
    export default {
        name: 'Tweet',
        data() {
            return {
                imageProps: { blank: true, blankColor: '#979', height: 60, width: 60 },
            }
        },
        props: {
            tweetData: Object,
            username: String
        },
        computed: {
            tweeted() {
                var d = this.tweetData.createdAt;
                return d.getHours() + ":" + d.getMinutes() + " on " + d.getMonth() + "/" + d.getDay() + "/" + d.getFullYear();
            }
        }
    }
</script>