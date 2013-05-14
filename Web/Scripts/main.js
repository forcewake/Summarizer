// prepare the form when the DOM is ready 
$(document).ready(function () {

    var options = {
        target: '#test',   // target element(s) to be updated with server response  
        success: showResponse  // post-submit callback 

    };

    // bind form using 'ajaxForm' 
    $('#summarizerForm').ajaxForm(options);
});

// post-submit callback 
function showResponse(responseText, statusText, xhr, $form) {

    var node = new PrettyJSON.view.Node({
        el: $('#elem'),
        data: responseText
    });

    Tempo.prepare("sentences").render(responseText.selectedSentences);
    Tempo.prepare("keywords").render(responseText.keywords);
    
    $('#output').toggle();

    $('html, body').animate({ scrollTop: $('#output').position().top }, 'slow');
}


/*
* Form Validation
* This script will set Bootstrap error classes when form.submit is called.
* The errors are produced by the MVC unobtrusive validation.
*/
$(function () {
    
    $("#loading").ajaxStart(function () {
        $(this).show();
    });

    $("#loading").ajaxStop(function () {
        $(this).hide();
    });

    $('summarizerForm').submit(function () {
        $(this).find('div.control-group').each(function () {
            if ($(this).find('span.field-validation-error').length == 0) {
                $(this).removeClass('error');
            }
        });

        if (!$(this).valid()) {
            $(this).find('div.control-group').each(function () {
                if ($(this).find('span.field-validation-error').length > 0) {
                    $(this).addClass('error');
                }
            });
        }
    });
    $('summarizerForm').each(function () {
        $(this).find('div.control-group').each(function () {
            if ($(this).find('span.field-validation-error').length > 0) {
                $(this).addClass('error');
            }
        });
    });
});
var page = function () {
    //Update that validator
    $.validator.setDefaults({
        highlight: function (element) {
            $(element).closest(".control-group").addClass("error");
        },
        unhighlight: function (element) {
            $(element).closest(".control-group").removeClass("error");
        }
    });
}();
/* End Form Validation */
