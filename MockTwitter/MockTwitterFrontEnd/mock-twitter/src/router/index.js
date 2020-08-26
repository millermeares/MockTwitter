import Vue from 'vue'
import VueRouter from 'vue-router'
//import LoginComponent from "../views/Login.vue"
//import HomeComponent from "../views/Home.vue"
//import CreateAccountComponent from "../views/CreateAccount.vue"
//import AboutComponent from "../views/About.vue"
//import SettingsComponent from "../views/Settings.vue"
//import AppComponent from "../App.vue"

Vue.use(VueRouter)
const routes = [
    
    //component: () => import(/* webpackChunkName: "about" */ '../views/About.vue')
]

const router = new VueRouter({
    mode: 'history',
    base: process.env.BASE_URL,
    routes
})

export default router
