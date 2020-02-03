<template>
    <div v-if="!loading">
        <v-card>
            <v-card-title> General Format </v-card-title>
            <v-card-text>
                • Each record is located on a separate line, delimited by a line break. For example: aaa,bbb,ccc CRLF
                zzz,yyy,xxx CRLF <br>
                • The last record in the file may or may not have an ending line break. <br>
                • Each record should contain the same number of comma-separated fields. <br>
                • Spaces are considered part of a field and should not be ignored. <br>
                • Each field may or may not be enclosed in double quotes. <br>
                • Fields containing a line-break, double-quote, or commas must be quoted. <br>
                • If double-quotes are used to enclose fields, then a double quote appearing inside a field must be escaped by preceding it with another double quote. <br>
                For example: “aaa”, “b””bb”, “ccc”<br>
            </v-card-text>
            <v-card-title> Instance Format </v-card-title>
            <v-card-text>
                • hostname – required always; RFC-1034-compliant string<br>
                • rack – required always; string; the address of a rack is by a row letter (A-Z) and rack number (positive integer); there is no separator between the row letter and rack number<br>
                • rack position – required always; positive integer; refers to the vertical location (on a rack, measured in U) of the bottom of the equipment<br>
                • vendor – required always; string; refers to the vendor of the model with which this instance is associated<br>
                • model number – required always; string; refers to the model number of the model with which this instance is associated<br>
                • owner – optional; string; refers to the username of an existing user in the system who owns this equipment<br>
                • comment – optional; string; must be enclosed by double quotes if value contains line breaks<br>
            </v-card-text>
        </v-card>
    </div>
</template>

<script>
export default {
    name: 'model-input-format-info',
    data () {
        return {
            loading: false,
        };
    }
}   
</script>