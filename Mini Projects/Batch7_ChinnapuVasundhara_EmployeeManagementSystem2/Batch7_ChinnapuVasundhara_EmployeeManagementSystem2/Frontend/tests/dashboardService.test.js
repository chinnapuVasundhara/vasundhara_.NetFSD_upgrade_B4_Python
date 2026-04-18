// Mock the storageService dependency
global.storageService = {
    getDashboardSummary: jest.fn()
};

const dashboardService = require('../js/dashboardService');

describe('Dashboard Service', () => {
    afterEach(() => {
        jest.clearAllMocks();
    });

    test('getSummary delegates to storageService.getDashboardSummary', async () => {
        // Arrange
        const mockSummary = {
            total: 10,
            active: 8,
            inactive: 2,
            departments: 3,
            breakdown: [],
            recent: []
        };
        global.storageService.getDashboardSummary.mockResolvedValue(mockSummary);

        // Act
        const result = await dashboardService.getSummary();

        // Assert
        expect(global.storageService.getDashboardSummary).toHaveBeenCalledTimes(1);
        expect(result).toEqual(mockSummary);
        expect(result.total).toBe(10);
    });
});