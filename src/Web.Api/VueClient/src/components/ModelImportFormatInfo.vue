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

            <v-card-title> Model Format </v-card-title>
            <v-card-text>
                • vendor – required always; string <br>
                • model number – required always; string <br>
                • height – required always; positive integer; refers to the height in U of the model <br>
                • display color – optional; 6-digit, 3-byte hex triplet (RGB) preceded by a pound sign (#); case insensitive; e.g. #7FFFD4, #7fffd4 <br>
                • ethernet ports – optional; non-negative integer <br>
                • power ports – optional; non-negative integer <br>
                • cpu – optional; string <br>
                • memory – optional; non-negative integer <br>
                • storage – optional; string <br>
                • comment – optional; string; must be enclosed by double quotes if value contains line breaks <br>
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