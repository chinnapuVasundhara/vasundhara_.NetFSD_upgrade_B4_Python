// Mock the storageService dependency
global.storageService = {
    getAll: jest.fn(),
    getById: jest.fn(),
    add: jest.fn(),
    update: jest.fn(),
    remove: jest.fn()
};

const employeeService = require('../js/employeeService');

describe('Employee Service', () => {
    afterEach(() => {
        jest.clearAllMocks();
    });

    test('getAll delegates to storageService with correct arguments', async () => {
        // Arrange
        const mockPagedResult = { data: [], totalCount: 0, page: 1 };
        global.storageService.getAll.mockResolvedValue(mockPagedResult);

        // Act
        const result = await employeeService.getAll('John', 'HR', 'Active', 'salary', 'desc', 2);

        // Assert
        expect(global.storageService.getAll).toHaveBeenCalledWith('John', 'HR', 'Active', 'salary', 'desc', 2);
        expect(result).toEqual(mockPagedResult);
    });

    test('getById delegates to storageService', async () => {
        // Arrange
        const mockEmp = { id: 5, firstName: 'Test' };
        global.storageService.getById.mockResolvedValue(mockEmp);

        // Act
        const result = await employeeService.getById(5);

        // Assert
        expect(global.storageService.getById).toHaveBeenCalledWith(5);
        expect(result).toEqual(mockEmp);
    });

    test('add delegates to storageService', async () => {
        // Arrange
        const newEmp = { firstName: 'New', lastName: 'User' };
        global.storageService.add.mockResolvedValue(newEmp);

        // Act
        const result = await employeeService.add(newEmp);

        // Assert
        expect(global.storageService.add).toHaveBeenCalledWith(newEmp);
    });

    test('getUniqueDepartments returns static array', () => {
        // Act
        const depts = employeeService.getUniqueDepartments();

        // Assert
        expect(depts).toContain('Engineering');
        expect(depts).toContain('Finance');
        expect(depts.length).toBe(5);
    });
});