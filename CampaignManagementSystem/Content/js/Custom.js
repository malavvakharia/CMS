$(document).ready(function () {
    toastr.options = {
        "closeButton": false,
        "positionClass": "toast-bottom-right",
        "showDuration": "3000",
        "hideDuration": "1000",
        "timeOut": "3000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    };

    $('#login').click(function () {
        if (!$('#Email').val()) {
            toastr.error('Please enter email')
        } else if (!$('#Password').val()) {
            toastr.error('Please enter password')
        } else {
            $.ajax({
                url: 'Login',
                method: 'post',
                data: {
                    email: $('#store_name').val(),
                    password: $('#phone').val(),
                    email: $('#email').val(),
                    address: $('#address').val(),
                },
                success: function (result) {
                    console.log(result)
                }, error: function (error) {
                    console.log(error)
                }
            });
        }
    });

    $('#forgot-password').click(function () {
        if (!$('#Email').val()) {
            toastr.error('Please enter email')
        }else {
            $.ajax({
                url: 'Login',
                method: 'post',
                data: {
                    email: $('#store_name').val(),
                    password: $('#phone').val(),
                    email: $('#email').val(),
                    address: $('#address').val(),
                },
                success: function (result) {
                    console.log(result)
                }, error: function (error) {
                    console.log(error)
                }
            });
        }
    })
});  