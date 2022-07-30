function LoadSosChart() {
    var urlChartSOS = "/ContentWidget/GetDataRoutineSummary?routineCode=" + "SOS";
    var chartSizeSos = $('#chartContentSos').width();
    CallSericeChart(function (data) {
        ChartRoutineSummary(data, 'graficoSOS', function (levelCode) { LoadDataWidget("SOS", levelCode) }, chartSizeSos);
    }, urlChartSOS);
}

function LoadVimsChart() {
    var urlChartVIMS = "/ContentWidget/GetDataRoutineSummary?routineCode=" + "VIMS";
    var chartSizeVims = $('#chartContentVims').width();
    CallSericeChart(function (data) {
        ChartRoutineSummary(data, 'graficoVIMS', function (levelCode) { LoadDataWidget("VIMS", levelCode) }, chartSizeVims);
    }, urlChartVIMS);
}

function LoadTamChart() {
    var urlChartTAM = "/ContentWidget/GetDataRoutineSummary?routineCode=" + "VL";
    var chartSizeVisionLink = $('#chartContentVisionLink').width();
    CallSericeChart(function (data) {
        ChartRoutineSummary(data, 'graficoTAM', function (levelCode) { LoadDataWidget("VL", levelCode) }, chartSizeVisionLink);
    }, urlChartTAM);
}

function LoadActionPlanChart() {
    var urlChartAP = "/ContentWidget/GetDataActionPlanSummary";
    var chartSizeActionPlan = $('#chartContentActionPlan').width();
    CallSericeChart(function (data) {
        ChartRoutineSummaryAP(data, 'graficoActionPlan', function (levelCode) { LoadDataActionPlan(levelCode) }, chartSizeActionPlan);
    }, urlChartAP);
}

function LoadDataActionPlan(levelCode) {
    CallBackGet(function (data) {
        $("#dashboard").html(data);
    }, "/DataAnalysis/GetDataActionPlanView/", { status: levelCode })
}

function ChartRoutineSummary(data, container, callback, chartSize) {
    new pvc.PieChart({
        canvas: container,
        crosstabMode: false,

        // Main plot
        valuesVisible: true,
        valuesFont: '700 12px "Open Sans"',
        explodedSliceRadius: '2%',
        slice_innerRadiusEx: '0%',

        // Panels
        legend: false,

        // Panels
        titleSize: { height: '0%' },
        titlePaddings: '0%',
        titleFont: 'lighter 30px "Open Sans"',
        title_fillStyle: '#FFFFFF',
        titleLabel_textStyle: '#333333',
        // Chart/Interaction
        selectable: true,
        hoverable: true,
        animate: true,
        tooltipClassName: 'light',
        clickable:true,
        clickAction: function (scene) {
            levelCode = this.scene.atoms["category"].value;
            callback(levelCode);    
        },
        height: 200,
        width: chartSize,
        // Color axes
        colors: [
            '#e32037', '#f1e63a', '#449539'
        ]
    })
    .setData(data)
    .render();
}

function ChartRoutineSummaryAP(data, container, callback, chartSize) {
    new pvc.PieChart({
        canvas: container,
        crosstabMode: false,

        // Main plot
        valuesVisible: true,
        valuesFont: '700 12px "Open Sans"',
        explodedSliceRadius: '2%',
        slice_innerRadiusEx: '0%',

        // Panels
        legend: true,

        // Panels
        titleSize: { height: '0%' },
        titlePaddings: '0%',
        titleFont: 'lighter 30px "Open Sans"',
        title_fillStyle: '#FFFFFF',
        titleLabel_textStyle: '#333333',
        // Chart/Interaction
        selectable: true,
        hoverable: true,
        animate: true,
        tooltipClassName: 'light',
        clickable: true,
        clickAction: function (scene) {
            levelCode = this.scene.atoms["category"].value;
            callback(levelCode);
        },
        height: 200,
        width: chartSize,
        // Color axes
    })
    .setData(data)
    .render();
}

function LoadTableIncidencesSite() {
    function formatTableToolsButton(node) {
        $(node).removeClass('DTTT_button');
        $(node).addClass('btn btn-default');
    }
    var TitleName = $("#indicencesTable").parent().children(".portlet-title").children(".caption").text().trim();
    var table1 = $('#table_IncidencesSite').dataTable({
        "destroy": true,
        "dom": 'T<"clear">lfrtip',
        "tableTools": {
            "sSwfPath": "../../Assets/copy_csv_xls_pdf.swf",
            "aButtons": [{
                "sExtends": "collection",
                "fnInit": function (node) { formatTableToolsButton(node, 'ui-icon-print'); },
                "sButtonText": '<i class="fa fa-download"></i>',
                "aButtons": [{
                    "sExtends": "pdf",
                    "sButtonText": "PDF",
                    "sTitle": TitleName
                }, {
                    "sExtends": "csv",
                    "sButtonText": "CSV",
                    "sTitle": TitleName
                }, {
                    "sExtends": "xls",
                    "sButtonText": "Excel",
                    "sTitle": TitleName
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
        "bSort": true,
        "sAjaxSource": "/ContentWidget/GetDataIncidencesSite",
        "sAjaxDataProp": "",
        "aaSorting": [],
        "bProcessing": false,
        "columnDefs": [{
                          "className": 'criticidadAlta text-right clickable',
                          "targets": [2]
                        },{
                            "className": 'routine clickable',
                            "targets": [1]
                        },
                        {
                          "className": 'criticidadMedia text-right clickable',
                          "targets": [3]
                        },{
                            "visible": false,
                            "searchable": false,
                            "targets": [0]
                        }],
        "aoColumns": [
                        { "mDataProp": "RoutineCode" },
                        { "mDataProp": "Name" },
                        { "mDataProp": "CountRed" },
                        { "mDataProp": "CountYellow" },

        ]
    });

    
    $("#table_IncidencesSite tbody td").live("click", function (event) {
        if ($(this).hasClass('criticidadAlta')) {
            levelCode = 3;
        } else {
            if ($(this).hasClass('criticidadMedia')) {
                levelCode = 2;
            }
        }
        var row = $(this).parent();
        var routineCode = table1.fnGetData(row)["RoutineCode"];

        if (levelCode == 3 || levelCode == 2) {
            LoadDataWidget(routineCode, levelCode);
        }
    });
}

function LoadDataWidget(routineCode, criticalLevelCode)
{
    var params = { routineCode: routineCode, criticalLevelCode: levelCode };
    CallBackGet(function (data) {
        $("#dashboard").html(data);
        AddLastState(params);
    }, "/DataAnalysis/Index/", params, true)

    
}

function LoadTableGeneralStatus() {
    function formatTableToolsButton(node) {
        $(node).removeClass('DTTT_button');
        $(node).addClass('btn btn-default');
    }

        var TitleName = $('#generalStatus').parent().children(".portlet-title").children(".caption").text().trim();
        var table = $('#table_GeneralStatus').dataTable({
        "sScrollYInner": "100%",
        "sScrollX": "100%",
        "dom": 'T<"clear">lfrtip',
        "tableTools": {
            "sSwfPath": "../../Assets/copy_csv_xls_pdf.swf",
            "aButtons": [{
                "sExtends": "collection",
                "fnInit": function (node) { formatTableToolsButton(node, 'ui-icon-print'); },
                "sButtonText": '<i class="fa fa-download"></i>',

                "aButtons": [{
                    "sExtends": "pdf",
                    "sButtonText": "PDF",
                    "sTitle": TitleName
                }, {
                    "sExtends": "csv",
                    "sButtonText": "CSV",
                    "sTitle": TitleName
                }, {
                    "sExtends": "xls",
                    "sButtonText": "Excel",
                    "sTitle": TitleName
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
        "sScrollXInner": "100%",
        "scrollCollapse": true,
        "destroy": true,
        "bServerSide": false,
        "bSort": true,
        "sAjaxSource": "/ContentWidget/GetDataGeneralStatus",
        "sAjaxDataProp": "",
        "bProcessing": false,
        "aaSorting": [],
        "columnDefs": [{
            className: 'clickable',
            targets: [0,1,2,3]
        },
                        {
                            className: 'criticidadAlta text-right clickable',
                         targets: [5]
                        },
                        {
                            className: 'text-right clickable',
                            type: 'date-eu',
                            targets: [4]
                        },
                        {
                            className: 'criticidadMedia text-right clickable',
                         targets: [6]
                        },
                        {
                            className: 'criticidadBaja text-right clickable',
                        targets: [7]
                        }, {
                            "visible": false,
                            "searchable": false,
                            targets: [8]
                        }, {
                            "visible": false,
                            "searchable": false,
                            targets: [9]
                        }],
        "aoColumns": [
                        { "mDataProp": "MineName" },
                        { "mDataProp": "EquipmentName" },
                        { "mDataProp": "ComponentName" },
                        { "mDataProp": "SampleName" },
                        { "mDataProp": "Date" },
                        { "mDataProp": "CountRed" },
                        { "mDataProp": "CountYellow" },
                        { "mDataProp": "CountGreen" },
                        { "mDataProp": "ComponentId" },
                        { "mDataProp": "RoutineCode" }


        ]
    });
    $("#table_GeneralStatus tbody tr").live("click", function (event) {
        var componentId = table.fnGetData(this)["ComponentId"];
        var routine = table.fnGetData(this)["RoutineCode"];
        var obj = new Object;
        obj.ComponentId = componentId;
        obj.RoutineCode = routine;

        CallBackGet(function (data) {
            $("#dashboard").html(data);
        }, "/Analysis/GetRoutineAnalysisManageByComponentId", obj)

        //AddLastState();
    });
    //AddLastState();
}


function LoadEquipments(siteId, ddlEquipments) {
    var params = new Object;
    params.siteIds = siteId;

    if (params.siteIds != null) {
        CallSericeAjax(function (data) {
            LoadValuesDropDownList(ddlEquipments, data);
        }, "/Equipment/GetEquipmentsBySite/", params, HttpActions.POST);
    }
}

function LoadEquipmentsByDefaultValue(siteId, ddlEquipments, value) {
    var params = new Object;
    params.siteIds = siteId;
    if (params.siteIds != null) {
        CallSericeAjax(function (data) {
            LoadValuesDropDownList(ddlEquipments, data);
            if (value != "") {
                $(ddlEquipments).val(value);
                $(ddlEquipments).multiselect("refresh");
            }
        }, "/Equipment/GetEquipmentsBySite/", params, HttpActions.POST);
    }
}

function LoadEquipmentModels(siteId, ddlEquipmentModels) {
    var params = new Object;
    params.siteIds = siteId;

    CallSericeAjax(function (data) {
        LoadValuesDropDownList(ddlEquipmentModels, data);
    }, "/Equipment/GetEquipmentModelsBySite/", params, HttpActions.POST);
}

function LoadEquipmentModelsByDefaultValue(siteId, ddlEquipmentModels, value) {
    var params = new Object;
    params.siteIds = siteId;

    CallSericeAjax(function (data) {
        LoadValuesDropDownList(ddlEquipmentModels, data);
        if (value != "") {
            $(ddlEquipmentModels).val(value);
            $(ddlEquipmentModels).multiselect("refresh");
        }
    }, "/Equipment/GetEquipmentModelsBySite/", params, HttpActions.POST);
}

function LoadEquipmentByModels(siteId, equipmentModelId, ddlEquipment) {
    var params = new Object;
    params.siteIds = siteId;
    params.equipmentModelIds = equipmentModelId;

    CallSericeAjax(function (data) {
        LoadValuesDropDownList(ddlEquipment, data);
    }, "/Equipment/GetEquipmentByModels/", params, HttpActions.POST);
}

function LoadFleets(siteId, ddlFleets) {
    var params = new Object;
    params.siteIds = siteId;

    CallSericeAjax(function (data) {
        LoadValuesDropDownList(ddlFleets, data);
    }, "/Equipment/GetFleetsBySite/", params, HttpActions.POST);
}

function LoadFleetsByDefaultValue(siteId, ddlFleets, value) {
    var params = new Object;
    params.siteIds = siteId;

    CallSericeAjax(function (data) {
        LoadValuesDropDownList(ddlFleets, data);
        if (value != "")
        {
            $(ddlFleets).val(value);
            $(ddlFleets).multiselect("refresh");
        }
    }, "/Equipment/GetFleetsBySite/", params, HttpActions.POST);
}

function LoadComponents(equipmentId, ddlComponents) {
    var params = new Object;
    params.equipmentId = equipmentId;

    CallSericeAjax(function (data) {
        LoadValuesDropDownList(ddlComponents, data);
    }, "/Component/GetComponentsByEquipmentId/", params, HttpActions.POST);
}

function LoadComponentsByComponentModel(componentModelId, ddlComponents) {
    var params = new Object;
    params.componentModelId = componentModelId;

    CallSericeAjax(function (data) {
        LoadValuesDropDownList(ddlComponents, data);
    }, "/Component/GetComponentsByComponentModel/", params, HttpActions.POST);
}

function LoadSubRoutines(routineId, ddlSubRoutines) {
    var params = new Object;
    params.RoutineId = routineId;

    CallSericeAjax(function (data) {
        LoadValuesDropDownList(ddlSubRoutines, data);
    }, "/Routine/GetSubRoutinesByRoutine/", params, 'POST');
}

function LoadSubRoutinesByDefaultValue(routineId, ddlSubRoutines, value) {
    var params = new Object;
    params.RoutineId = routineId;

    CallSericeAjax(function (data) {
        LoadValuesDropDownList(ddlSubRoutines, data);
        if(value != null)
        {
            $(ddlSubRoutines).val(value);
            $(ddlSubRoutines).multiselect("refresh");
        }
    }, "/Routine/GetSubRoutinesByRoutine/", params, 'POST');
}

function LoadTests(routineId, ddlTests) {
    var params = new Object;
    params.RoutineId = routineId;
    params.attachFile = true;
    CallSericeAjax(function (data) {
        LoadValuesDropDownList(ddlTests, data);
    }, "/Test/GetTestsByRoutineId/", params, 'POST');
}

function LoadRoutinesByEquipment(equipmentId, ddlRoutines) {
    var params = new Object;
    params.equipmentId = equipmentId;

    if (params.equipmentId != null) {
        CallSericeAjax(function (data) {
            LoadValuesDropDownList(ddlRoutines, data);
        }, "/Routine/GetRoutinesByEquipment/", params, HttpActions.POST);
    }
}
