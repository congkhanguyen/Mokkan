<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="MkaWeb._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>削屑木簡データベース</title>
    <link rel="shortcut icon" href="images/mka.ico">
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
    <div id="outer">
        <div id="header">
            <div id="logo">
                <h1>
                    削屑木簡データベース</h1>
            </div>
            <div id="nav">
                <ul class="lavaLampWithImage" id="lava_menu">
                    <li class="current"><a href="Default.aspx">ホーム</a> </li>
                    <li><a href="SimpleSearch.aspx">簡単検索</a> </li>
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
                        お知らせ</h2>
                    <img class="left" src="images/mokkan.jpg" width="200" height="183" alt="" />
                    <p>
                        ★ この削屑木簡データベースは、木簡の個々の文字のさまざまな画像と、釈文などの木簡そのもののデータをリンクさせたデータベースです。<br />
                        ★ 奈良文化財研究所が保管する資料、及び許可をいただいた他機関（別掲）の資料について、奈良文化財研究所が作成・公開しています。<br />
                        ★ 画像やデータの無許可での転載・再配布は堅くお断りいたします。
                    </p>
                </div>
            </div>
        </div>
    </div>
    <div id="copyright">
        Copyright ©2012 <a href="http://www.nabunken.go.jp/" target="_blank">Research Institute
            for Cultural Properties</a>, Nara. All rights reserved.
    </div>
</body>
</html>
