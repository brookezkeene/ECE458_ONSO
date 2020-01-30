<template>
    <v-card>
        <v-card-title>
          Create User Account
        </v-card-title>
        <v-form
            ref="form"
            v-model = "valid">
            <v-text-field
                v-model="email"
                :rules="emailRules"
                label="Email"
                required
            ></v-text-field>
            <v-text-field
                v-model="password1"
                :append-icon="show ? 'mdi-eye' : 'mdi-eye-off'"
                :rules="[rules.required, passwordConfirmationRule]"
                :type="show ? 'text' : 'password'"
                label="Password"
                required
                @click:append="show = !show"
            ></v-text-field>
            <v-text-field
                v-model="password2"
                :append-icon="show1 ? 'mdi-eye' : 'mdi-eye-off'"                
                :rules="[rules.required, passwordConfirmationRule]"
                :type="show1 ? 'text' : 'password'"
                label="Re-enter Password"
                required
                @click:append="show1 = !show1"
            ></v-text-field>
        </v-form>
        <v-btn
            :disabled="!valid"
            color="success"
            class="mr-4"
            @click="submit"
        >
            Sign In
      </v-btn>
    </v-card>
</template>

<script>
export default {
    data: () => ({
      show: false,
      show1: false,
      valid: true,
      email: '',
      emailRules: [
        v => !!v || 'E-mail is required',
        v => /.+@.+\..+/.test(v) || 'E-mail must be valid',
      ],
      password1: '',
      password2: '',
      rules: {
      required: value => !!value || "Required.",
      emailMatch: () => "The email and password you entered don't match"
      }
    }),
    methods: {
      submit () {
        this.$v.$touch()
      },
      checkPassMatch() {
        if(this.password1 != this.password2) {
          this.snackbar = false
        }
      },
    },
    computed: {
      passwordConfirmationRule() {
        return this.password1 === this.password2 || "Passwords must match";      },
    }
}
</script>