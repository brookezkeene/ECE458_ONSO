<template>
    <v-card>
        <v-toolbar flat color="white">
            <v-toolbar-title>Racks</v-toolbar-title>
            <v-divider class="mx-4"
                       inset
                       vertical></v-divider>
        </v-toolbar>
        <rack-form />
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