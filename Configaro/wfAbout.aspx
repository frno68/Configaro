﻿<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfAbout.aspx.vb" Inherits="Configaro.wfAbout" %>
<!DOCTYPE html>
<html>
<head runat="server">
    <title>About</title>
    <script language="javascript" type="text/javascript">
        $(document).ready( function window_onload(){
            $('#imgHome').click(function () {
                window.location='wfDefault.aspx';
            });
        });
    </script>
</head>
<body class="about">
    <form id="form1" runat="server">
    <h1>
        About Configaro</h1>
        Configaro simplifies the configuration scenario for a complex products.
        <br />
        It offers the possibility to set up building blocks that can be used in different scenarios and together with other blocks form larger complex product configurations.
        <br />
        The selection of building blocks and their content is made by a flexible question protocol which guides the user thru the initial phase of the product configuration.
        <br />
        The selected blocks can then be selected and put on a grid and glued together with other blocks in all possible combinations.
        <br />
        It is also possible to set restrictions for each block telling what type of blocks that are allowed in its surroundings.
        <br />
        This is a way to guide the user not to build configurations that arent allowed.
        <br />
        <br />
        <h2>Question Protocol</h2>
        <h3>Question</h3>
        &nbsp;A question is built up by a number of properties: <br />
&nbsp;<em>ID:</em> identifies the Question and does not have to be altered. It is used and generated by the platform to keep it unique.<br />
&nbsp;<em>Sort order:</em> tells where in the protocol the question appears.<br />
    &nbsp;<em>Description:</em> the actual question.<br />
&nbsp;<em>Question type:</em> tells if its an ordinary question or a linked choice. A linked choice tells that the question makes its selection based on a selection made for a previous question set by the field <em>Linked Question</em>.<br />
&nbsp;<em>Proceed with:</em> tells that an unconditional jump to a question further down in the protocol is performed.<br />
        <h3>Answer</h3>
        &nbsp;Answer is built up of the following properties:<br />
&nbsp;<em>ID</em> identifies the answer as unique in the protocol<br />
&nbsp;<em>Selected</em> sets the default answer when displaying the answer alternatives for a certain question<br />
&nbsp;<em>Proceed with</em> tells the parser to make a jump to another question when the answer is selected.<br />
&nbsp;<em>Description</em> is the actual answer text.
        <h2>Building blocks</h2>
    Building blocks are set up by defining: <br />
    <em>ID</em> is used for representing the block in the grid when building a product<br />
    <em>Representation</em> is used to display an image that is displayed in the grid. Usually an &quot;&lt;img /&gt;&quot; tag<br />
    <em>Content </em>Displays information related to the pricing of the product. It can contain a complex html-based structure that will displayed to the user when selections are made to the configuration. The html structure can contain pricing information and criterias for including and excluding blocks of pricing from the summary of the finished configuration<br />
    <em>Summary </em>Contains information on a block level and is of a more summarizing nature.<br />
    <em>Block id</em>, which is mandatory, identifies the block in the environment and makes it aware of other blocks also having Id&#39;s. 
        <br />
        <h3>Representation></h3>
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
        i.e:
        &lt;div amount="175" currency="USD"&gt;
            sheet of metal&lt;/div&gt;
        <br />
        In the content structure, expressions can be added to remove certain parts of the Content based on selections made in the question protocol. 
        <br />
        i.e:
        &lt;div amount="175" currency="USD" expression="11"&gt;
            sheet of metal from sweden&lt;/div&gt;
        <br />
        &lt;div amount="16" currency="EUR" expression="12"&gt;
            sheet of metal from finland&lt;/div&gt;
        <br />
        Expression has basic support for logical expression such as &quot;AND&quot;, &quot;OR&quot; and &quot;NOT&quot; i.e. &quot;11 AND 12&quot; 
        <br />
        The digit in the expression above relates to an id of an answer in the question protocol. 
        <br />
        <h3>
   Summary</h3>
    Summary is built in the same manner as Content but is supposed to hold a description of the building block and not the content of it. 
        <br />
        It has the same support as Content except for the pricing. 
        
        <h2>
        Grid or assembly area</h2>
        This is where the assembly takes place and the blocks form a more complex product
    </form>

 </body>
</html>
