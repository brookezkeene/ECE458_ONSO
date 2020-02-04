<template>
    <v-container fill-height fluid>
        <v-row justify="center">
            <v-img src="./example/hyposoft_logo.png"
                   max-width="300px"></v-img>
        </v-row>
        
        <v-row justify="center">
            <v-card width="50%">
                <v-card-title class="justify-center">
                    Log In
                </v-card-title>
                <v-card-text class="justify-center">
                    <v-form ref="form"
                            v-model="valid">
                        <v-container>
                            <v-row>
                                <v-text-field v-model="username"
                                              :rules="usernameRules"
                                              label="Username"
                                              required></v-text-field>
                            </v-row>
                            <v-row>
                                <v-text-field v-model="password"
                                              :rules="passwordRules"
                                              :type="'password'"
                                              label="Password"
                                              required></v-text-field>
                            </v-row>
                        </v-container>
                    </v-form>
                    <v-btn :disabled="!valid"
                           color="primary"
                           class="mr-4"
                           @click="validate">
                        Sign In
                    </v-btn>
                </v-card-text>
            </v-card>
        </v-row>
    </v-container>
</template>

<script>
export default {
    data: () => ({
      valid: true,
      email: '',
      usernameRules: [
        v => !!v || 'Username is required',
      ],
      password: '',
      passwordRules: [
          v => !!v || 'Password is required',
      ]
    }),
    methods: {
      validate () {
        if (this.$refs.form.validate()) {
          this.snackbar = true
          this.$router.push('Models')
        }
      },
    }
}
</script>