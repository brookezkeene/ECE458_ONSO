<template>
    <v-card flat>
        <v-card-title>Racks</v-card-title>
        <v-card-content class="center-justify">
            <rack-form />
        </v-card-content>
    </v-card>
</template>

<script>
import RackForm from "./RackForm"

export default {
    components: {
      RackForm,
    }
}
</script>