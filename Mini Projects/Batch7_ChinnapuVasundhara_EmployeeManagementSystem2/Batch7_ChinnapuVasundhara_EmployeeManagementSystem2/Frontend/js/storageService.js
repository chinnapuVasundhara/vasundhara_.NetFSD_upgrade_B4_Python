const storageService = (() => {
    
    // Helper to generate headers with the JWT token
    const _getHeaders = () => {
        const headers = { 'Content-Type': 'application/json' };
        const token = authService.getToken();
        if (token) {
            headers['Authorization'] = `Bearer ${token}`;
        }
        return headers;
    };

    return {
        // We now pass all filter/sort/pagination params to the backend!
        getAll: async (search = '', department = '', status = '', sortBy = 'name', sortDir = 'asc', page = 1) => {
            // Build the query string with the page number
            const params = new URLSearchParams({
                search: search || '',
                department: department || '',
                status: status || '',
                sortBy: sortBy,
                sortDir: sortDir,
                page: page,                 // <--- TELLS .NET WHICH PAGE TO GET
                pageSize: CONFIG.PAGE_SIZE  // <--- TELLS .NET HOW MANY PER PAGE
            });

            const response = await fetch(`${CONFIG.API_BASE_URL}/employees?${params.toString()}`, {
                headers: _getHeaders()
            });
            if (!response.ok) throw new Error('Failed to fetch employees');
            
            // Returns the PagedResult wrapper from your API
            return await response.json(); 
        },

        getById: async (id) => {
            const response = await fetch(`${CONFIG.API_BASE_URL}/employees/${id}`, { headers: _getHeaders() });
            if (!response.ok) return null;
            return await response.json();
        },
        
        add: async (employeeData) => {
            const response = await fetch(`${CONFIG.API_BASE_URL}/employees`, {
                method: 'POST',
                headers: _getHeaders(),
                body: JSON.stringify(employeeData)
            });
            
            const data = await response.json();
            if (!response.ok) throw data; 
            return data;
        },
        
        update: async (id, updatedData) => {
            const response = await fetch(`${CONFIG.API_BASE_URL}/employees/${id}`, {
                method: 'PUT',
                headers: _getHeaders(),
                body: JSON.stringify(updatedData)
            });

            const data = await response.json();
            if (!response.ok) throw data;
            return data;
        },
        
        remove: async (id) => {
            const response = await fetch(`${CONFIG.API_BASE_URL}/employees/${id}`, {
                method: 'DELETE',
                headers: _getHeaders()
            });
            if (!response.ok) throw new Error('Failed to delete employee');
            return true;
        },

        getDashboardSummary: async () => {
            const response = await fetch(`${CONFIG.API_BASE_URL}/employees/dashboard`, { headers: _getHeaders() });
            if (!response.ok) throw new Error('Failed to fetch dashboard');
            return await response.json();
        }
    };
})();
if (typeof module !== 'undefined') module.exports = storageService;