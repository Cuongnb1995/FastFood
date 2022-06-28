$('.add').on('click', function () {
   var a = $(this).parents('.cart-detail__input').find('#input_price').val();
   var b = Number(a)+1; 
   $(this).parents('.cart-detail__input').find('#input_price').val(b);
});

$('.mute').on('click', function () {
    var a = $(this).parents('.cart-detail__input').find('#input_price').val();
    if (a>1) {
        var b = Number(a)-1; 
        $(this).parents('.cart-detail__input').find('#input_price').val(b);
        }
    }
);



// $('.name').on('input', function () {
//     var a = $(this).val();
//     if  (a=='') {
//         return console.log('Vui lòng nhập họ tên')
//     }
//     // console.log(a)
// });

$('.bnt-register').on('click', function (e) {
    checkName();
    checkMobi();
    checkAddress();
    checkEmail();
    checkPassword();
    checkPasswordConfirm();
    $(document).on('keyup', function () {
        checkName();
        checkMobi();
        checkEmail();
        checkAddress();
        checkPassword();
        checkPasswordConfirm();
    });
});


var nameId = $('.name');
var mobiId = $('.mobi');
var emailId = $('.email');
var addressId = $('.address');
var passwordId = $('.password');
var passwordConfirmId = $('.password-confirm');
var passwordId = $('.password');


// Check name
function checkName() {
    if (checkBlank(nameId.val())==true) {
       return addError(nameId, 'Vui lòng nhập họ tên')
    }
    else return addSuccess(nameId)
}
// Check mobi
function checkMobi() {
    if (checkBlank(mobiId.val())==true) {
       return addError(mobiId, 'Vui lòng nhập số điện thoại')
    }
    else return addSuccess(mobiId)
}
// Check address
function checkAddress() {
    if (checkBlank(addressId.val()) == true) {
        return addError(addressId, 'Vui lòng địa chỉ')
    }
    else return addSuccess(addressId)
}
// Check email
function checkEmail() {
    if (checkBlank(emailId.val())==true) {
       return addError(emailId, 'Vui lòng nhập email')
    }
    else return addSuccess(emailId)
}
// Check password
function checkPassword() {
    if (checkBlank(passwordId.val())==true) {
       return addError(passwordId, 'Vui lòng nhập mật khẩu')
    }
    else return addSuccess(passwordId)
}

// Check passwordConfirm
function checkPasswordConfirm() {
    if (checkBlank(passwordConfirmId.val())==true) {
       return addError(passwordConfirmId, 'Vui lòng nhập mật khấu')
    }
    if (!(passwordConfirmId.val() == passwordId.val())) {
        return addError(passwordConfirmId, 'Mật khẩu không khớp')
    }
    else return addSuccess(passwordConfirmId)
}

// Check blank
function checkBlank(value) {
    if (value == '') {
        return true
    }
    else return false
}

// Add class erro

function addError(value, message) {
    value.parents('.register-form-feild').find('small').text(message);
    value.addClass('error');
    value.removeClass('success');
}

// Add class success

function addSuccess(value) {
    value.parents('.register-form-feild').find('small').text('');
    value.addClass('success');
    value.removeClass('error');
}

