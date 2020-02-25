<template>
    <div v-if="!loading">
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
                        <v-col cols="12" sm="6" md="6">
                            <v-select v-model="editedItem.role" label="Permission Level" :items="permissions"></v-select> <!--Add permissions field to v-model-->
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
            permissions: [
                { text: 'Regular', value: 'basic' },
                { text: 'Administrator', value: 'admin' },
            ],
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
            loading: false,
            editedItem: {
                firstName: '',
                lastName: '',
                username: '',
                email: '',
                password: '',
                role: ''
            },
            roleItem: {
                id:'',
                name:'',
            },
            users: [],
        };
        },

    
        async created() {
            this.initialize();
            
        },

    methods: {
        async initialize() {
            this.users = await this.userRepository.list();

            this.editedItem = typeof this.id === 'undefined'
                ? this.editedItem
                : this.users.find(o => o.id === this.id);
        },
        checkPassMatch() {
            if (this.password1 != this.password2) {
                this.snackbar = false
            } else {
                this.editedItem.password = this.password1;
            }
        },
        save() {
            this.userRepository.create(this.editedItem)
                .then(async () => {
                            await this.initialize();
                })
            this.close()
        },
        close() {
            this.$router.push({name: 'users'})
        }
    },
    computed: {
        passwordConfirmationRule() {
            return this.password1 === this.password2 || "Passwords must match";
        },
    }
}
</script>