
<template>
    <v-snackbar class="green--text text--darken-4" v-model="changePlan" top color="#8eff47" :timeout=0>
        <span>
            Editing Change Plan:
            <strong>
                {{changePlanName}}
            </strong>
        </span>

        <v-icon @click="save">mdi-content-save-outline</v-icon>
    </v-snackbar>
</template>

<script>
    export default {
        name: 'change-plan-status',
        computed: {
            changePlan() {
                return this.$store.getters.isChangePlan;
            },
            changePlanName() {
                return this.$store.getters.changePlan.name;
            }
        },
        data() {
            return {
            }
        },
        methods: {
            save() {
                if (confirm('Are you sure you would like to finish editing this change plan?')) {
                    this.$store.dispatch('endChangePlan');
                    this.$router.push({ name:'change-plan' })
                    // Save the change plan via api call
                }
            },
            cancel() {
                if (confirm('Are you sure you would like to abandon these changes to the change plan?')) {
                    /*eslint-disable*/
                    this.$store.dispatch('endChangePlan');
                    this.$router.push({ name:'change-plan' })
                }
            }
        }

    }
</script>