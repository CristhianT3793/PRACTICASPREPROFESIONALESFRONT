//correcto
function alertacorrecto() {
    Swal.fire({
        icon: 'success',
        text: 'Registro guardado correctamente',
    })
}

//incorrecto
function alertaincorrecto() {
    Swal.fire({
        icon: 'error',
        text: 'No se pudo guardar el registro',
    })
}
//actualizado correcto
function alertaActualizado() {
    Swal.fire({
        icon: 'success',
        text: 'Registro actualizado correctamente',
    })
}
//actualizado correcto
function alertaParametro(icono,texto) {
    Swal.fire({
        icon: icono,
        text: texto,
    })
}


var object_acept = { status: false, ele: null };
function Acepta(ev) {
    if (object_acept.status) { return true; }
    Swal.fire({
        title: 'Advertencia',
        text: "Está seguro de aprobar la pasantía, no será capaz de cambiar el estado de los FPP",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#29CB12',
        cancelButtonColor: '#d33',
        confirmButtonText: 'SI',
        cancelButtonText: 'NO'
    }).then((result) => {
        if (result.isConfirmed) {
            object_acept.status = true;
            object_acept.ele = ev;
            object_acept.ele.click();
        }
    })
    return false;
}