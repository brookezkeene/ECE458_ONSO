<template>
    <div>
        <v-card flat>
            <v-card-title>
                <span class="headline">New User</span>
            </v-card-title>

            <v-card-text>
                <v-form ref="form"
                        v-model="valid">
                    <v-row>
                        <v-col cols="12" sm="6" md="6">
                            <v-text-field v-model="editedItem.firstName" label="First Name" :rules="[rules.required]"></v-text-field>
                        </v-col>
                        <v-col cols="12" sm="6" md="6">
                            <v-text-field v-model="editedItem.lastName" label="Last Name" :rules="[rules.required]"></v-text-field>
                        </v-col>
                    </v-row>
                    <v-row>
                        <v-col cols="12" sm="6" md="6">
                            <v-text-field v-model="editedItem.username" label="Username" :rules="[rules.required]"></v-text-field>
                        </v-col>
                        <v-col cols="12" sm="6" md="6">
                            <v-text-field v-model="editedItem.email" label="Email" :rules="[rules.required]"></v-text-field>
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
                </v-form>

                <v-card-actions>
                    <v-spacer></v-spacer>
                    <v-btn color="primary" text @click="close">Cancel</v-btn>
                    <v-btn color="primary" text @click="save" :disabled="!valid">Create User</v-btn>
                </v-card-actions>

            </v-card-text>
            <v-snackbar v-model="updateSnackbar.show"
                        :bottom=true
                        class="black--text"
                        :color="updateSnackbar.color"
                        :timeout=5000>
                {{updateSnackbar.message}}
                <v-btn dark
                       class="black--text"
                       text
                       @click="updateSnackbar.show = false">
                    Close
                </v-btn>
            </v-snackbar>
        </v-card>
    </div>
</template>

<script>
export default {
    name: 'user-form',
    inject: ['userRepository'],
    item: null,
    props: {
    },
    data () {
        return {
            updateSnackbar: {
                    show: false,
                    message: '',
                    color: ''
             },
            show: false,
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
                passwordLengthRules:
                    v => {
                        if (v.length < 6) {
                            return 'Need stronger password with at least 6 characters';
                        } else {
                            return !!v;
                        }
                    },
            },
            editedItem: {
                firstName: '',
                lastName: '',
                username: '',
                email: '',
                password: '',
            }
        };
    },
    methods: {
        async save() {
            var result = await this.userRepository.create(this.editedItem)
            if (result != null &&  result.id.length == 0) {
                this.updateSnackbar.show = true;
                this.updateSnackbar.color = 'red lighten-4';
                this.updateSnackbar.message = 'Failed to create users. Users cannot have the same username';
                return;
            }
            this.close()
        },
        close() {
            this.$router.push({name: 'users'})
        },
        validationAfterReturning(result) {
            if (result != null && (result.id == null || result.id.length == 0)) {
                this.updateSnackbar.show = true;
                this.updateSnackbar.color = 'red lighten-4';
                this.updateSnackbar.message = 'Failed to create users. Users cannot have the same username';
                return 1;
            }
            return 0;
        },
        validationInputs(item) {
            var count = 0;
            if (item.firstName == null || item.firstName.length == 0) {
                this.updateSnackbar.show = true;
                this.updateSnackbar.color = 'red lighten-4';
                this.updateSnackbar.message = 'First name is required. ';
                count++;
            }
            if (item.lastName == null || item.lastName.length == 0) {
                this.updateSnackbar.show = true;
                this.updateSnackbar.color = 'red lighten-4';
                this.updateSnackbar.message = this.updateSnackbar.message + 'Last name is required. ';
                count++;
            }
            if (item.email == null || item.email.length == 0 || !(/.+@.+\..+/.test(item.email))) {
                this.updateSnackbar.show = true;
                this.updateSnackbar.color = 'red lighten-4';
                this.updateSnackbar.message = this.updateSnackbar.message + 'A valid email address is required. ';
                count++;
            }
            if (item.username == null || item.username.length == 0) {
                this.updateSnackbar.show = true;
                this.updateSnackbar.color = 'red lighten-4';
                this.updateSnackbar.message = this.updateSnackbar.message + 'A username is required. ';
                count++;
            }
            return count;
        }
    }
}
</script>