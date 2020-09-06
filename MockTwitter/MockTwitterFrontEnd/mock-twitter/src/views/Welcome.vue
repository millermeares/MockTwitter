<template>
    <div class="jumbotron vertical-center">

        <b-container class="w-50 mx-auto">
            <h1 class="mb-2 text-primary text-center">Welcome to fake twitter!!</h1>
            <b-row><br /></b-row>

            <div>
                <b-row class="mb-2">
                    <b-col align="center"><b-button variant="outline-primary" to="/login">Login</b-button></b-col>
                    <b-col align="center"><b-button variant="outline-primary" to="/createaccount">Create An Account!</b-button></b-col>
                </b-row>
            </div>
        </b-container>
        <router-view />
    </div>
</template>
<style>
    .vertical-center {
        min-height: 100vh; 
        display: flex;
        align-items: center;
    }
</style>