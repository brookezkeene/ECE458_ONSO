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

            <v-card-title> Asset Format </v-card-title>
            <v-card-text>
                • asset_number – six-digit integer; must be unique unless modifying an existing asset; if left blank, a value will be
                generated automatically; matching an existing asset number indicates that this row should modify an existing asset in the system <br>
                • hostname – complies with RFC 1034 definition of "label" <br>
                • datacenter – string; refers to the abbreviated form of an existing datacenter in the system (e.g. RTP1) <br>
                • rack – string; the address of a rack is by a row letter (A-Z) and rack number (positive integer); there is no separator
                between the row letter and rack number<br>
                • rack_position – positive integer; refers to the vertical location (on a rack, measured in U) of the bottom of the equipment<br>
                • vendor – string; refers to the vendor of the model with which this asset is associated <br>
                • model_number – string; refers to the model number of the model with which this asset is associated <br>
                • owner – string; refers to the username of an existing user in the system who owns this equipment <br>
                • comment – string; must be enclosed by double quotes if value contains line breaks <br>
                • power_port_connection_1 – reference to a PDU in the rack (L or R) and a port number (1-24); e.g. L5, R24;
                if left blank the power port will be disconnected <br>
                • power_port_connection_2 – reference to a PDU in the rack (L or R) and a port number (1-24); e.g. L5, R24;
                if left blank the power port will be disconnected <br>
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