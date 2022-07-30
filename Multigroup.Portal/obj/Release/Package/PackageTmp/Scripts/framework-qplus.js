var statusResponse = { "success": 200, "error": 400 }
var HttpActions = { "GET": "GET", "POST": "POST", "DELETE": "DELETE" }
var currentState = { data: "", url: "", callback: "" }
var historyStates = [];

function CallSericeChart(callback, url) {
    $.ajax({
        async: true,//options.async,
        type: 'GET',//options.type,
        url: url,

        //data: options.data,
        contentType: "application/json; charset=utf-8",
        cache: false,//options.cache, 
        dataType: "json",
        timeout: 60000,//options.timeout,
        beforeSend: function () { $('#spinner').show(); },
        complete: function () { $('#spinner').hide(); },
        error: function (xhr) {
            Redirect(xhr);
        },
        success: function (data, textStatus, jqXHR) {
            if (textStatus === "success") {
                if (typeof callback === "function") callback(data);
            }
            else {
                alert("Error");
            }
        }
    });
}

function CallBackGet(callback, url, info, isSaveHistory) {
    $.ajax({
        async: true,//options.async,
        type: 'GET',//options.type,
        url: url,
        data: info,//info, 
        contentType: "text/html; charset=utf-8",
        traditional: true,
        cache: false,//options.cache,
        dataType: "html",
        timeout: 60000,//options.timeout,
        beforeSend: function () { $('#spinner').show(); },
        complete: function () { $('#spinner').hide(); },
        error: function (xhr) {
            Redirect(xhr);
        },
        success: function (data, textStatus) {
            if (textStatus === "success") {
                SaveHistory(isSaveHistory, url);
                if (typeof callback === "function") callback(data);
            }
            else {
                alert("Error");
            }
        }
    });
}

function CallBackPost(callback, url, info, isSaveHistory) {
    $.ajax({
        async: true,//options.async,
        type: 'POST',//options.type,
        url: url,
        data: JSON.stringify(info),//info, 
        contentType: "application/json; charset=utf-8",
        traditional: true,
        cache: false,//options.cache,
        dataType: "html",
        timeout: 60000,//options.timeout,
        beforeSend: function () { $('#spinner').show(); },
        complete: function () { $('#spinner').hide(); },
        error: function (xhr) {
            Redirect(xhr);
            ResponseMessage(xhr.status, xhr.statusText);
        },
        success: function (data, textStatus) {
            if (textStatus === "success") {
                SaveHistory(isSaveHistory, url);
                if (typeof callback === "function") callback(data);
            }
            else {
                alert("Error");
            }
        }
    });
}


function CallSericeAjax(callback, url, params, action, saveHistory) {
    $.ajax({
        async: true,//options.async,
        type: action,//options.type,
        url: url,
        data: JSON.stringify(params),
        contentType: "application/json; charset=utf-8",
        cache: false,//options.cache, 
        dataType: "json",
        timeout: 60000,//options.timeout,
        beforeSend: function () { $('#spinner').show(); },
        complete: function () { $('#spinner').hide(); },
        success: function (data, textStatus, jqXHR) {
            if (textStatus === "success") {
                if (typeof callback === "function")
                {
                    callback(data);
                }
            }
            else
            {
                alert("Error"); 
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            Redirect(xhr);
            ResponseMessage(xhr.status, xhr.statusText);
        },
    });
}

function Redirect(xhr)
{
    if (xhr.status === 401) {
        window.location = "/";
    }
}

function Spinner()
{
        $(document).ready(function () {
            $('#spinner').ajaxStart(function () {
                $(this).show();
            }).ajaxComplete(function () {
                $(this).hide();
            });
        });
}

function ResponseMessage(response, statusText) {
    var typerMsg;
    if (response <= statusResponse.success)
    {
        typerMsg = "success"
    }
    else
    {
        typerMsg = "error";
    }

    Command: toastr[typerMsg](statusText)
    toastr.options = {
        "closeButton": true,
        "debug": false,
        "newestOnTop": false,
        "progressBar": false,
        "positionClass": "toast-top-center",
        "preventDuplicates": true,
        "onclick": null,
        "showDuration": "300",
        "hideDuration": "1000",
        "timeOut": "5000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    }
}

function SuccessMessage(typeMsg, Message)
{
    Command: toastr[typeMsg](Message)

    toastr.options = {
        "closeButton": true,
        "debug": false,
        "newestOnTop": false,
        "progressBar": false,
        "positionClass": "toast-top-center",
        "preventDuplicates": false,
        "onclick": null,
        "showDuration": "10",
        "hideDuration": "10",
        "timeOut": "5000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    }
}

function NotifyMessage(typeMsg, Message) {
    Command: toastr[typeMsg](Message)

    toastr.options = {
        "closeButton": true,
        "debug": false,
        "newestOnTop": false,
        "progressBar": false,
        "positionClass": "toast-top-center",
        "preventDuplicates": false,
        "onclick": null,
        "showDuration": "10",
        "hideDuration": "10",
        "timeOut": "5000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    }
}

function ConfirmMessage(typeMsg, Message) {
    Command: toastr[typeMsg](Message)

    toastr.options = {
        "closeButton": false,
        "debug": false,
        "newestOnTop": false,
        "progressBar": false,
        "positionClass": "toast-top-center",
        "preventDuplicates": false,
        "onclick": null,
        "showDuration": "300",
        "hideDuration": "1000",
        "timeOut": 0,
        "extendedTimeOut": 0,
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut",
        "tapToDismiss": false
    }
}

function SuccessMessageDelete() {
    Command: toastr["success"]("Los datos se eliminaron correctamente")

    toastr.options = {
        "closeButton": true,
        "debug": false,
        "newestOnTop": false,
        "progressBar": false,
        "positionClass": "toast-top-center",
        "preventDuplicates": false,
        "onclick": null,
        "showDuration": "10",
        "hideDuration": "10",
        "timeOut": "5000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    }
}

function SuccessMessage() {
    Command: toastr["success"]("Los datos se guardaron correctamente")

    toastr.options = {
        "closeButton": true,
        "debug": false,
        "newestOnTop": false,
        "progressBar": false,
        "positionClass": "toast-top-center",
        "preventDuplicates": false,
        "onclick": null,
        "showDuration": "10",
        "hideDuration": "10",
        "timeOut": "5000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    }
}

function FailMessage(xhr, status, error) {
    ResponseMessage(statusResponse.error, error);
}

function OnSuccess(data, status, xhr) {
    ResponseMessage(statusResponse.success, data.Message)
}

function OnStart() {
    event.preventDefault();
    $('#spinner').show();
}

function OnComplete() { $('#spinner').hide(); }

function LoadValuesDropDownList(ddlOptions, data) {
    $(ddlOptions).empty();
    $(ddlOptions).multiselect('destroy');  // tell widget to clear itself
    $.each(data.ListItems, function (index, item) {
        $(ddlOptions).get(0).options[$(ddlOptions).get(0).options.length] = new Option(item.Text, item.Value);
    });

    var points = new Array();
    if (data.SelectedItems != null) {
        $.each(data.SelectedItems, function (index, item) {
            points.push(item.Value)
        });
    }

    $(ddlOptions).val(points);
    $(ddlOptions).multiselect({
        includeSelectAllOption: true,
        enableFiltering: true
    }); // re-initialize the widget
    $(ddlOptions).multiselect("refresh");
}

function OnSuccessDownloadFile(data)
{
    window.location = data;
}

function GetHistory() {
    if (historyStates.length > 0) {
        state = GetLastState();

        CallBackGet(function (data) {
            $("#dashboard").html(data);
            DeleteLastState(state);
        }, state.url, state.data)
    }
    else
        window.location = "/";
}

function SaveHistory(save, url) {

    if (typeof save === 'undefined' && currentState.url == "") {
        currentState.url = "";
    }
    else {
        if (save === true) {
            currentState.url = url;
        }
    }

    if (typeof save === 'undefined' && url != "")
    {
        var state = new Object;
        state.url = url;
        DeleteLastState(state)
    }
}

function AddLastState(data) {

    if (currentState.url != "")
    {
        var state = new Object;
        state.data = data;
        state.url = currentState.url;
        historyStates.push(state);
        currentState.url = "";
    }
}

function AddNewState(url, data) {

        var state = new Object;
        state.data = data;
        state.url = url;
        historyStates.push(state);
}

function UpdateLastState(data)
{
    var state = GetLastState();
    if (state == undefined) {
        AddLastState(data);
    } else state.data = data; 
}

function GetLastState()
{
    var last = historyStates[historyStates.length - 1]
    return last;
}

function DeleteLastState(state)
{
    if (historyStates.length > 1)
    {
        $.each(historyStates, function (i) {
            if (historyStates[i].url === state.url) {
                historyStates.splice(i, 1);
                return false;
            }
        });
    }
}

function RefreshHistory()
{
    historyStates = [];
}

//function GetHistoryAjaxForm(url, container) {
//    var state = GetLastState();
//    if (state != undefined) {
//        CallBackGet(function (data) {
//            $(container).html(data);
//        }, url, state.data)
//    }
//}
