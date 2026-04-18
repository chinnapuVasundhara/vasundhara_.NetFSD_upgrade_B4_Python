const authService = (() => {
    // Private variables to store session state in-memory (lost on refresh, which is secure)
    let _token = null;
    let _username = null;
    let _role = null;

    return {
        signup: async (username, password) => {
            try {
                const response = await fetch(`${CONFIG.API_BASE_URL}/auth/register`, {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify({ username, password, role: 'Viewer' }) // Defaulting to viewer
                });
                
                const data = await response.json();
                if (!response.ok) return { success: false, message: data.message || 'Signup failed' };
                return { success: true };
            } catch (error) {
                return { success: false, message: 'Network error occurred.' };
            }
        },

        login: async (username, password) => {
            try {
                const response = await fetch(`${CONFIG.API_BASE_URL}/auth/login`, {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify({ username, password })
                });

                const data = await response.json();
                
                if (response.ok && data.success) {
                    // Store token and user details in-memory
                    _token = data.token;
                    _username = data.username;
                    _role = data.role;
                    return true;
                }
                return false;
            } catch (error) {
                console.error("Login error:", error);
                return false;
            }
        },

        logout: () => {
            _token = null;
            _username = null;
            _role = null;
        },

        isLoggedIn: () => _token !== null,
        getCurrentUser: () => _username,
        getToken: () => _token,
        isAdmin: () => _role === 'Admin'
    };
})();
if (typeof module !== 'undefined') module.exports = authService;