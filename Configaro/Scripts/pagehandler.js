function wfDefaultClass() {
    this.pageinit = function () {
        $('#btnLogin').on('click', function () {
            var m_Data = "{" +
                "'p_UserName':'" + $('#txtUserName').val() + "'" +
                ",'p_Password':'" + $('#txtPassword').val() + "'" +
            "}";
            $.ajax({
                type: "POST"
                , url: "wfDefault.aspx/Login"
                , async: true
                , data: m_Data
                , contentType: "application/json; charset=utf-8"
                , dataType: "xml"
                , error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert('ajax call failed');
                }
                , success: function (xml) {
                    var LoginStatus = '';
                    if ($(xml).find('IsAuthenticated')[0].textContent == undefined) {
                        LoginStatus = $(xml).find('IsAuthenticated')[0].text;
                    } else {
                        LoginStatus = $(xml).find('IsAuthenticated')[0].textContent;
                    }
                    if (LoginStatus == 'true') {
                        $.mobile.navigate("#wfdefault");
                    } else {
                        alert('Failed');
                    }
                }
            });
            return false;
        });
        $('#btnLogout').on('click', function (evt) {
            var m_Data = "{}";
            $.ajax({
                type: "POST"
            , url: "wfDefault.aspx/Logout"
            , async: true
            , data: m_Data
            , contentType: "application/json; charset=utf-8"
            , dataType: "xml"
            , error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert('ajax call failed');
            }
            , success: function (xml) {
                var LoginStatus = '';
                if ($(xml).find('IsAuthenticated')[0].textContent == undefined) {
                    LoginStatus = $(xml).find('IsAuthenticated')[0].text;
                } else {
                    LoginStatus = $(xml).find('IsAuthenticated')[0].textContent;
                }
                if (LoginStatus == 'false') {
                    window.location = "wfdefault.aspx";
                } else {
                    alert('Failed');
                }
            }
            , error: function (e) {
                //Failed somehow. Lets go to loginpage
                $.mobile.navigate("#wfdefault");
            }
            });
            return false;
        });
    }
}

function wfLoginClass() {
    this.pageinit = function () {
        $('#btnLogin').on('click', function () {
            var m_Data = "{" +
                "'p_UserName':'" + $('#txtUserName').val() + "'" +
                ",'p_Password':'" + $('#txtPassword').val() + "'" +
            "}";
            $.ajax({
                type: "POST"
                , url: "wfLogin.aspx/Login"
                , async: true
                , data: m_Data
                , contentType: "application/json; charset=utf-8"
                , dataType: "xml"
                , error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert('ajax call failed');
                }
                , success: function (xml) {
                    var LoginStatus = '';
                    if ($(xml).find('IsAuthenticated')[0].textContent == undefined) {
                        LoginStatus = $(xml).find('IsAuthenticated')[0].text;
                    } else {
                        LoginStatus = $(xml).find('IsAuthenticated')[0].textContent;
                    }
                    if (LoginStatus == 'true') {
                        window.location = "wfdefault.aspx";
                    } else {
                        alert('Failed');
                    }
                }
            });
            return false;
        });
    }
}

function wfProjectsClass() {

    this.pageinit = function () {

        $('button[command=new]').off('click');
        $('button[command=new]').on('click', function (event) {
            addProject();
            return false;
        });
        getProjects();

    }

    function addProject() {
        executeCommand('add', '-1');
    };

    function getProjects(p_Command) {
        var strData = "{}";
        $.ajax({
            type: "POST"
            , url: "wsProjects.asmx/GetProjects"
            , async: true
            , data: strData
            , contentType: "application/json; charset=utf-8"
            , dataType: "xml"
            , error: function (XMLHttpRequest, textStatus, errorThrown) {
                $('#Projects').html('Error');
            }
            , success: function (xml) {
                if ($(xml).find('Projects')[0].textContent == undefined) {
                    $('#Projects').html($(xml).find('Projects')[0].text).trigger('create');
                } else {
                    $('#Projects').html($(xml).find('Projects')[0].textContent).trigger('create');
                }
                $('a[ProjectId]').on('click', function () {
                    event.preventDefault();
                    localStorage.ProjectId = $(this).attr('ProjectId');
                    $.mobile.navigate("#wfprojectprotocol");
                });
            }
        });

    }

    function executeCommand(p_Command, p_ProjectId) {

        var m_Data = "{"
        m_Data = m_Data + "'p_ProjectID':'" + escape(p_ProjectId) + "'";
        m_Data = m_Data + ",'p_Command':'" + p_Command + "'";
        m_Data = m_Data + "}"

        $.ajax({
            type: "POST"
            , url: "wsProjects.asmx/ExecuteCommand"
            , async: true
            , data: m_Data
            , contentType: "application/json; charset=utf-8"
            , dataType: "json"
            , error: function (XMLHttpRequest, textStatus, errorThrown) {
                $('#Projects').html('Error');
            }
            , success: function (xml) {
                getProjects(p_Command);
            }
        });

    }


}

function wfProjectProtocolClass() {

    var ProjectID = -1;
    var splitter = '•';

    this.pageinit = function () {

        ProjectID = localStorage.ProjectId;

        getProtocol();

        function getProtocol(p_fnCallBack) {

            var m_Data = "{" +
            "'p_ProjectID':'" + escape(ProjectID) + "'" +
            "," + "'p_Edit':'" + false + "'" +
            "}";

            if ($.trim($('#Protocol').html()).length != 0) {
                $('#Protocol input[QuestionID]').off('change');
            }

            $('#Protocol').on('create', function (e) {
                $('#Protocol input[QuestionID]').off('change');
                $('#Protocol input[QuestionID]').on('change', function () {
                    saveAnswer($(this).attr("QuestionID"), $(this).val(), function () {
                        getProtocol();
                    });
                    return false;
                });
            });

            $.ajax({
                type: "POST"
                , url: "wsGrid.asmx/GetProtocol"
                , async: true
                , data: m_Data
                , contentType: "application/json; charset=utf-8"
                , dataType: "xml"
                , error: function (XMLHttpRequest, textStatus, errorThrown) {
                    $('#Protocol').html('Error in wsGrid.asmx/GetProtocol ' + m_Data);
                }
                , success: function (xml) {
                    if ($(xml).find('Protocol')[0].textContent == undefined) {
                        //IE
                        $('#Protocol').empty();
                        $('#Protocol').append($(xml).find('Protocol')[0].text);
                        $('#Protocol').trigger('create');
                        $('#hidPath').val($(xml).find('Path')[0].text);
                    } else {
                        //Chrome
                        $('#Protocol').empty();
                        $('#Protocol').append($(xml).find('Protocol')[0].textContent);
                        $('#Protocol').trigger('create');
                        $('#hidPath').val($(xml).find('Path')[0].textContent);
                    }
                    (p_fnCallBack != undefined) ? p_fnCallBack() : function () { return false; }
                }
            });
        }

        function saveAnswer(p_QuestionID, p_Value, p_fnCallBack) {
            var m_Data = "{"
            m_Data = m_Data + "'p_ProjectID':'" + escape(ProjectID) + "'"; // Forward slash has to be replaced
            m_Data = m_Data + ",'p_QuestionID':'" + p_QuestionID + "'";
            m_Data = m_Data + ",'p_Value':'" + p_Value + "'";
            m_Data = m_Data + "}"

            $.ajax({
                type: "POST"
                , url: "wsGrid.asmx/SaveAnswer"
                , async: true
                , data: m_Data
                , contentType: "application/json; charset=utf-8"
                , dataType: "json"
                , error: function (XMLHttpRequest, textStatus, errorThrown) {
                    $('#Protocol').html('Error');
                }
                , success: function (p_result) {
                    (p_fnCallBack != undefined) ? p_fnCallBack() : function () { return false; }
                }
            });
        }
    }
}

function wfProjectGridClass() {

    var ProjectID = -1;
    var splitter = '•';

    this.pageinit = function () {

        ProjectID = unescape(localStorage.ProjectId);

        $('button[command=delete]').hide();

        $('button[command=delete]').off('click');
        $('button[command=delete]').on('click', function (event) {
            $('button[command=delete]').hide();
            $('.itemselected').each(function () {
                var originatingCell = $(this).parent();
                $(this).remove();
                deactivateDropZones(function () {
                    saveGrid();
                });
            });
            return false;
        });

        getRepresentation(function () {
            getGridEmpty(function () {
                getGrid(function () {
                    removeNotRepresentedBlocksFromGrid();
                    addSelectability();
                    saveGrid();
                });
            });
        })

        function getRepresentation(p_fnCallBack) {

            var m_Data = "{'p_ProjectID':'" + escape(ProjectID) + "'}";
            $.ajax({
                type: "POST"
                , url: "wsGrid.asmx/GetRepresentation"
                , async: true
                , data: m_Data
                , contentType: "application/json; charset=utf-8"
                , dataType: "xml"
                , error: function (XMLHttpRequest, textStatus, errorThrown) {
                    $('#Representation').html('Error in wsGrid.asmx/GetRepresentation ' + m_Data);
                }
                , success: function (xml) {
                    $('#Representation').empty();
                    $(xml).find('Representation').each(function () {
                        //Place it on the page
                        $('#Representation').append($(this).text());
                    });
                    (p_fnCallBack != undefined) ? p_fnCallBack() : function () { return false; }
                }
            });

        }

        function getGridEmpty(p_fnCallBack) {

            var m_Data = "{}";
            $.ajax({
                type: "POST"
                , url: "wsGrid.asmx/getGridEmpty"
                , async: true
                , data: m_Data
                , contentType: "application/json; charset=utf-8"
                , dataType: "xml"
                , error: function (XMLHttpRequest, textStatus, errorThrown) {
                }
                , success: function (xml) {
                    $(xml).find('Grid').each(function () {
                        var html = $(this).text();
                        if (html != '') {
                            $('#GridContainer').empty();
                            $('#GridContainer').append(html).trigger('create'); ;
                        }
                    });
                    (p_fnCallBack != undefined) ? p_fnCallBack() : function () { return false; }
                }
            });
        }

        function getGrid(p_fnCallBack) {

            var m_Data = "{'p_ProjectID':'" + escape(ProjectID) + "'}";
            $.ajax({
                type: "POST"
                , url: "wsGrid.asmx/GetGrid"
                , async: true
                , data: m_Data
                , contentType: "application/json; charset=utf-8"
                , dataType: "xml"
                , error: function (XMLHttpRequest, textStatus, errorThrown) {
                }
                , success: function (xml) {
                    $(xml).find('Grid').each(function () {
                        var html = $(this).text();
                        if (html != '') {
                            $('#GridContainer').empty();
                            $('#GridContainer').append(html).trigger('create');
                        }
                    });
                    (p_fnCallBack != undefined) ? p_fnCallBack() : function () { return false; }
                }
            });
        }

        function removeNotRepresentedBlocksFromGrid() {
            var theIdForTheBlock = '';
            //Loop over all blocks in the grid
            $('img[id*="•"]').each(function () {
                theIdForTheBlock = $(this).attr('id');
                theIdForTheBlock = theIdForTheBlock.substr(0, theIdForTheBlock.indexOf('•'));
                if ($('#' + theIdForTheBlock).length == 0) {
                    //This one should be deleted
                    $(this).remove();
                       } else {
                    //refresh the image for the block representation
                    $(this).attr('src', $('#' + theIdForTheBlock).attr('src'));
                 }
            });
        }

        function saveGrid(p_fnCallBack) {
            var m_Data = "{" +
                "'p_ProjectID':'" + escape(ProjectID) + "'" +
                ",'p_strHtml':'" + $('#GridContainer').html() + "'" +
                 "}";
            $.ajax({
                type: "POST"
                , url: "wsGrid.asmx/SaveGrid"
                , async: true
                , data: m_Data
                , contentType: "application/json; charset=utf-8"
                , dataType: "xml"
                , error: function (XMLHttpRequest, textStatus, errorThrown) {
                }
                , success: function (xml) {
                    (p_fnCallBack != undefined) ? p_fnCallBack() : function () { return false; }
                }
            });

        }

        function SurroundingCells(p_This) {

            var northOfThis = $('<div></div>');
            var westOfThis = $('<div></div>');
            var southOfThis = $('<div></div>');
            var eastOfThis = $('<div></div>');

            var row = 0;
            var col = 0;
            var gridSize = $('#hidGridSize').val();

            row = p_This.attr('ID').split(splitter)[0];
            col = p_This.attr('ID').split(splitter)[1];

            if (row > 0) {
                //Point out the object North of the one being checked
                northOfThis = $('#' + (parseInt(row) - 1) + splitter + col);
            }
            if (col > 0) {
                //Point out the object West of the one being checked
                westOfThis = $('#' + row + splitter + (parseInt(col) - 1));
            }
            if (row < gridSize - 1) {
                //Point out the object South of the one being checked
                southOfThis = $('#' + (parseInt(row) + 1) + splitter + col);
            }
            if (col < gridSize - 1) {
                //Point out the object East of the one being checked
                eastOfThis = $('#' + row + splitter + (parseInt(col) + 1));
            }

            this.NorthOfThis = northOfThis;
            this.WestOfThis = westOfThis;
            this.SouthOfThis = southOfThis;
            this.EastOfThis = eastOfThis;

        }

        function checkSurrounding(p_cell) {

            var surroundingCells = new SurroundingCells(p_cell);

            if (surroundingCells.NorthOfThis.html() != '') {
                checkRules(surroundingCells.NorthOfThis);
            }
            if (surroundingCells.WestOfThis.html() != '') {
                checkRules(surroundingCells.WestOfThis);
            }
            if (surroundingCells.SouthOfThis.html() != '') {
                checkRules(surroundingCells.SouthOfThis);
            }
            if (surroundingCells.EastOfThis.html() != '') {
                checkRules(surroundingCells.EastOfThis);
            }

        }

        function deactivateDropZones(p_fnCallBack) {
            $('.gridcell').each(function () {
                $(this).removeClass('droparea');
                $(this).off('click');
            });
            $('button[command=delete]').hide();

            (p_fnCallBack != undefined) ? p_fnCallBack() : function () { return false; }
        }

        function applyRulesFor(p_cell) {

            var surroundingCells = new SurroundingCells(p_cell);

            var directionArray;
            var itemAllowedByNorth = false;
            var itemAllowedByWest = false;
            var itemAllowedBySouth = false;
            var itemAllowedByEast = false;

            var item = p_cell.children().first();
            //In case the ID contains a '•' we have to extract the first part of the ID
            var itemID = $('.itemselected').attr('id').indexOf(splitter) != -1 ? $('.itemselected').attr('id').split(splitter)[0] : $('.itemselected').attr('id');

            if ((surroundingCells.NorthOfThis.html() == '') && ($(item).attr('north') != '') && ($(item).attr('north') != undefined)) {
                //There is a definition in this direction
                directionArray = $(item).attr('north').split(splitter);
                //Loop thru all allowed objects in this direction
                $.each(directionArray, function (i) {
                    if (directionArray[i] == itemID) { itemAllowedByNorth = true; }
                });
            } else {
                //No definition created for this direction
                itemAllowedByNorth = true;
            }

            if ((surroundingCells.WestOfThis.html() == '') && ($(item).attr('west') != '') && ($(item).attr('west') != undefined)) {
                directionArray = $(item).attr('west').split(splitter);
                //Loop thru all allowed objects in this direction
                $.each(directionArray, function (i) {
                    if (directionArray[i] == itemID) { itemAllowedByWest = true; }
                });
            } else {
                //No definition created for this direction
                itemAllowedByWest = true;
            }

            if ((surroundingCells.SouthOfThis.html() == '') && ($(item).attr('south') != '') && ($(item).attr('south') != undefined)) {
                directionArray = $(item).attr('south').split(splitter)
                //Loop thru all allowed objects in this direction
                $.each(directionArray, function (i) {
                    if (directionArray[i] == itemID) { itemAllowedBySouth = true; }
                });
            } else {
                //No definition created for this direction
                itemAllowedBySouth = true;
            }

            if ((surroundingCells.EastOfThis.html() == '') && ($(item).attr('east') != '') && ($(item).attr('east') != undefined)) {
                directionArray = $(item).attr('east').split(splitter)
                //Loop thru all allowed objects in this direction
                $.each(directionArray, function (i) {
                    if (directionArray[i] == itemID) { itemAllowedByEast = true; }
                });
            } else {
                //No definition created for this direction
                itemAllowedByEast = true;
            }

            if (!itemAllowedByNorth) {
                $(surroundingCells.NorthOfThis).removeClass('droparea');
                $(surroundingCells.NorthOfThis).off('click');
            }
            if (!itemAllowedByEast) {
                $(surroundingCells.EastOfThis).removeClass('droparea');
                $(surroundingCells.EastOfThis).off('click');
            }
            if (!itemAllowedBySouth) {
                $(surroundingCells.SouthOfThis).removeClass('droparea');
                $(surroundingCells.SouthOfThis).off('click');
            }
            if (!itemAllowedByWest) {
                $(surroundingCells.WestOfThis).removeClass('droparea');
                $(surroundingCells.WestOfThis).off('click');
            }
        }

        function limitDropZonesFromRules() {
            $('.gridcell').each(function () {
                if (($(this).html() != '') && !($(this).children().first().hasClass('itemselected'))) {
                    applyRulesFor($(this));
                }
            });
        }

        function addSelectability() {
            $('.item').each(function () {
                $(this).on('click', function () {
                    itemClicked($(this));
                });
            })
        }

        function itemClicked(objThis) {
            if ($(objThis).hasClass('itemselected')) {
                $(objThis).removeClass('itemselected');
            } else {
                $('.itemselected').removeClass('itemselected');
                $(objThis).addClass('itemselected');
            }

            if ($(objThis).hasClass('itemselected')) {
                activateDropZones();
                limitDropZonesFromRules();
            } else {
                deactivateDropZones();
            }

        }

        function activateDropZones() {

            if ($('.itemselected').attr('id').indexOf(splitter) != -1) {
                //ID contains the splitter so we are working on an item inside the grid
                $('button[command=delete]').show();
            }

            $('.gridcell').each(function () {
                $(this).off('click');
                if ($(this).html() != '') {
                    $(this).removeClass('droparea');
                } else {
                    $(this)
                    .addClass('droparea')
                    .on('click', function () {
                        if ($('.itemselected').attr('id').indexOf(splitter) != -1) {
                            //ID contains the splitter so we are working on an item inside the grid
                            //Lets do a move
                            var objToBeAppended = $('.itemselected')
                            $('.itemselected').parent().html('');
                            $(this).append(objToBeAppended)
                            $(objToBeAppended).on('click', function () {
                                itemClicked($(this));
                            });
                        } else {
                            //We are working on an item in the toolbox
                            //So this is a clone and a new creation
                            $(this).append(
                                $('.itemselected')
                                .clone()
                                .attr('id', function () {
                                    return $(this).attr('id') + splitter + $('div[id^="' + $(this).attr('id') + '"]').length;
                                }).on('click', function () {
                                    itemClicked($(this));
                                })
                            );
                        }
                        $('.itemselected').removeClass('itemselected');
                        deactivateDropZones();
                        saveGrid();
                        return false;
                    });
                }
            });
        }
    }
}

function wfProjectOutputClass() {

    var ProjectID = -1;
    var splitter = '•';

    this.pageinit = function () {

        ProjectID = localStorage.ProjectId;
        var GridContainer = '';

        //The button in the header of the page
        $('button[command=save]').off('click');
        $('button[command=save]').on('click', function (event) {
            saveProject();
            return false;
        });

        $('button[command=deleteproject]').off('click');
        $('button[command=deleteproject]').on('click', function () {
            executeCommand('delete');
            ProjectID = -1;
            $.mobile.changePage('#wfprojects', {
                changeHash: false
            });
            return false;
        });

        function executeCommand(p_Command) {

            var m_Data = "{"
            m_Data = m_Data + "'p_ProjectID':'" + escape(ProjectID) + "'";
            m_Data = m_Data + ",'p_Command':'" + p_Command + "'";
            m_Data = m_Data + "}"

            $.ajax({
                type: "POST"
                , url: "wsProjects.asmx/ExecuteCommand"
                , async: false
                , data: m_Data
                , contentType: "application/json; charset=utf-8"
                , dataType: "json"
                , error: function (XMLHttpRequest, textStatus, errorThrown) {
                }
                , success: function (xml) {
                }
            });
        }

        getProject(function () {
            getGrid(function () {
                getContent(function () {
                    getSummary(function () {
                    });
                });
            });
        });

        function getProject(p_fnCallBack) {

            var m_Data = "{"
            m_Data = m_Data + "'p_ProjectID':'" + escape(ProjectID) + "'";
            m_Data = m_Data + "}";
            $.ajax({
                type: "POST"
                , url: "wsProjects.asmx/GetProject"
                , async: true
                , data: m_Data
                , contentType: "application/json; charset=utf-8"
                , dataType: "xml"
                , error: function (XMLHttpRequest, textStatus, errorThrown) {
                    $('#Projects').html('Error wsProjects.asmx/GetProject ' + m_Data);
                }
                , success: function (xml) {
                    $('#txtProjectId').val('');
                    $(xml).find('Object').children().each(function (index, value) {
                        if (index == 0) {
                            $('#txtProjectId').val(unescape($(this).attr('id')));
                        }
                    });
                    (p_fnCallBack != undefined) ? p_fnCallBack() : function () { return false; }
                }
            });
        }

        function saveProject(p_fnCallBack) {

            var m_Data = "{"
            m_Data = m_Data + "'p_ProjectId':'" + escape(ProjectID) + "'";
            m_Data = m_Data + ",'p_ProjectIDNew':'" + escape($('#txtProjectId').val()) + "'";
            m_Data = m_Data + "}"
            $.ajax({
                type: "POST"
                , url: "wsProjects.asmx/SaveProject"
                , async: true
                , data: m_Data
                , contentType: "application/json; charset=utf-8"
                , dataType: "json"
                , error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(XMLHttpRequest.responseText);
                }
                , success: function (p_result) {
                    ProjectID = $('#txtProjectId').val();
                    localStorage.ProjectId = $('#txtProjectId').val();
                    (p_fnCallBack != undefined) ? p_fnCallBack() : function () { return false; }
                }
            });
        }

        function getGrid(p_fnCallBack) {

            var m_Data = "{'p_ProjectID':'" + escape(ProjectID) + "'}";
            $.ajax({
                type: "POST"
                , url: "wsGrid.asmx/GetGrid"
                , async: true
                , data: m_Data
                , contentType: "application/json; charset=utf-8"
                , dataType: "xml"
                , error: function (XMLHttpRequest, textStatus, errorThrown) {
                }
                , success: function (xml) {
                    $(xml).find('Grid').each(function () {
                        var html = $(this).text();
                        if (html != '') {
                            GridContainer = $.parseHTML(html);
                        }
                    });
                    (p_fnCallBack != undefined) ? p_fnCallBack() : function () { return false; }
                }
            });
        }

        function getContent(p_fnCallBack) {

            var m_Data = "{'p_ProjectID':'" + escape(ProjectID) + "'}";
            $.ajax({
                type: "POST"
                , url: "wsGrid.asmx/GetContent"
                , async: true
                , data: m_Data
                , contentType: "application/json; charset=utf-8"
                , dataType: "xml"
                , error: function (XMLHttpRequest, textStatus, errorThrown) {
                    $('#Content').html('Error');
                }
                , success: function (xml) {
                    var m_Content = '';
                    $('#Content').html('');
                    var noOf = 0;
                    $(xml).find('Content').each(function () {
                        //var noOf = $('[id^="' + $(this).attr('Id') + splitter + '"]').length;
                        noOf = $(GridContainer).find('[id^="' + $(this).attr('Id') + splitter + '"]').length;
                        if (noOf > 0) {
                            m_Content += noOf + ' pcs: ' + $(this).text();
                        }
                    });
                    m_Content = unescape(m_Content);
                    $('#Content').html(m_Content).trigger('create');
                    $('#ContentCost').html('');
                    var m_SEK = 0; var m_USD = 0; var m_EUR = 0;

                    $(xml).find('SEK').each(function () {
                        //var noOf = $('[id^="' + $(this).attr('Id') + splitter + '"]').length;
                        noOf = $(GridContainer).find('[id^="' + $(this).attr('Id') + splitter + '"]').length;
                        if (noOf > 0) {
                            m_SEK += noOf * parseFloat($(this).text());
                        }
                    });
                    $(xml).find('USD').each(function () {
                        //var noOf = $('[id^="' + $(this).attr('Id') + splitter + '"]').length;
                        noOf = $(GridContainer).find('[id^="' + $(this).attr('Id') + splitter + '"]').length;
                        if (noOf > 0) {
                            m_USD += noOf * parseFloat($(this).text());
                        }
                    });
                    $(xml).find('EUR').each(function () {
                        //var noOf = $('[id^="' + $(this).attr('Id') + splitter + '"]').length;
                        noOf = $(GridContainer).find('[id^="' + $(this).attr('Id') + splitter + '"]').length;
                        if (noOf > 0) {
                            m_EUR += noOf * parseFloat($(this).text());
                        }
                    });
                    if (m_SEK > 0 || m_USD > 0 || m_EUR > 0) {
                        $('#ContentCost').html(m_SEK + ' Kr, ' + m_USD + ' $, ' + m_EUR + ' €');
                    }

                    (p_fnCallBack != undefined) ? p_fnCallBack() : function () { return false; }

                }
            });
        }

        function getSummary(p_fnCallBack) {

            var m_Data = "{'p_ProjectID':'" + escape(ProjectID) + "'}";
            $.ajax({
                type: "POST"
                , url: "wsGrid.asmx/GetSummary"
                , async: true
                , data: m_Data
                , contentType: "application/json; charset=utf-8"
                , dataType: "xml"
                , error: function (XMLHttpRequest, textStatus, errorThrown) {
                    $('#Summary').html('Error in wsGrid.asmx/GetSummary ' + m_Data);
                }
                , success: function (xml) {
                    var m_Summary = '';
                    $('#Summary').html('');
                    $(xml).find('Summary').each(function () {
                        //var noOf = $('[id^="' + $(this).attr('ID') + splitter + '"]').length;
                        var noOf = $(GridContainer).find('[id^="' + $(this).attr('Id') + splitter + '"]').length;
                        if (noOf > 0) {
                            m_Summary += $(this).text();
                        }
                    });
                    m_Summary = unescape(m_Summary);
                    $('#Summary').html(m_Summary).trigger('create');
                    (p_fnCallBack != undefined) ? p_fnCallBack() : function () { return false; }

                }
            });
        }


    }
}

function wfResourcesClass() {

    this.pageinit = function () {
        getResources();
    }

    function executeCommand(p_Command, p_ResourceID) {

        var m_Data = "{"
        m_Data = m_Data + "'p_ResourceID':'" + escape(p_ResourceID) + "'";
        m_Data = m_Data + ",'p_Command':'" + p_Command + "'";
        m_Data = m_Data + "}"

        $.ajax({
            type: "POST"
            , url: "wsResources.asmx/ExecuteCommand"
            , async: true
            , data: m_Data
            , contentType: "application/json; charset=utf-8"
            , dataType: "json"
            , error: function (XMLHttpRequest, textStatus, errorThrown) {
                $('#Protocol').html('Error');
            }
            , success: function (xml) {
                getResources(p_Command);
            }
        });

    }

    function getResources(p_Command) {
        var strData = "{}";
        $.ajax({
            type: "POST"
            , url: "wsResources.asmx/GetResources"
            , async: true
            , data: strData
            , contentType: "application/json; charset=utf-8"
            , dataType: "xml"
            , error: function (XMLHttpRequest, textStatus, errorThrown) {
                $('#Resources').html('Error');
            }
            , success: function (xml) {
                if ($(xml).find('Resources')[0].textContent == undefined) {
                    $('#Resources').html($(xml).find('Resources')[0].text).trigger('create');
                } else {
                    $('#Resources').html($(xml).find('Resources')[0].textContent).trigger('create');
                }
                $('button[Command="delete"][ResourceId]').on('click', function () {
                    if (confirm('are you sure?') == true) {
                        executeCommand($(this).attr('Command'), $(this).attr('ResourceId'));
                    }
                    return false;
                });

            }
        });
    }

    function saveResource(p_Dialog) {
        __doPostBack('btnUpload', '');
    }

}

function wfBlocksClass() {

    this.pageinit = function () {
        getBlocks();
    }

    function executeCommand(p_Command, p_BlockId) {

        var m_Data = "{"
        m_Data = m_Data + "'p_BlockID':'" + escape(p_BlockId) + "'";
        m_Data = m_Data + ",'p_Command':'" + p_Command + "'";
        m_Data = m_Data + "}"

        $.ajax({
            type: "POST"
        , url: "wsBlocks.asmx/ExecuteCommand"
        , async: true
        , data: m_Data
        , contentType: "application/json; charset=utf-8"
        , dataType: "json"
        , error: function (XMLHttpRequest, textStatus, errorThrown) {
            $('#Protocol').html('Error');
        }
        , success: function (xml) {
            getBlocks(p_Command);
        }
        });

    }

    function getBlocks(p_Command) {
        var strData = "{}";
        $.ajax({
            type: "POST"
            , url: "wsBlocks.asmx/GetBlocks"
            , async: true
            , data: strData
            , contentType: "application/json; charset=utf-8"
            , dataType: "xml"
            , error: function (XMLHttpRequest, textStatus, errorThrown) {
                $('#Blocks').html('Error');
            }
            , success: function (xml) {

                if ($(xml).find('Blocks')[0].textContent == undefined) {
                    $('#Blocks').html($(xml).find('Blocks')[0].text).trigger('create');
                } else {
                    $('#Blocks').html($(xml).find('Blocks')[0].textContent).trigger('create');
                }

                $('a[BlockId]').on('click', function () {
                    event.preventDefault();
                    localStorage.BlockId = $(this).attr('BlockId');
                    $.mobile.navigate("#wfblock");
                });

//                $('button[Command="delete"][BlockId]').on('click', function () {
//                    if (confirm('are you sure?') == true) {
//                        executeCommand($(this).attr('Command'), $(this).attr('BlockId'));
//                    }
//                    return false;
//                });
            }
        });
    }
}

function wfBlockClass() {

    this.pageinit = function () {

        $('button[command=save]').off('click');
        $('button[command=save]').on('click', function (event) {
            saveBlock();
            return false;
        });


        getBlock(localStorage.BlockId);
    }

    function getBlock(p_BlockId) {
        var m_Data = "{"
        m_Data = m_Data + "'p_BlockID':'" + escape(p_BlockId) + "'";
        m_Data = m_Data + "}";
        $.ajax({
            type: "POST"
            , url: "wsBlocks.asmx/GetBlock"
            , async: true
            , data: m_Data
            , contentType: "application/json; charset=utf-8"
            , dataType: "xml"
            , error: function (XMLHttpRequest, textStatus, errorThrown) {
                $('#Blocks').html('Error');
            }
            , success: function (xml) {
                $('#txtBlockId').val('');
                $('#txtExpression').val('');
                $('#txtRepresentation').val('');
                $('#txtContent').val('');
                $('#txtSummary').val('');
                $('#txtBlockId').val($(xml).find('Object').attr('id'))
                $('#txtExpression').val($(xml).find('Object').attr('expression'))
                $('#txtNorth').val($(xml).find('Object').attr('north'))
                $('#txtEast').val($(xml).find('Object').attr('east'))
                $('#txtSouth').val($(xml).find('Object').attr('south'))
                $('#txtWest').val($(xml).find('Object').attr('west'))

                $(xml).find('Representation').children().each(function (index, value) {
                    $(this).removeAttr('id');  //Hidden attribute
                    $(this).removeAttr('toolbox'); //Hidden attribute
                    $(this).removeAttr('class'); //Hidden attribute
                    $('#txtRepresentation').val($('#txtRepresentation').val() + unescape(getXML(value)));
                    $('#txtRepresentation').format({ method: 'xml' });
                });
                $(xml).find('Content').children().each(function (index, value) {
                    $(this).removeAttr('id'); //Hidden attribute
                    $('#txtContent').val($('#txtContent').val() + unescape(getXML(value)));
                    $('#txtContent').format({ method: 'xml' });

                });
                $(xml).find('Summary').children().each(function (index, value) {
                    $(this).removeAttr('id'); //Hidden attribute
                    $('#txtSummary').val($('#txtSummary').val() + unescape(getXML(value)));
                    $('#txtSummary').format({ method: 'xml' });
                });
            }
        });
    }

    function saveBlock() {
        var m_Data = "{"
        m_Data = m_Data + "'p_BlockId':'" + escape($('#txtBlockId').val()) + "'";
        m_Data = m_Data + ",'p_Expression':'" + escape($('#txtExpression').val()) + "'";
        m_Data = m_Data + ",'p_North':'" + escape($('#txtNorth').val()) + "'";
        m_Data = m_Data + ",'p_East':'" + escape($('#txtEast').val()) + "'";
        m_Data = m_Data + ",'p_South':'" + escape($('#txtSouth').val()) + "'";
        m_Data = m_Data + ",'p_West':'" + escape($('#txtWest').val()) + "'";
        m_Data = m_Data + ",'p_Representation':'" + escape($('#txtRepresentation').val()) + "'";
        m_Data = m_Data + ",'p_Content':'" + escape($('#txtContent').val()) + "'";
        m_Data = m_Data + ",'p_Summary':'" + escape($('#txtSummary').val()) + "'";
        m_Data = m_Data + "}"
        $.ajax({
            type: "POST"
            , url: "wsBlocks.asmx/SaveBlock"
            , async: true
            , data: m_Data
            , contentType: "application/json; charset=utf-8"
            , dataType: "json"
            , error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(XMLHttpRequest.responseText);
            }
            , success: function (p_result) {
            }
        });
    }
}

function wfProtocolClass() {

    this.pageinit = function () {
        getProtocol();

        $('button[command=edit]').off('click');
        $('button[command=edit]').on('click', function () {
            if (localStorage.AnswerId == '-1') {
                $.mobile.navigate("#wfquestion");
            } else {
                $.mobile.navigate("#wfanswer");
            }
            return false;
        });
        $('button[command=moveup]').off('click');
        $('button[command=moveup]').on('click', function () {
            executeCommand('moveUp');
            return false;
        });
        $('button[command=movedown]').off('click');
        $('button[command=movedown]').on('click', function () {
            executeCommand('moveDown');
            return false;
        });
        $('button[command=addbefore]').off('click');
        $('button[command=addbefore]').on('click', function () {
            executeCommand('addBefore');
            return false;
        });
        $('button[command=addafter]').off('click');
        $('button[command=addafter]').on('click', function () {
            executeCommand('addAfter');
            return false;
        });
        $('button[command=add]').off('click');
        $('button[command=add]').on('click', function () {
            executeCommand('add');
            return false;
        });
        $('button[command=delete]').off('click');
        $('button[command=delete]').on('click', function () {
            executeCommand('delete');
            return false;
        });

    }

    function getProtocol(p_Command) {

        var m_Data = "{}";
        $.ajax({
            type: "POST"
            , url: "wsProtocol.asmx/GetProtocol"
            , async: true
            , data: m_Data
            , contentType: "application/json; charset=utf-8"
            , dataType: "xml"
            , error: function (XMLHttpRequest, textStatus, errorThrown) {
                $('#Protocol').html('Error');
            }
            , success: function (xml) {
                if ($(xml).find('Protocol')[0].textContent == undefined) {
                    //IE
                    $('#ProtocolEdit').empty();
                    $('#ProtocolEdit').append($(xml).find('Protocol')[0].text);
                    $('#ProtocolEdit').trigger('create');
                    $('#hidPath').val($(xml).find('Path')[0].text);
                } else {
                    //Chrome
                    $('#ProtocolEdit').empty();
                    $('#ProtocolEdit').append($(xml).find('Protocol')[0].textContent);
                    $('#ProtocolEdit').trigger('create');
                    $('#hidPath').val($(xml).find('Path')[0].textContent);
                }
                localStorage.QuestionId = '-1';
                localStorage.AnswerId = '-1';

                $('input[QuestionId][AnswerId]').on('click', function () {
                    localStorage.QuestionId = $(this).attr('QuestionId');
                    localStorage.AnswerId = $(this).attr('AnswerId');
                    var QuestionId = $(this).attr("QuestionId");
                    var AnswerId = $(this).attr("AnswerId");
                    $('input[QuestionId][AnswerId]').each(function () {
                        if (!((QuestionId == $(this).attr("QuestionId")) && (AnswerId == $(this).attr("AnswerId")))) {
                            //Uncheck all other objects on the page
                            $(this).prop('checked', false).checkboxradio("refresh");
                        }
                    });
                });
            }
        });
    }

    function executeCommand(p_Command) {

        var m_Data = "{"
        m_Data = m_Data + "'p_Path':'" + escape($('#hidPath').val()) + "'"; // Forward slash has to be replaced
        m_Data = m_Data + ",'p_QuestionID':'" + localStorage.QuestionId + "'";
        m_Data = m_Data + ",'p_AnswerID':'" + localStorage.AnswerId + "'";
        m_Data = m_Data + ",'p_Command':'" + p_Command + "'";
        m_Data = m_Data + "}"

        $.ajax({
            type: "POST"
            , url: "wsProtocol.asmx/ExecuteCommand"
            , async: false
            , data: m_Data
            , contentType: "application/json; charset=utf-8"
            , dataType: "json"
            , error: function (XMLHttpRequest, textStatus, errorThrown) {
                $('#Protocol').html('Error');
            }
            ,success: function (xml) {
                getProtocol(p_Command);
            }
        });
    }
}

function wfQuestionClass() {

    this.pageinit = function () {
        getQuestion(localStorage.QuestionId);

        $('button[command=save]').off('click');
        $('button[command=save]').on('click', function (event) {
            saveQuestion(localStorage.QuestionId);
            return false;
        });
    }

    function getQuestion(p_QuestionId) {
        var m_Data = "{"
        m_Data = m_Data + "'p_Path':'" + escape($('#hidPath').val()) + "'"; // Forward slash has to be replaced
        m_Data = m_Data + ",'p_QuestionID':'" + p_QuestionId + "'";
        m_Data = m_Data + "}"
        $.ajax({
            type: "POST"
            , url: "wsProtocol.asmx/GetQuestion"
            , async: true
            , data: m_Data
            , contentType: "application/json; charset=utf-8"
            , dataType: "xml"
            , error: function (XMLHttpRequest, textStatus, errorThrown) {
                $('#Protocol').html('Error');
            }
            , success: function (xml) {
                $(xml).find('Question').each(function () {
                    $('#txtQId').val($(this).attr('ID'));
                    $('#lstQQuestionType').val($(this).attr('QuestionType'));
                    $('#txtQSortOrder').val($(this).attr('SortOrder'));
                    $('#txtQLinkedQuestion').val($(this).attr('LinkedQuestion'));
                    $('#txtQProceed_With').val($(this).attr('Proceed_With'));
                    $('#lstQDisplayType').val($(this).attr('DisplayType'));
                    $('#txtQDescription').val($(this).attr('Description'));
                });
            }
        });
    }

    function saveQuestion(p_QuestionId) {
        var m_Data = "{"
        m_Data = m_Data + "'p_Path':'" + escape($('#hidPath').val()) + "'"; // Forward slash has to be replaced
        m_Data = m_Data + ",'p_QuestionID':'" + p_QuestionId + "'";
        m_Data = m_Data + ",'p_QuestionType':'" + $('#lstQQuestionType').val() + "'";
        m_Data = m_Data + ",'p_LinkedQuestion':'" + $('#txtQLinkedQuestion').val() + "'";
        m_Data = m_Data + ",'p_Proceed_With':'" + $('#txtQProceed_With').val() + "'";
        m_Data = m_Data + ",'p_Description':'" + escape($('#txtQDescription').val()) + "'";
        m_Data = m_Data + "}"
        $.ajax({
            type: "POST"
            , url: "wsProtocol.asmx/SaveQuestion"
            , async: true
            , data: m_Data
            , contentType: "application/json; charset=utf-8"
            , dataType: "json"
            , error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(XMLHttpRequest.responseText);
            }
            , success: function (p_result) {
            }
        });
    }


}

function wfAnswerClass() {

    this.pageinit = function () {
        getAnswer(localStorage.QuestionId, localStorage.AnswerId);

        $('button[command=save]').off('click');
        $('button[command=save]').on('click', function (event) {
            saveAnswer(localStorage.QuestionId, localStorage.AnswerId);
            return false;
        });

    }

    function getAnswer(p_QuestionID, p_AnswerID) {
        var m_Data = "{"
        m_Data = m_Data + "'p_Path':'" + escape($('#hidPath').val()) + "'"; // Forward slash has to be replaced
        m_Data = m_Data + ",'p_QuestionID':'" + p_QuestionID + "'";
        m_Data = m_Data + ",'p_AnswerID':'" + p_AnswerID + "'";
        m_Data = m_Data + "}"
        $.ajax({
            type: "POST"
            , url: "wsProtocol.asmx/GetAnswer"
            , async: true
            , data: m_Data
            , contentType: "application/json; charset=utf-8"
            , dataType: "xml"
            , error: function (XMLHttpRequest, textStatus, errorThrown) {
                $('#Protocol').html('Error');
            }
            , success: function (xml) {
                $(xml).find('Answer').each(function () {
                    $('#txtAId').val($(this).attr('ID'));
                    if ($(this).attr('Selected') == 'True') { $('#chkASelected').attr('checked', true); } else { $('#chkASelected').attr('checked', false); }
                    $('#txtAProceed_With').val($(this).attr('Proceed_With'));
                    $('#txtADescription').val($(this).attr('Description'));
                });
            }
        });
    }

    function saveAnswer(p_QuestionID, p_AnswerID) {
        var m_Data = "{"
        m_Data = m_Data + "'p_Path':'" + escape($('#hidPath').val()) + "'"; // Forward slash has to be replaced
        m_Data = m_Data + ",'p_QuestionID':'" + p_QuestionID + "'";
        m_Data = m_Data + ",'p_AnswerID':'" + p_AnswerID + "'";
        m_Data = m_Data + ",'p_Proceed_With':'" + $('#txtAProceed_With').val() + "'";
        m_Data = m_Data + ",'p_Selected':'" + (($('#chkASelected').is(':checked')) ? 'True' : 'False') + "'";
        m_Data = m_Data + ",'p_Description':'" + escape($('#txtADescription').val()) + "'";
        m_Data = m_Data + "}"

        $.ajax({
            type: "POST"
            , url: "wsProtocol.asmx/SaveAnswer"
            , async: true
            , data: m_Data
            , contentType: "application/json; charset=utf-8"
            , dataType: "json"
            , error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(XMLHttpRequest.responseText);
            }
            , success: function (p_result) {
            }
        });
    }

}

