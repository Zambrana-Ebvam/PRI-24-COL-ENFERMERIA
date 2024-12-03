$(document).ready(function () {
    loadKardexes();

    // Manejo del formulario
    $('#formKardex').off('submit').on('submit', function (e) {
        e.preventDefault();
        if (validateForm()) {
            saveKardex();
        }
    });

    // Botón para limpiar el formulario
    $('#btnClear').off('click').on('click', function () {
        clearForm();
    });

    // Validación en tiempo real para todos los campos
    $('#formKardex input, #formKardex textarea, #formKardex select').on('input change blur', function () {
        validateField($(this));
    });

    // Restricciones específicas para ciertos campos
    $('#Height, #Weight, #OxygenLevel, #Temperature').on('input', function () {
        this.value = this.value.replace(/[^0-9,]/g, '').replace(/\./g, ',');
    });
});

// Validación de un solo campo
function validateField(field) {
    const errorId = `#${field.attr('id')}Error`;
    let value = field.val().trim();
    let isValid = true;
    let errorMessage = '';

    if (field.prop('required') && value === '') {
        errorMessage = 'Este campo es obligatorio.';
        isValid = false;
    }

    const minLength = field.attr('minlength');
    if (isValid && minLength && value.length < minLength) {
        errorMessage = `Debe tener al menos ${minLength} caracteres.`;
        isValid = false;
    }

    const dataType = field.data('type');
    if (isValid && dataType === 'number' && !/^\d+([,]\d+)?$/.test(value)) {
        errorMessage = 'Debe ser un número válido.';
        isValid = false;
    }

    if (!isValid) {
        $(errorId).text(errorMessage).show();
        field.addClass('is-invalid').removeClass('is-valid');
    } else {
        $(errorId).hide();
        field.addClass('is-valid').removeClass('is-invalid');
    }
}

// Validación del formulario completo
function validateForm() {
    let isValid = true;
    $('#formKardex input, #formKardex textarea, #formKardex select').each(function () {
        validateField($(this));
        if ($(this).hasClass('is-invalid')) {
            isValid = false;
        }
    });
    return isValid;
}

// Cargar lista de Kardexes
function loadKardexes() {
    $.get('/Kardex/GetAllKardexes', function (data) {
        const tbody = $('#kardexTableBody');
        tbody.empty();
        data.forEach(kardex => {
            tbody.append(`
                <tr>
                    <td>${kardex.height}</td>
                    <td>${kardex.weight}</td>
                    <td>${kardex.oxygenLevel}</td>
                    <td>${kardex.temperature}</td>
                    <td>${kardex.description}</td>
                    <td>
                        <button class="btn btn-primary" onclick="editKardex(${kardex.id})">Editar</button>
                        <button class="btn btn-danger" onclick="deleteKardex(${kardex.id})">Eliminar</button>
                    </td>
                </tr>`);
        });
    });
}

// Guardar Kardex
function saveKardex() {
    const formData = $('#formKardex').serialize();
    $.post('/Kardex/Save', formData, function (response) {
        if (response.success) {
            Swal.fire('Éxito', 'Datos guardados correctamente', 'success');
            loadKardexes();
            clearForm();
        } else {
            Swal.fire('Error', 'No se pudo guardar', 'error');
        }
    });
}

// Limpiar formulario
function clearForm() {
    $('#formKardex')[0].reset();
    $('#formKardex input, #formKardex textarea').removeClass('is-valid is-invalid');
    $('#formKardex span').hide();
}
