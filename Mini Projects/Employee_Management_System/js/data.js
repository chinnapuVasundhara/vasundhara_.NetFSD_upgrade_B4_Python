const appData = {
    admin: { username: 'admin', password: 'admin@123' },
    employees: [
        { id: 1, firstName: 'Raja', lastName: 'Chinnapu', email: 'rajak@gmail.com', phone: '9376543210', department: 'Engineering', designation: 'Software Engineer', salary: 580000, joinDate: '2021-03-15', status: 'Active' },
        { id: 2, firstName: 'Roopa', lastName: 'Chinnapu', email: 'roopam@gmail.com', phone: '9523456780', department: 'Marketing', designation: 'Marketing Exec', salary: 760000, joinDate: '2020-07-01', status: 'Active' },
        { id: 3, firstName: 'Vishnu', lastName: 'Kasireddy', email: 'vishnu@gmail.com', phone: '9676512340', department: 'HR', designation: 'HR Executive', salary: 850000, joinDate: '2019-11-20', status: 'Active' },
        { id: 4, firstName: 'Devaki', lastName: 'Poojari', email: 'devaki@gmail.com', phone: '9788776655', department: 'Finance', designation: 'Financial Analyst', salary: 520000, joinDate: '2022-01-10', status: 'Active' },
        { id: 5, firstName: 'Pushpa', lastName: 'Poola', email: 'pushpa@gmail.com', phone: '9423123123', department: 'Operations', designation: 'Operations Mgr', salary: 890000, joinDate: '2018-05-05', status: 'Active' },
        { id: 6, firstName: 'Vasundhara', lastName: 'Chinnapu', email: 'vassu@gmail.com', phone: '9088998899', department: 'Engineering', designation: 'Senior Dev', salary: 2100000, joinDate: '2017-09-12', status: 'Active' },
        { id: 7, firstName: 'Eashwar', lastName: 'Mavilla', email: 'eashwar@gmail.com', phone: '9201002003', department: 'Marketing', designation: 'Content Strategist', salary: 540000, joinDate: '2023-02-28', status: 'Inactive' },
        { id: 8, firstName: 'Lucky', lastName: 'Katuru', email: 'lucky@gmail.com', phone: '9152233445', department: 'Finance', designation: 'Accounts Mgr', salary: 900000, joinDate: '2020-04-17', status: 'Active' },
        { id: 9, firstName: 'Kumar', lastName: 'Kamatam', email: 'kumar@gmail.com', phone: '9098887776', department: 'Engineering', designation: 'DevOps Eng', salary: 500000, joinDate: '2021-08-22', status: 'Active' },
        { id: 10, firstName: 'Chakri', lastName: 'Nallamangala', email: 'ckakri@gmail.com', phone: '9387766554', department: 'Operations', designation: 'Supply Chain Analyst', salary: 750000, joinDate: '2022-11-15', status: 'Active' },
        { id: 11, firstName: 'Vasanthi', lastName: 'Neeli', email: 'vasanthi@gmail.com', phone: '9176655443', department: 'Marketing', designation: 'Brand Manager', salary: 620000, joinDate: '2019-03-10', status: 'Active' },
        { id: 12, firstName: 'Shreshta', lastName: 'Penderi', email: 'shreshta@gmail.com', phone: '9065544332', department: 'Finance', designation: 'Tax Consultant', salary: 550000, joinDate: '2021-06-05', status: 'Inactive' },
        { id: 13, firstName: 'Radha', lastName: 'Badam', email: 'radha@gmail.com', phone: '9354433221', department: 'Engineering', designation: 'QA Engineer', salary: 980000, joinDate: '2022-09-01', status: 'Active' },
        { id: 14, firstName: 'Bunny', lastName: 'Araveeti', email: 'bunny@gmai;.com', phone: '9143322110', department: 'HR', designation: 'Recruiter', salary: 500000, joinDate: '2023-01-20', status: 'Active' },
        { id: 15, firstName: 'Hethwik', lastName: 'karate', email: 'hetwik@gmail.com', phone: '9632211009', department: 'Operations', designation: 'Logistics Coord', salary: 640000, joinDate: '2020-10-12', status: 'Inactive' },
        { id: 16, firstName: 'Hem', lastName: 'Chaganti', email: 'hem@gmail.com', phone: '9132211009', department: 'Operations', designation: 'Logistics Coord', salary: 640000, joinDate: '2020-10-12', status: 'Inactive' },
        { id: 17, firstName: 'Vardhan', lastName: 'Garikipati', email: 'vardhan@gmail.com', phone: '9242211709', department: 'Operations', designation: 'Logistics Coord', salary: 640000, joinDate: '2020-10-12', status: 'Inactive' },
        { id: 18, firstName: 'Manvik', lastName: 'Mallela', email: 'manvik@gmail.com', phone: '9632881009', department: 'Operations', designation: 'Logistics Coord', salary: 640000, joinDate: '2020-10-12', status: 'Inactive' },
        { id: 19, firstName: 'Naresh', lastName: 'Narpala', email: 'naresh@gmail.com', phone: '9876452119', department: 'Operations', designation: 'Logistics Coord', salary: 640000, joinDate: '2020-10-12', status: 'Inactive' },
        { id: 20, firstName: 'Harith', lastName: 'Penderi', email: 'harish@gmail.com', phone: '973611859', department: 'Operations', designation: 'Logistics Coord', salary: 640000, joinDate: '2020-10-12', status: 'Inactive' },



    ]
};

if (typeof module !== 'undefined') module.exports = appData;