// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.
function printPDF() {
    var element = document.getElementById('mainTable')
    html2pdf().from(element).set({
        margin: [30, 10, 5, 10],
        pagebreak: { avoid: 'tr' },
        jsPDF: { orientation: 'landscape', unit: 'pt', format: 'letter', compressPDF: true }
    }).save()
}

// Write your JavaScript code.
