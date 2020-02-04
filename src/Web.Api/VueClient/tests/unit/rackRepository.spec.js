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
})