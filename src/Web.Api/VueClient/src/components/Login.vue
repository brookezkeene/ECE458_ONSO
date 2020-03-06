<template>
    <v-container fill-height fluid>
        <v-row justify="center">
            <v-img src="../assets/hyposoft_logo.png"
                   max-width="300px"></v-img>
        </v-row>
        
        <v-row justify="center">
            <v-card width="50%" class="pa-10">
                <v-card-title class="justify-center">
                    Log In
                </v-card-title>
                <v-card-text class="justify-center">
                    <v-form ref="form"
                            v-model="valid"
                            @keyup.native.enter="valid && submit($event)">
                        <v-container>
                            <v-row>
                                <v-text-field v-model="username"
                                              prepend-inner-icon="mdi-account"
                                              :rules="usernameRules"
                                              label="Username"
                                              required></v-text-field>
                            </v-row>
                            <v-row>
                                <v-text-field v-model="password"
                                              class="pb-5"
                                              prepend-inner-icon="mdi-lock"
                                              :rules="passwordRules"
                                              :type="'password'"
                                              label="Password"
                                              required></v-text-field>
                            </v-row>
                        </v-container>
                    </v-form>
                    <v-card flat>
                        <v-layout align-center justify-center>
                            <v-btn :disabled="!valid"
                                    color="primary"
                                    type="submit"
                                    @click="submit">
                                Sign In
                            </v-btn>
                            <v-spacer></v-spacer>
                            <v-form action="/api/login/external" method="post">
                                <v-btn outlined color="primary" type="submit">Sign in with Duke NetID</v-btn>
                            </v-form>
                        </v-layout>
                    </v-card>
                </v-card-text>
            </v-card>
        </v-row>
    </v-container>
</template>

<script>
import Auth from "../auth"

export default {
    data: () => ({
      valid: true,
      username: '',
      usernameRules: [
        v => !!v || 'Username is required',
      ],
      password: '',
      passwordRules: [
          v => !!v || 'Password is required',
      ],
    }),
    methods: {
        submit() {
            if (this.$refs.form.validate()) {
                Auth.login(this.username, this.password)
                    .then(() => {
                        this.$router.push(this.$route.query.redirect || { name: 'App' } )
                    })
                    .catch((error) => {
                        alert(error.message)
                    });
            }
        }
     
    }
}
</script>