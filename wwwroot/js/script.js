var supplierIdError = document.getElementById('SupplierId-error');
var invoiceNoError = document.getElementById('invoiceno_error');
var submitError = document.getElementById('submit_error');
var medicineError = document.getElementById('medicine_error');
var quantityError = document.getElementById('quantity_error');

function validateSupplier() {
    var supplier = document.getElementById('Supplier_Id').value;


    if (supplier == "") {
        supplierIdError.innerHTML = 'Supplier is required';
        return false;
    }

    supplierIdError.innerHTML = '<i class="fa-solid fa-circle-check"></i>';
    return true;
}


function validateInvoiceNo() {
    var invoiceNo = document.getElementById('Invoice_No').value;
    if (invoiceNo.length == 0) {
        invoiceNoError.innerHTML = 'Invoice No is required';
        return false;
    }

    invoiceNoError.innerHTML = '<i class="fa-solid fa-circle-check"></i>';
    return true;
}
function validateMedicine() {

    var medicineId = document.getElementById('Medicine_Id').value;
    if (medicineId.length == 0) {
        medicineError.innerHTML = 'Required';
        return false;
    }

    medicineError.innerHTML = '<i class="fa-solid fa-circle-check"></i>';
    return true;
}

function validateQuantity() {

    var quantity = document.getElementById('Quantity_1').value;
    if (quantity.length == 0) {
        quantityError.innerHTML = 'Required';
        return false;
    }

    quantityError.innerHTML = '<i class="fa-solid fa-circle-check"></i>';
    return true;
}

function validateForm() {
    if (!validateSupplier() && !validateInvoiceNo() && !validateMedicine() && !validateQuantity()) {
        submitError.style.display = 'block';
        submitError.innerHTML = 'Please fix error to submit';
        setTimeout(function () { submitError.style.display = 'none'; }, 3000);
        return false;
    }
}