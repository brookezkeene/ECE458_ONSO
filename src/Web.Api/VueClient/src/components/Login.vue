<template>
    <v-card>
        <v-card-title>
          Please sign in to your Object Not So Oriented Account!
        </v-card-title>
        <v-form
            ref="form"
            v-model = "valid">
            <v-text-field
                v-model="username"
                :rules="usernameRules"
                label="Username"
                required
            ></v-text-field>
            <v-text-field
                v-model="password"
                :rules="passwordRules"
                :type="'password'"
                label="Password"
                required
            ></v-text-field>
        </v-form>
        <v-btn
            :disabled="!valid"
            color="success"
            class="mr-4"
            @click="validate"
        >
            Sign In
        </v-btn>
    </v-card>
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
          this.$router.push('Main')
        }
      },
    }
}
</script>