# Global Office 365 Developer Bootcamp 2018 Japan

[Global Office 365 Developer Bootcamp 2018 Japan](https://connpass.com/event/91901/) の Microsoft Graph ハンズオンの資料です。

## 事前準備

ハンズオンを受講するにあたり以下のソフトウェアをインストールする必要があります。

- [.NET Core 2.1 SDK](https://www.microsoft.com/net/download)
- [Visual Studio Code](https://code.visualstudio.com)
  - 拡張機能: C#
- [Git](https://git-scm.com/downloads)
- [Fiddler](https://www.telerik.com/fiddler)

コンピュータの OS は問いません (上記のソフトウェアは Windows, MacOS, Linux のいずれもインストールできます) が、発表者は Windows 10 を使って説明します。OS に固有の設定がある場合は、その都度説明を行います。

## プロジェクト

### AuthorizationCodeGrant

委任されたアクセス許可 (Authorization Code Grant による認可フロー) を体験するためのサンプル プロジェクト (ASP.NET Core MVC アプリケーション) です。一般的な Web アプリケーションにおいてアクセス トークンを取得する手順を確認します。

### ClientCredentialsGrant

アプリケーションのアクセス許可 (Client Credentials Grant による認可フロー) を体験するためのサンプル プロジェクト (.NET Core コンソール アプリケーション) です。バックエンドで動作するアプリケーションにおいてアクセス トークンを取得する手順を確認します。

### MsalAndGraphSdk

[Microsoft Authentication Library (MSAL)](https://www.nuget.org/packages/Microsoft.Identity.Client) および [Microsoft Graph SDK](https://www.nuget.org/packages/Microsoft.Graph) を使用して Authorization Code Grant による認可フローを体験するためのサンプル プロジェクト (ASP.NET Core MVC アプリケーション) です。ライブラリを使用しない AuthorizationCodeGrant との実装方法の違いを確認します。
