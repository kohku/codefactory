try { 
 document.getElementById("FileUploadDetailsView_TheFileUploader").SetReturnValue(__flash__toXML(FileUploadDetailsView_TheFileUploader_SetChangedFileStates([{ id: 1, state: 5, httpStatus: 200, isLast: true}])())); } catch (e) { document.getElementById("FileUploadDetailsView_TheFileUploader").SetReturnValue("<undefined/>"); }



/*****/

if (typeof (Flajaxian) == "undefined") Flajaxian = {};

Flajaxian.File_Selected = 1;
Flajaxian.File_Uploading = 2;
Flajaxian.File_Uploaded = 3;
Flajaxian.File_Cancelled = 4;
Flajaxian.File_Error = 5;


Flajaxian.bind = function(method, object) {
    return function() { return method.apply(object, arguments); }
}
Flajaxian.$ = function(id) {
    return document.getElementById(id);
}
Flajaxian.isIE = function(id) {
    return (navigator.appName.indexOf("Microsoft") != -1);
}
Flajaxian.addHandler = function(element, eventName, handler) {
    if (element.addEventListener) {
        element.addEventListener(eventName, handler, false);
    } else if (element.attachEvent) {
        element.attachEvent('on' + eventName, handler);
    }
}
Flajaxian.removeHandler = function(element, eventName, handler) {
    if (element.removeEventListener) {
        element.removeEventListener(eventName, handler, false);
    } else if (element.detachEvent) {
        element.detachEvent('on' + eventName, handler);
    }
}
Flajaxian.removeChildren = function(elem) {
    if (!elem) { return false; }
    if (!elem.hasChildNodes()) { return true; }
    while (elem.hasChildNodes()) {
        elem.removeChild(elem.lastChild);
    }
    return true;
}
Flajaxian.setElementText = function(element, text) {
    Flajaxian.removeChildren(element);
    element.appendChild(document.createTextNode((text === null) ? "" : text));
}
Flajaxian.ensureWordLength = function(text, length) {
    var arr = text.split(' ');
    for (var i = 0; i < arr.length; i++) if (arr[i].length > length) arr[i] = Flajaxian.splitByLength(arr[i], length).join(' ');
    return arr.join(' ');
}
Flajaxian.splitByLength = function(text, length) {
    if (text.length < length || length < 0) return [text];
    var arr = [];
    var start = 0;
    var end = length;
    while (true) {
        arr.push(text.substring(start, end));
        start = end;
        end = ((end + length) >= text.length) ? text.length : end + length;
        if (start >= end) break;
    }
    return arr;
}
Flajaxian.createElement = function(tag, id, cssClass) {
    var element = document.createElement(tag);
    if (!!id) element.id = id;
    if (!!cssClass) element.className = cssClass;
    return element;
}
Flajaxian.getPosition = function(element) {
    var currX = currY = 0;
    if (element.offsetParent) {
        currX = element.offsetLeft;
        currY = element.offsetTop;
        while (element = element.offsetParent) {
            currX += element.offsetLeft;
            currY += element.offsetTop;
        }
    }
    return { x: currX, y: currY };
}
Flajaxian.round = function(num, digits) {
    var power = Math.pow(10, digits);
    return Math.round(power * num) / power;
}
Flajaxian.verifyFunc = function(func) {
    if (typeof (func) != "function") {
        alert("Error:'" + func + "' is not function");
        return function() { };
    }
    return func;
}
Flajaxian.findFirstPostForm = function() {
    for (var i = 0; i < document.forms.length; i++) {
        var curr = document.forms[i];
        if (curr.method.toLowerCase() == "post") { return curr; }
    }
    return null;
}
Flajaxian.rowClassByState = function(state) {
    var main = "Flajaxian_FileBoxFileListRow";
    var f = Flajaxian;
    return state == f.File_Selected ? main :
            state == f.File_Uploading ? main + "Uploading" :
            state == f.File_Uploaded ? main + "Uploaded" :
            state == f.File_Cancelled ? main + "Cancelled" : main + "Error";
}
Flajaxian.generateBaseHtml = function(uploader) {
    var box = Flajaxian.createElement("div", uploader.get_id() + "_FileBox", "Flajaxian_FileBox");
    uploader.set_fileBoxUI(box); // setting File Box

    var header = Flajaxian.createElement("div", uploader.get_id() + "_FileBoxHeader", "Flajaxian_FileBoxHeader");
    box.appendChild(header);

    var headerTxt = Flajaxian.createElement("div", uploader.get_id() + "_FileBoxHeaderText", "Flajaxian_FileBoxHeaderText");
    header.appendChild(headerTxt);

    var percTxt = Flajaxian.createElement("span", uploader.get_id() + "_FileBoxHeaderPercentage", "Flajaxian_FileBoxHeaderPercentage");
    headerTxt.appendChild(percTxt);
    uploader.set_percentageUI(percTxt); // setting percentage holder

    headerTxt.appendChild(document.createTextNode("   "));

    var headerArrow = Flajaxian.createElement("div", uploader.get_id() + "_FileBoxHeaderArrowHolder", "Flajaxian_FileBoxHeaderArrowHolder");
    header.appendChild(headerArrow);

    var arrow = Flajaxian.createElement("img", uploader.get_id() + "_FileBoxHeaderArrowImg", "Flajaxian_FileBoxHeaderArrowImg");
    arrow.src = uploader.get_openedArrowUrl();
    arrow.border = "0";
    headerArrow.appendChild(arrow);
    uploader.set_arrowUI(arrow); // setting arrow image

    var fileList = Flajaxian.createElement("div", uploader.get_id() + "_FileBoxFileList", "Flajaxian_FileBoxFileList");
    box.appendChild(fileList);
    uploader.set_fileListUI(fileList); // setting file list
}
Flajaxian.disposeFileRow = function(uploader, row) { }
Flajaxian.generateFileRow = function(uploader, row, container) {
    if (!container) { return; }
    var div = Flajaxian.createElement("div", uploader.get_id() + "_FileBoxFileListRow_" + row.id, Flajaxian.rowClassByState(row.state));
    row.ui = div;

    var closeImg = Flajaxian.createElement("img", uploader.get_id() + "_FileBoxFileListRowCloseBtn_" + row.id, "Flajaxian_FileBoxFileListRowCloseBtn");
    row.btnUI = closeImg;
    closeImg.src = uploader.get_closeBtnUrl();
    div.appendChild(closeImg);

    var span = Flajaxian.createElement("span", uploader.get_id() + "_FileBoxFileListRowText_" + row.id, "Flajaxian_FileBoxFileListRowText");
    row.textUI = span;
    Flajaxian.setElementText(span, Flajaxian.ensureWordLength(row.name, 22));
    div.appendChild(span);

    container.appendChild(div);
}
Flajaxian.percentageChangedFunc = function(uploader, percUI, evt) {
    if (!percUI) { return; }
    var num = Flajaxian.round(((evt.loaded / evt.total) * 100) * 0.99, 1).toString();
    if (num.indexOf(".") < 0) num += ".0";
    Flajaxian.setElementText(percUI, num + "%\u00A0\u00A0\u00A0\u00A0");
}
Flajaxian.positionFilesList = function(uploader) {
    var pos = Flajaxian.getPosition(uploader.get_flash());
    pos.y += parseInt(uploader.get_flashHeight()) + 1;
    return pos;
}
Flajaxian.renderFilesList = function(uploader) {
    uploader.get_flash().parentNode.insertBefore(uploader.get_fileBoxUI(), uploader.get_flash());
}
Flajaxian.fileStateChanged = function(uploader, file, httpStatus, isLast) {
    file.btnUI.style.visibility = (file.state > Flajaxian.File_Uploading) ? "hidden" : "visible";
    file.ui.className = Flajaxian.rowClassByState(file.state);
}
Flajaxian.maxFileNumberReached = function(uploader) {
    alert(uploader.get_strings().maxFileNumberReached);
}
Flajaxian.maxFileSizeReached = function(uploader) {
    alert(uploader.get_strings().maxFileSizeReached);
}
Flajaxian.maxQueueSizeReached = function(uploader) {
    alert(uploader.get_strings().maxQueueSizeReached);
}
Flajaxian.requestAsPostBack = function(uploader, array) {
    var form = Flajaxian.findFirstPostForm();
    if (!form) { return; }
    for (var i = 0; i < form.elements.length; i++) {
        var element = form.elements[i];
        var tag = element.tagName.toLowerCase();
        var val = null;
        if (tag == "input" && (element.type.toLowerCase() == "checkbox" || element.type.toLowerCase() == "radio")) {
            if (element.checked) val = element.value;
            else continue;
        } else if (tag == "select" && element.selectedIndex >= 0) {
            val = element.options[element.selectedIndex].value;
        } else val = element.value;
        if (typeof val == "undefined") val = null;
        array.push({ key: (element.name || element.id), value: val, fileID: 0 });
    }
}
Flajaxian.FileUploader2 = function(id) {
    this._id = id;
    this.reset();
}
Flajaxian.FileUploader2.prototype = {
    reset: function() {
        this._state = {};
        this._arrowIsOpen = true;
        this._filesList = [];
        this._queuedActions = [];
        this._uploadPreProcessors = [];
        this._stateChangeProcessors = [];
        this._queuedActionAttempts = 0;
        this._flash = null;
        this._initialized = false;
    },
    addUploadPreProcessor: function(proc) {
        this._uploadPreProcessors.push(proc);
    },
    addStateChangeProcessors: function(proc) {
        this._stateChangeProcessors.push(proc);
    },
    setFileList: function(list) {
        this.ensureFlashRef();
        var percUI = this.get_percentageUI();
        if (!!percUI) { Flajaxian.setElementText(percUI, ""); }
        var box = this.get_fileBoxUI();
        var pos = this.get_positionFilesListFunc()(this);

        box.style.display = "block";
        box.style.left = pos.x + "px";
        box.style.top = pos.y + "px";

        for (var i = 0; i < this._filesList.length; i++) {
            var f = this._filesList[i];
            if (!!f.btnUI) {
                Flajaxian.removeHandler(f.btnUI, 'click', Flajaxian.bind(this._cancelFileClicked, this));
            }
            this.get_disposeFileRowFunc()(this, f);
        }
        this._filesHashtable = {};
        this._filesList = [];
        var listUI = this.get_fileListUI();
        if (!!listUI) { Flajaxian.removeChildren(listUI); }
        if (list.length === 0) {
            box.style.display = "none";
            return;
        }
        for (var i = 0; i < list.length; i++) {
            var curr = list[i];
            this._filesHashtable["F" + curr.id] = curr;
            this._filesList.push(curr);
            this.get_generateFileRowFunc()(this, curr, this.get_fileListUI());
            if (!!curr.btnUI) {
                Flajaxian.addHandler(curr.btnUI, 'click', Flajaxian.bind(this._cancelFileClicked, this));
            }
        }
    },
    setUploadProgress: function(evt) {
        this.get_percentageChangedFunc()(this, this.get_percentageUI(), evt);
    },
    setChangedFileStates: function(list) {
        for (var i = 0; i < list.length; i++) {
            var curr = list[i];
            var file = this.getFileInfoByID(curr.id);
            if (!!file) {
                file.state = curr.state;
                this.get_fileStateChangedFunc()(this, file, curr.httpStatus, curr.isLast);
                for (var j = 0; j < this._stateChangeProcessors.length; j++) {
                    this._stateChangeProcessors[j](this, file, curr.httpStatus, curr.isLast);
                }
                if (curr.isLast && this.get_clearListAtEnd()) {
                    var percUI = this.get_percentageUI();
                    if (percUI) { Flajaxian.setElementText(percUI, ""); }
                    this.get_fileBoxUI().style.display = "none";
                }
            }
        }
    },
    limitReached: function(type) {
        if (type == 'fileNumber') this.get_maxFileNumberReachedFunc()(this);
        else if (type == 'fileSize') this.get_maxFileSizeReachedFunc()(this);
        else if (type == 'queueSize') this.get_maxQueueSizeReachedFunc()(this);
    },
    ensureFlashRef: function() {
        this._flash = Flajaxian.$(this._id);
    },
    enable: function() {
        if (!this.get_initialized()) { return; }
        this.ensureFlashRef();
        this._flash.CallFunction("<invoke name=\"enable\" returntype=\"javascript\"></invoke>");
    },
    disable: function() {
        if (!this.get_initialized()) { return; }
        this.ensureFlashRef();
        this._flash.CallFunction("<invoke name=\"disable\" returntype=\"javascript\"></invoke>");
    },
    cancel: function() {
        if (!this.get_initialized()) { return; }
        this.ensureFlashRef();
        this._flash.CallFunction("<invoke name=\"cancelAll\" returntype=\"javascript\"></invoke>");
    },
    setStateVariable: function(varKey, varValue, varFileID) {
        if (!varKey) { return; }
        if (!varFileID) { varFileID = 0; }
        this.setStateVariablesList([{ key: varKey, value: varValue, fileID: varFileID}], false);
    },
    setStateVariablesList: function(list, initiateUpload) {
        this._initiateQueuedFlashAction("<invoke name=\"setStateVariables\" returntype=\"javascript\">" + this._flash_argumentsToXML([list, initiateUpload], 0) + "</invoke>");
    },
    setVisibility: function(bln) {
        this._initiateQueuedFlashAction("<invoke name=\"setVisibility\" returntype=\"javascript\">" + this._flash_argumentsToXML([bln], 0) + "</invoke>");
    },
    deleteStateVariables: function() {
        this._initiateQueuedFlashAction("<invoke name=\"deleteStateVariables\" returntype=\"javascript\"></invoke>");
    },
    startUpload: function(bln) {
        if (!this.get_initialized() || this.get_isUploading()) { return; }
        this._flash = Flajaxian.$(this._id);
        this._flash.CallFunction("<invoke name=\"startUpload\" returntype=\"javascript\"></invoke>");
    },
    confirmUpload: function() {
        var arr = [];
        var e = { cancel: false };
        this.deleteStateVariables();
        for (var i = 0; i < this._uploadPreProcessors.length; i++) {
            this._uploadPreProcessors[i](this, arr, e);
        }
        this.setStateVariablesList(arr, !e.cancel);
    },
    getFileInfoByID: function(id) {
        return this._filesHashtable["F" + id];
    },
    fileWithUploadingStatusExists: function() {
        var l = this._filesList;
        for (var i = 0; i < l.length; i++) {
            if (l[i].state == Flajaxian.File_Uploading) { return true; }
        }
        return false;
    },
    markInitialized: function() { this._initialized = true; },
    dispose: function() {
        if (!!this._flash && this._flash.tagName != "div") this.cancel();
        this.reset();
        var box = this.get_fileBoxUI();
        var arrow = this.get_arrowUI();
        if (!!arrow) { Flajaxian.removeHandler(arrow, 'click', Flajaxian.bind(this._arrowClicked, this)); }
        if (!!box && box.parentNode) { box.parentNode.removeChild(box); }
    },
    initialize: function() {
        this._flash = Flajaxian.$(this._id);
        this.get_generateBaseHtmlFunc()(this);
        this.get_renderFilesListFunc()(this)
        var arrow = this.get_arrowUI();
        if (!!arrow) { Flajaxian.addHandler(arrow, 'click', Flajaxian.bind(this._arrowClicked, this)); }
    },
    get_id: function() { return this._id; },
    get_flash: function() { return this._flash; },
    get_filesList: function() { return this._filesList; },
    get_flashWidth: function() { return this._flashWidth; },
    set_flashWidth: function(value) { this._flashWidth = value; },
    get_initialized: function() { return this._initialized; },
    get_flashHeight: function() { return this._flashHeight; },
    set_flashHeight: function(value) { this._flashHeight = value; },
    get_isUploading: function(value) { return this._filesList.length > 0 && this.fileWithUploadingStatusExists(); },
    set_generateBaseHtmlFunc: function(func) { this._generateBaseHtmlFunc = Flajaxian.verifyFunc(func); },
    get_generateBaseHtmlFunc: function() { return this._generateBaseHtmlFunc; },
    set_generateFileRowFunc: function(func) { this._generateFileRowFunc = Flajaxian.verifyFunc(func); },
    get_generateFileRowFunc: function() { return this._generateFileRowFunc; },
    set_disposeFileRowFunc: function(func) { this._disposeFileRowFunc = Flajaxian.verifyFunc(func); },
    get_disposeFileRowFunc: function() { return this._disposeFileRowFunc; },
    set_fileStateChangedFunc: function(func) { this._fileStateChangedFunc = Flajaxian.verifyFunc(func); },
    get_fileStateChangedFunc: function() { return this._fileStateChangedFunc; },
    set_percentageChangedFunc: function(func) { this._percentageChangedFunc = Flajaxian.verifyFunc(func); },
    get_percentageChangedFunc: function() { return this._percentageChangedFunc; },
    set_maxFileNumberReachedFunc: function(func) { this._maxFileNumberReachedFunc = Flajaxian.verifyFunc(func); },
    get_maxFileNumberReachedFunc: function() { return this._maxFileNumberReachedFunc; },
    set_maxFileSizeReachedFunc: function(func) { this._maxFileSizeReachedFunc = Flajaxian.verifyFunc(func); },
    get_maxFileSizeReachedFunc: function() { return this._maxFileSizeReachedFunc; },
    set_maxQueueSizeReachedFunc: function(func) { this._maxQueueSizeReachedFunc = Flajaxian.verifyFunc(func); },
    get_maxQueueSizeReachedFunc: function() { return this._maxQueueSizeReachedFunc; },
    set_positionFilesListFunc: function(func) { this._positionFilesListFunc = Flajaxian.verifyFunc(func); },
    get_positionFilesListFunc: function() { return this._positionFilesListFunc; },
    set_renderFilesListFunc: function(func) { this._renderFilesListFunc = Flajaxian.verifyFunc(func); },
    get_renderFilesListFunc: function() { return this._renderFilesListFunc; },
    set_strings: function(value) { this._strings = value; },
    get_strings: function() { return this._strings; },
    set_openedArrowUrl: function(value) { this._openedArrowUrl = value; },
    get_openedArrowUrl: function() { return this._openedArrowUrl; },
    set_closedArrowUrl: function(value) { this._closedArrowUrl = value; },
    get_closedArrowUrl: function() { return this._closedArrowUrl; },
    set_closeBtnUrl: function(value) { this._closeBtnUrl = value; },
    get_closeBtnUrl: function() { return this._closeBtnUrl; },
    set_fileBoxUI: function(value) { this._fileBoxUI = value; },
    get_fileBoxUI: function() { return this._fileBoxUI; },
    set_arrowUI: function(value) { this._arrowUI = value; },
    get_arrowUI: function() { return this._arrowUI; },
    set_percentageUI: function(value) { this._percentageUI = value; },
    get_percentageUI: function() { return this._percentageUI; },
    set_fileListUI: function(value) { this._fileListUI = value; },
    get_fileListUI: function() { return this._fileListUI; },
    set_clearListAtEnd: function(value) { this._clearListAtEnd = value; },
    get_clearListAtEnd: function() { return this._clearListAtEnd; },
    set_uploadRequiresJsConfirmation: function(value) { this._uploadRequiresJsConfirmation = value; },
    get_uploadRequiresJsConfirmation: function() { return this._uploadRequiresJsConfirmation; },
    _initiateQueuedFlashAction: function(action) {
        if (this.get_initialized()) {
            this._flash = Flajaxian.$(this._id);
            this._flash.CallFunction(action);
        } else {
            this._queuedActions.push(action);
            window.setTimeout(Flajaxian.bind(this._initiateQueuedFlashActionAfterWait, this), 400);
        }
    },
    _initiateQueuedFlashActionAfterWait: function() {
        this._queuedActionAttempts++;
        if (this._queuedActionAttempts > 50) {// give up
            this._queuedActionAttempts = 0;
            this._queuedActions = [];
            return;
        }
        if (this.get_initialized()) {
            if (this._queuedActions.length > 0) {
                var action = this._queuedActions.shift();
                this._flash.CallFunction(action);
                if (this._queuedActions.length == 0) { this._queuedActionAttempts = 0; }
            }
        } else {
            window.setTimeout(Flajaxian.bind(this._initiateQueuedFlashActionAfterWait, this), 400);
        }
    },
    _arrowClicked: function(e) {
        var list = this.get_fileListUI();
        var arrow = this.get_arrowUI();
        if (!!list && !!arrow) {
            this._arrowIsOpen = !this._arrowIsOpen;
            list.style.display = this._arrowIsOpen ? "block" : "none";
            arrow.src = this._arrowIsOpen ? this.get_openedArrowUrl() : this.get_closedArrowUrl();
        }
    },
    _cancelFileClicked: function(e) {
        if (!this.get_initialized()) { return; }
        var arr = (e.srcElement || e.target).id.split("_");
        var id = arr[arr.length - 1];
        var file = this.getFileInfoByID(id);
        if (!!file) {
            Flajaxian.removeHandler(file.btnUI, 'click', Flajaxian.bind(this._cancelFileClicked, this));
            file.ui.style.display = "none";
        }
        this._flash.CallFunction("<invoke name=\"fileRemoved\" returntype=\"javascript\">" + this._flash_argumentsToXML([id], 0) + "</invoke>");
    },
    _flash_argumentsToXML: function(obj, index) {
        var sb = [];
        sb.push("<arguments>");
        for (var i = index; i < obj.length; i++) { sb.push(this._flash_toXML(obj[i])); }
        sb.push("</arguments>");
        return sb.join("");
    },
    _flash_toXML: function(value) {
        var type = typeof (value);
        return (type == "string") ? "<string>" + this._flash_escapeXML(value) + "</string>" :
        (type == "undefined") ? "<undefined/>" :
        (type == "number") ? "<number>" + value + "</number>" :
        (value == null) ? "<null/>" :
        (type == "boolean") ? (value ? "<true/>" : "<false/>") :
        (value instanceof Date) ? "<date>" + value.getTime() + "</date>" :
        (value instanceof Array) ? this._flash_arrayToXML(value) :
        (type == "object") ? this._flash_objectToXML(value) : "<null/>";
    },
    _flash_escapeXML: function(s) {
        return s.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;").replace(/"/g, "&quot;").replace(/'/g, "&apos;");
    },
    _flash_arrayToXML: function(obj) {
        var sb = [];
        sb.push("<array>");
        for (var i = 0; i < obj.length; i++) { sb.push("<property id=\"" + i + "\">" + this._flash_toXML(obj[i]) + "</property>"); }
        sb.push("</array>");
        return sb.join('');
    },
    _flash_objectToXML: function(obj) {
        var sb = [];
        sb.push("<object>");
        for (var prop in obj) { sb.push("<property id=\"" + prop + "\">" + this._flash_toXML(obj[prop]) + "</property>"); }
        sb.push("</object>");
        return sb.join('');
    }
}

/*****/

