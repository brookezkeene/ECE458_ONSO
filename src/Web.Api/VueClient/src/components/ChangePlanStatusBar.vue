
<template>
    <v-snackbar class="green--text text--darken-4" v-model="changePlan" top color="#8eff47" :timeout=0>
        <span>
            Editing Change Plan:&nbsp; 
        </span>
        <strong>
            {{changePlanName}}
        </strong>
        <v-icon @click="save">mdi-content-save-outline</v-icon>
        <v-icon @click="cancel">mdi-close</v-icon>
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
                if (confirm('Are you sure you would like to save this change plan?')) {
                    this.$store.dispatch('endChangePlan');
                    this.$router.push({ name:'change-planner' })
                    // Save the change plan via api call
                }
            },
            cancel() {
                if (confirm('Are you sure you would like to abandon these changes to the change plan?')) {
                    /*eslint-disable*/
                    this.$store.dispatch('endChangePlan');
                    this.$router.push({ name:'change-planner' })
                }
            }
        }

    }
</script>