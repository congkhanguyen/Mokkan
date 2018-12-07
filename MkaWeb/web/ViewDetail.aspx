<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewDetail.aspx.cs" Inherits="MkaWeb.PopUp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>詳細表示</title>
    <link rel="stylesheet" type="text/css" href="pstyle.css" />
</head>
<body>
    <form id="frmMokkanDetail" runat="server">
    <table>
        <tr>
            <td id="popImg">
                 <asp:Image ID="imgMokkan" runat="server"/>
            </td>
            <td>
                <table class="popTbl">
                    <tr>
                        <td colspan="2">
                            <h3>出土情報</h3>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <a>発掘調査次数：</a>
                        </td>
                        <td>
                            <asp:Label ID="lblChousaJisuu" runat="Server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <a>調査地区：</a>
                        </td>
                        <td>
                            <asp:Label ID="lblChikuBangou" runat="Server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <a>遺構名：</a>
                        </td>
                        <td>
                            <asp:Label ID="lblIkoumei" runat="Server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <a>土層名：</a>
                        </td>
                        <td>
                            <asp:Label ID="lblDosoumei" runat="Server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <a>グリッド：</a>
                        </td>
                        <td>
                            <asp:Label ID="lblGrid" runat="Server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <a>出土日付：</a>
                        </td>
                        <td>
                            <asp:Label ID="lblDate" runat="Server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <a>バット番号：</a>
                        </td>
                        <td>
                            <asp:Label ID="lblBatBangou" runat="Server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <a>ガラス板番号：</a>
                        </td>
                        <td>
                            <asp:Label ID="lblGlassItaBangou" runat="Server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <h3>個別木簡情報</h3>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <a>R番号：</a>
                        </td>
                        <td>
                            <asp:Label ID="lblRBangou" runat="Server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <a>仮釈文：</a>
                        </td>
                        <td>
                            <asp:Label ID="lblKariShakubun" runat="Server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <a>概報所収情報：</a>
                        </td>
                        <td>
                            <asp:Label ID="lblGaihouShosuuJyouhou" runat="Server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <a>写真番号情報：</a>
                        </td>
                        <td>
                            <asp:Label ID="lblShasinBangouJyouhou" runat="Server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <a>備考：</a>
                        </td>
                        <td>
                            <asp:Label ID="lblBikou" runat="Server"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
