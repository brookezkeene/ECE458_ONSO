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
                For example: “aaa”, “b””bb”, “ccc”<br> <br>
                • A header row is NOT optional. See Section 2 for a list of valid column headers. <br>
                • On import, both *nix (LF) and Windows-style (CRLF) line endings will be accepted. <br>
                On export, Windows-style line endings (CRLF) will be emitted in conformance to the
                standard. <br>
                – The same rules apply to line breaks within multi-line string values, i.e. Model →
                Comment and Asset → Comment.<br>
                • For both import and export, the UTF-8 charset will be used.<br>
                – Double quotes should use code U+0022 QUOTATION MARK.<br>
                – Commas should use code U+002C COMMA.<br>
                • Both files containing and not containing a BOM character at the beginning of the file
                will be accepted on import, but a BOM will not be included on export<br>

            </v-card-text>

            <v-card-title> Model Format </v-card-title>
            <v-card-text>
                • vendor – required always; string <br>
                • model_number – required always; string <br>
                • height – required always; positive integer; refers to the height in U of the model <br>
                • display_color – optional; 6-digit, 3-byte hex triplet (RGB) preceded by a pound sign (#); case insensitive; e.g. #7FFFD4, #7fffd4 <br>
                • ethernet_ports – optional; non-negative integer <br>
                • power_ports – optional; non-negative integer <br>
                • cpu – optional; string <br>
                • memory – optional; non-negative integer <br>
                • storage – optional; string <br>
                • comment – optional; string; must be enclosed by double quotes if value contains line breaks <br>
                • network_port_name_1 – string; if left blank and network port exists, value defaults to "1"; ignored if network ports less than 1 <br>
                • network_port_name_2 – string; if left blank and network port exists, value defaults to "2"; ignored if network ports less than 2 <br>
                • network_port_name_3 – string; if left blank and network port exists, value defaults to ”3”; ignored if network ports less than 3 <br>
                • network_port_name_4 – string; if left blank and network port exists, value defaults to ”4”; ignored if network ports less than 4 <br>

            </v-card-text>


        </v-card>
    </div>
</template>

<script>
    export default {
        name: 'model-input-format-info',
        data() {
            return {
                loading: false,
            };
        }
    }
</script>