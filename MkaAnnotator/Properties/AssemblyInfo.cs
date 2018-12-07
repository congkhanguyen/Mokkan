using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using MokkAnnotator.MkaCommon;

// アセンブリに関する一般情報は以下の属性セットをとおして制御されます。
// アセンブリに関連付けられている情報を変更するには、
// これらの属性値を変更してください。
[assembly: AssemblyTitle(MkaDefine.AssemblyTitle)]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany(MkaDefine.AssemblyCompany)]
[assembly: AssemblyProduct(MkaDefine.AssemblyProduct)]
[assembly: AssemblyCopyright(MkaDefine.AssemblyCopyright)]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// ComVisible を false に設定すると、その型はこのアセンブリ内で COM コンポーネントから 
// 参照不可能になります。COM からこのアセンブリ内の型にアクセスする場合は、
// その型の ComVisible 属性を true に設定してください。
[assembly: ComVisible(false)]

// 次の GUID は、このプロジェクトが COM に公開される場合の、typelib の ID です
[assembly: Guid("27c475ed-929c-4014-9be5-e35fe597e2f5")]

// アセンブリのバージョン情報は、以下の 4 つの値で構成されています:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// すべての値を指定するか、下のように '*' を使ってビルドおよびリビジョン番号を 
// 既定値にすることができます:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion(MkaDefine.AssemblyVersion)]
[assembly: AssemblyFileVersion("1.0.0.0")]

// log4net
[assembly: log4net.Config.XmlConfigurator(Watch = true)]