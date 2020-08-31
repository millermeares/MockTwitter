import Vue from 'vue'
import VueRouter from 'vue-router'
import LoginComponent from "../views/Login.vue"
import HomeComponent from "../views/Home.vue"
import CreateAccountComponent from "../views/CreateAccount.vue"
import WelcomeComponent from "../views/Welcome.vue"
//import AboutComponent from "../views/About.vue"
//import SettingsComponent from "../views/Settings.vue"
import { store } from '../store/store.js'

Vue.use(VueRouter)
const routes = [
    //{ name: "App", path: "/", component: AppComponent}
    //
    //
    //component: () => import(/* webpackChunkName: "about" */ '../views/About.vue')
    {
        path: '/login', 
        component: LoginComponent
    },
    {
        path: '/createaccount',
        component: CreateAccountComponent
    },
    {
        path: '/home',
        component: HomeComponent,
        beforeEnter(to, from, next) {
            if (store.getters.isAuthenticated) {
                next()
            } else {
                next('/login')
            }
        }
    },
    {
        path: '/', component: WelcomeComponent,
    }
]

const router = new VueRouter({
    mode: 'history',
    base: process.env.BASE_URL,
    routes
})

export default router
