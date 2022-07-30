function LoadTableAnalysisHistory(tableId, url) {
    function formatTableToolsButton(node) {
        $(node).removeClass('DTTT_button');
        $(node).addClass('btn btn-default');
    }
    var tableHistory = $(tableId).dataTable({
        "destroy": true,
        "aaSorting": [],
        "sScrollY": "200px",
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
        "sScrollYInner": "100%",
        "sScrollX": "100%",
        "sScrollXInner": "100%",
        "scrollCollapse": true,
        "bServerSide": false,
        "sAjaxSource": url,
        "sAjaxDataProp": "",
        "bProcessing": false,
        "columnDefs": [
            { "title": "Fechas", "targets": 0 ,"type": 'date-eu'},
            { "title": "Estado", "targets": 1 ,"className": 'text-right'},
            { "title": "SMU", "targets": 2 ,"className": 'text-right'},
            { "title": "Plan de Acción", "targets": 4 },
            { "title": "Comentarios", "targets": 3 },
            { "title": "Analista", "targets": 5 }

        ],
        "aoColumns": [
                        { "mDataProp": "AnalysisDate" },
                        { "mDataProp": "AnalysisStatus" },
                        { "mDataProp": "SMU" },
                        { "mDataProp": "Comments" },
                        { "mDataProp": "Observations" },
                        { "mDataProp": "UserName" },
        ]
    });

    $("#tableAnlysisHistory tbody tr").live("click", function (event) {
        //routine = tableHistory.fnGetData(this)["RoutineCode"];
    });

    $('#tableAnlysisHistory tbody').unbind("click").on('click', 'tr', function () {
        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected');
            $("#btnViewHistory").attr("disabled", "disabled");
            $("#idTooltipHistory").removeClass('tooltipHistory');
            $('#idTooltipHistory').tooltip({
                container: 'body',
                title: 'Seleccione un Registro'
            });
        }
        else {
            $('#tableAnlysisHistory tr.selected').removeClass('selected');
            $(this).addClass('selected');
            $("#btnViewHistory").removeAttr("disabled", "disabled");
            $("#idTooltipHistory").tooltip('destroy');
            // aaiID = tableHistory.fnGetData(this)["ActionPlanId"];
        }
    });

}