//Validacion del Insert Prooedor

function closeModal(modalId) {


    var modalInstance = bootstrap.Modal.getInstance(document.getElementById(modalId));
    if (modalInstance) {
        modalInstance.hide();
    }

    // Asegúrate de eliminar cualquier clase que bloquee la interacción
    $("body").removeClass("modal-open");  // Remueve la clase modal-open del body
    $(".modal-backdrop").remove();  // Remueve cualquier overlay de fondo sobrante
}
$(document).ready(function () {
    const form = document.getElementById('providerForm');
    const btnAddSupplier = document.getElementById('btn_Add_Supplier');

    // Escuchar cambios en los campos
    const inputs = [
        document.getElementById('providerName'),
        document.getElementById('providerAddress'),
        document.getElementById('providerPhone')
    ];
    inputs.forEach(input => {
        input.addEventListener('input', validateFields);
    });

    // Manejar el envío del formulario
    $('#btn_Add_Supplier').on('click', function (event) {
        event.preventDefault(); // Evitar el envío del formulario por defecto
        const btnAddSupplier = $(this); // Referencia al botón

        if (btnAddSupplier.prop('disabled')) return; // Si el botón está deshabilitado, no hacer nada

        const name = $('#providerName').val().trim();
        const adress = $('#providerAddress').val().trim();
        const phone = $('#providerPhone').val().trim();
        const userId = 1; // Cambia esto por la lógica para obtener el userId

        const assignmentData = { name, adress, phone, userId }; // Datos a enviar

        // Verifica los valores antes de enviarlos
        console.log("Nombre: ", name);
        console.log("Dirección: ", adress);
        console.log("Teléfono: ", phone);
        console.log("Datos a enviar: ", assignmentData);

        // Realiza la llamada AJAX para insertar el proveedor
        $.ajax({
            url: '/Supplier/InsertSupplier', // Cambia esta URL por tu ruta real
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(assignmentData),
            success: function (response) {
                //closeModal('providerModal');
                showAlert('Éxito', response, true); // Llama a showAlert con título y mensaje de éxito
                console.log("Proveedor agregado exitosamente.");
               // $('#providerModal').modal('hide');
             
                clearValidation(); // Limpia validaciones
                btnAddSupplier.prop('disabled', true); // Desactiva el botón
            },
            error: function (xhr) {
                showAlert('Error', xhr.responseText || "Error al crear el proveedor.", false); // Llama a showAlert con título y mensaje de error
            }
        });
    });

    $('#btn_Add_Cancel').on('click', function () {
       


   
    });


    function validateFields() {
        let isValid = true;

        // Validar el nombre
        const providerName = document.getElementById('providerName');
        const nameValue = providerName.value.trim();
        const nameRegex = /^(?!.*\s\s)(?!\s)(?!.*\d).*$/; // No aceptar dobles espacios, no terminar en espacio, no contener números

        const nameErrorMsg = document.getElementById('nameErrorMsg');
        if (nameValue === '') {
            providerName.classList.remove('is-valid');
            providerName.classList.remove('is-invalid');
            nameErrorMsg.textContent = ''; // Limpiar el mensaje
        } else if (!nameRegex.test(nameValue)) {
            providerName.classList.add('is-invalid');
            providerName.classList.remove('is-valid');
            isValid = false;

            // Mostrar mensajes de error específicos
            if (nameValue.endsWith(' ')) {
                nameErrorMsg.textContent = 'No debe terminar con un espacio.';
            } else if (/\s\s/.test(nameValue)) {
                nameErrorMsg.textContent = 'No se permiten dobles espacios.';
            } else if (/\d/.test(nameValue)) {
                nameErrorMsg.textContent = 'No se permiten números.';
            }
        } else {
            providerName.classList.add('is-valid');
            providerName.classList.remove('is-invalid');
            nameErrorMsg.textContent = ''; // Limpiar el mensaje
        }

        // Validar la dirección
        const providerAddress = document.getElementById('providerAddress');
        const addressValue = providerAddress.value.trim();

        const addressErrorMsg = document.getElementById('addressErrorMsg');
        if (addressValue === '') {
            providerAddress.classList.remove('is-valid');
            providerAddress.classList.remove('is-invalid');
            addressErrorMsg.textContent = ''; // Limpiar el mensaje
        } else if (addressValue.startsWith(' ') || addressValue.endsWith(' ')) {
            providerAddress.classList.add('is-invalid');
            providerAddress.classList.remove('is-valid');
            isValid = false;
            addressErrorMsg.textContent = 'No debe iniciar ni terminar con un espacio.';
        } else {
            providerAddress.classList.add('is-valid');
            providerAddress.classList.remove('is-invalid');
            addressErrorMsg.textContent = ''; // Limpiar el mensaje
        }

        // Validar el teléfono
        const providerPhone = document.getElementById('providerPhone');
        const phoneValue = providerPhone.value.trim();
        const phoneRegex = /^[1-9][0-9]{7}$/; // Solo 8 dígitos, no puede iniciar en 0

        const phoneErrorMsg = document.getElementById('phoneErrorMsg');
        if (phoneValue === '') {
            providerPhone.classList.remove('is-valid');
            providerPhone.classList.remove('is-invalid');
            phoneErrorMsg.textContent = ''; // Limpiar el mensaje
        } else if (!phoneRegex.test(phoneValue)) {
            providerPhone.classList.add('is-invalid');
            providerPhone.classList.remove('is-valid');
            isValid = false;

            // Mostrar mensajes de error específicos
            if (phoneValue.startsWith('0')) {
                phoneErrorMsg.textContent = 'No debe iniciar con 0.';
            } else {
                phoneErrorMsg.textContent = 'Debe tener exactamente 8 dígitos.';
            }
        } else {
            providerPhone.classList.add('is-valid');
            providerPhone.classList.remove('is-invalid');
            phoneErrorMsg.textContent = ''; // Limpiar el mensaje
        }

        // Habilitar o deshabilitar el botón
        btnAddSupplier.disabled = !isValid;
    }

    // Función para limpiar campos y mensajes de error
    function clearValidation() {
        const inputs = [
            document.getElementById('providerName'),
            document.getElementById('providerAddress'),
            document.getElementById('providerPhone')
        ];

        // Limpiar los valores de los campos y remover las clases de validación
        inputs.forEach(input => {
            input.value = ''; // Limpia el valor del input
            input.classList.remove('is-valid', 'is-invalid'); // Eliminar clases de validación

            // Limpiar mensajes de error
            const errorMsg = document.querySelector(`#${input.id} ~ .invalid-feedback`);
            if (errorMsg) errorMsg.innerText = ''; // Limpia el mensaje de error
        });

        // Eliminar la clase 'was-validated' del formulario
        const form = document.getElementById('providerForm');
        form.classList.remove('was-validated'); // Eliminar la clase de validación
    }
});





//Validacion del Actualizar Prooedor
$(document).ready(function () {
    const form = document.getElementById('providerFormUp');
    const btnUpdateSupplier = document.getElementById('btn_Update_Supplier');

    function validateFields() {
        let isValid = true;

        // Validar el nombre
        const providerName = document.getElementById('providerNameUp');
        const nameValue = providerName.value.trim();
        const nameRegex = /^(?!.*\s\s)(?!\s)(?!.*\d).*$/; // No aceptar dobles espacios, no terminar en espacio, no contener números

        const nameErrorMsg = document.getElementById('nameErrorMsgUp');
        if (nameValue === '') {
            providerName.classList.remove('is-valid');
            providerName.classList.remove('is-invalid');
            nameErrorMsg.textContent = ''; // Limpiar el mensaje
        } else if (!nameRegex.test(nameValue)) {
            providerName.classList.add('is-invalid');
            providerName.classList.remove('is-valid');
            isValid = false;

            // Mostrar mensajes de error específicos
            if (nameValue.endsWith(' ')) {
                nameErrorMsg.textContent = 'No debe terminar con un espacio.';
            } else if (/\s\s/.test(nameValue)) {
                nameErrorMsg.textContent = 'No se permiten dobles espacios.';
            } else if (/\d/.test(nameValue)) {
                nameErrorMsg.textContent = 'No se permiten números.';
            }
        } else {
            providerName.classList.add('is-valid');
            providerName.classList.remove('is-invalid');
            nameErrorMsg.textContent = ''; // Limpiar el mensaje
        }

        // Validar la dirección
        const providerAddress = document.getElementById('providerAddressUp');
        const addressValue = providerAddress.value.trim();

        const addressErrorMsg = document.getElementById('addressErrorMsgUp');
        if (addressValue === '') {
            providerAddress.classList.remove('is-valid');
            providerAddress.classList.remove('is-invalid');
            addressErrorMsg.textContent = ''; // Limpiar el mensaje
        } else if (addressValue.startsWith(' ') || addressValue.endsWith(' ')) {
            providerAddress.classList.add('is-invalid');
            providerAddress.classList.remove('is-valid');
            isValid = false;
            addressErrorMsg.textContent = 'No debe iniciar ni terminar con un espacio.';
        } else {
            providerAddress.classList.add('is-valid');
            providerAddress.classList.remove('is-invalid');
            addressErrorMsg.textContent = ''; // Limpiar el mensaje
        }

        // Validar el teléfono
        const providerPhone = document.getElementById('providerPhoneUp');
        const phoneValue = providerPhone.value.trim();
        const phoneRegex = /^[1-9][0-9]{7}$/; // Solo 8 dígitos, no puede iniciar en 0

        const phoneErrorMsg = document.getElementById('phoneErrorMsgUp');
        if (phoneValue === '') {
            providerPhone.classList.remove('is-valid');
            providerPhone.classList.remove('is-invalid');
            phoneErrorMsg.textContent = ''; // Limpiar el mensaje
        } else if (!phoneRegex.test(phoneValue)) {
            providerPhone.classList.add('is-invalid');
            providerPhone.classList.remove('is-valid');
            isValid = false;

            // Mostrar mensajes de error específicos
            if (phoneValue.startsWith('0')) {
                phoneErrorMsg.textContent = 'No debe iniciar con 0.';
            } else {
                phoneErrorMsg.textContent = 'Debe tener exactamente 8 dígitos.';
            }
        } else {
            providerPhone.classList.add('is-valid');
            providerPhone.classList.remove('is-invalid');
            phoneErrorMsg.textContent = ''; // Limpiar el mensaje
        }

        // Habilitar o deshabilitar el botón
        btnUpdateSupplier.disabled = !isValid;
    }

    // Función para limpiar campos y mensajes de error
    function clearValidation() {
        const inputs = [
            document.getElementById('providerNameUp'),
            document.getElementById('providerAddressUp'),
            document.getElementById('providerPhoneUp')
        ];

        // Limpiar los valores de los campos y remover las clases de validación
        inputs.forEach(input => {
            input.value = ''; // Limpia el valor del input
            input.classList.remove('is-valid', 'is-invalid'); // Eliminar clases de validación

            // Limpiar mensajes de error
            const errorMsg = document.querySelector(`#${input.id} ~ .invalid-feedback`);
            if (errorMsg) errorMsg.innerText = ''; // Limpia el mensaje de error
        });

        // Eliminar la clase 'was-validated' del formulario
        const form = document.getElementById('providerForm');
        form.classList.remove('was-validated'); // Eliminar la clase de validación
    }

    // Escuchar cambios en los campos
    const inputs = [
        document.getElementById('providerNameUp'),
        document.getElementById('providerAddressUp'),
        document.getElementById('providerPhoneUp'),
        document.getElementById('IdProviderUp')
    ];
    inputs.forEach(input => {
        input.addEventListener('input', validateFields);
    });

    
    // Manejo del evento click para actualizar el proveedor
    $('#btn_Update_Supplier').on('click', function (event) {
        event.preventDefault();

        // Desactiva el botón de actualización mientras se realiza la solicitud
        if (btnUpdateSupplier.disabled) return;

        const id = currentSupplierId;
        const name = $('#providerNameUp').val().trim();
        const adress = $('#providerAddressUp').val().trim();
        const phone = $('#providerPhoneUp').val().trim();
        const userId = 1; // Cambia esto por la lógica para obtener el userId

        const assignmentData = { id, name, adress, phone, userId }; // Datos a enviar

        // Verifica los valores antes de enviarlos
        console.log("id: ", id);
        console.log("Nombre: ", name);
        console.log("Dirección: ", adress);
        console.log("Teléfono: ", phone);
        console.log("Datos a enviar: ", assignmentData);

        // Realiza la llamada AJAX para actualizar el proveedor
        $.ajax({
            url: '/Supplier/UpdateSupplier', // Cambia esta URL por tu ruta real
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(assignmentData),
            beforeSend: function () {
                // Desactivar el botón de actualización y mostrar un mensaje de carga si es necesario
                btnUpdateSupplier.disabled = true; // Desactiva el botón para evitar múltiples clics
                showAlert('Actualizando...', 'Por favor, espere...', true); // Mostrar alerta de actualización
            },
            success: function (response) {
                showAlert('Éxito', response, true); // Llama a showAlert con título y mensaje de éxito
                console.log("Proveedor actualizado exitosamente.");
                CloseModalUpdate();
                clearValidation(); // Limpia validaciones
                loadProviders();

            },
            error: function (xhr) {
                showAlert('Error', xhr.responseText || "Error al actualizar el proveedor.", false); // Llama a showAlert con título y mensaje de error
            },
            complete: function () {
                // Habilitar nuevamente el botón de actualización
                btnUpdateSupplier.disabled = false; // Reactivar el botón después de completar la solicitud
            }
        });
    });

    // Limpieza de campos al cerrar el modal
    $('#ResignacionModal').on('hidden.bs.modal', function () {
        clearValidation(); // Limpia validaciones
        btnUpdateSupplier.disabled = true; // Desactiva el botón
    });

    
});



$(document).ready(function () {
    loadProviders();
});
    

    let assignments = []; 
    let currentPage = 1;
    const itemsPerPage = 5;

    function loadProviders() {
        $.ajax({
            url: '/Supplier/GetActiveSuppliers',
            type: 'GET',
            success: function (data) {
                // Asegúrate de que 'data' sea un array o un objeto que contiene un array
                if (Array.isArray(data)) {
                    assignments = data; // Almacenar los proveedores en 'assignments'
                    console.log(assignments); // Ver los datos en la consola para depuración
                    displayAssignments(currentPage); // Mostrar los proveedores de la primera página
                } else {
                    console.error("Error: El formato de 'data' no es un array válido", data);
                    alert("Error al cargar los proveedores. Intenta nuevamente más tarde.");
                }
            },
            error: function (error) {
                console.error("Error al cargar proveedores: ", error);
            }
        });
    }

    // Función para mostrar las asignaciones en la página actual
    function displayAssignments(page, startDateFilter = null, endDateFilter = null) {
        if (assignments.length === 0) {
            console.warn("No hay proveedores disponibles para mostrar.");
            $('#providerList').empty().append('<tr><td colspan="4">No hay proveedores disponibles.</td></tr>');
            return;
        }

        let filteredAssignments = assignments;

        // Aplicar el filtro de fechas si están presentes
        if (startDateFilter && endDateFilter) {
            const startDate = new Date(startDateFilter);
            const endDate = new Date(endDateFilter);
            filteredAssignments = assignments.filter(supplier => {
                const registerDate = new Date(supplier.registerDate);
                return registerDate >= startDate && registerDate <= endDate;
            });

            // Si no hay proveedores en el rango, mostrar mensaje
            if (filteredAssignments.length === 0) {
                $('#providerList').empty().append('<tr><td colspan="4">No hay proveedores dentro del rango de fechas especificado.</td></tr>');
                return;
            }
        }

        const start = (page - 1) * itemsPerPage;
        const end = Math.min(start + itemsPerPage, filteredAssignments.length); // Asegurarse de no exceder el límite
        const paginatedAssignments = filteredAssignments.slice(start, end); // Obtener los proveedores de la página actual

        $('#providerList').empty(); // Vaciar la lista de proveedores

        // Cargar proveedores en la tabla
        paginatedAssignments.forEach(function (supplier) {
            $('#providerList').append(`
        <tr>
            <td>${supplier.name || 'N/A'}</td>
            <td>${supplier.adress || 'N/A'}</td> <!-- Verifica que este nombre sea correcto -->
            <td>${supplier.phone || 'N/A'}</td>
            <td>${supplier.registerDate || 'N/A'}</td> <!-- Muestra la fecha de registro -->
            <td>
                <button class="btn btn-warning btn-sm" onclick="editSupplier(${supplier.id})">Editar</button>
                <button class="btn btn-danger btn-sm" onclick="deleteSupplier(${supplier.id})">Eliminar</button>
            </td>
        </tr>
    `);
        });

        // Actualizar los botones de paginación
        updatePaginationButtons(filteredAssignments);
    }


    // Función para actualizar los botones de paginación
    function updatePaginationButtons(filteredAssignments) {
        const totalPages = Math.ceil(filteredAssignments.length / itemsPerPage);
        $('#pagination').empty(); // Limpiar la navegación anterior

        // Botón "Atrás"
        if (currentPage > 1) {
            $('#pagination').append('<button class="btn btn-outline-success" id="prevBtn">Atrás</button>');
            $('#prevBtn').on('click', function () {
                currentPage--;
                displayAssignments(currentPage, $('#startDateFilter').val(), $('#endDateFilter').val());
            });
        }

        // Botón "Siguiente"
        if (currentPage < totalPages) {
            $('#pagination').append('<button class="btn btn-outline-success" id="nextBtn">Siguiente</button>');
            $('#nextBtn').on('click', function () {
                currentPage++;
                displayAssignments(currentPage, $('#startDateFilter').val(), $('#endDateFilter').val());
            });
        }

        // Indicador de la página actual
        $('#pagination').append('<span> Página ' + currentPage + ' de ' + totalPages + '</span>');
    }


    $('#btnFiltrar').on('click', function () {
        const startDate = $('#startDateFilter').val();
        const endDate = $('#endDateFilter').val();

        // Verificar que las fechas estén presentes
        if (startDate && endDate) {
            currentPage = 1; // Reiniciar a la primera página
            displayAssignments(currentPage, startDate, endDate); // Filtrar localmente
        } else {
            alert("Por favor, selecciona una fecha de inicio y una fecha de fin.");
        }
    });

    $('#btnLimpiarF').on('click', function () {
        $('#startDateFilter').val(''); // Limpiar el campo de fecha de inicio
        $('#endDateFilter').val('');   // Limpiar el campo de fecha de fin
        currentPage = 1; // Reiniciar a la primera página
        displayAssignments(currentPage); // Mostrar todos los proveedores sin filtro
    });

// Función para editar un proveedor
let currentSupplierId = null;
function editSupplier(id) {
    // Asignar el ID al label
    currentSupplierId = id; // Asigna el ID a la variable global
    document.getElementById("IdProviderUp").innerText = id;

    // Lógica para obtener los datos del proveedor y llenar el formulario
    console.log("Editar proveedor con ID:", id);

    // Aquí puedes implementar la lógica para obtener los detalles del proveedor, por ejemplo:
    const supplier = assignments.find(s => s.id === id); // Suponiendo que 'assignments' contiene todos los proveedores.

    if (supplier) {
        // Rellenar el formulario con los datos del proveedor
        document.getElementById("providerNameUp").value = supplier.name || '';
        document.getElementById("providerAddressUp").value = supplier.adress || '';
        document.getElementById("providerPhoneUp").value = supplier.phone || '';
    }

    // Abrir el modal para editar
    openModalUpdate();
}
// Función para eliminar un proveedor
function deleteSupplier(id) {
    // Muestra una alerta de confirmación usando SweetAlert2
    Swal.fire({
        title: '¿Estás seguro?',
        text: "Esta acción desactivará el proveedor.",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Sí, desactivar',
        cancelButtonText: 'Cancelar'
    }).then((result) => {
        if (result.isConfirmed) {
            // Llamar a la API para eliminar (desactivar) el proveedor
            $.ajax({
                url: `/Supplier/DeleteSupplier?id=${id}`, // Cambia esta URL por tu ruta real
                type: 'POST',
                success: function (response) {
                    showAlert('Éxito', response, true); // Llama a showAlert con título y mensaje de éxito
                    console.log("Proveedor desactivado exitosamente.");
                    loadProviders(); // Vuelve a cargar la lista de proveedores
                },
                error: function (xhr) {
                    showAlert('Error', xhr.responseText || "Error al desactivar el proveedor.", false); // Llama a showAlert con título y mensaje de error
                }
            });
        }
    });
}

// Función para abrir el modal de actualización
function openModalUpdate() {
    $("#ResignacionModal").modal("show");
}
function CloseModalUpdate() {
    $("#ResignacionModal").modal("hide");
}





function showAlert(title, text, isSuccess) {
    Swal.fire({
        title: title,
        text: text,
        icon: isSuccess ? 'success' : 'error',
        confirmButtonText: 'OK',
        timer: isSuccess ? 5000 : undefined, // Solo cerrar automáticamente en éxito
        timerProgressBar: true,
        position: 'center',
        showConfirmButton: true,
        customClass: {
            title: 'custom-title',
            confirmButton: 'custom-button'
        }
    });
}


$(document).ready(function () {
    // Inicializar el modal de Bootstrap
    const providerModal = new bootstrap.Modal(document.getElementById('providerModal'));

    // Abrir el modal al hacer clic en el botón "Agregar Proveedor"
    $('#openModalButton').on('click', function () {
        providerModal.show();
    });

   
    // Limpiar el formulario cuando el modal se cierra
    $('#providerModal').on('hidden.bs.modal', function () {
        clearForm();
    });

    // Limpiar formulario y validaciones
    function clearForm() {
        $('#providerForm')[0].reset(); // Reiniciar los campos
        $('#providerForm').find('.is-valid').removeClass('is-valid'); // Eliminar marcas de campos válidos
        $('#providerForm').find('.is-invalid').removeClass('is-invalid'); // Eliminar marcas de campos inválidos
        $('#btn_Add_Supplier').prop('disabled', true); // Deshabilitar el botón de agregar
    }
});
