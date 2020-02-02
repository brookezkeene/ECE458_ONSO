import auth from '@/auth';

describe('auth service', () => {
    test('resolves admin properly', async () => {
        await expect(auth.login('admin', '')).resolves.toBe(true);
        expect(auth.isAdmin()).toBe(true);
        await auth.logout();
    })

    test('other users are not admins', async () => {
        await expect(auth.login('user1', '')).resolves.toBe(true);
        expect(auth.isAdmin()).toBe(false);
        await auth.logout();
    })
})