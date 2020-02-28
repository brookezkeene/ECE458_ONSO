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

            <v-card-title> Network Port Format </v-card-title>
            <v-card-text>
                • src_hostname – string; matches the hostname of an existing asset in the system <br>
                • src_port – string; matches a network port name defined by the source asset’s model <br>
                • src_mac – six-byte MAC address; format must comply with Requirement 2.2.1.5; sets
                this value for the associated src port <br>
                • dest_hostname – string; matches the hostname of an existing asset in the system;
                leaving blank will disconnect src port if it’s currently connected <br>
                • dest_port – string; matches a network port name defined by the destination asset’s
                model; must be given a value if a value is given for dest hostname; must be left blank
                if dest hostname is left blank <br>
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