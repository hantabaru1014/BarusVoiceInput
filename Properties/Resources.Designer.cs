﻿//------------------------------------------------------------------------------
// <auto-generated>
//     このコードはツールによって生成されました。
//     ランタイム バージョン:4.0.30319.42000
//
//     このファイルへの変更は、以下の状況下で不正な動作の原因になったり、
//     コードが再生成されるときに損失したりします。
// </auto-generated>
//------------------------------------------------------------------------------

namespace BarusVoiceInput.Properties {
    using System;
    
    
    /// <summary>
    ///   ローカライズされた文字列などを検索するための、厳密に型指定されたリソース クラスです。
    /// </summary>
    // このクラスは StronglyTypedResourceBuilder クラスが ResGen
    // または Visual Studio のようなツールを使用して自動生成されました。
    // メンバーを追加または削除するには、.ResX ファイルを編集して、/str オプションと共に
    // ResGen を実行し直すか、または VS プロジェクトをビルドし直します。
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   このクラスで使用されているキャッシュされた ResourceManager インスタンスを返します。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("BarusVoiceInput.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   すべてについて、現在のスレッドの CurrentUICulture プロパティをオーバーライドします
        ///   現在のスレッドの CurrentUICulture プロパティをオーバーライドします。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   &lt;!DOCTYPE html&gt;
        ///&lt;html lang=&quot;ja&quot;&gt;
        ///    &lt;head&gt;
        ///        &lt;meta charset=&quot;utf-8&quot;&gt;
        ///        &lt;title&gt;音声認識&lt;/title&gt;
        ///        &lt;script src=&quot;main.js&quot;&gt;&lt;/script&gt;
        ///    &lt;/head&gt;
        ///    &lt;body onload=&quot;onLoad()&quot; onunload=&quot;onUnload()&quot; onkeydown=&quot;return false;&quot; oncontextmenu=&quot;return false;&quot;&gt;
        ///        &lt;div style=&quot;text-align:center;background-color:aqua;&quot;&gt;&lt;label id=&apos;msg&apos; style=&quot;display:inline-block;vertical-align:middle;&quot;&gt;準備中です。&lt;/label&gt;&lt;/div&gt;
        ///        &lt;div style=&apos;display:block;position:absolute;bottom:0px;left:0px;background:#fff;he [残りの文字列は切り詰められました]&quot;; に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string html {
            get {
                return ResourceManager.GetString("html", resourceCulture);
            }
        }
        
        /// <summary>
        ///   
        ///let _ws = null;
        ///let _ws_flag = false;
        ///let _ws_stop = false;
        ///let re = null;
        ///let status_label = null;
        ///let msg_label = null;
        ///let lang = &apos;ja-JP&apos;;
        ///
        ///let RecogEngine = function(lang){
        ///    let wsr = new webkitSpeechRecognition();
        ///    wsr.lang = lang;
        ///    wsr.onstart = function(){
        ///        this.next = new RecogEngine(lang);
        ///        status_label.innerHTML = &apos;音声待機&apos;;
        ///    };
        ///    wsr.onspeechstart = function(event){
        ///        status_label.innerHTML = &apos;認識中&apos;;
        ///    };
        ///    wsr.onresult = function(event){
        ///    [残りの文字列は切り詰められました]&quot;; に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string javascript {
            get {
                return ResourceManager.GetString("javascript", resourceCulture);
            }
        }
        
        /// <summary>
        ///   &lt;!DOCTYPE html&gt;
        ///&lt;html lang=&quot;ja&quot;&gt;
        ///    &lt;head&gt;
        ///        &lt;meta charset=&quot;utf-8&quot;&gt;
        ///        &lt;title&gt;設定&lt;/title&gt;
        ///        &lt;script src=&quot;settings_js.js&quot;&gt;&lt;/script&gt;
        ///    &lt;/head&gt;
        ///    &lt;body onload=&quot;onLoad()&quot; onunload=&quot;onUnload()&quot;&gt;
        ///        &lt;div style=&quot;text-align:center;background-color:aqua;&quot;&gt;&lt;label id=&apos;msg&apos; style=&quot;display:inline-block;vertical-align:middle;&quot;&gt;準備中です。&lt;/label&gt;&lt;/div&gt;
        ///        &lt;div style=&apos;display:block;position:absolute;bottom:0px;left:0px;background:#fff;height:auto;width:100%&apos;&gt;認識状況：&lt;span id=&apos;status&apos;&gt;起動中&lt;/s [残りの文字列は切り詰められました]&quot;; に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string settings_html {
            get {
                return ResourceManager.GetString("settings_html", resourceCulture);
            }
        }
        
        /// <summary>
        ///    に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string settings_js {
            get {
                return ResourceManager.GetString("settings_js", resourceCulture);
            }
        }
    }
}
