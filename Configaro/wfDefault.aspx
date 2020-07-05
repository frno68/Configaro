<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfDefault.aspx.vb" Inherits="Configaro.wfDefault" %>
<!DOCTYPE html>
<html>
<head runat="server">
    <title>Page Title</title>
    <script language="javascript" type="text/javascript">
        $(document).ready(function window_onload() {
            $('a').on('click', function (event, data) {
                if ((event.target.hash == '#wfprojectprotocol')
                    || (event.target.hash == '#wfprojectgrid')
                    || (event.target.hash == '#wfprojectoutput')) {
                    //These pages are to be excluded from navigation history
                    $.mobile.changePage($(this).attr('href'), {
                        changeHash: false
                    });
                    return false;
                }
            });

            var wfDefault = new wfDefaultClass();
            wfDefault.pageinit();

            $(document).on("pagebeforeshow", "#wfprojects", function (event) {
                var wfProjects = new wfProjectsClass();
                wfProjects.pageinit();
            });

            $(document).on("pagebeforeshow", "#wfprojectprotocol", function (event) {
                var wfProjectProtocol = new wfProjectProtocolClass();
                wfProjectProtocol.pageinit();
            });

            $(document).on("pagebeforeshow", "#wfprojectgrid", function (event) {
                var wfProjectGrid = new wfProjectGridClass();
                wfProjectGrid.pageinit();
            });

            $(document).on("pagebeforeshow", "#wfprojectoutput", function (event) {
                var wfProjectOutput = new wfProjectOutputClass();
                wfProjectOutput.pageinit();
            });

            $(document).on("pagebeforeshow", "#wfprotocol", function (event) {
                var wfProtocol = new wfProtocolClass();
                wfProtocol.pageinit();
            });

            $(document).on("pagebeforeshow", "#wfquestion", function (event) {
                var wfQuestion = new wfQuestionClass();
                wfQuestion.pageinit();
            });

            $(document).on("pagebeforeshow", "#wfanswer", function (event) {
                var wfAnswer = new wfAnswerClass();
                wfAnswer.pageinit();
            });

            $(document).on("pagebeforeshow", "#wfblocks", function (event) {
                var wfBlocks = new wfBlocksClass();
                wfBlocks.pageinit();
            });

            $(document).on("pagebeforeshow", "#wfblock", function (event) {
                var wfBlock = new wfBlockClass();
                wfBlock.pageinit();
            });

            $(document).on("pagebeforeshow", "#wfresources", function (event) {
                var wfResources = new wfResourcesClass();
                wfResources.pageinit();
            });

            $(document).on("pagebeforeshow", "#wfabout", function (event) {
            });

        });
    </script>
</head>
<body>
    <form id="wfDefault" runat="server">
    <asp:HiddenField runat="server" ID="hidGridSize" />
    <asp:HiddenField runat="server" ID="hidSplitter" />
    <asp:HiddenField runat="server" ID="hidPath" />
    <asp:HiddenField runat="server" ID="hidProjectID" />
    <div data-role="page" id="wflogin">
        <div data-role="header" data-position="fixed">
            <h1>
                Welcome to Configaro-Web</h1>
            <button class="ui-btn-right" data-icon="check" id="btnLogin">
                Login</button>
        </div>
        <div role="main" class="ui-content">
            <p>
                <fieldset>
                    <input runat="server" id="txtUserName" placeholder="Username" type="text" value="frno" />
                    <input runat="server" id="txtPassword" placeholder="Password" type="text" value="frno" />
                </fieldset>
            </p>
        </div>
        <div data-role="footer" data-position="fixed">
            <h4>
                Page Footer</h4>
        </div>
    </div>
    <!--/////////////////////////////////////////////////////////////-->
    <div data-role="page" id="wfdefault">
        <div data-role="header" data-position="fixed">
            <h1>
                Start page</h1>
            <button class="ui-btn-right" data-icon="check" id="btnLogout">
                Logout</button>
        </div>
        <div role="main" class="ui-content">
            <p>
                <ul data-role="listview" data-inset="true">
                    <li><a href="#wfprojects">Projects</a></li>
                    <li><a href="#wfprotocol">Protocol</a></li>
                    <li><a href="#wfblocks">Blocks</a></li>
                    <li><a href="#wfresources">Resources</a></li>
                    <li><a href="#wfabout">About</a></li>
                </ul>
            </p>
        </div>
        <div data-role="footer" data-position="fixed">
            <h4>
                Page Footer</h4>
        </div>
    </div>
    <!--/////////////////////////////////////////////////////////////-->
    <div data-role="page" id="wfprojects">
        <div data-role="header" data-position="fixed">
            <h1>
                Projects</h1>
            <a data-icon="back" href="#" data-rel="back" command="back">Back</a>
            <button data-icon="plus" command="new">
                New project</button>
        </div>
        <div role="main" class="ui-content">
            <p>
                <div id="Projects">
                </div>
            </p>
        </div>
        <div data-role="footer" data-position="fixed">
            <h4>
                Page Footer</h4>
        </div>
    </div>
    <!--/////////////////////////////////////////////////////////////-->
    <div data-role="page" id="wfprojectprotocol">
        <div data-role="header" data-position="fixed">
            <h1>
                Project</h1>
            <a data-icon="back" href="#" data-rel="back" command="back">Back</a>
            <div data-role="navbar" data-iconpos="right">
                <ul>
                    <li><a href="#wfprojectprotocol" class="ui-btn-active" data-icon="bullets">Protocol</a></li>
                    <li><a href="#wfprojectgrid" data-icon="grid">Grid</a></li>
                    <li><a href="#wfprojectoutput" data-icon="bars">Output</a></li>
                </ul>
            </div>
        </div>
        <div role="main" class="ui-content">
            <div id="Protocol">
            </div>
        </div>
        <div data-role="footer" data-position="fixed">
        </div>
    </div>
    <!--/////////////////////////////////////////////////////////////-->
    <div data-role="page" id="wfprojectgrid">
        <div data-role="header" data-position="fixed">
            <h1>
                Project</h1>
            <a data-icon="back" href="#" data-rel="back" command="back">Back</a>
            <div data-role="navbar" data-iconpos="right">
                <ul>
                    <li><a href="#wfprojectprotocol" data-icon="bullets">Protocol</a></li>
                    <li><a href="#wfprojectgrid" class="ui-btn-active" data-icon="grid">Grid</a></li>
                    <li><a href="#wfprojectoutput" data-icon="bars">Output</a></li>
                </ul>
            </div>
        </div>
        <div role="main" class="ui-content">
            <fieldset>
                <div id="Representation">
                </div>
                <div id="GridContainer">
                </div>
            </fieldset>
        </div>
        <div data-role="footer" data-position="fixed">
            <button data-icon="minus" command="delete">
                Delete</button>
        </div>
    </div>
    <!--/////////////////////////////////////////////////////////////-->
    <div data-role="page" id="wfprojectoutput">
        <div data-role="header" data-position="fixed">
            <h1>
                Project</h1>
            <a data-icon="back" href="#" data-rel="back" command="back">Back</a> <span class="ui-btn-right">
                <button data-icon="delete" command="deleteproject">
                    Delete</button>
                <button data-icon="check" command="save">
                    Save</button>
            </span>
            <div data-role="navbar" data-iconpos="right">
                <ul>
                    <li><a href="#wfprojectprotocol" data-icon="bullets">Protocol</a></li>
                    <li><a href="#wfprojectgrid" data-icon="grid">Grid</a></li>
                    <li><a href="#wfprojectoutput" class="ui-btn-active" data-icon="bars">Output</a></li>
                </ul>
            </div>
        </div>
        <div role="main" class="ui-content">
            <fieldset>
                <fieldset>
                    <legend>
                        <h2>
                            Project name</h2>
                    </legend>
                    <input runat="server" id="txtProjectId" placeholder="Project Name" type="text" value="" />
                </fieldset>
                <legend>
                    <h2>
                        Output</h2>
                </legend>
                <fieldset>
                    <legend>
                        <h3>
                            Cost</h3>
                    </legend>
                    <div id="ContentCost">
                    </div>
                </fieldset>
                <fieldset>
                    <legend>
                        <h3>
                            Cost information</h3>
                    </legend>
                    <div id="Content">
                    </div>
                </fieldset>
                <fieldset>
                    <legend>
                        <h3>
                            Summary</h3>
                    </legend>
                    <div id="Summary">
                    </div>
                </fieldset>
            </fieldset>
        </div>
        <div data-role="footer" data-position="fixed">
        </div>
    </div>
    <!--/////////////////////////////////////////////////////////////-->
    <div data-role="page" id="wfprotocol">
        <div data-role="header" data-position="fixed">
            <h1>
                Protocol</h1>
            <a data-icon="back" href="#" data-rel="back" command="back">Back</a>
        </div>
        <div role="main" class="ui-content">
            <div id="ProtocolEdit">
            </div>
        </div>
        <div data-role="footer" data-position="fixed">
            <fieldset data-role="controlgroup" data-type="horizontal" data-mini="true">
                <button data-icon="edit" command="edit">
                    Edit</button>
                <button data-icon="minus" command="delete">
                    Delete</button>
                <button data-icon="plus" command="addbefore">
                    Add before</button>
                <button data-icon="plus" command="addafter">
                    Add after</button>
                <button data-icon="arrow-u" command="moveup">
                    Move up</button>
                <button data-icon="arrow-d" command="movedown">
                    Move down</button>
                <button data-icon="plus" command="add">
                    Add</button>
            </fieldset>
        </div>
    </div>
    <!--/////////////////////////////////////////////////////////////-->
    <div data-role="page" id="wfquestion">
        <div data-role="header" data-position="fixed">
            <h1>
                Question</h1>
            <a data-icon="back" href="#" data-rel="back" command="back">Back</a>
            <button data-icon="check" command="save">
                Save</button>
        </div>
        <div role="main" class="ui-content">
            <fieldset>
                <legend>
                    <h2>
                        Main info</h2>
                </legend>
                <label for="txtQId">
                    Id</label>
                <input id="txtQId" />
                <label for="txtQSortOrder">
                    Sort order
                </label>
                <input id="txtQSortOrder" />
            </fieldset>
            <fieldset>
                <legend>Description </legend>
                <textarea id="txtQDescription" style="width: 100%; height: 80px"></textarea>
            </fieldset>
            <fieldset>
                <legend>
                    <h2>
                        Navigation properties</h2>
                </legend>
                <label for="lstQQuestionType">
                    Question type</label>
                <select id="lstQQuestionType">
                    <option value="">Default</option>
                    <option value="Linked_Choice">Linked choice</option>
                    <option value="Free_Text">Free text (clears answers)</option>
                </select>
                <label for="txtQLinkedQuestion">
                    Linked question (Id)
                </label>
                <input id="txtQLinkedQuestion" />
                <label for="txtQProceed_With">
                    Proceed with (Sort order)</label>
                <input id="txtQProceed_With" />
            </fieldset>
        </div>
        <div data-role="footer" data-position="fixed">
            <h4>
                Page Footer</h4>
        </div>
    </div>
    <!--/////////////////////////////////////////////////////////////-->
    <div data-role="page" id="wfanswer">
        <div data-role="header" data-position="fixed">
            <h1>
                Answer</h1>
            <a data-icon="back" href="#" data-rel="back" command="back">Back</a>
            <button data-icon="check" command="save">
                Save</button>
        </div>
        <div role="main" class="ui-content">
            <fieldset>
                <legend>
                    <h2>
                        Main info</h2>
                </legend>
                <label for="txtAId">
                    Id
                </label>
                <input id="txtAId" />
                <label for="chkASelected">
                    Selected (Default)
                </label>
                <input type="checkbox" id="chkASelected" />
            </fieldset>
            <fieldset>
                <legend>Description </legend>
                <textarea id="txtADescription" style="width: 100%; height: 80px"></textarea>
            </fieldset>
            <fieldset>
                <legend>
                    <h2>
                        Navigation properties</h2>
                </legend>
                <label for="txtAProceed_With">
                    Proceed with (SortOrder)</label>
                <input id="txtAProceed_With" />
            </fieldset>
        </div>
        <div data-role="footer" data-position="fixed">
            <h4>
                Page Footer</h4>
        </div>
    </div>
    <!--/////////////////////////////////////////////////////////////-->
    <div data-role="page" id="wfblocks">
        <div data-role="header" data-position="fixed">
            <h1>
                Blocks</h1>
            <a data-icon="back" href="#" data-rel="back" command="back">Back</a>
        </div>
        <div role="main" class="ui-content">
            <div id="Blocks">
            </div>
        </div>
        <div data-role="footer" data-position="fixed">
            <h4>
                Page Footer</h4>
        </div>
    </div>
    <!--/////////////////////////////////////////////////////////////-->
    <div data-role="page" id="wfblock">
        <div data-role="header" data-position="fixed">
            <h1>
                Block</h1>
            <a data-icon="back" href="#" data-rel="back" command="back">Back</a>
            <button data-icon="check" command="save">
                Save</button>
        </div>
        <div role="main" class="ui-content">
            <fieldset>
                <legend>
                    <h2>
                        Main info</h2>
                </legend>
                <label for="txtBlockId" style="width: 180px;">
                    Id (Changing the id of an existing saves it as a new block)</label>
                <input id="txtBlockId" />
                <label for="txtExpression" style="width: 180px;">
                    Expression for including this block in the selection
                </label>
                <input id="txtExpression" />
            </fieldset>
            <fieldset>
                <legend>Representation </legend>
                <textarea id="txtRepresentation" style="width: 100%; height: 80px"></textarea>
            </fieldset>
            <fieldset>
                <legend>Summary </legend>
                <textarea id="txtSummary" style="width: 100%; height: 80px"></textarea>
            </fieldset>
            <fieldset>
                <legend>Content </legend>
                <textarea id="txtContent" style="width: 100%; height: 80px"></textarea>
            </fieldset>
            <fieldset>
                <legend>
                    <h2>
                        Block Id's limiting surrounding connections</h2>
                </legend>
                <label for="txtNorth">
                    North
                </label>
                <input id="txtNorth" />
                <label for="txtEast">
                    East
                </label>
                <input id="txtEast" />
                <label for="txtSouth">
                    South
                </label>
                <input id="txtSouth" />
                <label for="txtWest">
                    West
                </label>
                <input id="txtWest" />
            </fieldset>
        </div>
        <div data-role="footer" data-position="fixed">
            <h4>
                Page Footer</h4>
        </div>
    </div>
    <!--/////////////////////////////////////////////////////////////-->
    <div data-role="page" id="wfresources">
        <div data-role="header" data-position="fixed">
            <h1>
                Resources</h1>
            <a data-icon="back" href="#" data-rel="back" command="back">Back</a>
        </div>
        <div role="main" class="ui-content">
            <asp:FileUpload runat="server" ID="FileUpload" />
            <asp:button runat="server" ID="Upload" Text="Upload" />
            <div id="Resources">
            </div>
        </div>
        <div data-role="footer" data-position="fixed">
            <h4>
                Page Footer</h4>
        </div>
    </div>
    <!--/////////////////////////////////////////////////////////////-->
    <div data-role="page" id="wfabout">
        <div data-role="header" data-position="fixed">
            <h1>
                About Configaro</h1>
            <a data-icon="back" href="#" data-rel="back" command="back">Back</a>
        </div>
        <div role="main" class="ui-content">
            Configaro simplifies the configuration scenario for a complex products.
            <br />
            It offers the possibility to set up building blocks that can be used in different scenarios and together with other blocks form larger complex product configurations.
            <br />
            The selection of building blocks and their content is made by a flexible question protocol which guides the user thru the initial phase of the product configuration.
            <br />
            The selected building blocks can then be dragged onto a grid and glued together with other building blocks in all possible combinations.
            <br />
            It is also possible to set restrictions for each block telling what type of blocks that are allowed in its surroundings.
            <br />
            This is a way to guide the user not to build configurations that arent allowed.
            <br />
            <br />
            <h2>
                Question Protocol</h2>
            <h3>
                Question</h3>
            &nbsp;A question is built up of the following properties: ID, Question type, Linked Question, SortOrder; Proceed with, Display type, Description<br />
            &nbsp;ID identifies the Question and does not have to be altered. It is used and generated by the platform to keep it unique.<br />
            &nbsp;Question type tells if its an ordinary question or a linked choice. A linked choice tells that the question makes its selection based on a selection made for a previous question set by the field Linked Question.<br />
            &nbsp;SortOrder tells where in the protocol the question appears.<br />
            &nbsp;Proceed with, tells that an unconditional jump to a question further down in the protocol is performed.<br />
            &nbsp;Display type makes it possible to alternate the display of the answers between dropdown and radio buttons.<br />
            &nbsp;Description is the actual question.
            <h3>
                Answer</h3>
            &nbsp;Answer is built up of the following properties: ID, Selected, Proceed with, Description<br />
            &nbsp;ID identifies the answer as unique in the protocol<br />
            &nbsp;Selected sets the default answer when displaying the answer alternatives for a certain question<br />
            &nbsp;Proceed with tells the parser to make a jump to another question when the answer is selected.<br />
            &nbsp;Description is the actual answer text.
            <h2>
                Building blocks</h2>
            &nbsp;Building blocks are set up by defining the ID for the block, defining the Representation, Content and Summary. Block id, which is mandatory, identifies the block in the environment and makes it aware of other blocks also having Id&#39;s.
            <br />
            <h3>
                Representation</h3>
            &nbsp;Representation defines how the block is displayed in the tool and holds content defined as HTML.
            <br />
            <h3>
                Content</h3>
            &nbsp;Content defines the physical representaiton of the block,
            <br />
            That is a description of its content with pricing in desired currency (SEK, USD, EUR)
            <br />
            Content is also represented as HTML for a clean and flexible output to the client
            <br />
            Custom attributes for money representation are applied in the html structure where needed
            <br />
            i.e: &lt;div amount="175" currency="USD"&gt; sheet of metal&lt;/div&gt;
            <br />
            In the content structure, expressions can be added to remove certain parts of the Content based on selections made in the question protocol.
            <br />
            i.e: &lt;div amount="175" currency="USD" expression="11"&gt; sheet of metal from sweden&lt;/div&gt;
            <br />
            &lt;div amount="16" currency="EUR" expression="12"&gt; sheet of metal from finland&lt;/div&gt;
            <br />
            Expression has basic support for logical expression such as &quot;AND&quot;, &quot;OR&quot; and &quot;NOT&quot; i.e. &quot;11 AND 12&quot;
            <br />
            The digit in the expression above relates to an id of an answer in the question protocol.
            <br />
            <h3>
                Summary</h3>
            &nbsp;Summary is built in the same manner as Content but is supposed to hold a description of the building block and not the content of it.
            <br />
            It has the same support as Content except for the pricing.
            <h2>
                Grid or assembly area</h2>
            This is where the assembly takes place and the blocks form a more complex product
        </div>
        <div data-role="footer" data-position="fixed">
            <h4>
                Page Footer</h4>
        </div>
    </div>
    </form>
</body>
</html>
