const employeeService = {
    // All methods are now async delegates to storageService
    getAll: async (search, dept, status, sortBy, sortDir, page) => {
        return await storageService.getAll(search, dept, status, sortBy, sortDir, page);
    },
    
    getById: async (id) => {
        return await storageService.getById(id);
    },
    
    add: async (data) => {
        return await storageService.add(data);
    },
    
    update: async (id, data) => {
        return await storageService.update(id, data);
    },
    
    remove: async (id) => {
        return await storageService.remove(id);
    },
    
    // Departments are static based on our DB schema
    getUniqueDepartments: () => {
        return ['Engineering', 'Marketing', 'HR', 'Finance', 'Operations'];
    }
};
if (typeof module !== 'undefined') module.exports = employeeService;