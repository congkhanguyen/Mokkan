<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DetailSearch.aspx.cs" Inherits="MkaWeb.DetailSearch" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>削屑木簡データベース-詳細検索</title>
    <link rel="shortcut icon" href="images/mka.ico" />
    <link rel="stylesheet" type="text/css" href="style.css" />
    <link rel="stylesheet" type="text/css" href="thickbox.css" media="screen" />
    <link rel="stylesheet" type="text/css" href="jquery-ui-1.7.3.custom.css" media="screen" />

    <script type="text/javascript" src="js/jquery-1.4.1.js"></script>

    <script type="text/javascript" src="js/jquery-1.4.1-vsdoc.js"></script>

    <script type="text/javascript" src="js/thickbox.js"></script>

    <script type="text/javascript" src="js/jquery-1.3.2.min.js"></script>

    <script type="text/javascript" src="js/jquery.easing.min.js"></script>

    <script type="text/javascript" src="js/jquery.lavalamp.min.js"></script>

    <script type="text/javascript" src="js/jquery-ui-1.7.3.custom.min.js"></script>

    <script type="text/javascript" src="js/jquery.ui.datepicker-ja.js"></script>

    <script type="text/javascript">
    $(function() {
		$("#txtDate").datepicker({changeMonth: true,	
		                            changeYear: true,
		                            minDate: new Date(1990, 1 - 1, 15),
		                            maxDate: '0',
		                            dateFormat: 'yy/mm/dd'
		                         });
		$("#txtDate").datepicker( $.txtDate.regional[ "ja" ] );
		$("#txtDate").datepicker( "option", "minDate", new Date(1990, 1 - 1, 1) );
});
    
    </script>

    <script type="text/javascript">
function jsDecimals(e) {
 
    var evt = (e) ? e : window.event;
    var key = (evt.keyCode) ? evt.keyCode : evt.which;
    if (key != null) {
        key = parseInt(key, 10);
        if ((key < 48 || key > 57) && (key < 96 || key > 105)) {
            if (!jsIsUserFriendlyChar(key, "Decimals")) {
                return false;
            }
        }
        else {
            if (evt.shiftKey) {
                return false;
            }
        }
    }
    return true;
}
 
// Function to check for user friendly keys  
//------------------------------------------
function jsIsUserFriendlyChar(val, step) {
    // Backspace, Tab, Enter, Insert, and Delete  
    if (val == 8 || val == 9 || val == 13 || val == 45 || val == 46) {
        return true;
    }
    // Ctrl, Alt, CapsLock, Home, End, and Arrows  
    if ((val > 16 && val < 21) || (val > 34 && val < 41)) {
        return true;
    }
    
    // The rest  
    return false;
}
    </script>

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
    <form id="frmDetailSearch" runat="server">
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
                    <li class="current"><a href="DetailSearch.aspx">詳細検索</a> </li>
                    <li><a href="DbExplorer.aspx">一覧表示</a> </li>
                    <li><a href="Configure.aspx">設定</a> </li>
                </ul>
            </div>
        </div>
        <div id="main">
            <div id="content">
                <div id="box">
                    <h2>
                        詳細検索</h2>
                    <h3>
                        ■ 検索条件</h3>
                    <h4>
                        ★ 出土情報</h4>
                    <div align="center">
                        <table width="95%" id="tbl">
                            <tr>
                                <td>
                                    発掘調査次数:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtChousaJisuu" runat="server" Height="20px" Width="100px" MaxLength="4" />
                                </td>
                                <td>
                                    調査地区－大地区:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtOoChiku" runat="server" Height="20px" Width="100px" MaxLength="4" />
                                </td>
                                <td>
                                    調査地区－中小地区:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtChuushouChiku" runat="server" Height="20px" Width="100px" MaxLength="4" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    遺構名:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtIkoumei" runat="server" Height="20px" Width="100px" MaxLength="8" />
                                </td>
                                <td>
                                    土層名:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtDosoumei" runat="server" Height="20px" Width="100px" MaxLength="40" />
                                </td>
                                <td>
                                    グリッド:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtGrid" runat="server" Height="20px" Width="100px" MaxLength="3"
                                        OnKeyDown="return jsDecimals(event);" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    出土日付:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtDate" runat="server" Height="20px" Width="100px" MaxLength="10" />
                                    <asp:RegularExpressionValidator ID="validateDate" runat="server" ControlToValidate="txtDate"
                                        ErrorMessage="yyyy/mm/dd" ValidationExpression="(19|20)\d\d/([1-9]|0[1-9]|1[012])/([1-9]|0[1-9]|[12][0-9]|3[01])"></asp:RegularExpressionValidator>
                                </td>
                                <td>
                                    バット番号:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtBatBangou" runat="server" Height="20px" Width="100px" MaxLength="30" />
                                </td>
                                <td>
                                    ガラス板番号:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtGlassBangou" runat="server" Height="20px" Width="100px" MaxLength="40" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <h4>
                        ★ 個別木簡情報</h4>
                    <div align="center">
                        <table width="75%" id="tbl">
                            <tr>
                                <td>
                                    R番号:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtRBangou" runat="server" Height="20px" Width="100px" MaxLength="6"
                                        OnKeyDown="return jsDecimals(event);" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    仮釈文:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtKariShakubun" runat="server" Height="20px" Width="200px" MaxLength="200" />
                                </td>
                                <td>
                                    概報所収情報:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtGaihouShoshuuJyouhou" runat="server" Height="20px" Width="200px"
                                        MaxLength="200" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    写真番号情報:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtShasinBangouJyouhou" runat="server" Height="20px" Width="200px"
                                        MaxLength="200" />
                                </td>
                                <td>
                                    備考:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtBikou" runat="server" Height="20px" Width="200px" MaxLength="200" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4" align="center">
                                    <asp:RadioButtonList ID="rblSearch" runat="server" RepeatDirection="Horizontal" Width="100%"
                                        Font-Size="Medium">
                                        <asp:ListItem>あいまい検索</asp:ListItem>
                                        <asp:ListItem Selected="True">部分一致検索</asp:ListItem>
                                        <asp:ListItem>完全一致検索</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                        </table>
                        <div>
                            <div align="left">
                                <asp:Button ID="btnClear" runat="server" Text="クリア" Width="80px" Height="25px" OnClick="btnClear_Click" /></div>
                            <asp:Button ID="btnSearch" runat="server" Text="検索" Width="80px" Height="25px" OnClick="btnSearch_Click" />
                        </div>
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
