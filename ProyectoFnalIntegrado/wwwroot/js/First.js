$(document).ready(function () {
    loadSchools();
    loadMedicines();
    let responsibleGet = [];

    $('#school').on('change', function () {
        const schoolId = $(this).val();
        if (schoolId) {
            loadResponsible(schoolId);
        }
        validateSelect('school', 'schoolErrorMsg', 'Selecciona un colegio');
        validateSelect('firstAidKitType', 'firstAidKitTypeErrorMsg', 'Seleccione un tipo');
        validateSelect('responsible', 'responsibleErrorMsg', 'Seleccione un responsable');
        validateDescription();
        toggleSubmitButton(); // Verificar estado del botón al cambiar
    });

    $('#firstAidKitType').on('change', function () {
        validateSelect('school', 'schoolErrorMsg', 'Selecciona un colegio');
        validateSelect('firstAidKitType', 'firstAidKitTypeErrorMsg', 'Seleccione un tipo');
        validateSelect('responsible', 'responsibleErrorMsg', 'Seleccione un responsable');
        validateDescription();
        toggleSubmitButton(); // Verificar estado del botón al cambiar
    });

    $('#responsible').on('change', function () {
        validateSelect('school', 'schoolErrorMsg', 'Selecciona un colegio');
        validateSelect('responsible', 'responsibleErrorMsg', 'Seleccione un responsable');
        validateSelect('firstAidKitType', 'firstAidKitTypeErrorMsg', 'Seleccione un tipo');
        validateDescription();
        toggleSubmitButton(); // Verificar estado del botón al cambiar
    });

    function isValidDescription(text) {
        return text !== "" && !(/^\s/.test(text) || /\s$/.test(text) || /\s{2,}/.test(text));
    }

    function validateSelect(id, errorId, invalidValue = "") {
        const selectElement = document.getElementById(id);
        const value = selectElement.value;

        if (value === "" || value === invalidValue) {
            selectElement.classList.add('is-invalid');
            selectElement.classList.remove('is-valid');
            document.getElementById(errorId).style.display = "block"; // Muestra el mensaje de error
        } else {
            selectElement.classList.add('is-valid');
            selectElement.classList.remove('is-invalid');
            document.getElementById(errorId).style.display = "none"; // Oculta el mensaje de error
        }
    }

    function validateDescription() {
        const description = document.getElementById("description");
        const text = description.value.trim();

        if (text === "") {
            description.classList.add('is-invalid');
            description.classList.remove('is-valid');
            document.getElementById("descriptionErrorMsg").textContent = "La descripción no puede estar vacía.";
            document.getElementById("descriptionErrorMsg").style.display = "block"; // Muestra el mensaje de error
        } else if (!isValidDescription(text)) {
            description.classList.add('is-invalid');
            description.classList.remove('is-valid');
            document.getElementById("descriptionErrorMsg").textContent = "La descripción no puede iniciar ni terminar con espacio, ni tener doble espacio.";
            document.getElementById("descriptionErrorMsg").style.display = "block"; // Muestra el mensaje de error
        } else {
            description.classList.add('is-valid');
            description.classList.remove('is-invalid');
            document.getElementById("descriptionErrorMsg").style.display = "none"; // Oculta el mensaje de error
        }
    }

    // Verificar si todos los campos son válidos
    function areAllFieldsValid() {
        const isSchoolValid = document.getElementById('school').classList.contains('is-valid');
        const isFirstAidKitTypeValid = document.getElementById('firstAidKitType').classList.contains('is-valid');
        const isResponsibleValid = document.getElementById('responsible').classList.contains('is-valid');
        const isDescriptionValid = document.getElementById('description').classList.contains('is-valid');

        return isSchoolValid && isFirstAidKitTypeValid && isResponsibleValid && isDescriptionValid;
    }

    // Activar o desactivar el botón según el estado de validación
    function toggleSubmitButton() {
        const submitButton = document.getElementById('btnAdd');

        if (areAllFieldsValid()) {
            
            submitButton.innerHTML = '<i class="bi bi-plus-circle"></i> Crear Botiquín';
            submitButton.disabled = false;
        } else {
            submitButton.innerHTML = '<i class="bi bi-plus-circle"></i> Agregar Medicamento';
            submitButton.disabled = true;
        }
    }


    // Validar la descripción en tiempo real
    $('#description').on('input', function () {
        validateDescription();
        validateSelect('school', 'schoolErrorMsg', 'Selecciona un colegio');
        validateSelect('responsible', 'responsibleErrorMsg', 'Seleccione un responsable');
        validateSelect('firstAidKitType', 'firstAidKitTypeErrorMsg', 'Seleccione un tipo');
        toggleSubmitButton(); // Verificar estado del botón al cambiar
    });
    function loadResponsible(schoolId) {
        console.log("school ID:", schoolId);

        $.ajax({
            url: `/First_Aid_Kit/GetNursesBySchool?schoolId=${schoolId}`,
            type: 'GET',
            success: function (data) {

                responsibleGet = data;
                let responsibleSelect = $('#responsible');
                responsibleSelect.empty();
                responsibleSelect.append('<option value="">Seleccione un responsable</option>');

                $.each(responsibleGet, function (index, responsible) {
                    responsibleSelect.append($('<option>', {
                        value: responsible.nurseId,
                        text: responsible.fullName
                    }));
                });

                validateSelect('responsible', 'responsibleErrorMsg', 'Seleccione un responsable');
            },
            error: function () {
                alert('Error al cargar los enfermeros');
            }
        });
    }

});






let schoolsGet = [];
function loadSchools() {
    $.ajax({
        url: '/First_Aid_Kit/GetActiveSchools',
        type: 'GET',
        success: function (data) {
            schoolsGet = data;
            let schoolSelect = $('#school');
            schoolSelect.empty();
            schoolSelect.append('<option value="">Seleccione un colegio</option>');

            $.each(schoolsGet, function (index, school) {
                schoolSelect.append($('<option>', {
                    value: school.id,
                    text: school.name
                }));
            });

            // Llamar a la validación después de agregar las opciones
            validateSelect('school', 'schoolErrorMsg', 'Seleccione un colegio'); // Añadir validación
        },
        error: function () {
            alert('Error al cargar los colegios');
        }
    });
}



let medicines = []; // Almacena los medicamentos
let currentPage = 1;
const itemsPerPage = 5;

// Función para cargar los medicamentos activos desde el servidor
function loadMedicines() {
    $.ajax({
        url: '/First_Aid_Kit/GetActiveMedicines',
        type: 'GET',
        success: function (data) {
            if (Array.isArray(data)) {
                medicines = data; 
                console.log(medicines); // Ver los datos en la consola para depuración
                displayMedicines(currentPage); // Mostrar los medicamentos de la primera página
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
// Función para mostrar medicamentos
function displayMedicines(page) {
    if (medicines.length === 0) {
        console.warn("No hay medicamentos disponibles para mostrar.");
        $('#MedicineList').empty().append('<tr><td colspan="4">No hay medicamentos disponibles.</td></tr>');
        return;
    }

    const start = (page - 1) * itemsPerPage;
    const end = Math.min(start + itemsPerPage, medicines.length); // Asegurarse de no exceder el límite
    const paginatedMedicines = medicines.slice(start, end); // Obtener los medicamentos de la página actual

    $('#MedicineList').empty(); // Vaciar la lista de medicamentos

    // Cargar medicamentos en la tabla
    paginatedMedicines.forEach(function (medicine) {
        $('#MedicineList').append(`
        <tr data-medicine-id="${medicine.medicineId}">
            <td>${medicine.medicineName || 'N/A'}</td>
            <td>${medicine.medicineDescription || 'N/A'}</td>
          <td style="display: none;">${medicine.medicineStock || '0'}</td>
            <td>
                <button class="btn btn-success btn-sm"  onclick="AddMedicine(${medicine.medicineId})">Asignar</button>
            </td>
        </tr`);
    });

    // Actualizar los botones de paginación
    updatePaginationButtons();
}

// Función para filtrar medicamentos
function filterMedicines(searchTerm) {
    const filteredMedicines = medicines.filter(medicine => {
        const nameMatch = medicine.medicineName.toLowerCase().includes(searchTerm.toLowerCase());
        const descriptionMatch = medicine.medicineDescription.toLowerCase().includes(searchTerm.toLowerCase());
        return nameMatch || descriptionMatch;
    });

    // Mostrar los medicamentos filtrados
    $('#MedicineList').empty(); // Vaciar la lista actual
    filteredMedicines.forEach(function (medicine) {
        $('#MedicineList').append(`
        <tr data-medicine-id="${medicine.medicineId}">
            <td>${medicine.medicineName || 'N/A'}</td>
            <td>${medicine.medicineDescription || 'N/A'}</td>
      <td style="display: none;">${medicine.medicineStock || '0'}</td>
            <td>
                <button class="btn btn-success btn-sm" onclick="AddMedicine(${medicine.medicineId})">Asignar</button>
            </td>
        </tr`);
    });

    if (filteredMedicines.length === 0) {
        $('#MedicineList').append('<tr><td colspan="4">No se encontraron medicamentos.</td></tr>');
    }
}

// Evento para la búsqueda en tiempo real
document.getElementById('searchMedicine').addEventListener('input', function () {
    const searchTerm = this.value.trim();
    if (searchTerm) {
        filterMedicines(searchTerm);
    } else {
        displayMedicines(1); // Mostrar todos los medicamentos si el campo está vacío
    }
});

// Evento para el botón "Limpiar"
document.getElementById('clearSearch').addEventListener('click', function () {
    document.getElementById('searchMedicine').value = ''; // Limpiar el campo de búsqueda
    displayMedicines(1); // Volver a cargar todos los medicamentos
});

// Función para actualizar los botones de paginación
function updatePaginationButtons() {
    const totalPages = Math.ceil(medicines.length / itemsPerPage);
    $('#pagination').empty(); // Limpiar la navegación anterior

    // Botón "Atrás"
    if (currentPage > 1) {
        $('#pagination').append('<button class="btn btn-outline-success" id="prevBtn">Atrás</button>');
        $('#prevBtn').on('click', function () {
            currentPage--;
            displayMedicines(currentPage);
        });
    }

    // Botón "Siguiente"
    if (currentPage < totalPages) {
        $('#pagination').append('<button class="btn btn-outline-success" id="nextBtn">Siguiente</button>');
        $('#nextBtn').on('click', function () {
            currentPage++;
            displayMedicines(currentPage);
        });
    }

    // Indicador de la página actual
    $('#pagination').append('<span> Página ' + currentPage + ' de ' + totalPages + '</span>');
}

function AddMedicine(id) {
    const tableBody = document.getElementById('medicinesTableBody');
    const medicineForm = document.getElementById("assignQuantityForm");
    displayMedicines(1);

    const existingRow = Array.from(tableBody.rows).find(row => {
        return row.cells[0].innerText === String(id); // Comparar el ID en la primera celda
    });

    // Si existe el medicamento, mostramos una alerta y no permitimos agregarlo de nuevo
    if (existingRow) {
        // Obtener el nombre del medicamento de la segunda celda
        const existingMedicineName = existingRow.cells[1].innerText; // Suponiendo que el nombre está en la segunda celda
        showAlert('Observación', `El medicamento "${existingMedicineName}"  ya está asignado.`, false);
        closeModal('addMedicineModal');
        return; // Detenemos la ejecución aquí para evitar agregar el medicamento
    }

    // Si no existe el medicamento en la tabla, continuamos con el flujo normal
    medicineForm.reset(); // Limpiamos los campos del formulario
    currentSupplierId = id; // Asignamos el ID del medicamento actual

    // Buscamos el medicamento en la lista
    const medicine = medicines.find(m => m.medicineId === id);

    // Si encontramos el medicamento, llenamos el formulario con sus datos
    if (medicine) {
        document.getElementById("AsMedicineId").innerText = id;
        document.getElementById("medicineName").value = medicine.medicineName || '';
        document.getElementById("medicineDescription").value = medicine.medicineDescription || '';
        document.getElementById("currentStock").value = medicine.medicineStock || '0';
    }

    // Cerramos el modal de agregar medicamento y abrimos el modal para asignar la cantidad
    closeModal('addMedicineModal');
    $("#assignQuantityModal").modal("show");
}


function ClearIcon() {
    document.getElementById('assignedQuantity').classList.remove('is-valid');
    document.getElementById('assignedErrorMsg').classList.remove('is-invalid');
    document.getElementById('ExpiryDate').classList.remove('is-valid');
    document.getElementById('expiryDateErrorMsg').classList.remove('is-invalid');
}


$('#cancelButton').on('click', function () {
    closeModal('assignQuantityModal');
    ClearIcon();
});



$('#btnAdd').on('click', async function () {
    const btnAdd = document.getElementById('btnAdd');
    const buttonText = btnAdd.textContent.trim(); // Obtiene solo el texto del botón, sin el ícono

    if (buttonText === "Crear Botiquín") {
        // Si el contenido es "Crear Botiquín", envía los datos al backend
        try {
            const firstAidKit = {
                SchoolId: parseInt(document.getElementById('school').value),
                ResponsibleId: parseInt(document.getElementById('responsible').value),
                Name: document.getElementById('firstAidKitType').value,
                Description: document.getElementById('description').value,
                Status: 1, // Por defecto, activo
            };

            console.log('Datos que se están enviando:', firstAidKit);
            const response = await fetch('/First_Aid_Kit/CreateFirstAidKit', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(firstAidKit),
            });

            if (!response.ok) {
                throw new Error('Error al crear el botiquín');
            }

            const result = await response.json();

            // Asigna los datos devueltos por el backend
            document.getElementById('firstAidKitId').textContent = result.firstAidKitId;
            document.getElementById('schoolName').value = result.establishment.name;

            // Bloquea los campos y cambia el botón a "Agregar Medicamento"
            document.getElementById('school').disabled = true;
            document.getElementById('responsible').disabled = true;
            document.getElementById('firstAidKitType').disabled = true;
            document.getElementById('description').disabled = true;

            btnAdd.innerHTML = '<i class="bi bi-plus-circle"></i> Agregar Medicamento';

        } catch (error) {
            console.error(error);
            alert('Error al enviar los datos. Intenta nuevamente.');
        }

    } else if (buttonText === "Agregar Medicamento") {
        // Si el contenido es "Agregar Medicamento", abre el modal
        closeModal('assignQuantityModal');
        $("#addMedicineModal").modal("show");
    }
});




//$('#btnAdd').on('click', function () {
//    closeModal('assignQuantityModal');
//    $("#addMedicineModal").modal("show");
//});

$('#btnClose').on('click', function () {
    closeModal('assignQuantityModal');
    ClearIcon();
});

$('#btn-Close1').on('click', function () {
    closeModal('addMedicineModal');
   
});

$('#assignQuantityModal').on('hidden.bs.modal', function () {
  
    document.getElementById('assignedQuantity').classList.remove('is-valid');
    document.getElementById('assignedErrorMsg').classList.remove('is-invalid');
    document.getElementById('ExpiryDate').classList.remove('is-valid');
    document.getElementById('expiryDateErrorMsg').classList.remove('is-invalid');
});




document.getElementById('assignedQuantity').addEventListener('input', function () {
    const assignedQuantity = document.getElementById('assignedQuantity');
    const errorMsg = document.getElementById('assignedErrorMsg');
    const saveButton = document.getElementById('saveAssignedQuantity');

    if (assignedQuantity.value <= 0) {
        assignedQuantity.classList.add('is-invalid');
        assignedQuantity.classList.remove('is-valid');
        errorMsg.textContent = 'La cantidad debe ser mayor a 0';
        saveButton.disabled = true;
    } else {
        assignedQuantity.classList.remove('is-invalid');
        assignedQuantity.classList.add('is-valid');
        errorMsg.textContent = '';
        saveButton.disabled = false;
    }
});


document.getElementById('ExpiryDate').addEventListener('input', function () {
    const expiryDateInput = document.getElementById('ExpiryDate');
    const errorMsg = document.getElementById('expiryDateErrorMsg');
    const saveButton = document.getElementById('saveAssignedQuantity');

    const today = new Date();
    today.setHours(0, 0, 0, 0); 
    const selectedDate = new Date(expiryDateInput.value);

    if (selectedDate <= today) {
        expiryDateInput.classList.add('is-invalid');
        expiryDateInput.classList.remove('is-valid');
        errorMsg.textContent = 'La fecha de expiración debe ser posterior a la fecha actual.';
        saveButton.disabled = true;
    } else {
        expiryDateInput.classList.remove('is-invalid');
        expiryDateInput.classList.add('is-valid');
        errorMsg.textContent = '';
        saveButton.disabled = false;
    }
});


const ConfirmButton = document.getElementById('ConfirmButton');
ConfirmButton.style.display = 'none';





document.getElementById('saveAssignedQuantity').addEventListener('click', function () {
    const medicineId = document.getElementById('AsMedicineId').innerText;
    const medicineName = document.getElementById('medicineName').value;
    const medicineDescription = document.getElementById('medicineDescription').value;
    const currentStock = document.getElementById('currentStock').value;
    const assignedQuantity = document.getElementById('assignedQuantity').value;
    const assignedDate = document.getElementById('ExpiryDate').value;

   

    // Validar la cantidad asignada antes de agregar a la tabla
    if (assignedQuantity > 0) {
        ConfirmButton.style.display = 'block';
        const tableBody = document.getElementById('medicinesTableBody');
        const existingRow = Array.from(tableBody.rows).find(row => {
            return row.cells[0].innerText === medicineId; // Comprobar el ID en la primera celda
        });

        // Verificamos si el medicamento ya está en la tabla
        if (existingRow) {
            const rowIndex = this.getAttribute('data-row');
            if (rowIndex) {
                // Si estamos actualizando, no mostramos el mensaje de duplicado
                updateMedicineRow(existingRow, medicineName, medicineDescription, currentStock, assignedQuantity, assignedDate);
                closeModal('assignQuantityModal');
            } else {
                // Mostrar mensaje de advertencia si el ID ya existe al intentar asignar
           
                showAlert('Observación', `${medicineName} ya está asignado`, true);
         
                closeModal('assignQuantityModal');
            }
        } else {
            // Si no es un duplicado, agregamos el medicamento a la tabla
            showAlert('Éxito', 'Medicamento Agregado', true);
            addMedicineRow(medicineId, medicineName, medicineDescription, currentStock, assignedQuantity, assignedDate);
            closeModal('assignQuantityModal');
        }

        // Limpiar el formulario después de guardar
        document.getElementById("assignQuantityForm").reset();
        document.getElementById('assignedQuantity').classList.remove('is-valid');
        document.getElementById('ExpiryDate').classList.remove('is-valid');
    }
});

function addMedicineRow(medicineId, medicineName, medicineDescription, currentStock, assignedQuantity, assignedDate) {
    const tableBody = document.getElementById('medicinesTableBody');
    const newRow = `
        <tr>
            <td style="display: none;">${medicineId}</td>
            <td>${medicineName}</td>
            <td>${medicineDescription}</td>
         <td>${currentStock}</td>
            <td>${assignedQuantity}</td>
            <td>${assignedDate}</td>
            <td>
                <button type="button" class="btn btn-sm btn-warning" onclick="UpdateAmount(this)">Modificar</button>
                <button type="button" class="btn btn-sm btn-danger" onclick="removeMedicine('${medicineId}')">Eliminar</button>
            </td>
        </tr>
    `;
    tableBody.insertAdjacentHTML('beforeend', newRow);
}


function UpdateAmount(button) {
    // Accedemos a la fila donde se encuentra el botón de "Modificar"
    const row = button.closest('tr');

    // Obtenemos los valores de las celdas
    const medicineId = row.cells[0].innerText;
    const medicineName = row.cells[1].innerText;
    const medicineDescription = row.cells[2].innerText;
    const currentStock = row.cells[3].innerText;
    const assignedQuantity = row.cells[4].innerText;
    const assignedDate = row.cells[5].innerText;

   

    // Asignamos los valores al formulario
    document.getElementById("AsMedicineId").innerText = medicineId;
    document.getElementById("medicineName").value = medicineName;
    document.getElementById("medicineDescription").value = medicineDescription;
    document.getElementById("currentStock").value = currentStock;
    document.getElementById('assignedQuantity').value = assignedQuantity;
    document.getElementById('ExpiryDate').value = assignedDate;
    ClearIcon();
    // Abrimos el modal para que el usuario pueda modificar los valores
    const modal = new bootstrap.Modal(document.getElementById('assignQuantityModal'));
    modal.show();

    // Guardamos la referencia a la fila que se está editando
    document.getElementById('saveAssignedQuantity').setAttribute('data-row', row.rowIndex);
}

function updateMedicineRow(row, medicineName, medicineDescription, currentStock, assignedQuantity, assignedDate) {
    row.cells[1].innerText = medicineName;
    row.cells[2].innerText = medicineDescription;
    row.cells[3].innerText = currentStock;
   
    row.cells[4].innerText = assignedQuantity;
    row.cells[5].innerText = assignedDate;
    showAlert('Éxito', 'Medicamento Actualizado', true);
}










// Función para eliminar una fila de la tabla
function removeMedicine(medicineId) {
    const tableBody = document.getElementById('medicinesTableBody');
    const rows = tableBody.getElementsByTagName('tr');

    // Mostrar SweetAlert para confirmar la eliminación
    Swal.fire({
        title: '¿Estás seguro?',
        text: "Esta acción quitará el medicamento de la lista.",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Sí, quitar',
        cancelButtonText: 'Cancelar'
    }).then((result) => {
        if (result.isConfirmed) {
            // Si el usuario confirma, proceder a eliminar
            for (let i = 0; i < rows.length; i++) {
                const row = rows[i];
                if (row.cells[0].innerText === medicineId) {
                    tableBody.deleteRow(i);
                    break;
                }
            }

            // Verificar si la tabla está vacía y ocultar el botón si es necesario
            if (tableBody.rows.length === 0) {
                const confirmButton = document.getElementById('ConfirmButton'); // Asegúrate de que este ID coincida con el botón que deseas ocultar
                confirmButton.style.display = 'none'; // Ocultar el botón
            }

            // Mensaje de éxito
            Swal.fire(
                'Eliminado!',
                'El medicamento ha sido quitado de la lista.',
                'success'
            );
        }
    });
}






function closeModal(modalId) {
    var modalInstance = bootstrap.Modal.getInstance(document.getElementById(modalId));
    if (modalInstance) {
        modalInstance.hide();
    }

    // Asegúrate de eliminar cualquier clase que bloquee la interacción
    $("body").removeClass("modal-open");  // Remueve la clase modal-open del body
    $(".modal-backdrop").remove();  // Remueve cualquier overlay de fondo sobrante
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

// Cerrar el modal cuando se hace clic fuera de él
window.onclick = function (event) {
    const modal = document.getElementById('addMedicineModal');
    // Verificar si el clic ocurrió fuera del contenido del modal
    if (event.target === modal) {
        closeModal('addMedicineModal');
    }
};

window.onclick = function (event) {
    const modal = document.getElementById('assignQuantityModal');
    // Verificar si el clic ocurrió fuera del contenido del modal
    if (event.target === modal) {
        ClearIcon();
        closeModal('assignQuantityModal');

    }
};

function SendButton() {
    const schoolId = document.getElementById('school').value;
    const firstAidKitId = document.getElementById('firstAidKitId').textContent.trim();
    console.log('ID Botiquín:', firstAidKitId);
    const responsibleId = document.getElementById('responsible').value;
    const firstAidKitType = document.getElementById('firstAidKitType').value;
    const description = document.getElementById('description').value;

    // Validar si todos los campos obligatorios están llenos
    if (!schoolId || !responsibleId || !firstAidKitType || !description) {
        alert("Por favor, complete todos los campos obligatorios.");
        return;
    }

    // Recuperar los valores de los medicamentos en la tabla
    const tableRows = document.getElementById('medicinesTableBody').rows;
    const medicines = [];

    for (let i = 0; i < tableRows.length; i++) {
        const cells = tableRows[i].cells;
        const medicineId = cells[0].innerText;
        const assignedQuantity = cells[4].innerText;
        const assignedDate = cells[5].innerText;

        // Crear objeto de medicamento
        const medicine = {
            IdMedicine: parseInt(medicineId),
            Quantity: parseInt(assignedQuantity),
            ExpirationDate: assignedDate ? new Date(assignedDate).toISOString() : null
        };
        medicines.push(medicine);
    }

    // Verificar que se hayan agregado medicamentos
    if (medicines.length === 0) {
        alert("Debe agregar al menos un medicamento.");
        return;
    }


    console.log("Medicamentos:", medicines);

    // Enviar los datos al servidor mediante AJAX
    $.ajax({
        url: '/First_Aid_Kit/AddMedicinesToFirstAidKit',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({
            FirstAidKitId: parseInt(firstAidKitId),  // Asegúrate de convertir a número
            Medicines: medicines
        }),
        success: function (response) {
            showAlert('Éxito', 'Botiquín y medicamentos agregados correctamente.', true);
            console.log("Respuesta del servidor:", response);
            clearFormAndTable();
        },
        error: function (xhr, status, error) {
            alert(`Error al crear el botiquín: ${xhr.responseText}`);
            console.log("Error:", xhr.responseText);
        }
    });
}

function clearFormAndTable() {
    // Limpiar campos del formulario
    document.getElementById('school').value = '';
    document.getElementById('responsible').value = '';
    document.getElementById('firstAidKitType').value = '';
    document.getElementById('description').value = '';

    // Limpiar la tabla de medicamentos
    document.getElementById('medicinesTableBody').innerHTML = '';

    // Limpiar validaciones
    const fields = ['school', 'responsible', 'firstAidKitType', 'description'];
    fields.forEach(function (fieldId) {
        const field = document.getElementById(fieldId);
        field.classList.remove('is-valid', 'is-invalid'); // Eliminar clases de validación
    });

    // Ocultar los mensajes de error
    document.getElementById('schoolErrorMsg').style.display = "none";
    document.getElementById('responsibleErrorMsg').style.display = "none";
    document.getElementById('firstAidKitTypeErrorMsg').style.display = "none";
    document.getElementById('descriptionErrorMsg').style.display = "none";

    // Deshabilitar el botón de envío
    document.getElementById('btnAdd').disabled = true;
}



//$(document).off('click', '#ConfirmButton').on('click', '#ConfirmButton', function () {
//    event.preventDefault();
//    console.log('ConfirmButton clicado.');
//    $(this).prop('disabled', true);

//    // Recuperar los valores del formulario del botiquín
//    const schoolId = document.getElementById('school').value;
//    const responsibleId = document.getElementById('responsible').value;
//    const firstAidKitType = document.getElementById('firstAidKitType').value;
//    const description = document.getElementById('description').value;

//    // Validar si todos los campos obligatorios están llenos
//    if (!schoolId || !responsibleId || !firstAidKitType || !description) {
//        alert("Por favor, complete todos los campos obligatorios.");
//        $(this).prop('disabled', false); // Habilitar de nuevo el botón
//        return;
//    }

//    // Crear el objeto FirstAidKit
//    const firstAidKit = {
//        SchoolId: parseInt(schoolId),
//        ResponsibleId: parseInt(responsibleId),
//        Name: firstAidKitType,
//        Description: description,
//        Status: 1 // Por defecto está activo
//    };

//    // Recuperar los valores de los medicamentos en la tabla
//    const tableRows = document.getElementById('medicinesTableBody').rows;
//    const medicines = [];

//    for (let i = 0; i < tableRows.length; i++) {
//        const cells = tableRows[i].cells;
//        const medicineId = cells[0].innerText;
//        const assignedQuantity = cells[4].innerText;
//        const assignedDate = cells[5].innerText;

//        // Crear objeto de medicamento
//        const medicine = {
//            IdMedicine: parseInt(medicineId),
//            Quantity: parseInt(assignedQuantity),
//            ExpirationDate: assignedDate ? new Date(assignedDate).toISOString() : null
//        };
//        medicines.push(medicine);
//    }

//    // Verificar que se hayan agregado medicamentos
//    if (medicines.length === 0) {
//        alert("Debe agregar al menos un medicamento.");
//        $(this).prop('disabled', false); // Habilitar el botón si no hay medicamentos
//        return;
//    }

//    // Log para verificar lo que se enviará
//    console.log("Datos del botiquín:", firstAidKit);
//    console.log("Medicamentos:", medicines);

//    // Enviar los datos al servidor mediante AJAX
//    $.ajax({
//        url: '/First_Aid_Kit/CreateFirstAidKit',
//        type: 'POST',
//        contentType: 'application/json',
//        data: JSON.stringify({
//            firstAidKit: firstAidKit,
//            medicines: medicines
//        }),
//        success: function (response) {
          
//            console.log("Respuesta del servidor:", response);
//            // Aquí puedes redirigir o actualizar la página si es necesario
//        },
//        error: function (xhr, status, error) {
        
//            console.log("Error:", xhr.responseText);
//        },
//        complete: function () {
//            // Habilitar el botón después de que la solicitud se haya completado
//            $('#ConfirmButton').prop('disabled', false);
//        }
//    });
//});




