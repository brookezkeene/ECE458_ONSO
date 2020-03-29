<template>
    <v-card flat>
        <changePlanBar></changePlanBar>
        <v-card-title>Change Planner</v-card-title>
        <v-container>
            <v-card>
                <v-spacer></v-spacer>
                <!--TODO: change for change plans -->
                <v-data-table :headers="headers"
                              :items="assets"
                              :search="search"
                              class="elevation-1 pa-10"
                              multi-sort
                              @click:row="showDetails">

                    <template v-slot:item.name="props">
                        <v-edit-dialog :return-value.sync="props.item.name">
                            {{ props.item.name }}
                            <template v-if="!props.item.executedDate" v-slot:input>
                                <v-text-field v-model="props.item.name"
                                                :rules="[rules.nameRules]"
                                                label="Edit"
                                                single-line
                                                counter
                                                @input="saveName(props.item)"></v-text-field>
                                </template>
                        </v-edit-dialog>
                    </template>

                    <template v-slot:top>
                        <v-toolbar flat class="mb-6">
                            <v-label>Filter by ... </v-label>
                            <v-spacer></v-spacer>
                            <v-autocomplete prepend-inner-icon="mdi-magnify"
                                            :items="assets"
                                            :search-input.sync="search"
                                            cache-items
                                            flat
                                            hide-no-data
                                            hide-details
                                            item-text="vendor"
                                            label="Search"
                                            single-line
                                            solo-inverted></v-autocomplete>
                            <v-spacer></v-spacer>
                            <v-btn color="primary" dark class="mb-2" @click="addItem">Add Change Plan</v-btn>
                        </v-toolbar>
                    </template>

                    <template v-slot:item.executed="{ item }">
                        <div v-if="item.executedDate">
                            <v-icon color="primary">
                                mdi-check-circle-outline
                            </v-icon>
                        </div>
                        <div v-else>
                            <v-icon>
                                mdi-minus
                            </v-icon>
                        </div>
                    </template>

                    <template v-slot:item.action="{ item }">
                        <v-row>
                            <v-tooltip top>
                                <template v-if="!item.executedDate" v-slot:activator="{ on }">
                                    <v-btn icon v-on="on"
                                           @click="editItem(item)">
                                        <v-icon medium
                                                class="mr-2">
                                            mdi-pencil
                                        </v-icon>
                                    </v-btn>
                                </template>

                                <span>Edit</span>
                            </v-tooltip>

                            <v-tooltip top>
                                <template v-slot:activator="{ on }">
                                    <v-btn icon v-on="on"
                                           @click="printItem(item)">
                                        <v-icon medium
                                                class="mr-2">
                                            mdi-printer
                                        </v-icon>
                                    </v-btn>
                                </template>

                                <span>Print Work Order</span>
                            </v-tooltip>

                            <v-tooltip top>
                                <template v-if="!item.executedDate" v-slot:activator="{ on }">
                                    <v-btn icon v-on="on"
                                           @click="deleteItem(item)">
                                        <v-icon medium
                                                class="mr-2">
                                            mdi-delete
                                        </v-icon>
                                    </v-btn>
                                </template>

                                <span>Delete</span>
                            </v-tooltip>
                        </v-row>
                    </template>
                </v-data-table>
            </v-card>
        </v-container>
    </v-card>
</template>

<script>

    import changePlanBar from '@/components/ChangePlanStatusBar';

    export default {
        components: {
            changePlanBar
        },
        inject: ['assetRepository','changePlanRepository'],
        data() {
            return {
            loading: true,
            search: '',
            // TODO: replace with change plan data
            headers: [
                { text: 'Datacenter', value: 'datacenterName' },
                { text: 'Change Plan Name', value: 'name' },
                { text: 'Executed', value: 'executed', },
                { text: 'Date & Time', value: 'createdDate' },
                { text: 'Actions', value: 'action', sortable: false },
            ],
            assets: [],
            defaultItem: {
                id: '',
                datacenter: '',
                modelId: '',
                hostname: '',
                rackId: '',
                rackPosition: '',
                ownerId: '',
                comment: '',
                poweredOn: false,
            },
                editing: false,
            rules: {
                nameRules: v => /^(?=\s*\S).*$/.test(v) || 'Name is required',
                // TODO: add rule for datacenter
            },
        }
    },
    async created() {
        this.initialize()
    },

    methods: {
        /*eslint-disable*/
        async initialize() {
            this.assets = await this.changePlanRepository.list(this.$store.getters.userId); //only list change plans for a given user
            console.log(this.assets);
            this.loading = false;
        },
        deleteItem (item) {
            this.editing = true;
            console.log("delete" + item);
            confirm('Are you sure you want to delete this asset?') && this.changePlanRepository.delete(item)
                    .then(async () => {
                        await this.initialize();
                    })
        },
        printItem (item) {
            //TODO: print work order code
            console.log("print" + item);
        },
        editItem(item) {
            this.editing = true;
            //TODO: edit change plan code, change this probably
            console.log(item);
            var changePlan = {
                name: item.name,
                datacenterName: item.datacenterName,
                datacenterDescription: item.datacenterDescription,
                datacenterId: item.datacenterId,
                changePlanId: item.id
            };
            this.$store.dispatch('startChangePlan', changePlan);
            this.$router.push({ name: 'assets'})
        },
        addItem(item) {
            //TODO: adde new change plan code, change this probably
            
            this.$router.push({ name: 'change-plan-new' })
        },
        // Don't think we will need clickable rows here since we have the drop down
        showDetails(item) {
            //TODO: show change plan summary code
            if (!this.editing) {
                console.log(item);
                this.$router.push({ name: 'change-plan-details', params: {id: item.id } })
            }
            this.editing = false;
        },
        async getChangePlanItems(changeplanId) {
            var items = await this.changePlanRepository.listItems(changeplanId);
            /*eslint-disable*/
            console.log(items);
            return items;
        },
        saveName(item) {
            console.log(item);
            this.changePlanRepository.update(item)
        }
    },
  }
</script>