<template>
    <div v-if="!loading">
        <v-card flat>
            <v-card-title>
            <span class="headline">New User</span>
            </v-card-title>

            <v-card-text>
            <v-container>
                <v-row>
                <v-col cols="12" sm="6" md="6">
                    <v-text-field v-model="editedItem.firstName" label="First Name"></v-text-field>
                </v-col>
                <v-col cols="12" sm="6" md="6">
                    <v-text-field v-model="editedItem.lastName" label="Last Name"></v-text-field>
                </v-col>
                </v-row>
                <v-row>
                    <v-col cols="12" sm="6" md="6">
                        <v-text-field v-model="editedItem.username" label="Username"></v-text-field>
                    </v-col>
                    <v-col cols="12" sm="6" md="6">
                        <v-text-field v-model="editedItem.email" label="Email"></v-text-field>
                    </v-col>
                </v-row>
                <v-row>

                <v-col>
                    <v-text-field v-model="editedItem.password"
                                  :append-icon="show ? 'mdi-eye' : 'mdi-eye-off'"
                                  :rules="[rules.passwordLengthRules]"
                                  :type="show ? 'text' : 'password'"
                                  label="Password"
                                  required
                                  @click:append="show = !show"></v-text-field>
                </v-col>
                </v-row>
            </v-container>
            </v-card-text>
        </v-card>
    </div>
</template>

<script>
export default {
    name: 'user-form',
    inject: ['userRepository'],
    item: null,
    props: {
        editedItem: Object
    },
    data () {
        return {
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
                emailMatch: () => "The email and password you entered don't match",
                passwordLengthRules:
                    v => {
                        if (v.length < 6) {
                            return 'Need stronger password with at least 6 characters';
                        } else {
                            return !!v;
                        }
                    },
            },
            loading: false
        };
    },
    methods: {
        submit() {
            this.$v.$touch()
        },
        checkPassMatch() {
            if (this.password1 != this.password2) {
                this.snackbar = false
            } else {
                this.editedItem.password = this.password1;
            }
        },
    },
    computed: {
        passwordConfirmationRule() {
            return this.password1 === this.password2 || "Passwords must match";
        },
    }
}
</script>