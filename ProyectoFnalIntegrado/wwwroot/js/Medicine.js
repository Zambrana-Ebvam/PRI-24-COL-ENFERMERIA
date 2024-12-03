
$(document).ready(function () {
    loadCategories();
    loadSuppliers();
    loadMedicines();
});
$(document).ready(function () {
    const medicineModal = new bootstrap.Modal(document.getElementById('medicineModal'));

    // Abrir el modal al hacer clic en el botón
    $('#openMedicineModal').on('click', function () {
        medicineModal.show();
    });

    // Limpiar formulario y validaciones al cerrar el modal
    $('#medicineModal').on('hidden.bs.modal', function () {
        clearForm();
    });

    // Limpiar formulario y validaciones
    function clearForm() {
        $('#newMedicineForm')[0].reset(); // Reiniciar los campos
        $('#newMedicineForm').find('.is-valid').removeClass('is-valid');
        $('#newMedicineForm').find('.is-invalid').removeClass('is-invalid');
        $('#saveMedicineBtn').prop('disabled', true); // Deshabilitar el botón de guardar
    }

    // Validaciones en cada campo
    $('#newMedicineForm input, #newMedicineForm textarea, #newMedicineForm select').on('input change', function () {
        validateForm();
    });

    function validateForm() {
        let isValid = true;

        // Validar Nombre (sin espacios dobles ni caracteres especiales)
        const name = $('#medicineName').val().trim();
        const nameRegex = /^[A-Za-z0-9áéíóúÁÉÍÓÚñÑ\s]+$/; // Sin caracteres especiales
        if (!name || name !== name.replace(/\s{2,}/g, ' ') || !nameRegex.test(name)) {
            $('#medicineName').addClass('is-invalid');
            $('#nameErrorMsg').text('El nombre no debe tener espacios dobles ni caracteres especiales.');
            isValid = false;
        } else {
            $('#medicineName').removeClass('is-invalid').addClass('is-valid');
        }

        // Validar Descripción (sin espacios dobles, no puede empezar o terminar con espacios)
        const description = $('#medicineDescription').val().trim();
        if (!description || description !== description.replace(/\s{2,}/g, ' ') || description.startsWith(' ') || description.endsWith(' ')) {
            $('#medicineDescription').addClass('is-invalid');
            $('#descriptionErrorMsg').text('La descripción no debe tener espacios dobles ni empezar/terminar con espacios.');
            isValid = false;
        } else {
            $('#medicineDescription').removeClass('is-invalid').addClass('is-valid');
        }

        // Validar Categoría
        const category = $('#medicineCategory').val();
        if (!category) {
            $('#medicineCategory').addClass('is-invalid');
            $('#categoryErrorMsg').text('Debe seleccionar una categoría.');
            isValid = false;
        } else {
            $('#medicineCategory').removeClass('is-invalid').addClass('is-valid');
        }

        // Validar Proveedor
        const provider = $('#medicineProvider').val();
        if (!provider) {
            $('#medicineProvider').addClass('is-invalid');
            $('#providerErrorMsg').text('Debe seleccionar un proveedor.');
            isValid = false;
        } else {
            $('#medicineProvider').removeClass('is-invalid').addClass('is-valid');
        }

        // Validar Fecha de Expiración
        const expiryDate = $('#providerExpiryDate').val();
        const currentDate = new Date();
        const selectedDate = new Date(expiryDate);
        if (!expiryDate) {
            $('#providerExpiryDate').addClass('is-invalid');
            $('#expiryDateErrorMsg').text('La fecha de expiración es obligatoria.');
            isValid = false;
        } else if (selectedDate <= currentDate) {
            $('#providerExpiryDate').addClass('is-invalid');
            $('#expiryDateErrorMsg').text('La fecha de expiración debe ser posterior a la fecha actual.');
            isValid = false;
        } else {
            $('#providerExpiryDate').removeClass('is-invalid').addClass('is-valid');
        }

        // Habilitar el botón "Guardar Medicamento" solo si todo es válido
        $('#saveMedicineBtn').prop('disabled', !isValid);
    }

    // Manejar el envío del formulario
    $('#newMedicineForm').on('submit', function (e) {
        e.preventDefault(); // Evitar el envío por defecto
        if ($('#saveMedicineBtn').prop('disabled') === false) {
            // Obtener los elementos del formulario
            const medicineName = $('#medicineName');
            const medicineDescription = $('#medicineDescription');
            const providerExpiryDate =  $('#providerExpiryDate').val();
            const categoryId = $('#medicineCategory').val();
            const supplierId = $('#medicineProvider').val();

            // Mostrar la fecha en la consola para depurar
            console.log("Fecha de expiración: ", providerExpiryDate);

            // Verificar que todos los campos están presentes
            if (medicineName.length && medicineDescription.length && providerExpiryDate.length) {

                const medicineData = {
                    name: medicineName.val().trim(),
                    description: medicineDescription.val().trim(),
                    expirationDate: providerExpiryDate,
                    categoryId: categoryId,
                    supplierId: supplierId,
                };

              

                // Realizar la solicitud AJAX para guardar el medicamento
                $.ajax({
                    url: '/Medicine/InsertMedicine', // URL del endpoint
                    type: 'POST', // Método HTTP
                    contentType: 'application/json', // Tipo de contenido
                    data: JSON.stringify(medicineData), // Convertir a JSON
                    success: function (response) {
                        showAlert('Éxito', response, true); // Llama a showAlert con título y mensaje de éxito
                        loadMedicines(); 
                        medicineModal.hide(); // Cerrar el modal después de guardar
                        clearForm(); // Limpiar el formulario
                    },
                    error: function (xhr, status, error) {
                        // Manejar errores
                        alert('Error al guardar el medicamento: ' + xhr.responseText);
                    }
                });
            } else {
                alert('Por favor, completa todos los campos obligatorios.');
            }
        }
    });
});


$(document).ready(function () {
    const updateMedicineModal = new bootstrap.Modal(document.getElementById('updateMedicineModal'));

   

    // Limpiar formulario y validaciones al cerrar el modal de actualización
    $('#updateMedicineModal').on('hidden.bs.modal', function () {
        clearUpdateForm();
    });

    // Limpiar formulario y validaciones
    function clearUpdateForm() {
        $('#updateMedicineForm')[0].reset(); // Reiniciar los campos
        $('#updateMedicineForm').find('.is-valid').removeClass('is-valid');
        $('#updateMedicineForm').find('.is-invalid').removeClass('is-invalid');
        $('#updateMedicineBtn').prop('disabled', true); // Deshabilitar el botón de actualizar
    }

    // Validaciones en cada campo del formulario de actualización
    $('#updateMedicineForm input, #updateMedicineForm textarea, #updateMedicineForm select').on('input change', function () {
        validateUpdateForm();
    });

    function validateUpdateForm() {
        let isValid = true;

        // Validar Nombre (sin espacios dobles ni caracteres especiales)
        const name = $('#updateMedicineName').val().trim();
        const nameRegex = /^[A-Za-z0-9áéíóúÁÉÍÓÚñÑ\s]+$/; // Sin caracteres especiales
        if (!name || name !== name.replace(/\s{2,}/g, ' ') || !nameRegex.test(name)) {
            $('#updateMedicineName').addClass('is-invalid');
            $('#updateNameErrorMsg').text('El nombre no debe tener espacios dobles ni caracteres especiales.');
            isValid = false;
        } else {
            $('#updateMedicineName').removeClass('is-invalid').addClass('is-valid');
        }

        // Validar Descripción (sin espacios dobles, no puede empezar o terminar con espacios)
        const description = $('#updateMedicineDescription').val().trim();
        if (!description || description !== description.replace(/\s{2,}/g, ' ') || description.startsWith(' ') || description.endsWith(' ')) {
            $('#updateMedicineDescription').addClass('is-invalid');
            $('#updateDescriptionErrorMsg').text('La descripción no debe tener espacios dobles ni empezar/terminar con espacios.');
            isValid = false;
        } else {
            $('#updateMedicineDescription').removeClass('is-invalid').addClass('is-valid');
        }

        // Validar Categoría
        const category = $('#updateMedicineCategory').val();
        if (!category) {
            $('#updateMedicineCategory').addClass('is-invalid');
            $('#updateCategoryErrorMsg').text('Debe seleccionar una categoría.');
            isValid = false;
        } else {
            $('#updateMedicineCategory').removeClass('is-invalid').addClass('is-valid');
        }

        // Validar Proveedor
        const provider = $('#updateMedicineProvider').val();
        if (!provider) {
            $('#updateMedicineProvider').addClass('is-invalid');
            $('#updateProviderErrorMsg').text('Debe seleccionar un proveedor.');
            isValid = false;
        } else {
            $('#updateMedicineProvider').removeClass('is-invalid').addClass('is-valid');
        }

        // Validar Fecha de Expiración
        const expiryDate = $('#providerExpiryDateUp').val();
        const currentDate = new Date();
        const selectedDate = new Date(expiryDate);
        if (!expiryDate) {
            $('#providerExpiryDateUp').addClass('is-invalid');
            $('#expiryDateErrorMsgUp').text('La fecha de expiración es obligatoria.');
            isValid = false;
        } else if (selectedDate <= currentDate) {
            $('#providerExpiryDateUp').addClass('is-invalid');
            $('#expiryDateErrorMsgUp').text('La fecha de expiración debe ser posterior a la fecha actual.');
            isValid = false;
        } else {
            $('#providerExpiryDateUp').removeClass('is-invalid').addClass('is-valid');
        }

        // Habilitar el botón "Actualizar Medicamento" solo si todo es válido
        $('#updateMedicineBtn').prop('disabled', !isValid);
    }

    // Manejar el envío del formulario de actualización
    $('#updateMedicineBtn').on('click', function () {
        if (!$('#updateMedicineBtn').prop('disabled')) {
            const updatedMedicine = {
                id: $('#updateMedicineId').val().trim(),
                name: $('#updateMedicineName').val().trim(),
                description: $('#updateMedicineDescription').val().trim(),
                categoryId: $('#updateMedicineCategory').val(), // Asegúrate de que los nombres de las propiedades coincidan
                supplierId: $('#updateMedicineProvider').val(),
                expirationDate: $('#providerExpiryDateUp').val(),
            };

            console.log('Medicamento a actualizar:', updatedMedicine);

            $.ajax({
                url: '/Medicine/UpdateMedicine',
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify(updatedMedicine),
                beforeSend: function () {
                    showAlert('Actualizando...', 'Por favor, espere...', true);
                },
                success: function (response) {
                    setTimeout(function () {
                        showAlert('Éxito', 'Medicamento actualizado exitosamente.', true);
                        loadMedicines();
                        updateMedicineModal.hide();
                        clearUpdateForm();
                    }, 2000);
                },
                error: function (xhr) {
                    setTimeout(function () {
                        showAlert('Error', xhr.responseText || "Error al actualizar el medicamento.", false);
                    }, 2000);
                }
            });
        }
    });

});






let medicines = []; // Almacena los medicamentos
let currentPage = 1;
const itemsPerPage = 5;

// Función para cargar los medicamentos activos desde el servidor
function loadMedicines(startDate = null, endDate = null) {
    $.ajax({
        url: '/Medicine/GetActiveMedicines',
        type: 'GET',
        data: {
            startDate: startDate, // Enviar fecha de inicio si está disponible
            endDate: endDate // Enviar fecha de fin si está disponible
        },
        success: function (data) {
            if (Array.isArray(data)) {
                medicines = data; // Almacenar los medicamentos
                console.log(medicines); // Ver los datos en la consola para depuración
                if (medicines.length > 0) {
                    displayMedicines(currentPage); // Mostrar los medicamentos de la primera página
                } else {
                    $('#MedicineList').empty().append('<tr><td colspan="7">No hay medicamentos disponibles en este rango de fechas.</td></tr>');
                }
            } else {
                console.error("Error: El formato de 'data' no es un array válido", data);
                alert("Error al cargar los medicamentos. Intenta nuevamente más tarde.");
            }
        },
        error: function (error) {
            console.error("Error al cargar medicamentos: ", error);
        }
    });
}

// Función para mostrar los medicamentos en la página actual
function displayMedicines(page, startDateFilter = null, endDateFilter = null) {
    if (medicines.length === 0) {
        console.warn("No hay medicamentos disponibles para mostrar.");
        $('#MedicineList').empty().append('<tr><td colspan="7">No hay medicamentos disponibles.</td></tr>');
        return;
    }

    let filteredMedicines = medicines;

    // Aplicar el filtro de fechas si están presentes
    if (startDateFilter && endDateFilter) {
        const startDate = new Date(startDateFilter);
        const endDate = new Date(endDateFilter);
        filteredMedicines = medicines.filter(medicine => {
            const registerDate = new Date(medicine.registerDate);
            return registerDate >= startDate && registerDate <= endDate;
        });

        // Si no hay medicamentos en el rango, mostrar mensaje
        if (filteredMedicines.length === 0) {
            $('#MedicineList').empty().append('<tr><td colspan="7">No hay medicamentos dentro del rango de fechas especificado.</td></tr>');
            return;
        }
    }

    const start = (page - 1) * itemsPerPage;
    const end = Math.min(start + itemsPerPage, filteredMedicines.length); // Asegurarse de no exceder el límite
    const paginatedMedicines = filteredMedicines.slice(start, end); // Obtener los medicamentos de la página actual

    $('#MedicineList').empty(); // Vaciar la lista de medicamentos

    // Cargar medicamentos en la tabla
    paginatedMedicines.forEach(function (medicine) {
        $('#MedicineList').append(`
            <tr data-medicine-id="${medicine.medicineId}" data-category-id="${medicine.categoryId}" data-supplier-id="${medicine.supplierId}">
                <td>${medicine.medicineName || 'N/A'}</td>
                <td>${medicine.medicineDescription || 'N/A'}</td>
                <td>${medicine.registerDate || 'N/A'}</td> <!-- Mostrar la fecha de registro -->
                <td>${medicine.expirationDate || 'N/A'}</td> <!-- Mostrar la fecha de expiración -->
                <td>${medicine.categoryName || 'N/A'}</td>
                <td>${medicine.supplierName || 'N/A'}</td>
                <td>
                    <button class="btn btn-warning btn-sm" onclick="editMedicine(${medicine.medicineId})">Editar</button>
                    <button class="btn btn-danger btn-sm" onclick="deleteMedicine(${medicine.medicineId})">Eliminar</button>
                </td>
            </tr>
        `);
    });

    // Actualizar los botones de paginación
    updatePaginationButtons(filteredMedicines);
}

// Función para actualizar los botones de paginación
function updatePaginationButtons(filteredMedicines) {
    const totalPages = Math.ceil(filteredMedicines.length / itemsPerPage);
    $('#pagination').empty(); // Limpiar la navegación anterior

    // Botón "Atrás"
    if (currentPage > 1) {
        $('#pagination').append('<button class="btn btn-outline-success" id="prevBtn">Atrás</button>');
        $('#prevBtn').on('click', function () {
            currentPage--;
            displayMedicines(currentPage, $('#startDateFilter').val(), $('#endDateFilter').val());
        });
    }

    // Botón "Siguiente"
    if (currentPage < totalPages) {
        $('#pagination').append('<button class="btn btn-outline-success" id="nextBtn">Siguiente</button>');
        $('#nextBtn').on('click', function () {
            currentPage++;
            displayMedicines(currentPage, $('#startDateFilter').val(), $('#endDateFilter').val());
        });
    }

    // Indicador de la página actual
    $('#pagination').append('<span> Página ' + currentPage + ' de ' + totalPages + '</span>');
}

// Controlador de eventos para el botón de filtrar
$('#btnFiltrar').on('click', function () {
    const startDate = $('#startDateFilter').val();
    const endDate = $('#endDateFilter').val();

    // Verificar que las fechas estén presentes
    if (startDate && endDate) {
        currentPage = 1; // Reiniciar a la primera página
        displayMedicines(currentPage, startDate, endDate); // Filtrar localmente
    } else {
        alert("Por favor, selecciona una fecha de inicio y una fecha de fin.");
    }
});
// Controlador de eventos para el botón de limpiar
$('#btnLimpiarF').on('click', function () {
    $('#startDateFilter').val(''); // Limpiar el campo de fecha de inicio
    $('#endDateFilter').val('');   // Limpiar el campo de fecha de fin
    currentPage = 1; // Reiniciar a la primera página
    displayMedicines(currentPage); // Mostrar todos los medicamentos sin filtro
});

// Cargar medicamentos al iniciar la página
$(document).ready(function () {
    loadMedicines(); // Cargar los medicamentos al iniciar
});




let currentMedicineId = null;

function editMedicine(id) {
    // Asignar el ID del medicamento a la variable global
    currentMedicineId = id;

    // Buscar el medicamento correspondiente en la lista
    const medicine = medicines.find(m => m.medicineId === id);

    if (!medicine) {
        console.error("Medicamento no encontrado para el ID:", id);
        return;
    }

    // Llenar los campos del formulario con los valores actuales del medicamento
    $('#updateMedicineId').val(medicine.medicineId); // ID del medicamento (puede ser oculto o de solo lectura)
    $('#updateMedicineName').val(medicine.medicineName); // Nombre del medicamento
    $('#updateMedicineDescription').val(medicine.medicineDescription); // Descripción del medicamento
    $('#providerExpiryDateUp').val(medicine.expirationDate); // Descripción del medicamento

    // Asignar la categoría seleccionada en el combobox
    let categoryExists = false; // Variable para verificar si existe la categoría
    $('#updateMedicineCategory').empty(); // Limpiar el select antes de agregar nuevas opciones
    $('#updateMedicineCategory').append('<option value="">Seleccione una categoría</option>'); // Opción por defecto

    // Recorrer las categorías y agregar las opciones al select
    $.each(categories, function (index, category) {
        const optionText = category.name; // Mostrar nombre junto con ID
        const optionValue = category.id;
        $('#updateMedicineCategory').append($('<option>', { value: optionValue, text: optionText }));

        // Verificar si la categoría actual es la seleccionada
        if (category.id === medicine.categoryId) {
            $('#updateMedicineCategory').val(optionValue); // Asignar el valor seleccionado
            categoryExists = true; // Cambiar el estado si existe
        }
    });

    // Si la categoría no existe, mostrar un mensaje
    if (!categoryExists) {
        $('#updateMedicineCategory').append('<option disabled>No se encontró la categoría</option>');
    }

    // Asignar el proveedor seleccionado en el combobox
    let supplierExists = false; // Variable para verificar si existe el proveedor
    $('#updateMedicineProvider').empty(); // Limpiar el select antes de agregar nuevas opciones
    $('#updateMedicineProvider').append('<option value="">Seleccione un proveedor</option>'); // Opción por defecto

    // Recorrer los proveedores y agregar las opciones al select
    $.each(suppliers, function (index, supplier) {
        const optionText = supplier.name; // Mostrar nombre junto con ID
        const optionValue = supplier.id;
        $('#updateMedicineProvider').append($('<option>', { value: optionValue, text: optionText }));

        // Verificar si el proveedor actual es el seleccionado
        if (supplier.id === medicine.supplierId) {
            $('#updateMedicineProvider').val(optionValue); // Asignar el valor seleccionado
            supplierExists = true; // Cambiar el estado si existe
        }
    });

    // Si el proveedor no existe, mostrar un mensaje
    if (!supplierExists) {
        $('#updateMedicineProvider').append('<option disabled>No se encontró el proveedor</option>');
    }

    // Mostrar el modal de edición
    $('#updateMedicineModal').modal('show');
}



// Función para eliminar un proveedor
function deleteMedicine(id) {
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
                url: `/Medicine/DeleteMedicine?id=${id}`, // Cambia esta URL por tu ruta real
                type: 'POST',
                success: function (response) {
                    showAlert('Éxito', response, true); // Llama a showAlert con título y mensaje de éxito
                    console.log("Medicamento desactivado exitosamente.");
                    loadMedicines(); 
                },
                error: function (xhr) {
                    showAlert('Error', xhr.responseText || "Error al desactivar el medicamento.", false); // Llama a showAlert con título y mensaje de error
                }
            });
        }
    });
}



let categories = []; // Colección para almacenar categorías
let suppliers = [];  // Colección para almacenar proveedores

function loadCategories() {
    $.ajax({
        url: '/Medicine/GetActiveCategories',
        type: 'GET',
        success: function (data) {
            categories = data; // Guardar las categorías en la colección
            let categorySelect = $('#medicineCategory');
            categorySelect.empty(); // Limpiar el select antes de agregar nuevas opciones
            categorySelect.append('<option value="">Seleccione una categoría</option>');

            // Recorrer las categorías y agregar las opciones al select
            $.each(categories, function (index, category) {
                categorySelect.append($('<option>', {
                    value: category.id,
                    text: category.name
                }));
            });
        },
        error: function () {
            alert('Error al cargar las categorías');
        }
    });
}

// Función para cargar los proveedores en el select de proveedores
function loadSuppliers() {
    $.ajax({
        url: '/Medicine/GetActiveSuppliers',
        type: 'GET',
        success: function (data) {
            suppliers = data; // Guardar los proveedores en la colección
            let supplierSelect = $('#medicineProvider');
            supplierSelect.empty(); // Limpiar el select antes de agregar nuevas opciones
            supplierSelect.append('<option value="">Seleccione un proveedor</option>');

            // Recorrer los proveedores y agregar las opciones al select
            $.each(suppliers, function (index, supplier) {
                supplierSelect.append($('<option>', {
                    value: supplier.id,
                    text: supplier.name
                }));
            });
        },
        error: function () {
            alert('Error al cargar los proveedores');
        }
    });
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