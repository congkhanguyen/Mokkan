<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SimpleSearch.aspx.cs" Inherits="MkaWeb.SimpleSearch" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<script runat="server"></script>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>削屑木簡データベース-簡単検索</title>
    <link rel="shortcut icon" href="images/mka.ico" />
    <link rel="stylesheet" type="text/css" href="style.css" />
    <link rel="stylesheet" type="text/css" href="thickbox.css" media="screen" />

    <script type="text/javascript" src="js/jquery-1.4.1.js"></script>

    <script type="text/javascript" src="js/jquery-1.4.1-vsdoc.js"></script>

    <script type="text/javascript" src="js/thickbox.js"></script>

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
    <form id="frmSimpleSearch" runat="server">
    <div id="outer">
        <div id="header">
            <div id="logo">
                <h1>
                    削屑木簡データベース</h1>
            </div>
            <div id="nav">
                <ul class="lavaLampWithImage" id="lava_menu">
                    <li><a href="Default.aspx">ホーム</a> </li>
                    <li class="current"><a href="SimpleSearch.aspx">簡単検索</a> </li>
                    <li><a href="DetailSearch.aspx">詳細検索</a> </li>
                    <li><a href="DbExplorer.aspx">一覧表示</a> </li>
                    <li><a href="Configure.aspx">設定</a> </li>
                </ul>
            </div>
        </div>
        <div id="main">
            <div id="content">
                <div id="box">
                    <h2>
                        簡単検索</h2>
                    <h3>
                        ■ 検索条件</h3>
                    <div align="center">
                        <table width="50%" id="tbl">
                            <tr>
                                <td colspan="3" align="center">
                                    <span>検索したい文字を入力し検索ボタンを押してください。</span>
                                </td>
                            </tr>
                            <tr>
                                <td width="30%">
                                    <asp:Label ID="lblSearch" runat="server" Text="検索文字:" />
                                </td>
                                <td width="50%">
                                    <asp:TextBox ID="txtKeyword" runat="server" Height="20px" Width="200px" />
                                </td>
                                <td width="20%">
                                    <asp:Button ID="btnSearch" runat="server" Text="検索" Width="80px" Height="25px" OnClick="btnSearch_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" align="center">
                                    <asp:RadioButtonList ID="rblSearch" runat="server" RepeatDirection="Horizontal" Width="100%"
                                        Font-Size="Medium">
                                        <asp:ListItem>あいまい検索</asp:ListItem>
                                        <asp:ListItem Selected="True">部分一致検索</asp:ListItem>
                                        <asp:ListItem>完全一致検索</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                        </table>
                        <div id="lblRet">
                            <asp:Label ID="lblResult" runat="server" Text="120木簡が見つけました." Visible="False"></asp:Label>
                        </div>
                        <div id="container">
                            <asp:GridView ID="gvResults" runat="server" AutoGenerateColumns="False" GridLines="None"
                                AllowPaging="True" CssClass="mGrid" PagerStyle-CssClass="pageStyle" AlternatingRowStyle-CssClass="altStyle"
                                OnPageIndexChanging="gvResults_PageIndexChanging" OnRowDataBound="gvResults_RowDataBound">
                                <RowStyle Height="70px" />
                                <Columns>
                                    <asp:TemplateField HeaderText="木簡画像">
                                        <ItemTemplate>
                                            <asp:Image ID="木簡画像" runat="server" ImageUrl='<%#"ImageHttpHandler.ashx?RBangou="+Eval("R番号")%>' />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="R番号">
                                        <ItemStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblRBangou" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="仮釈文">
                                        <ItemStyle HorizontalAlign="Right" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblKariShakubun" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="概報所収情報">
                                        <ItemStyle HorizontalAlign="Right" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblGaihouShoshuuJyouhou" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="写真番号情報">
                                        <ItemStyle HorizontalAlign="Right" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblShasinBangouJyouhou" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="備考">
                                        <ItemStyle HorizontalAlign="Right" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblBikou" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="ガラス板番号">
                                        <ItemStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblGlassItaBangou" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="バット番号">
                                        <ItemStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblBatBangou" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="詳細表示">
                                        <ItemStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:HyperLink ID="hplDetail" runat="server">詳細</asp:HyperLink>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <PagerStyle CssClass="pageStyle"></PagerStyle>
                                <HeaderStyle Height="30px" />
                                <AlternatingRowStyle CssClass="altStyle"></AlternatingRowStyle>
                            </asp:GridView>
                        </div>
                    </div>
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
