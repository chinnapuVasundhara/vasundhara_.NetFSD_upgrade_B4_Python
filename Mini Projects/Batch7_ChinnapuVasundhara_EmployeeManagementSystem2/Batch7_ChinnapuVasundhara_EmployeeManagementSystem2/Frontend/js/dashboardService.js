const dashboardService = {
    getSummary: async () => {
        // The backend computes KPIs, breakdown, and recent employees in one single SQL query!
        return await storageService.getDashboardSummary();
    }
};
if (typeof module !== 'undefined') module.exports = dashboardService;