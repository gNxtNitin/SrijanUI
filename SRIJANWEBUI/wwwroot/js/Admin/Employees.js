﻿var employee_table;
$(document).ready(function () {






    ToastWrapper.init({
        positionClass: 'toast-top-right',
        timeOut: 3000,
        progressBar: true

    });




    employee_table = $('.datatables-employees').DataTable({

        ajax: {
            url: '/admin/GetAllEmployees',
            dataSrc: ''
        },
        columns: [

            { data: 'empId', title: 'EMP ID' },
            { data: 'eName', title: 'Name' },
            { data: 'designation', title: 'Designation' },
            { data: 'mobile', title: 'Mobile' },
            { data: 'email', title: 'Email' },
            { data: 'user_role_id', title: 'Role' },
            { data: 'department', title: 'Zone' }, // No matching model property — handle accordingly
            { data: 'account_manager', title: 'Account Manager' },
            { data: 'logDate', title: 'Created Date' },
            { data: null, title: 'Actions' }
            //{ data: 'userType', title: 'userType' },
            //{ data: 'userStatus', title: 'userStatus' },
            //{ data: null, title: 'Action' },


        ],

        columnDefs: [
            //{
            //    target: 0,
            //    className: 'text-end',
            //    render: function (data, type, row, meta) {
            //        return meta.row + 1;
            //    }
            //},
            //{
            //    target: 0,
            //    visible: false,
            //},

            //{
            //    target: 4,
            //    className: 'text-end',
            //},
            //{
            //    target: 5,
            //    className: 'text-end',
            //},
            //{
            //    target: 6,
            //    className: 'text-end',
            //},
            //{
            //    // For Responsive
            //    className: 'control',
            //    searchable: false,
            //    orderable: false,
            //    responsivePriority: 2,
            //    targets: 0,
            //    render: function (data, type, full, meta) {
            //        return '';
            //    }
            //},



            //{
            //    targets: 3,
            //    responsivePriority: 4,
            //    render: function (data, type, full, meta) {
            //        return ("<div class='bg-success'>" + full['dob'] + "</div>")
            //    }
            //},

            //{
            //    // User Role
            //    targets: 6,
            //    render: function (data, type, full, meta) {
            //        var $role = full['userType'];
            //        var roleBadgeObj = {

            //            Author: '<i class="ti ti-edit ti-md text-warning me-2"></i>',
            //            User: '<i class="ti ti-user ti-md text-success me-2"></i>',
            //            Editor: '<i class="ti ti-chart-pie ti-md text-info me-2"></i>',
            //            Admin: '<i class="ti ti-device-desktop ti-md text-danger me-2"></i>'
            //        };
            //        return (
            //            "<span class='text-truncate d-flex align-items-center text-heading'>" +
            //            roleBadgeObj[$role] +
            //            $role +
            //            '</span>'
            //        );
            //    }
            //},

            //{
            //    // User Status
            //    targets: 7,
            //    render: function (data, type, full, meta) {
            //        var $status = full['userStatus'];

            //        return (
            //            '<span class="badge ' +
            //            statusObj[$status].class +
            //            '" text-capitalized>' +
            //            statusObj[$status].title +
            //            '</span>'
            //        );
            //    }
            //},

            {
                // Actions
                targets: -1,
                title: 'Actions',
                searchable: false,
                orderable: false,
                render: function (data, type, full, meta) {
                    //var hasInvoices = parseInt(full.invoices) > 0;
                    return (
                        '<div class="d-flex align-items-center">' +
                        '<a class="btn btn-icon btn-text-secondary waves-effect waves-light rounded-pill edit-record"><i class="ti ti-edit ti-md"></i></a>' +
                        '<a class="btn btn-icon btn-text-secondary waves-effect waves-light rounded-pill edit-password"><i class="ti ti-brand-samsungpass text-info ti-md"></i></a>' +
                        '<a class="btn btn-icon btn-text-secondary waves-effect waves-light rounded-pill delete-record"><i class="ti ti-trash text-danger"></i></a>' +
                        
                        '</div>' +
                        '</div>'
                    );
                }
            }
        ],


        order: [[0, 'asc']], // Sort by User ID
        responsive: true,
        dom:
            '<"row"' +
            '<"col-md-2"<"ms-n2"l>>' +
            '<"col-md-10"<"dt-action-buttons text-xl-end text-lg-start text-md-end text-start d-flex align-items-center justify-content-end flex-md-row flex-column mb-6 mb-md-0 mt-n6 mt-md-0"fB>>' +
            '>t' +
            '<"row"' +
            '<"col-sm-12 col-md-6"i>' +
            '<"col-sm-12 col-md-6"p>' +
            '>',

        language: {
            sLengthMenu: '_MENU_',
            search: '',
            searchPlaceholder: 'Search',
            paginate: {
                next: '<i class="ti ti-chevron-right ti-sm"></i>',
                previous: '<i class="ti ti-chevron-left ti-sm"></i>'
            }
        },


        buttons: [
            {
                text: '<i class="ti ti-plus me-0 me-sm-1 ti-xs"></i><span class="d-none d-sm-inline-block">Add Employee</span>',
                className: 'add-new btn btn-sm btn-primary waves-effect waves-light mx-3 add-new-school',
                //attr: {
                //    'asp-controller': 'Customer',
                //    'asp-action': 'OrderEntry'
                //},
                action: function (e, dt, node, config) {
                    //$('.modal-weigh').trigger('click');
                    // Build the URL

                }
            },

        ],
        initComplete: function (settings, json) {
            // Function init
            FormValidationNew();
            FormValidationPassword();
            deleteRecord();
            setPassword();
            // Evenet listener after table init
            $('.add-new-school').on('click', function () {
                offCanvasElement = document.querySelector('#add-new-record');

                offCanvasEl = new bootstrap.Offcanvas(offCanvasElement);
                $('.password-field').show();
                $('#flag').val('C');
                $('#empid, #empname, #fname, #email, #mobile, #address, #designation, #password, #cpassowrd').val('');
                $('#zone, #role, #manager').prop('selectedIndex', 0);
                $('input[name="gender"]').prop('checked', false);
                
                offCanvasEl.show();
                FormValidationNew();
            });
            // Evenet listener after table init
            $('.datatables-employees').on('click', '.edit-record', function () {
                $('#flag').val('U');
                $('.password-field').hide();
                var row = $(this).closest('tr');
                var data = employee_table.row(row).data();
                $('#empid').val(data.empId);
                $('#empname').val(data.eName);
                $('#fname').val(data.efName);
                $('#email').val(data.email);
                $('#mobile').val(data.mobile);
                $('#address').val(data.address);
                $('#designation').val(data.designation);
                $('#password').val(data.password);
                $('#cpassowrd').val(data.password); // Assuming you're filling Confirm Password with the same

                // Select Inputs
                $('#zone option').each(function () {
                    if ($(this).text().trim().toLowerCase() === data.department.trim().toLowerCase()) {
                        $(this).prop('selected', true);
                        return false; // break the loop
                    }
                }); // Assuming 'department' is zone
                $('#role').val(data.user_role_id);
                $('#manager option').each(function () {
                    if ($(this).text().trim().toLowerCase() === data.account_manager.trim().toLowerCase()) {
                        $(this).prop('selected', true);
                        return false; // break the loop
                    }
                });
                $('#manager').trigger('change');
                // Radio Buttons for Gender
                $('input[name="gender"][value="' + data.gender.toLowerCase() + '"]').prop('checked', true);
                $('#empid').prop('disabled', true);
                $('#email').prop('disabled', true);
                offCanvasElement = document.querySelector('#add-new-record');
                offCanvasEl = new bootstrap.Offcanvas(offCanvasElement);
                $(".offcanvas-title").text("Edit Employee");
                
                offCanvasEl.show();
                FormValidationExisting();
            });
            // Evenet listener after table init
            $('.add-new').on('click', function () {
                $(".offcanvas-title").text('Add Employee');

                $("#dt-flag").val("C");
                fv.resetForm(true);
                $('#empid').prop('disabled', false);
                $('#email').prop('disabled', false);
                $('#form-add-new-record select').val('').trigger('change');
            });

        }

    });








});



