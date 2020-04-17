<template>
    <v-container class="card" fill-height fluid v-resize-text>
        <v-row justify="center" class="logo">
            <v-img src="../assets/hyposoft_logo.png"
                   max-width="250px"
                   class="logo"
                   contain
                   ></v-img>
        </v-row>
        
        <v-row justify="center">
            <v-card width="300px" class="pa-5 mobile" v-resize-text>
                <v-card-title class="justify-center">
                    Log In
                </v-card-title>
                <v-card-text class="justify-center">
                    <v-form ref="form"
                            v-model="valid"
                            @keyup.native.enter="valid && submit($event)"
                            v-resize-text>
                        <v-container class="mobile">
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
                    <v-card flat v-resize-text>
                        <v-layout align-center justify-center class="buttons" style="buttons">
                            <v-btn :disabled="!valid"
                                    color="primary"
                                    type="submit"
                                    @click="submit"
                                   class="buttons">
                                Sign In
                            </v-btn>
                            <v-spacer></v-spacer>
                            <v-form action="/api/login/external" method="post">
                                <v-btn outlined color="primary" type="submit" class="buttons">Duke NetID</v-btn>
                            </v-form>
                        </v-layout>
                    </v-card>
                </v-card-text>
            </v-card>
        </v-row>
    </v-container>
</template>

<style>
    
    @media only screen and (max-width: 576px) {
      .logo {
        height: 100px;
        width: 100px;
      }
      .mobile {
        width: 75%;
      }
      .buttons {
          flex-direction: column;
          width: 175px;
          align-content: center;
      }
    }
</style>

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
                        this.$router.push(this.$route.query.redirect || { name: 'dashboard' } )
                    })
                    .catch((error) => {
                        alert(error.message)
                    });
            }
        }
     
    }
}
</script>