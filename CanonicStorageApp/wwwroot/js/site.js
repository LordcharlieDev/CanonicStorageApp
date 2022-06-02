// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.
function printPDF()
{
    var element = document.getElementById('mainTable')
    html2pdf().from(element).set({
        margin: [30, 10, 5, 10],
        pagebreak: { avoid: 'tr' },
        jsPDF: { orientation: 'landscape', unit: 'pt', format: 'letter', compressPDF: true }
    }).save()
}

function OpenPrintTab(action, controller)
{
    window.open("/" + controller + "/" + action, "_blank",
        "menubar=no, toolbar=no, resizable=no, top=100, left=200, width=500, height=500");
}

// Write your JavaScript code.
