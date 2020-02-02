import userRepository from '@/repositories/user';

describe('user repository', () => {
    const userToCreate = {
        "id": 99,
        "username": "tbletsch",
        "displayName": "Tyler Bletsch",
        "email": "Tyler.Bletsch@duke.edu"
    }

    test('find rejects if user not found', async () => {
        await expect(userRepository.find('definitely-not-a-real-id')).rejects.toBeUndefined();
    })

    test('find returns object', async () => {
        await expect(userRepository.find(1)).resolves.toBeInstanceOf(Object);
    })

    test('list returns array', async () => {
        await expect(userRepository.list()).resolves.toBeInstanceOf(Array);
    })

    test('creates new users', async () => {
        await expect(userRepository.create(userToCreate)).resolves.toBeUndefined();
        await expect(userRepository.find(userToCreate.id)).resolves.toEqual(userToCreate);
    })

    test('create rejects if user already exists', async () => {
        await expect(userRepository.create(userToCreate)).rejects.toBeUndefined();
    })

    test('updates existing users', async () => {
        const updatedUser = Object.assign({}, userToCreate);
        updatedUser.displayName = "changed";
        await expect(userRepository.update(updatedUser)).resolves.toBeUndefined();
        await expect(userRepository.find(updatedUser.id)).resolves.toEqual(updatedUser);
    })

    test('update rejects if user not found', async () => {
        const userToUpdate = {
            id: 'definitely-not-a-real-id'
        }
        await expect(userRepository.update(userToUpdate)).rejects.toBeUndefined();
    })

    test('deletes existing users', async () => {
        await expect(userRepository.delete(userToCreate)).resolves.toBeUndefined();
        await expect(userRepository.find(userToCreate.id)).rejects.toBeUndefined();
    })

    test('delete rejects if user not found', async () => {
        const userToDelete = {
            id: 'definitely-not-a-real-id'
        }
        await expect(userRepository.delete(userToDelete)).rejects.toBeUndefined();
    })
})