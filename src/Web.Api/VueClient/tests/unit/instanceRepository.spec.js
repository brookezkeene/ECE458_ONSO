import instanceRepository from '@/repositories/mock/instance'

describe('instance repository', () => {
    const instanceToCreate = {
        "id": 99,
        "hostname": "foobar"
    }

    test('find rejects if instance not found', async () => {
        await expect(instanceRepository.find('definitely-not-a-real-id')).rejects.toBeUndefined();
    })

    test('find returns object', async () => {
        await expect(instanceRepository.find(1)).resolves.toBeInstanceOf(Object);
    })

    test('list returns array', async () => {
        await expect(instanceRepository.list()).resolves.toBeInstanceOf(Array);
    })

    test('creates new instances', async () => {
        await expect(instanceRepository.create(instanceToCreate)).resolves.toBeUndefined();
        await expect(instanceRepository.find(instanceToCreate.id)).resolves.toEqual(instanceToCreate);
    })

    test('create rejects if instance already exists', async () => {
        await expect(instanceRepository.create(instanceToCreate)).rejects.toBeUndefined();
    })

    test('updates existing instances', async () => {
        const updatedInstance = Object.assign({}, instanceToCreate);
        updatedInstance.displayName = "changed";
        await expect(instanceRepository.update(updatedInstance)).resolves.toBeUndefined();
        await expect(instanceRepository.find(updatedInstance.id)).resolves.toEqual(updatedInstance);
    })

    test('update rejects if instance not found', async () => {
        const instanceToUpdate = {
            id: 'definitely-not-a-real-id'
        }
        await expect(instanceRepository.update(instanceToUpdate)).rejects.toBeUndefined();
    })

    test('deletes existing instances', async () => {
        await expect(instanceRepository.delete(instanceToCreate)).resolves.toBeUndefined();
        await expect(instanceRepository.find(instanceToCreate.id)).rejects.toBeUndefined();
    })

    test('delete rejects if instance not found', async () => {
        const instanceToDelete = {
            id: 'definitely-not-a-real-id'
        }
        await expect(instanceRepository.delete(instanceToDelete)).rejects.toBeUndefined();
    })
})