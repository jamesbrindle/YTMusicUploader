# YT Music Uploader

[![N|Solid](https://portfolio.jb-net.co.uk/shared/yt_logo-64.png)](https://github.com/jamesbrindle/YTMusicUploader)
&nbsp;

Automatically upload your local music personal library to YouTube music.
&nbsp;
&nbsp;

**[Download Version 1.2 Installer](https://github.com/jamesbrindle/YTMusicUploader/releases/tag/v1.2)**
&nbsp;
&nbsp;

### Application

This is a .Net application written in C# that uploads your person music library to YouTube. It has a minimalistic UI for basic settings such as:

- Choosing library folders.
- Option to start the application with windows (will start in hidden, accessible via the System Tray):

[![N|Solid](https://portfolio.jb-net.co.uk/shared/ytmusicuploader-sc2.png)](https://github.com/jamesbrindle/YTMusicUploader)

- Option to throttle the upload speed to it doesn't use all your bandwidth.

&nbsp;

[![N|Solid](https://portfolio.jb-net.co.uk/shared/ytmusicuploader-sc1.png)](https://github.com/jamesbrindle/YTMusicUploader)

[![N|Solid](https://portfolio.jb-net.co.uk/shared/ytmusicuploader-sc6.png)](https://github.com/jamesbrindle/YTMusicUploader)
&nbsp;
&nbsp;

### Reason for Creation

I used to have Google Play music and liked uploading my own content via its automatic library uploader application; I have a large library of music and you could stream your own uploaded songs from Google Play music without paying for a subscription...

Google Play music is on its way out in December and its replacement, YouTube music doesn't currently have a library watcher application. You can only drag and drop manually into the browser for a limited number of songs... So, I decided to create one.

I got a subscription in the end, so some might consider this pointless in the world of streaming anything you want these days.. So I suppose the only real benefit is the ability to:

- Upload songs that aren't on YouTube music.
- Backup your songs *(you can't download them again from YouTube music, but you can use [Google Takeout](https://takeout.google.com/settings/takeout?pli=1) to get them).*
&nbsp;
&nbsp;

### How it Works

Since, as far as I'm aware, YouTube Music doesn't have an official API to utilise. Therefore this application mimics the HTTP request and responses from the YouTube music site uses (F12 in Firefox is your friend).

YouTube Music uses an authentication cookie, and an authentication header consisting of a SAPISID hash from the cookie itself. Therefore, this application needs a browser so you can sign into YouTube in order to retrieve said cookie:
&nbsp;

[![N|Solid](https://portfolio.jb-net.co.uk/shared/ytmusicuploader-sc3.png)](https://github.com/jamesbrindle/YTMusicUploader)

[![N|Solid](https://portfolio.jb-net.co.uk/shared/ytmusicuploader-sc4.png)](https://github.com/jamesbrindle/YTMusicUploader)

[![N|Solid](https://portfolio.jb-net.co.uk/shared/ytmusicuploader-sc5.png)](https://github.com/jamesbrindle/YTMusicUploader)
&nbsp;

### Technology

- .Net 4.7.2
- WinForms
- SQLite
&nbsp;
&nbsp;

### IDE / Extensions

- Microsoft Visual Studio 2019
- Microsoft Visual Studio Install Project
&nbsp;
&nbsp;

### Libraries

- [Brotli.Net (Decompress Google HTTP resonse body)](https://www.nuget.org/packages/Brotli.NET) 
- [Dapper](https://github.com/StackExchange/Dapper) 
- [Metro Framework (UI Styling)](https://github.com/dennismagno/metroframework-modern-ui) 
- [Ookii Dialogues](http://www.ookii.org/software/dialogs)
&nbsp;
&nbsp;

### Tools

- [Doxygen (Source code HTML documentation)](https://www.doxygen.nl/index.html)
&nbsp;
&nbsp;

### Web Control

- [WebView2](https://docs.microsoft.com/en-us/microsoft-edge/webview2/)
&nbsp;

**WebView2** is a new web control plugin for WPF and WinForms that uses Microsoft Edge. At the time of writing its still in its infancy and has been very prone to error. The reason it's used here is because it seems to be the only web control that's able to view YouTube Music.

Using the native WPF or WinForms web control or GeckoFX you just get a web page asking to update the browser. Using CefSharp you get a page telling you the browser isn't secure enough.

I believe I've managed to get it stable, although you do need the latest version of Microsoft Edge installed (or maybe even still the verison of [Microsoft Edge from the Canary channel:](https://www.microsoftedgeinsider.com/en-us/download)). **The YT Music Uploader installer contains and installer for this a dependency for convenience.**

Dispite some of the issues found using this control, it is actually a very good browser control. It's fast, uses little system resources and renders everything very nicely.
&nbsp;
&nbsp;

**GOTCHAS**
&nbsp;

One thing to bear in mind, is that it required a writable location for cache files (and other random files), and you need to set this up **before** navigating to any URL:
&nbsp;

```
private async void InitializeBrowser()
{
    var env = await CoreWebView2Environment.CreateAsync(Global.EdgeFolder, Global.AppDataLocation);
    await browser.EnsureCoreWebView2Async(env);
    browser.Source = new Uri("https://music.youtube.com/", UriKind.Absolute);
}
```

Thank you [DjSt3rios](https://github.com/DjSt3rios) for working that out: https://github.com/MicrosoftEdge/WebViewFeedback/issues/297

Also, you need to wait for the ready state of the CoreWebView2WebResource context before trying to read any properties from it (like the request headers). This is easily achievable with events:
&nbsp;

```
private void Browser_CoreWebView2Ready(object sender, EventArgs e)
{
    browser.CoreWebView2.AddWebResourceRequestedFilter("*", CoreWebView2WebResourceContext.All);
    browser.CoreWebView2.WebResourceRequested += CoreWebView2_WebResourceRequested;
}
```

### How Long Will This Application Last?

Who knows... It's dependent on YouTube Music / Google not changing how their 'non-public' API methods are stuctured and work. Considering YouTube Music has only just recently added the ability to upload songs (and thus is very new), I suspect it's likely to change as YouTube music develops...

I'm going to be using this application for personal use, so I'll know straight aware when it's not working and will attempt to update the API call implementation as soon as possible.
&nbsp;
&nbsp;

### Further Development Considerations

- Could do with the ability to see if a music file is already uploaded to YouTube Music or not. I could not find a way of requesting a file hash from YouTube Music. I could only got a json object back with a typical free text search of the song title and artist and found it not reliable enough, with multiple 'fuzzy / AI logic' result sets for 'similar' songs.
- Would like to add the ability to upload playlists *([sigma67](https://ytmusicapi.readthedocs.io/en/latest/) has included this ability in his API so it's definately do-able).*
&nbsp;
&nbsp;

### Special Thanks

- [sigma67](https://ytmusicapi.readthedocs.io/en/latest/) - Who created a Python YouTube Music API that I could reference. [sigma67: Github](https://github.com/sigma67/ytmusicapi).
- [wilsone8](https://www.codeproject.com/Articles/38959/A-Faster-Directory-Enumerator) - Who created a very fast Windows directory enumerator.
- [Dave Thomas](https://stackoverflow.com/users/984724/dave-thomas) - Who worked out how to get the SAPISID hash from the the YouTube Music authentication cookie on a post on [StackOverflow](https://stackoverflow.com/a/32065323/5726546).
- [0xDEADBEEF](https://stackoverflow.com/users/909365/0xdeadbeef) - Who made a simple class to bandwidth throttle a byte stream on a post on [StackOverflow](https://stackoverflow.com/questions/371032/bandwidth-throttling-in-c-sharp).
- [DjSt3rios](https://github.com/DjSt3rios) - For some WebView2 insight.



