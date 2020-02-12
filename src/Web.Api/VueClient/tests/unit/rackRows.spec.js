import rackDiagram from '@/rackDiagram';

var instances = [
    {
        "id": 1,
        "model": {
            "id": 1,
            "vendor": "Dell",
            "modelNumber": "R710",
            "height": 4,
            "displayColor": "#82E0AA",
            "ethernetPorts": 2,
            "powerPorts": 1,
            "cpu": "Intel Xeon E5520 2.2GHz",
            "memory": 32,
            "storage": "2x500GB SSD RAID1",
            "comment": "Retired offering, no new purchasing"
        },
        "hostname": "server1",
        "rack": "B12",
        "rackPosition": 4,
        "owner": {
            "id": 1,
            "username": "tbletsch",
            "displayName": "Tyler Bletsch",
            "email": "Tyler.Bletsch@duke.edu",
        },
        "comment": "Reserved for Palaemon project"
    },
    {
        "id": 2,
        "model": {
            "id": 2,
            "vendor": "Dell",
            "modelNumber": "R710",
            "height": 4,
            "displayColor": "#C39BD3",
            "ethernetPorts": 2,
            "powerPorts": 1,
            "cpu": "Intel Xeon E5520 2.2GHz",
            "memory": 32,
            "storage": "2x500GB SSD RAID1",
            "comment": "This is my personal favorite!",
            "instances": ["instance3", "instance4", "instance5"]
        },
        "hostname": "server2",
        "rack": "B12",
        "rackPosition": 12,
        "owner": {
            "id": 1,
            "username": "tbletsch",
            "displayName": "Gaby Rodriguez-Florido",
            "email": "gr64@duke.edu",
        },
        "comment": "Reserved for 458 project"
    },
];

const slots = [
    { rackU: 1, value: '', style: '' },
    { rackU: 2, value: '', style: '' },
    { rackU: 3, value: '', style: '' },
    { rackU: 4, value: { text: 'Dell R710 server1', id: '1'}, style: { color: 'black', backgroundColor: '#82E0AA' } },
    { rackU: 5, value: '', style: { color: 'black', backgroundColor: '#82E0AA' } },
    { rackU: 6, value: '', style: { color: 'black', backgroundColor: '#82E0AA' } },
    { rackU: 7, value: '', style: { color: 'black', backgroundColor: '#82E0AA' } },
    { rackU: 8, value: '', style: '' },
    { rackU: 9, value: '', style: '' },
    { rackU: 10, value: '', style: '' },
    { rackU: 11, value: '', style: '' },
    { rackU: 12, value: { text: 'Dell R710 server2', id: '2'}, style: { color: 'black', backgroundColor: '#C39BD3' } },
    { rackU: 13, value: '', style: { color: 'black', backgroundColor: '#C39BD3' } },
    { rackU: 14, value: '', style: { color: 'black', backgroundColor: '#C39BD3' } },
    { rackU: 15, value: '', style: { color: 'black', backgroundColor: '#C39BD3' } },
    { rackU: 16, value: '', style: '' },
    { rackU: 17, value: '', style: '' },
    { rackU: 18, value: '', style: '' },
    { rackU: 19, value: '', style: '' },
    { rackU: 20, value: '', style: '' },
    { rackU: 21, value: '', style: '' },
    { rackU: 22, value: '', style: '' },
    { rackU: 23, value: '', style: '' },
    { rackU: 24, value: '', style: '' },
    { rackU: 25, value: '', style: '' },
    { rackU: 26, value: '', style: '' },
    { rackU: 27, value: '', style: '' },
    { rackU: 28, value: '', style: '' },
    { rackU: 29, value: '', style: '' },
    { rackU: 30, value: '', style: '' },
    { rackU: 31, value: '', style: '' },
    { rackU: 32, value: '', style: '' },
    { rackU: 33, value: '', style: '' },
    { rackU: 34, value: '', style: '' },
    { rackU: 35, value: '', style: '' },
    { rackU: 36, value: '', style: '' },
    { rackU: 37, value: '', style: '' },
    { rackU: 38, value: '', style: '' },
    { rackU: 39, value: '', style: '' },
    { rackU: 40, value: '', style: '' },
    { rackU: 41, value: '', style: '' },
    { rackU: 42, value: '', style: '' },
].reverse();

const rows = [
  
    {
        rowLetter: 'B',
        racks: [
            {
                address: 'B12',
                slots: slots
            },
            {
                address: 'B13',
                slots: slots
            },
        ],
    }
];

describe('rack repository', () => {

    test('create slots', async () => {
        await expect(rackDiagram.createSlot(instances)).toEqual(slots);
    })

    test('mock racks by rows', async () => {
        const start = "B12", end = "B13";
        const diagram = await rackDiagram.createRacksByRows(start, end);
        await expect(diagram).toEqual(rows);
    })

})