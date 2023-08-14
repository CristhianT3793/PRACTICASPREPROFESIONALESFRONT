function mayus(e) {
    e.value = e.value.toUpperCase();
}

function setFechaactual(id) {
    var d = new Date();
    var fecha = d.format("yyyy-MM-dd");
    var id_ = document.getElementById(id).value=fecha;
    console.log(id_);
}

function ShowProgress() {

    Page_ClientValidate("inicio");
    if (Page_IsValid) {
        var elems = document.getElementsByClassName("carga");
        for (var k = elems.length - 1; k >= 0; k--) {
            var parent = elems[k].parentNode;
            parent.removeChild(elems[k]);
        }
        setTimeout(function () {
            $('.modal').remove();
            var modal = $('<div />');
            modal.addClass("modal carga");
            $('body').append(modal);
            var loading = $(".loading");
            loading.show();
            var top = Math.max($(window).height() / 2 - loading[0].offsetHeight / 2, 0);
            var left = Math.max($(window).width() / 2 - loading[0].offsetWidth / 2, 0);
            loading.css({ top: top, left: left });
        }, 200);
    }
}


function soloNumeros(evt) {

    var code = (evt.which) ? evt.which : evt.keyCode;

    if (code == 8) { 
        return true;
    } else if (code >= 48 && code <= 57) {
        return true;
    } else { 
        return false;
    }
}