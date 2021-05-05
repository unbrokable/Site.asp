

$(document).ready(function () {
    $('input[name *= "bu"]').click(function () {

        var tempg = this.id.replace("Button_", '');

        if (ThereIs(tempg)) {
            var arrcookies = readCookie("Choose");
            arrcookies =arrcookies.split(',')
            removeItemOnce(arrcookies, tempg);
            
            createCookie("Choose", arrcookies );
            this.value = "Add";
            this.style.background = "#2BAA65";
            $("#dish_" + tempg).detach().appendTo(".content");
            return; 
        } 
        
        this.value = "Remove";
        this.style.background = "red";
        
       
        ThereIs(tempg);
        $("#dish_" + tempg).detach().appendTo("#Chosen_dish");
        if (readCookie("Choose") == null) {
           
            createCookie("Choose", tempg );
        }
        else {
            
            var temp = readCookie("Choose");
            
            if (temp != "") {
                temp += "," + this.id.replace("Button_", '');
            }
            else {
                temp =  this.id.replace("Button_", '');
            }
           
            createCookie("Choose",temp);
        }

    });



});

function ThereIs(Id) {
    if (readCookie("Choose") == null) return false;

    var arr = readCookie("Choose").split(',');
    for (var i = 0; i < arr.length; i++) {
        if (Id == arr[i]) {
            return true;
        }
    }
    
}
function createCookie(name, value, days) {
    var expires;

    if (days) {
        var date = new Date();
        date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
        expires = "; expires=" + date.toGMTString();
    } else {
        expires = "";
    }
    document.cookie = encodeURIComponent(name) + "=" + value + expires + "; path=/";
}
function readCookie(name) {
    var nameEQ = encodeURIComponent(name) + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) === ' ')
            c = c.substring(1, c.length);
        if (c.indexOf(nameEQ) === 0)
            return decodeURIComponent(c.substring(nameEQ.length, c.length));
    }
    return null;
}
function removeItemOnce(arr, value) {
    var index = arr.indexOf(value);
    if (index > -1) {
        arr.splice(index, 1);
    }
    return arr;
}

