
let _ws = null;
let form = null;
let settings_json = null;

function onLoad(){
    resizeTo(600, 600);
    form = document.forms.main;
    _ws = new WebSocket(getWSAdress());
    _ws.onopen = function() {
        _ws.send('5getSettingsJson');
    };
    _ws.onmessage = function(event){
        let code = parseInt((event.data).substr(0, 1));
        let msg = (event.data).slice(1);
        if (code == 5 && msg == 'close'){
            window.close();
        }else if (code == 6){
            let json = JSON.parse(msg);
            if (json.head == "settings_json"){
                settings_json = json.body;
                form.lang.value = settings_json.Language;
                form.modkey.value = settings_json.HotkeyMODKey;
                Object.keys(settings_json.ReplaceTable).forEach(function(key){
                    let option = document.createElement("option");
                    option.value = key+'::'+settings_json.ReplaceTable[key];
                    option.text = '"'+escape(key)+'" => "'+escape(settings_json.ReplaceTable[key])+'"';
                    form.replace_sel.appendChild(option);
                });
            }
        }
    };
}

function onUnload(){
    _ws.close();
}

function sendData(){
    settings_json.Language = form.lang.value;
    settings_json.HotkeyMODKey = parseInt(form.modkey.value);
    let temp_table = {};
    let options = form.replace_sel.options;
    for(var i = 0; i < options.length; i++){
        let splitted = options[i].value.split('::');
        temp_table[splitted[0]] = splitted[1];
    }
    settings_json.ReplaceTable = temp_table;

    let json_text = JSON.stringify({head: 'settings_json', body: settings_json});
    _ws.send('6'+json_text);
}

function addReplaceOption(){
    let option = document.createElement("option");
    option.value = unEscape(form.replace_before.value)+'::'+unEscape(form.replace_after.value);
    option.text = '"'+form.replace_before.value+'" => "'+form.replace_after.value+'"';
    form.replace_sel.appendChild(option);
    form.replace_before.value = '';
    form.replace_after.value = '';
}

function delSelectedOption(){
    let options = form.replace_sel.options;
    for(var i = 0; i < options.length; i++){
        if (options[i].selected){
            form.replace_sel.removeChild(options[i]);
        }
    }
}

function getParam(name, url) {
    if (!url) url = window.location.href;
    name = name.replace(/[\[\]]/g, "\\$&");
    var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
        results = regex.exec(url);
    if (!results) return null;
    if (!results[2]) return '';
    return decodeURIComponent(results[2].replace(/\+/g, " "));
}

function getWSAdress(){
    let host = location.host;
    return "ws://"+host;
}

function escape(str){
    return str
            .replace(/\\/g, '\\\\')
            .replace(/\n/g, '\\n')
            .replace(/\r/g, '\\r')
            .replace(/\t/g, '\\t')
            .replace(/\f/g, '\\f')
            .replace(/\'/g, "\\'")
            .replace(/\"/g, '\\"');
}

function unEscape(str){
    return str
            .replace(/\\n/g, '\n')
            .replace(/\\r/g, '\r')
            .replace(/\\t/g, '\t')
            .replace(/\\f/g, '\f')
            .replace(/\\'/g, "\'")
            .replace(/\\"/g, '\"')
            .replace(/\\\\/g, '\\');
}