var sampleId;

function LoadTableSampleDetailsPivot(tableId, url, params) {

    CallSericeAjax(function (data) {
        var table = $(tableId).dataTable({
            "destroy": true,
            "columnDefs": [{
                "title": "Column",
                "targets": 0
            }]
        });
        table.fnDestroy();
        table.empty();
        if (data.length > 0)
        {
            var columns = new Array();
            var properties = new Array();
            jQuery.each(data, function (i, val) {
                var index = 0;
                if (columns.length == 0) {
                    for (var propName in val) {
                        if (index == 2 || index == 3) {
                            var head = { title: propName, targets: index, type: 'date-eu'};
                            var prop = { "mDataProp": propName };
                            index++;
                            columns.push(head);
                            properties.push(prop);
                        }else{
                            var head = { title: propName, targets: index};
                            var prop = { "mDataProp": propName };
                            index++;
                            columns.push(head);
                            properties.push(prop);
                        } 


                    }
                }
            });
            function formatTableToolsButton(node) {
                $(node).removeClass('DTTT_button');
                $(node).addClass('btn btn-default');
            }
            $(tableId).dataTable({
                "destroy": true,
                "aaSorting": [],
                "sScrollY": "360px",
                "sScrollYInner": "100%",
                "sScrollX": "100%",
                "fnInitComplete": function () {
                    /**
                     * Go to plugin definition for
                     * instructions on how to use.
                     */
                    this.fnHideEmptyColumns(this);
                },
                "sScrollXInner": "100%",
                "scrollCollapse": true,
                "bServerSide": false,
                "aaData": data,
                "sAjaxDataProp": "",
                "bProcessing": false,
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
                
                "aoColumns": properties,
                "columnDefs": columns,

            });
            $(tableId).unbind("click").on('click', 'tr', function (event) {
                event.stopPropagation();
                if ($(this).hasClass('selected')) {
                    $(this).removeClass('selected');
                    $("#editSampleDetail").attr("disabled", "disabled");
                    $("#idtooltipSampleManage").removeClass('tooltipSampleManage');
                    $('#idtooltipSampleManage').tooltip({
                        container: 'body',
                        title: 'Seleccione una muestra'
                    });
                }
                else {
                    $(tableId + ' tr.selected').removeClass('selected');
                    $(this).addClass('selected');
                    $("#editSampleDetail").removeAttr("disabled", "disabled");
                    $("#analysisNew").removeAttr("disabled", "disabled");
                    $("#idtooltipSampleManage").tooltip('destroy');
                    sampleId = table.fnGetData(this)["SampleId"];
                }
            });
            $('.tooltipSampleManage').tooltip({
                container: 'body',
                title: 'Seleccione una muestra'
            });
        } else {
            var columns = new Array();
            var properties = new Array();
            jQuery.each(data, function (i, val) {
                var index = 0;
                if (columns.length == 0) {
                    for (var propName in val) {
                        if (index == 2 || index == 3) {
                            var head = { title: propName, targets: index, type: 'date-eu' };
                            var prop = { "mDataProp": propName };
                            index++;
                            columns.push(head);
                            properties.push(prop);
                        } else {
                            var head = { title: propName, targets: index };
                            var prop = { "mDataProp": propName };
                            index++;
                            columns.push(head);
                            properties.push(prop);
                        }


                    }
                }
            });
            function formatTableToolsButton(node) {
                $(node).removeClass('DTTT_button');
                $(node).addClass('btn btn-default');
            }
            $(tableId).dataTable({
                "destroy": true,
                "aaSorting": [],
                "sScrollY": "360px",
                "sScrollYInner": "100%",
                "sScrollX": "100%",
                "sScrollXInner": "100%",
                "scrollCollapse": true,
                "bServerSide": false,
                "aaData": data,
                "sAjaxDataProp": "",
                "bProcessing": false,
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

                "aoColumns": properties,
                "columnDefs": columns,

            });
        }

    }, url, params, 'POST');
}

function LoadTableSampleDetails(tableId, url, params) {
    var table = $(tableId).dataTable({
        "destroy": true,
        "columnDefs": [{
            "title": "Column",
            "targets": 0
        }]
    });
    table.fnDestroy();
    table.empty();

    $(tableId).dataTable({
        "destroy": true,
        "sScrollY": "250px",
        "sScrollYInner": "100%",
        "sScrollX": "100%",
        "bFilter": false,
        "sScrollXInner": "100%",
        "scrollCollapse": true,
        "bServerSide": true,
        "sAjaxSource": url,
        "sServerMethod": "POST",
        "bSort" : false,
        "fnServerParams": function (aoData) {
            aoData.push({ name: "filters", value: params }); // es necesario para realizar la paginacion
        },
        "bProcessing": true,
         "columnDefs": [
            {
                "targets": [4],
                "className": 'openModal text-right'
            },
            {
                "targets": [0,1, 2, 3,4],
                "className": 'openModal'
            }, 

            { "title": "Sub Rutina", "targets": 0 },
            { "title": "Tipo", "targets": 1 },
            { "title": "Fecha", "targets": 2 },
            { "title": "Test", "targets": 3 },
            { "title": "Nivel", "targets": 4 },
            { "title": "Valor", "targets": 5 }

        ],
        "aoColumns": [
                        { "mDataProp": "SubRoutine" },
                        { "mDataProp": "TestType" },
                        { "mDataProp": "TestDate" },
                        { "mDataProp": "TestName" },
                        { "mDataProp": "TestStatus" },
                        { "mDataProp": "Value" }
        ]
    });
    $(tableId).unbind("click").on('click', 'tr', function (event) {
        event.stopPropagation();
        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected');
            $("#BtnSampleManage").attr("disabled", "disabled");
            $("#idtooltipSampleManage").removeClass('tooltipSampleManage');
            $('#idtooltipSampleManage').tooltip({
                container: 'body',
                title: 'Seleccione una flota'
            });
        }
        else {
            $(tableId+' tr.selected').removeClass('selected');
            $(this).addClass('selected');
            $("#BtnSampleManage").removeAttr("disabled", "disabled");
            $("#analysisNew").removeAttr("disabled", "disabled");
            $("#idtooltipSampleManage").tooltip('destroy');
            //aaiID = table.fnGetData(this)["ActionPlanId"];
        } 
    });
    $('.tooltipSampleManage').tooltip({
        container: 'body',
        title: 'Seleccione una muestra'
    });
}

function LoadTableEventDetails(tableId, url, params) {
    var table = $(tableId).dataTable({
        "destroy": true,
        "columnDefs": [{
            "title": "Column",
            "targets": 0
        }]
    });
    table.fnDestroy();
    table.empty();
    $(tableId).dataTable({
        "destroy": true,
        "sScrollY": "250px",
        "sScrollYInner": "100%",
        "sScrollX": "100%",
        "bFilter": false,
        "sScrollXInner": "100%",
        "scrollCollapse": true,
        "bServerSide": true,
        "sAjaxSource": url,
        "sServerMethod": "POST",
        "bSort": false,
        "fnServerParams": function (aoData) {
            aoData.push({ name: "filters", value: params }); // es necesario para realizar la paginacion
        },
        "bProcessing": true,
        "columnDefs": [
            {
                "targets": [4],
                "className": 'openModal text-right'
            },
            {
                "visible": false,
                "targets": [0],
                "className": 'openModal text-right'
            },
            {
                "targets": [1, 2, 3, 4],
                "className": 'openModal'
            },
            { "title": "SampleDetailId", "targets": 0 },
            { "title": "Date", "targets": 1 },
            { "title": "Event", "targets": 2 },
            { "title": "SMU", "targets": 3 },
            { "title": "Limit", "targets": 4 },
            { "title": "Code", "targets": 5 },
            { "title": "WorstValue", "targets": 6 },
            { "title": "Criticidad", "targets": 7 },
            { "title": "Ocurrencia", "targets": 8 },

            { "title": "FMI", "targets": 9 },
            { "title": "Duration", "targets": 10 },
            { "title": "Ack Number", "targets": 11 },
            { "title": "AckDuration", "targets": 12 },
            { "title": "Operator", "targets": 13 }
        ],
        "aoColumns": [
                        { "mDataProp": "SampleDetailId" },
                        { "mDataProp": "SampleDetailDate" },

                        { "mDataProp": "TestName" },
                        { "mDataProp": "SMU" },
                        { "mDataProp": "Limit" },
                        { "mDataProp": "Code" },
                        { "mDataProp": "WorstValue" },
                        { "mDataProp": "SampleStatusName" },
                        { "mDataProp": "NumericValue" },
                        { "mDataProp": "FMI" },
                        { "mDataProp": "Duration" },
                        { "mDataProp": "AckNumber" },
                        { "mDataProp": "AckDuration" },
                        { "mDataProp": "Operator" }
        ]
    });

    $(tableId).unbind("click").on('click', 'tr', function (event) {
        event.stopPropagation();
        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected');
            $("#BtnSampleManage").attr("disabled", "disabled");
            $("#idtooltipSampleManage").removeClass('tooltipSampleManage');
            $('#idtooltipSampleManage').tooltip({
                container: 'body',
                title: 'Seleccione una flota'
            });
        }
        else {
            $(tableId + ' tr.selected').removeClass('selected');
            $(this).addClass('selected');
            $("#BtnSampleManage").removeAttr("disabled", "disabled");
            $("#analysisNew").removeAttr("disabled", "disabled");
            $("#idtooltipSampleManage").tooltip('destroy');
        }
    });
    $('.tooltipSampleManage').tooltip({
        container: 'body',
        title: 'Seleccione una muestra'
    });
}

function LoadTableTrendDetails(tableId, url, params) {
    var table = $(tableId).dataTable({
        "destroy": true,
        "columnDefs": [{
            "title": "Column",
            "targets": 0
        }]
    });
    table.fnDestroy();
    table.empty();
    $(tableId).dataTable({
        "destroy": true,
        "sScrollY": "250px",
        "sScrollYInner": "100%",
        "sScrollX": "100%",
        "bFilter": false,
        "sScrollXInner": "100%",
        "scrollCollapse": true,
        "bServerSide": true,
        "sAjaxSource": url,
        "sServerMethod": "POST",
        "bSort": false,
        "fnServerParams": function (aoData) {
            aoData.push({ name: "filters", value: params }); // es necesario para realizar la paginacion
        },
        "bProcessing": true,
        "columnDefs": [
            {
                "visible": false,
                "targets": [0],
                "className": 'openModal text-right'
            },
            {
                "targets": [1, 2, 3, 4],
                "className": 'openModal'
            },
            { "title": "SampleDetailId", "targets": 0 },
            { "title": "Date", "targets": 1 },
            { "title": "Tendencia", "targets": 2 },
            { "title": "SMU", "targets": 3 },
            { "title": "Valor", "targets": 4 },
            { "title": "Criticidad", "targets": 5 },
            { "title": "Tipo", "targets": 6 },
            { "title": "Condici&oacute;n", "targets": 7 },
            { "title": "Canal", "targets": 8},
            { "title": "FMI", "targets": 9 },

            { "title": "MID", "targets": 10 },
            { "title": "M&oacute;dulo", "targets": 11 }
        ],
        "aoColumns": [
                        { "mDataProp": "SampleDetailId" },
                        { "mDataProp": "SampleDetailDate" },

                        { "mDataProp": "TestName" },
                        { "mDataProp": "SMU" },
                        { "mDataProp": "NumericValue" },
                        { "mDataProp": "SampleStatusName" },
                        { "mDataProp": "Type" },
                        { "mDataProp": "Condition" },
                        { "mDataProp": "Channel" },
                        { "mDataProp": "FMI" },
                        { "mDataProp": "Mid" },
                        { "mDataProp": "Module" }
        ]
    });

    $(tableId).unbind("click").on('click', 'tr', function (event) {
        event.stopPropagation();
        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected');
            $("#BtnSampleManage").attr("disabled", "disabled");
            $("#idtooltipSampleManage").removeClass('tooltipSampleManage');
            $('#idtooltipSampleManage').tooltip({
                container: 'body',
                title: 'Seleccione una flota'
            });
        }
        else {
            $(tableId + ' tr.selected').removeClass('selected');
            $(this).addClass('selected');
            $("#BtnSampleManage").removeAttr("disabled", "disabled");
            $("#analysisNew").removeAttr("disabled", "disabled");
            $("#idtooltipSampleManage").tooltip('destroy');
        }
    });
    $('.tooltipSampleManage').tooltip({
        container: 'body',
        title: 'Seleccione una muestra'
    });
}

function LoadTableSampleDetailsParticulado(tableId, url, params) {
    var table = $(tableId).dataTable({
        "destroy": true,
        "columnDefs": [{
            "title": "Column",
            "targets": 0
        }]
    });
    table.fnDestroy();
    table.empty();

    function formatTableToolsButton(node) {
        $(node).removeClass('DTTT_button');
        $(node).addClass('btn btn-default');
    }

   var table = $(tableId).dataTable({
        "destroy": true,
        "sScrollY": "250px",
        "sScrollYInner": "100%",
        "sScrollX": "100%",
        "bFilter": false,
        "sScrollXInner": "100%",
        "scrollCollapse": true,
        "bServerSide": true,
        "sAjaxSource": url,
        "sServerMethod": "POST",
        "bSort": false,
        "fnServerParams": function (aoData) {
            aoData.push({ name: "filters", value: params }); // es necesario para realizar la paginacion
        },
        "bProcessing": true,
        "dom": 'T<"clear">lfrtip',
        "tableTools": {
            "sSwfPath": "../../Assets/copy_csv_xls_pdf.swf",
            "aButtons": [{
                "sExtends": "collection",
                "fnInit": function (node) { formatTableToolsButton(node, 'ui-icon-print'); },
                "sButtonText": '<i class="fa fa-download"></i>',
                "mColumns": [0, 1, 2, 3, 4],
                "aButtons": [{
                    "sExtends": "pdf",
                    "sButtonText": "PDF",
                    "mColumns": [0, 1, 2, 3, 4],
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
        "columnDefs": [
            {
                "targets": [6],
                "visible": false
            }, {
                "targets": [5],
                "visible": false
            },
            {
                "targets": [0, 1, 2, 3,4],
                "className": 'clickable'
            },

            { "title": "Fecha", "targets": 0 },
            { "title": "Test", "targets": 1 },
            { "title": "SMU", "targets": 2 },
            { "title": "Valor", "targets": 3 },
            { "title": "Criticidad", "targets": 4 },
            { "title": " ", "targets": 5 },
            { "title": " ", "targets": 6 }

        ],
        "aoColumns": [
                        { "mDataProp": "TestDate" },
                        { "mDataProp": "TestName" },
                        { "mDataProp": "SMU" },
                        { "mDataProp": "NumericValue" },
                        { "mDataProp": "TestStatus" },
                        { "mDataProp": "Observation" },
                        { "mDataProp": "ImageValue" }
        ]
    });

    $(tableId).unbind("click").on('click', 'tr', function (event) {
        event.stopPropagation();
        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected');
            $("#BtnSampleManage").attr("disabled", "disabled");
            $("#idtooltipSampleManage").removeClass('tooltipSampleManage');
            $('#idtooltipSampleManage').tooltip({
                container: 'body',
                title: 'Seleccione una flota'
            });
        }
        else {
            $(tableId + ' tr.selected').removeClass('selected');
            $(this).addClass('selected');
            $("#BtnSampleManage").removeAttr("disabled", "disabled");
            $("#analysisNew").removeAttr("disabled", "disabled");
            $("#idtooltipSampleManage").tooltip('destroy');
            image = table.fnGetData(this)["ImageValue"];
            if (image == null){
                $("#imageSample").hide('slow');
            } else {
                $("#imageSample").attr("src", "../../Files/Tests/img/" + image);
                $("#imageSample").show('slow');
            }
            $("#observationSample").val(table.fnGetData(this)["Observation"]);
        }
    });
    $('.tooltipSampleManage').tooltip({
        container: 'body',
        title: 'Seleccione una muestra'
    });
}

function LoadTableSampleDetailsByEquipment(tableId, data) {
    var table = $(tableId).dataTable({
        "destroy": true,
        "columnDefs": [{
            "title": "Column",
            "targets": 0
        }]
    });
    table.fnDestroy();
    table.empty();

    function formatTableToolsButton(node) {
        $(node).removeClass('DTTT_button');
        $(node).addClass('btn btn-default');
    }

    $(tableId).dataTable({
        "destroy": true,
        "sScrollY": "250px",
        "sScrollYInner": "100%",
        "sScrollX": "100%",
        "bFilter": false,
        "sScrollXInner": "100%",
        "scrollCollapse": true,
        "bSort": false,
        aaData : data,
        "bProcessing": true,
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
        "columnDefs": [
            { "title": "Nro Serie", "targets": 0 },
            { "title": "Id Interno", "targets": 1 },
            { "title": "Fecha", "targets": 2 },
            { "title": "Test", "targets": 3 },
            { "title": "Rutina", "targets": 4 },
            { "title": "Criticidad", "targets": 5 },
            { "title": "Recurrencia", "targets": 6 }

        ],
        "aoColumns": [
                        { "mDataProp": "EquipmentSerial" },
                        { "mDataProp": "InternalNumber" },
                        { "mDataProp": "TestDate" },
                        { "mDataProp": "TestName" },
                        { "mDataProp": "RutineName" },
                        { "mDataProp": "StatusName" },
                        { "mDataProp": "NumericValue" }

        ]
    });

    $(tableId).unbind("click").on('click', 'tr', function (event) {
        event.stopPropagation();
        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected');
            $("#BtnSampleManage").attr("disabled", "disabled");
            $("#idtooltipSampleManage").removeClass('tooltipSampleManage');
            $('#idtooltipSampleManage').tooltip({
                container: 'body',
                title: 'Seleccione una flota'
            });
        }
        else {
            $(tableId + ' tr.selected').removeClass('selected');
            $(this).addClass('selected');
            $("#BtnSampleManage").removeAttr("disabled", "disabled");
            $("#analysisNew").removeAttr("disabled", "disabled");
            $("#idtooltipSampleManage").tooltip('destroy');
            //aaiID = table.fnGetData(this)["ActionPlanId"];
        }
    });
    $('.tooltipSampleManage').tooltip({
        container: 'body',
        title: 'Seleccione una muestra'
    });
}
