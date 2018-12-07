<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Configure.aspx.cs" Inherits="MkaWeb.Configure" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>削屑木簡データベース-設定</title>
    <link rel="shortcut icon" href="images/mka.ico" />
    <link rel="stylesheet" type="text/css" href="style.css" />

    <script type="text/javascript" src="js/jquery-1.3.2.min.js"></script>

    <script type="text/javascript" src="js/jquery.easing.min.js"></script>

    <script type="text/javascript" src="js/jquery.lavalamp.min.js"></script>

    <script type="text/javascript">
    $(function() {
      $("#lava_menu").lavaLamp({
        fx: "backout",
        speed: 700
      });
    });
    </script>

</head>
<body>
    <form id="frmConfigure" runat="server">
    <div id="outer">
        <div id="header">
            <div id="logo">
                <h1>
                    削屑木簡データベース</h1>
            </div>
            <div id="nav">
                <ul class="lavaLampWithImage" id="lava_menu">
                    <li><a href="Default.aspx">ホーム</a> </li>
                    <li><a href="SimpleSearch.aspx">簡単検索</a> </li>
                    <li><a href="DetailSearch.aspx">詳細検索</a> </li>
                    <li><a href="DbExplorer.aspx">一覧表示</a> </li>
                    <li class="current"><a href="Configure.aspx">設定</a> </li>
                </ul>
            </div>
        </div>
        <div id="main">
            <div id="content">
                <div id="box">
                    <h2>
                        データベース接続の設定</h2>
                    <br />
                    <br />
                    <div align="center">
                        <table width="50%" id="tbl">
                            <tr>
                                <td>
                                    サーバーアドレス:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtAddress" runat="server" Height="20px" Width="200px" />
                                    <asp:RequiredFieldValidator runat="server" ID="addressRequiredFieldValidator" ControlToValidate="txtAddress"
                                        Text="<img src='Images/Exclamation.gif' title='サーバーアドレスを入力してください！'/>" ErrorMessage="サーバーアドレスを入力してください！"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    データベース名:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtDatabase" runat="server" Height="20px" Width="150px" />
                                    <asp:RequiredFieldValidator ID="databaseRequiredFieldValidator" runat="server" ControlToValidate="txtDatabase"
                                        Text="<img src='Images/Exclamation.gif' title='データベース名を入力してください！'/>" ErrorMessage="データベース名を入力してください！"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    ユーザ名:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtUsername" runat="server" Height="20px" Width="100px" />
                                    <asp:RequiredFieldValidator ID="idRequiredFieldValidator" runat="server" ControlToValidate="txtUsername"
                                        Text="<img src='Images/Exclamation.gif' title='ユーザー名を入力してください！'/>" ErrorMessage="ユーザー名を入力してください！"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    パスワード:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPassword" runat="server" Height="20px" Width="100px" TextMode="Password" />
                                    <asp:RequiredFieldValidator ID="passRequiredFieldValidator" runat="server" ControlToValidate="txtPassword"
                                        Text="<img src='Images/Exclamation.gif' title='パスワードを入力してください！'/>" ErrorMessage="パスワードを入力してください！"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" id="validate" align=center>
                                    <asp:Image ID="imgResult" runat="server" Visible="false" />
                                    <asp:Label ID="lblTestResult" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td align="right" style="padding: 10px 10px 0 0">
                                    <asp:Button ID="btnSave" runat="server" Text="保存" Width="100px" Height="25px" OnClick="btnSave_Click" />
                                </td>
                                <td align="left" style="padding: 10px 0 0 40px">
                                    <asp:Button ID="btnTest" runat="server" Text="接続テスト" Width="100px" Height="25px"
                                        OnClick="btnTest_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <asp:ValidationSummary runat="server" ID="vsumAll" CssClass="validationsummary" HeaderText="<div class='validationheader'>下記の項目を修正してください:</div>" />
                </div>
            </div>
        </div>
    </div>
    <div id="copyright">
        Copyright ©2012 <a href="http://www.nabunken.go.jp/" target="_blank">Research Institute
            for Cultural Properties</a>, Nara. All rights reserved.
    </div>
    </form>
</body>
</html>
