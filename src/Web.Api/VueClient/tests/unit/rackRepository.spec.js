import rackRepository from '@/repositories/mock/rack'

describe('rack repository', () => {
    const rackToCreate = {
        "id": 99,
        "rackname": "tbletsch",
        "displayName": "Tyler Bletsch",
        "email": "Tyler.Bletsch@duke.edu"
    }

    test('find rejects if rack not found', async () => {
        await expect(rackRepository.find('definitely-not-a-real-id')).rejects.toBeUndefined();
    })

    test('find returns object', async () => {
        await expect(rackRepository.find(1)).resolves.toBeInstanceOf(Object);
    })

    test('list returns array', async () => {
        await expect(rackRepository.list()).resolves.toBeInstanceOf(Array);
    })

    test('validates rack addresses', () => {
        const validAddresses = ['a1', 'A1', 'B10', 'B100', 'Z50'];
        const invalidAddresses = ['A1A', 'AA1', '10', '10A'];

        validAddresses.forEach(addr => expect(rackRepository.validAddress(addr)).toBe(true));
        invalidAddresses.forEach(addr => expect(rackRepository.validAddress(addr)).toBe(false));
    })

    test('splits rack addresses', () => {
        const inputExpectedOutput = [
            { in: 'A12', out: { rowLetter: 'A', rackNumber: 12 } },
            { in: 'B1', out: { rowLetter: 'B', rackNumber: 1 } },
            { in: 'b1', out: { rowLetter: 'B', rackNumber: 1 } },
        ];

        inputExpectedOutput.forEach(o => expect(rackRepository.splitAddress(o.in)).toEqual(o.out));
    })

    test('returns racks in range', async () => {
        const start = "B12", end = "B13";
        await expect(rackRepository.findInRange(start, end)).resolves.toHaveLength(2);
    })

    test('creates racks in range', async () => {
        const start = "A5", end = "B6";
        const originalLength = (await rackRepository.list()).length;
        await expect(rackRepository.createInRange(start, end)).resolves.toBeUndefined();
        await expect(rackRepository.list()).resolves.toHaveLength(originalLength + 4);
    })

    test('deletes racks in range', async () => {
        const start = "A5", end = "B6";
        const originalLength = (await rackRepository.list()).length;
        await expect(rackRepository.deleteInRange(start, end)).resolves.toBeUndefined();
        await expect(rackRepository.list()).resolves.toHaveLength(originalLength - 4);
    })

    test('create slots', async () => {
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
            { rackU: 4, value: 'Dell r710 server1', style: { color: 'black', backgroundColor: '#82E0AA' } },
            { rackU: 5, value: '', style: { color: 'black', backgroundColor: '#82E0AA' } },
            { rackU: 6, value: '', style: { color: 'black', backgroundColor: '#82E0AA' } },
            { rackU: 7, value: '', style: { color: 'black', backgroundColor: '#82E0AA' } },
            { rackU: 8, value: '', style: '' },
            { rackU: 9, value: '', style: '' },
            { rackU: 10, value: '', style: '' },
            { rackU: 11, value: '', style: '' },
            { rackU: 12, value: 'Dell r710 server2', style: { color: 'black', backgroundColor: '#C39BD3' } },
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
        ];
        await expect(rackRepository.createSlot(instances)).toEqual(slots);

    })
})