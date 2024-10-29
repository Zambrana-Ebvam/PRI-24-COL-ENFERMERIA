$(document).ready(function () {
    loadSuppliers();

    $('#providerSelect').on('change', function () {
        const selectedSupplierId = $(this).val();
        if (selectedSupplierId) {
            loadMedicinesBySupplier(selectedSupplierId);
        }
    });

    $('#updateStock').on('input', function () {
        validateStockInput($(this), $('#saveUpdatedStock'));
    });

    $('#quickUpdateStock').on('input', function () {
        validateStockInput($(this), $('#saveQuickUpdatedStock'));
    });

    $('#btn_Close').on('click', resetUpdateModal);
    $('#updateStockModal').on('hidden.bs.modal', resetUpdateModal);
    $('#quickUpdateStockModal').on('hidden.bs.modal', resetQuickUpdateModal);

    $('#saveUpdatedStock').on('click', function () {
        const medicineId = $('#updateMedicineId').val();
        const newStock = $('#updateStock').val();
        updateMedicineStock(medicineId, newStock);
        validateAcceptButton();
    });

    $('#saveQuickUpdatedStock').on('click', function () {
        const medicineId = $('#quickUpdateMedicineId').val();
        const newStock = $('#quickUpdateStock').val();
        updateMedicineStock(medicineId, newStock);
    });

    $(document).on('click', '#acceptButton', function () {
        const stockUpdates = [];
        $('#addedMedicinesList tr').each(function () {
            const medicineIdElement = $(this).attr('id');
            if (medicineIdElement) {
                const medicineId = parseInt(medicineIdElement.replace('medicineRow', ''));
                const newStock = parseInt($(this).find('td:eq(2)').text());
                stockUpdates.push({ MedicineId: medicineId, NewStock: newStock });
            }
        });

        if (stockUpdates.length === 0) {
            console.warn("No hay medicamentos para actualizar.");
            return;
        }

        $.ajax({
            url: '/Supplier/UpdateMedicinesStock',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(stockUpdates),
            success: function (response) {
                showAlert("Éxito", response, true);
                $('#providerSelect').val('');
                $('#addedMedicinesList').empty();
                validateAcceptButton();
            },
            error: function (xhr) {
                alert('Error al actualizar el stock: ' + xhr.responseText);
            }
        });
    });
});

function validateStockInput(inputField, saveButton) {
    const stockValue = inputField.val();
    const errorMsg = $('#newStockErrorMsg');

    if (isNaN(stockValue) || parseInt(stockValue) <= 0 || !Number.isInteger(parseFloat(stockValue))) {
        errorMsg.text('El stock debe ser un número entero positivo mayor que 0.');
        inputField.addClass('is-invalid').removeClass('is-valid');
        saveButton.prop('disabled', true);
    } else {
        errorMsg.text('');
        inputField.removeClass('is-invalid').addClass('is-valid');
        saveButton.prop('disabled', false);
    }
}

function loadSuppliers() {
    $.ajax({
        url: '/Supplier/GetActiveSuppliersMedicine',
        type: 'GET',
        success: function (data) {
            let supplierSelect = $('#providerSelect');
            supplierSelect.empty();
            supplierSelect.append('<option value="">Seleccione un proveedor</option>');

            $.each(data, function (index, supplier) {
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

function loadMedicinesBySupplier(supplierId) {
    if (isNaN(supplierId) || supplierId < 0 || supplierId > 255) {
        alert("El ID del proveedor debe estar entre 0 y 255.");
        return;
    }

    $.ajax({
        url: `/Supplier/GetMedicinesBySupplierId?id=${supplierId}`,
        type: 'POST',
        success: function (medicines) {
            populateMedicinesTable(medicines);
            $('#medicineModal').modal('show');
        },
        error: function (xhr) {
            showAlert('Error', xhr.responseText || "Error al desactivar el proveedor.", false);
        }
    });
}

function populateMedicinesTable(medicines) {
    const medicinesList = $('#providerMedicinesList');
    medicinesList.empty();
   
    $.each(medicines, function (index, medicine) {
        medicinesList.append(
            `<tr>
                <td>${medicine.name}</td>
                <td>${medicine.stock}</td>
                <td>
                    <button class="btn btn-success" onclick="assignMedicine(${medicine.id}, '${medicine.name}', ${medicine.stock})">Asignar</button>
                </td>
            </tr>`
        );
    });
}

function assignMedicine(medicineId, medicineName, currentStock) {
    $('#medicineModal').modal('hide');

    if ($(`#medicineRow${medicineId}`).length > 0) {
        Swal.fire({
            title: 'Medicamento Asignado',
            text: `${medicineName} ya está asignado.`,
            icon: 'info',
            confirmButtonText: 'Aceptar'
        });
        return; // Sale de la función
        
    } else {
        openUpdateStockModal(medicineId, medicineName, currentStock);
    }

 
}

function validateAcceptButton() {
    const addedMedicinesList = $('#addedMedicinesList');
    const acceptButtonContainer = $('#acceptButtonContainer');

    if (addedMedicinesList.children().length > 0) {
        if ($('#acceptButton').length === 0) {
            const acceptButton = `<button id="acceptButton" class="btn btn-primary btn-block w-100">Aceptar</button>`;
            acceptButtonContainer.append(acceptButton);
        }
    } else {
        $('#acceptButton').remove();
    }
}

function removeMedicine(medicineId) {
    $(`#medicineRow${medicineId}`).remove();
    validateAcceptButton();
}

function openUpdateStockModal(medicineId, medicineName, currentStock) {
    const currentAssignedStock = $(`#newStock${medicineId}`).text() || 0; // Obtiene el stock actual o 0 si no existe

    // Rellenar el modal con los valores correspondientes
    $('#updateMedicineId').val(medicineId);
    $('#updateMedicineName').val(medicineName);
    $('#currentStock').val(currentStock);
    $('#updateStock').val(currentAssignedStock);

    // Mostrar el modal
    $('#updateStockModal').modal('show');

    // Deshabilitar el botón de guardar inicialmente
    $('#saveUpdatedStock').prop('disabled', true);

    // Validar el input del stock en el modal
    $('#updateStock').on('input', function () {
        const newStockValue = $(this).val();
        validateStockInput($(this), $('#saveUpdatedStock'));
    });

    // Al hacer clic en el botón "Guardar Cambios"
    $('#saveUpdatedStock').off('click').on('click', function () {
        const newStock = $('#updateStock').val();

        // Validar que el nuevo stock sea un número positivo antes de agregarlo
        if (newStock > 0) {
            // Agregar el medicamento a la lista solo si no existe
            if ($(`#medicineRow${medicineId}`).length === 0) {
                $('#addedMedicinesList').append(
                    `<tr id="medicineRow${medicineId}">
                        <td>${medicineName}</td>
                        <td>${currentStock}</td>
                        <td id="newStock${medicineId}">${newStock}</td> <!-- Usando el nuevo stock -->
                        <td>
                            <button class="btn btn-info" onclick="openQuickUpdateStockModal(${medicineId}, '${medicineName}', ${newStock})">Cantidad Rápida</button>
                            <button class="btn btn-danger" onclick="removeMedicine(${medicineId})">Eliminar</button>
                        </td>
                    </tr>`
                );
                showAlert("Medicamento Agregado", "El medicamento se ha añadido exitosamente.", true);

                validateAcceptButton();
                $('#providerSelect').val(''); // Asegúrate de que el ID sea el correcto para tu select
           
            }
            $('#updateStockModal').modal('hide');
        } else {
            Swal.fire({
                title: 'Error',
                text: 'El stock debe ser un número positivo.',
                icon: 'error',
                confirmButtonText: 'Aceptar'
            });
        }
    });

}


function openQuickUpdateStockModal(medicineId, medicineName, currentStock) {
    $('#quickUpdateMedicineId').val(medicineId);
    $('#quickUpdateStock').val(currentStock); // Puedes establecer un valor por defecto aquí si lo deseas
    $('#quickUpdateStockModal').modal('show');
}

function updateMedicineStock(medicineId, newStock) {
    const row = $(`#medicineRow${medicineId}`);
    if (row.length > 0) {
        row.find('td:eq(2)').text(newStock);
        showAlert("Stock Actualizado", "La cantidad del medicamento se ha actualizado exitosamente.", true);
    }

    $('#updateStockModal').modal('hide');
    $('#quickUpdateStockModal').modal('hide');
    resetUpdateModal();
    resetQuickUpdateModal();
}

function resetUpdateModal() {
    $('#updateStock').removeClass('is-valid is-invalid');
    $('#updateStock').val('');
}

function resetQuickUpdateModal() {
    $('#quickUpdateMedicineId').val('');
    $('#quickUpdateStock').removeClass('is-valid is-invalid');
    $('#quickUpdateStock').val('');
}

function showAlert(title, text, isSuccess) {
    Swal.fire({
        title: title,
        text: text,
        icon: isSuccess ? 'success' : 'error',
        confirmButtonText: 'OK',
        timer: isSuccess ? 5000 : undefined,
        timerProgressBar: true,
        position: 'center',
        showConfirmButton: true,
        customClass: {
            title: 'custom-title',
            confirmButton: 'custom-button'
        }
    });
}

function loadNurseReport(nurseId) {
    $.ajax({
        url: `/YourController/NurseReport?nurseId=${nurseId}`,
        type: 'GET',
        success: function(report) {
            const tableBody = $('#nurseReportTableBody');
            tableBody.empty();

            if (!report || report.Medicines.length === 0) {
                tableBody.append('<tr><td colspan="6" class="text-center">No hay medicamentos asignados en el botiquín.</td></tr>');
                return;
            }

            $.each(report.Medicines, function(index, medicine) {
                tableBody.append(
                    `<tr>
                        <td>${report.NurseName}</td>
                        <td>${report.SchoolName}</td>
                        <td>${medicine.MedicineName}</td>
                        <td>${medicine.Provider}</td>
                        <td>${medicine.Stock}</td>
                        <td>${new Date(medicine.ExpirationDate).toLocaleDateString()}</td>
                    </tr>`
                );
            });
        },
        error: function(xhr) {
            alert('Error al cargar el reporte de la enfermera: ' + xhr.responseText);
        }
    });
}

// Llama a esta función cuando sea necesario (ej., al cargar la vista o seleccionar una enfermera)
$(document).ready(function () {
    const nurseId = 1; // Cambia este ID por el ID de la enfermera deseada
    loadNurseReport(nurseId);
});
