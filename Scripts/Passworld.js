


$(document).ready(function () {
    $('#XG').click(function () {
        if ($('#XG')[0].checked) {
            $("input[name *= 'ConfirmPassword']").get(0).type = "text";
        }
        else
        $("input[name *= 'ConfirmPassword']").get(0).type = "password";
    });

  

});
