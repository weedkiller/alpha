function GetCriticalName(levelCode) {
    var cname;
    switch (levelCode) {
        case 1:
            cname = "Baja";
            break;
        case 2:
            cname = "Media";
            break;
        case 3:
            cname = "Alta";
            break;
        default:
            "Error";
    }
    return cname;
}

function GetCriticalColor(levelCode) {
    var ccolor;
    switch (levelCode) {
        case 1:
            ccolor = '#449539';
            break;
        case 2:
            ccolor = "#f1e63a";
            break;
        case 3:
            ccolor = '#e32037';
            break;
        default:
            '#333333';
    }
    return ccolor;
}

function formattedDate(date) {
    var d = new Date(date || Date.now()),
        month = '' + (d.getMonth() + 1),
        day = '' + d.getDate(),
        year = d.getFullYear();

    if (month.length < 2) month = '0' + month;
    if (day.length < 2) day = '0' + day;

    return [day, month, year].join('/');
}