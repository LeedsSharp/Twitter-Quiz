$(function () {
    $('.datepicker').datetimepicker({
        language: 'en-GB',
        format: 'dd/MM/yyyy hh:mm:ss'
    });
    
    $('span.field-validation-valid, span.field-validation-error').each(function () {
        $(this).addClass('help-block');
    });

    var $form = $('form');
    var $validate = $form.validate();
    var errorClass = "has-error";
    if ($validate) {
        $validate.settings.errorClass = errorClass;
        var previousEPMethod = $validate.settings.errorPlacement;
        $validate.settings.errorPlacement = $.proxy(function(error, inputElement) {
            if (previousEPMethod) {
                previousEPMethod(error, inputElement);
            }
            inputElement.closest('.form-group').addClass(errorClass);
        }, $form[0]);

        var previousSuccessMethod = $validate.settings.success;
        $validate.settings.success = $.proxy(function(error) {
            //we first need to remove the class, cause the unobtrusive success method removes the node altogether
            error.parent().closest('.form-group').removeClass(errorClass);
            if (previousSuccessMethod) {
                previousSuccessMethod(error);
            }
        });
        $.validator.methods["date"] = function (value, element) { return true; }
    }
});