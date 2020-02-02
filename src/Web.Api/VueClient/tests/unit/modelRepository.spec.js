import modelRepository from '@/repositories/model';

describe('model repository', () => {
    const modelToCreate = {
        "id": 99,
        "modelNumber": "original model number"
    }

    test('find rejects if model not found', async () => {
        await expect(modelRepository.find('definitely-not-a-real-id')).rejects.toBeUndefined();
    })

    test('find returns object', async () => {
        await expect(modelRepository.find(1)).resolves.toBeInstanceOf(Object);
    })

    test('list returns array', async () => {
        await expect(modelRepository.list()).resolves.toBeInstanceOf(Array);
    })

    test('creates new models', async () => {
        await expect(modelRepository.create(modelToCreate)).resolves.toBeUndefined();
        await expect(modelRepository.find(modelToCreate.id)).resolves.toEqual(modelToCreate);
    })

    test('create rejects if model already exists', async () => {
        await expect(modelRepository.create(modelToCreate)).rejects.toBeUndefined();
    })

    test('updates existing models', async () => {
        const updatedModel = Object.assign({}, modelToCreate);
        updatedModel.displayName = "changed";
        await expect(modelRepository.update(updatedModel)).resolves.toBeUndefined();
        await expect(modelRepository.find(updatedModel.id)).resolves.toEqual(updatedModel);
    })

    test('update rejects if model not found', async () => {
        const modelToUpdate = {
            id: 'definitely-not-a-real-id'
        }
        await expect(modelRepository.update(modelToUpdate)).rejects.toBeUndefined();
    })

    test('deletes existing models', async () => {
        await expect(modelRepository.delete(modelToCreate)).resolves.toBeUndefined();
        await expect(modelRepository.find(modelToCreate.id)).rejects.toBeUndefined();
    })

    test('delete rejects if model not found', async () => {
        const modelToDelete = {
            id: 'definitely-not-a-real-id'
        }
        await expect(modelRepository.delete(modelToDelete)).rejects.toBeUndefined();
    })
})