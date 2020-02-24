
/* eslint-disable no-unused-vars, no-console */

const mockLogs = [
        {
            id: 0,
            event: 'Event',
            source: 'Source',
            category: 'Category',
            subjectIdentifier: "Subject Identifier",
            subjectName: "Subject Name",
            subjectType: "Subject Type",
            action: "action",
            data: "Data",
            created: "2020-02-23T04:15:58.773Z"
        }
];

export default {
    list() {
        return Promise.resolve(mockLogs);
    },

}