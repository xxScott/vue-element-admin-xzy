<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LoadBar.ascx.cs" Inherits="HrApp.Controls.LoadBar" %>
<script language="javascript" type="text/javascript">
    function show_loadbar() {
        $("#processMessage").css({ "left": ($(window).width() - $("#processMessage").outerWidth()) / 2 + "px", "top": ($(window).height() - $("#processMessage").outerHeight()) / 2 + "px" });
        document.getElementById("progressBackgroundFilter").style.display = "block";
        document.getElementById("processMessage").style.display = "block";
    }
    function hide_loadbar() {
        document.getElementById("progressBackgroundFilter").style.display = "none";
        document.getElementById("processMessage").style.display = "none";
    }
    </script>
    <style type="text/css">
        #progressBackgroundFilter
        {
            display: none;
            position: fixed;
            top: 0%;
            left: 0%;
            width: 100%;
            height: 100%;
            background-color: black;
            z-index: 1001;
            -moz-opacity: 0.7;
            opacity: .70;
            filter: alpha(opacity=70);
        }
        #processMessage
        {
            display: none;
            position: fixed;
            top: 45%;
            left: 45%;
            width: 200px;
            height: 30px;
            padding: 15px;
            border: 2px solid #8DB2E3;
            background-color: white;
            z-index: 1002;
            overflow: hidden;
            text-align:center;
            line-height:35px;
            float:left;
        }
    </style>
<div id="progressBackgroundFilter"></div>
<div id="processMessage">
     <div style="float:left;padding-left:10px"><img alt="Loading" src="/Images/loading.gif" /></div>正在加载中,请稍候...
</div>