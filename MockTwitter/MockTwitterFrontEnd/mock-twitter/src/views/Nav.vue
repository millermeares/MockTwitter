<b-container fluid>
    <div id="nav" v-if="authenticated">
        <b-navbar toggleable="lg" type="light" variant="primary">
            <b-navbar-brand to="/" align='center'>Mock Twitter</b-navbar-brand>
            <b-navbar-toggle target="nav-collapse"></b-navbar-toggle>
            <b-collapse id="nav-collapse" is-nav>
                <b-navbar-nav>
                    <b-nav-item to="/">Home</b-nav-item>
                    <b-nav-item to="/about">About</b-nav-item>
                </b-navbar-nav>

                <b-navbar-nav class="ml-auto">
                    <b-nav-item to="/settings">Settings</b-nav-item>
                    <b-nav-item to="/login" v-on:click="logout()" replace>Logout</b-nav-item>
                </b-navbar-nav>
            </b-collapse>
        </b-navbar>
    </div>
</b-container>