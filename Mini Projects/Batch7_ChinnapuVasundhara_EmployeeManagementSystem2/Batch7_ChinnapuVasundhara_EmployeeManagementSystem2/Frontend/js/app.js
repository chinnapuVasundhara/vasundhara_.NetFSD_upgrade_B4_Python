$(document).ready(() => {

    // --- State Management ---
    // We must track the current state to send to the API on every request
    let _state = {
        search: '',
        dept: '',
        status: '',
        sortBy: 'id',
        sortDir: 'asc',
        page: 1
    };

    // --- Routing & Initialization ---
    const checkAuthAndRoute = async () => {
        if (authService.isLoggedIn()) {
            $('#main-nav').removeClass('d-none');
            $('#login-view, #signup-view').addClass('d-none');

            // Setup Role UI (Hide edit/delete buttons if Viewer)
            const user = authService.getCurrentUser();

            $('#nav-username').text(`${user.charAt(0).toUpperCase() + user.slice(1)} (${authService.isAdmin() ? 'Admin' : 'Viewer'})`);            // Only admins see the Add button
            if (authService.isAdmin()) {
                $('#nav-add-btn').removeClass('d-none');
            } else {
                $('#nav-add-btn').addClass('d-none');
            }

            showView('dashboard');
            await refreshAppContent();
        } else {
            $('#main-nav').addClass('d-none');
            $('.view-section').addClass('d-none');
            $('#login-view').removeClass('d-none');
        }
    };

    const showView = (viewName) => {
        $('.view-section').addClass('d-none');
        $(`#${viewName}-view`).removeClass('d-none');
        $('.nav-link').removeClass('active');
        $(`#nav-${viewName}`).addClass('active');
    };

    const refreshAppContent = async () => {
        try {
            // 1. Refresh Dashboard
            const summary = await dashboardService.getSummary();
            uiService.renderDashboardCards(summary);
            uiService.renderDepartmentBreakdown(summary.breakdown);
            uiService.renderRecentEmployees(summary.recent);

            // 2. Refresh Employees
            uiService.populateDepartmentDropdown(employeeService.getUniqueDepartments());
            await triggerFilterSortUpdate();
        } catch (error) {
            console.error("Failed to load app content:", error);
            uiService.showToast('Failed to connect to server.', 'danger');
        }
    };

    const triggerFilterSortUpdate = async () => {
        try {
            // Sync UI values to state
            _state.search = $('#search-input').val();
            _state.dept = $('#filter-dept').val();
            _state.status = $('input[name="statusFilter"]:checked').val();

            // Fetch from API
            const pagedResult = await employeeService.getAll(
                _state.search, _state.dept, _state.status, _state.sortBy, _state.sortDir, _state.page
            );

            uiService.renderEmployeeTable(pagedResult); // Requires uiService update!
        } catch (error) {
            uiService.showToast('Failed to load employees.', 'danger');
        }
    };

    // --- Authentication Events ---
    $('#login-form').submit(async (e) => {
        e.preventDefault();
        const username = $('#login-username').val();
        const password = $('#login-password').val();

        const success = await authService.login(username, password);
        if (success) {
            $('#login-error').addClass('d-none');
            uiService.showToast('Login successful!');
            await checkAuthAndRoute();
        } else {
            $('#login-error').text('Invalid credentials or network error.').removeClass('d-none');
        }
    });

    $('#signup-form').submit(async (e) => {
        e.preventDefault();
        const u = $('#signup-username').val();
        const p = $('#signup-password').val();
        const c = $('#signup-confirm').val();

        const errors = validationService.validateAuthForm(u, p, c);
        uiService.showInlineErrors(errors);

        if (!errors) {
            const res = await authService.signup(u, p);
            if (res.success) {
                uiService.showToast('Signup successful. Please login.');
                $('#signup-view').addClass('d-none');
                $('#login-view').removeClass('d-none');
                uiService.clearForm('signup-form');
            } else {
                $('#err-signup-username').text(res.message).closest('.mb-3').find('input').addClass('is-invalid');
            }
        }
    });

    $('#logout-btn').click(() => {
        authService.logout();
        checkAuthAndRoute();
        uiService.clearForm('login-form');
    });

    $('#link-to-signup').click((e) => { e.preventDefault(); $('#login-view').addClass('d-none'); $('#signup-view').removeClass('d-none'); });
    $('#link-to-login').click((e) => { e.preventDefault(); $('#signup-view').addClass('d-none'); $('#login-view').removeClass('d-none'); });

    // --- Navigation Events ---
    $('#nav-dashboard').click(async (e) => { e.preventDefault(); showView('dashboard'); await refreshAppContent(); });
    $('#nav-employees').click(async (e) => { e.preventDefault(); showView('employees'); await triggerFilterSortUpdate(); });
    $('.navbar-brand').click(async (e) => { e.preventDefault(); showView('dashboard'); await refreshAppContent(); });

    // --- Table Filtering & Sorting Events ---
    // Debounce search to avoid hammering the API on every keystroke
    let searchTimeout;
    $('#search-input').on('input', () => {
        clearTimeout(searchTimeout);
        searchTimeout = setTimeout(() => {
            _state.page = 1; // Reset to page 1 on new search
            triggerFilterSortUpdate();
        }, 350);
    });

    $('#filter-dept, input[name="statusFilter"]').change(() => {
        _state.page = 1; // Reset to page 1 on filter change
        triggerFilterSortUpdate();
    });

    $('.sortable').click(function () {
        const field = $(this).data('sort');
        if (_state.sortBy === field) {
            _state.sortDir = _state.sortDir === 'asc' ? 'desc' : 'asc';
        } else {
            _state.sortBy = field;
            _state.sortDir = 'asc';
        }

        // Update UI icons
        $('.sortable i').removeClass('bi-arrow-up bi-arrow-down').addClass('bi-arrow-down-up text-muted');
        const icon = $(this).find('i');
        icon.removeClass('bi-arrow-down-up text-muted').addClass(_state.sortDir === 'asc' ? 'bi-arrow-up text-primary' : 'bi-arrow-down text-primary');

        triggerFilterSortUpdate();
    });

    // Handle pagination clicks (These buttons will be generated by uiService next)
    $(document).on('click', '.page-btn', function() {
        const newPage = $(this).data('page');
        if (newPage && newPage !== _state.page) {
            _state.page = newPage;
            triggerFilterSortUpdate();
        }
    });

    // --- CRUD Events ---
    $('#nav-add-btn').click(() => uiService.showModal('add'));

    $('#save-employee-btn').click(async () => {
        const id = $('#emp-id').val();
        const isEdit = !!id;

        const data = {
            firstName: $('#emp-firstName').val(),
            lastName: $('#emp-lastName').val(),
            email: $('#emp-email').val(),
            phone: $('#emp-phone').val(),
            department: $('#emp-department').val(),
            designation: $('#emp-designation').val(),
            salary: Number($('#emp-salary').val()),
            joinDate: $('#emp-joinDate').val(),
            status: $('#emp-status').val()
        };

        const errors = validationService.validateEmployeeForm(data, isEdit, id ? parseInt(id) : null);
        uiService.showInlineErrors(errors);

        if (!errors) {
            try {
                if (isEdit) {
                    await employeeService.update(parseInt(id), data);
                    uiService.showToast('Employee updated successfully');
                } else {
                    await employeeService.add(data);
                    uiService.showToast('Employee added successfully');
                }
                uiService.closeModal('employeeModal');
                await refreshAppContent(); 
            } catch (apiError) {
                // Handle 409 Conflict (Duplicate Email)
                if (apiError && apiError.message) {
                    $('#emp-email').addClass('is-invalid');
                    $('#emp-email').siblings('.invalid-feedback').text(apiError.message);
                }
            }
        }
    });

    $('#employee-table-body').on('click', '.btn-view', async function () {
        const id = $(this).data('id');
        const emp = await employeeService.getById(id);
        if (emp) uiService.showModal('view', emp);
    });

    $('#employee-table-body').on('click', '.btn-edit', async function () {
        const id = $(this).data('id');
        const emp = await employeeService.getById(id);
        if (emp) uiService.showModal('edit', emp);
    });

    $('#employee-table-body').on('click', '.btn-delete', async function () {
        const id = $(this).data('id');
        const emp = await employeeService.getById(id);
        if (emp) uiService.showModal('delete', emp);
    });

    $('#confirm-delete-btn').click(async function () {
        const id = $(this).data('id');
        try {
            await employeeService.remove(id);
            uiService.closeModal('deleteModal');
            uiService.showToast('Employee deleted successfully', 'danger');
            
            // If we delete the last item on a page, go back one page
            if ($('#employee-table-body tr').length === 1 && _state.page > 1) {
                _state.page--;
            }
            await refreshAppContent();
        } catch (e) {
            uiService.showToast('Error deleting employee.', 'danger');
        }
    });

    // --- Boot App ---
    checkAuthAndRoute();
});