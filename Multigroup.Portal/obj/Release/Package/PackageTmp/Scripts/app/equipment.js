function LoadComponentsTable() {
    function formatTableToolsButton(node) {
        $(node).removeClass('DTTT_button');
        $(node).addClass('btn btn-default');
    }
    var table = $('#tabla_id').dataTable({
        "aaSorting": [],
        "destroy": true,
        "bServerSide": false,
        "sScrollYInner": "100%",
        "dom": 'T<"clear">lfrtip',
        "tableTools": {
            "sSwfPath": "../../Assets/copy_csv_xls_pdf.swf",
            "aButtons": [{
                "sExtends": "collection",
                "fnInit": function (node) { formatTableToolsButton(node, 'ui-icon-print'); },
                "sButtonText": '<i class="fa fa-download"></i>',

                "aButtons": [{
                    "sExtends": "pdf",
                    "sButtonText": "PDF"
                }, {
                    "sExtends": "csv",
                    "sButtonText": "CSV"
                }, {
                    "sExtends": "xls",
                    "sButtonText": "Excel"
                }, {
                    "sExtends": "print",
                    "sButtonText": "Imprimir",
                }, {
                    "sExtends": "copy",
                    "sButtonText": "Copiar"
                }
                ],
            }],
        },
        "sScrollX": "100%",
        "sScrollXInner": "100%",
        "scrollCollapse": true,
        "bProcessing": false
    });

    $('.tooltipAai').tooltip({
        container: 'body',
        title: 'Seleccione un componente'
    });
    $('#tabla_id tbody').unbind("click").on('click', 'tr', function (event) {
        event.stopPropagation();
        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected');
            $("#btnEditComponent").attr("disabled", "disabled");
            $("#idTooltipAai").removeClass('tooltipAai');
            $('#idTooltipAai').tooltip({
                container: 'body',
                title: 'Seleccione una componente'
            });
        }
        else {
            $('#tabla_id tr.selected').removeClass('selected');
            $(this).addClass('selected');
            $("#btnEditComponent").removeAttr("disabled", "disabled");
            $("#idTooltipAai").tooltip('destroy');
            id = table.fnGetData(this)[0];
        }
    });
}