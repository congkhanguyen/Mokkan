<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DbExplorer.aspx.cs" Inherits="MkaWeb.DbExplorer" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>削屑木簡データベース-一覧表示</title>
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
    <form id="frmDBExplorer" runat="server">
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
                    <li class="current"><a href="DbExplorer.aspx">一覧表示</a> </li>
                    <li><a href="Configure.aspx">設定</a> </li>
                </ul>
            </div>
        </div>
        <div id="main">
            <div id="content">
                <div id="box">
                    <h2>
                        データベース一覧表示</h2>
                </div>
                <div id="box1">
                    <h3>
                        バット・ガラス板一覧
                    </h3>
                    <asp:TreeView ID="treeBatGlass" runat="server" Width="100%" ExpandDepth="0" OnSelectedNodeChanged="treeBatGlass_SelectedNodeChanged">
                        <HoverNodeStyle CssClass="tvHover" />
                        <SelectedNodeStyle CssClass="tvSelect" />
                        <RootNodeStyle ForeColor="Blue" />
                        <NodeStyle CssClass="tvNode" />
                    </asp:TreeView>
                </div>
                <div id="box2">
                    <h3>
                        木簡一覧
                    </h3>
                    <div id="container">
                        <asp:GridView ID="gvResults" runat="server" AutoGenerateColumns="False" GridLines="None"
                            DataKeyNames="R番号" AllowPaging="True" CssClass="mGrid" PagerStyle-CssClass="pageStyle"
                            AlternatingRowStyle-CssClass="altStyle" 
                            OnPageIndexChanging="gvResults_PageIndexChanging" 
                            onrowdatabound="gvResults_RowDataBound">
                            <RowStyle Height="70px" />
                            <Columns>
                                <asp:TemplateField HeaderText="木簡画像">
                                    <ItemTemplate>
                                        <asp:Image ID="imgMokkan" runat="server" ImageUrl='<%#"ImageHttpHandler.ashx?RBangou="+Eval("R番号")%>' />
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
    <div id="copyright">
        Copyright ©2012 <a href="http://www.nabunken.go.jp/" target="_blank">Research Institute
            for Cultural Properties</a>, Nara. All rights reserved.
    </div>
    </form>
</body>
</html>
