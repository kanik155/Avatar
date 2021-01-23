# VRoid SDK Multiplay Extension

## VRoid SDK Multiplay Extensionのインストール
ダウンロードしたVRoidSDKMultiplayExtension.unitypackageファイルを

Assets > Import Package > Custom Package...

からインポートすることでインストールが完了します

## SDK設定
- [アプリケーション管理ページ](https://hub.vroid.com/oauth/applications)からOAuth連携アプリケーションを作成し、multiplayスコープを設定する
- `Assets/VRoidSDK/SDKConfigurations/SDKConfiguration.assets`を開き、multiplayスコープにチェックを入れる

## Getting Started
### MultiplaySdkConfigurationの設定
VRMをキャッシュするときに暗号化をしており、その暗号化に使用する鍵が他のアプリと同じにならないようにアプリ固有のパスワードを指定する必要があります。
アプリ固有のパスワードはAuthentication.Instance.Initを実行する前に設定してください。
現在はマルチプレイのみでこのアプリ固有のパスワードを指定するようになっていますが、後々シングルプレイでも採用する予定なので、設定方法が変更される可能性があるのでご注意ください。

```cs
MultiplaySdkConfiguration.Instance.AppPassword = "Password for your app";
Authentication.Instance.Init(_configuration.AuthenticateMetaData);
```

### 認証ユーザーがロードしたキャラクターを他のユーザーもロードする方法

他のユーザーと通信してマルチプレイで遊ぶアプリを作る場合、自身がロードしているキャラクターを通信相手のユーザーもロードする機能が必要になります。
VRoid SDKでは、マルチプレイ用のダウンロードライセンスを発行することで、通信相手のユーザーが自身のキャラクターをロードできるようになります。

ダウンロードライセンスはVRoid HubからVRMをダウンロードするために作られる有効期限付きのデータです。
通常のダウンロードライセンスは発行したユーザー自身しか利用できませんが、マルチプレイ用のダウンロードライセンスはダウンロードライセンスの発行者以外も利用することができます。
そのため、通信相手のユーザーに発行したマルチプレイ用のダウンロードライセンスを渡すことで、通信相手のユーザーも自身のキャラクターをロードできます。
ダウンロードライセンスの有効期限が切れるとVRMをダウンロードできなくなるので、有効期限が切れた場合は新しくダウンロードライセンスを発行する必要があります。

マルチプレイ用のダウンロードライセンスを発行するには`HubMultiplayApi.PostDownloadLicenseMultiplay`を使用します。
そしてこのAPIで取得したダウンロードライセンスのIDを通信相手のユーザーに送信し、通信相手のユーザーは`HubMultiplayModelDeserializer.Instance.LoadCharacterAsync`を使用することでキャラクターをロードできます。
APIのエラーによって`HubMultiplayModelDeserializer.Instance.LoadCharacterAsync`のonErrorが呼ばれた場合はその変数の型が`ApiRequestFailException`になっており、キャストすることでエラーコードを確認できます。

ダウンロードライセンスを発行するとき、同じユーザーが同じアプリケーションで発行した同じキャラクターの古いダウンロードライセンスは無効化されます。
ダウンロードライセンスの有効期限が切れているときや無効化されているときに`HubMultiplayModelDeserializer.Instance.LoadCharacterAsync`を使用するとonErrorが呼ばれ、エラーコードには`COMMON_NOT_FOUND`が設定されます。

```cs
// マルチプレイ用のダウンロードライセンスを発行
HubMultiplayApi.PostDownloadLicenseMultiplay(
    characterModelId: model.id,
    onSuccess: (downloadLicense) =>
    {
        // マルチプレイ用のダウンロードライセンスのIDを相手に送信する
        SendToOtherUser(downloadLicense.id);
    }
    onError: onError
);

void ReceiveDownloadLicenseId(string downloadLicenseId)
{
    HubMultiplayModelDeserializer.Instance.LoadCharacterAsync(
        characterModelId: downloadLicenseId, // 相手から受け取ったDownloadLicenseのidを指定する
        onDownloadProgress: (float progress) => {
            // VRMファイルがキャッシュされておらずダウンロードが必要な場合に、進捗状況が0.0〜1.0の間で通知される
        },
        onLoadComplete: (GameObject characterObj) => {
            // UniVRMでデシリアライズされたVRMファイルのGameObjectが返される
        },
        onError: (Exception error) => {
            // 実行中にエラーが発生した場合、呼び出される
        }
    );
}
```


## サポートランタイム設定
|    Scripting Runtime Version    | Scripting Backend(※1) | Api Compatibility Level |
| ------------------------------- | --------------------- | ----------------------- |
| .NET 3.5 Equivalent(deprecated) |         Mono          |      .NET 2.0 Subset    |
| .NET 4.x Equivalent             |         Mono          |         .NET4.x         |

(※1) Android端末の場合、Scripting BackendにMonoを採用することはできません。IL2CPPをご利用ください

## パッケージビルドバージョン
Unity 2018.2.17f1
