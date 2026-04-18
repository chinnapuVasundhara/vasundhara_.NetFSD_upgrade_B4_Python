// Mock the global CONFIG object that authService relies on
global.CONFIG = { API_BASE_URL: 'http://localhost:5000/api' };

const authService = require('../js/authService'); // Adjust path if necessary

describe('Auth Service', () => {
    beforeEach(() => {
        // Reset fetch mock and auth state before each test
        global.fetch = jest.fn();
        authService.logout();
    });

    afterEach(() => {
        jest.clearAllMocks();
    });

    test('signup returns success when API responds with ok', async () => {
        // Arrange: Mock fetch to return a successful response
        global.fetch.mockResolvedValue({
            ok: true,
            json: async () => ({})
        });

        // Act
        const result = await authService.signup('newuser', 'password123');

        // Assert
        expect(result.success).toBe(true);
        expect(global.fetch).toHaveBeenCalledTimes(1);
    });

    test('signup returns failure message on API conflict (409)', async () => {
        // Arrange
        global.fetch.mockResolvedValue({
            ok: false,
            json: async () => ({ message: 'Username already exists.' })
        });

        // Act
        const result = await authService.signup('existinguser', 'password123');

        // Assert
        expect(result.success).toBe(false);
        expect(result.message).toBe('Username already exists.');
    });

    test('login stores token and returns true on valid credentials', async () => {
        // Arrange
        global.fetch.mockResolvedValue({
            ok: true,
            json: async () => ({ success: true, token: 'fake-jwt-token', username: 'admin', role: 'Admin' })
        });

        // Act
        const result = await authService.login('admin', 'admin123');

        // Assert
        expect(result).toBe(true);
        expect(authService.isLoggedIn()).toBe(true);
        expect(authService.getToken()).toBe('fake-jwt-token');
        expect(authService.isAdmin()).toBe(true);
        expect(authService.getCurrentUser()).toBe('admin');
    });

    test('login returns false on invalid credentials', async () => {
        // Arrange
        global.fetch.mockResolvedValue({
            ok: false,
            json: async () => ({ success: false, message: 'Invalid credentials.' })
        });

        // Act
        const result = await authService.login('admin', 'wrongpass');

        // Assert
        expect(result).toBe(false);
        expect(authService.isLoggedIn()).toBe(false);
    });

    test('logout clears in-memory session state', async () => {
        // Arrange: Force a login first
        global.fetch.mockResolvedValue({
            ok: true,
            json: async () => ({ success: true, token: 'token', username: 'admin', role: 'Admin' })
        });
        await authService.login('admin', 'admin123');
        expect(authService.isLoggedIn()).toBe(true);

        // Act
        authService.logout();

        // Assert
        expect(authService.isLoggedIn()).toBe(false);
        expect(authService.getToken()).toBeNull();
        expect(authService.getCurrentUser()).toBeNull();
    });
});