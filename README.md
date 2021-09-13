# YT Music Uploader


&nbsp;

[![N|Solid](https://portfolio.jb-net.co.uk/shared/yt_logo-64.png)](https://github.com/jamesbrindle/YTMusicUploader)

&nbsp;

**[Download Version 1.7.9 Installer](https://github.com/jamesbrindle/YTMusicUploader/releases/tag/v1.7.9)**
&nbsp;

&nbsp;


<a href="https://www.buymeacoffee.com/jamiebrindle" target="_blank"><img src="https://cdn.buymeacoffee.com/buttons/default-orange.png" alt="Buy Me A Coffee" height="41" width="174"></a>

&nbsp;
&nbsp;

### Application

This is a .Net application written in C# that uploads your personal music library to YouTube. It has a minimalistic UI for basic settings such as:

- Choosing library folders.
- Option to start the application with Windows (will start hidden, accessible via the System Tray):

[![N|Solid](https://portfolio.jb-net.co.uk/shared/ytmusicuploader-sc2.png)](https://github.com/jamesbrindle/YTMusicUploader)

- Option to throttle the upload speed so it doesn't use all your bandwidth.

&nbsp;

[![N|Solid](https://portfolio.jb-net.co.uk/shared/ytmusicuploader-sc1c.png)](https://github.com/jamesbrindle/YTMusicUploader)

[![N|Solid](https://portfolio.jb-net.co.uk/shared/ytmusicuploader-sc6c.png)](https://github.com/jamesbrindle/YTMusicUploader)

[![N|Solid](https://portfolio.jb-net.co.uk/shared/ytmusicuploader_ytmusicmanagec.png)](https://github.com/jamesbrindle/YTMusicUploader)
&nbsp;
&nbsp;

### Features

- Connect and authenticate with YouTube Music.
- Upload music to YouTube Music.
- Automatic creation of YouTube Music playlists from local .m3u, .m3u8, .wpl, .pls, ,zpl playlist files.
- Delete uploaded music from YouTube Music.
- Delete playlists from YouTube Music.
- Checks YouTube Music to see if the music file has already been uploaded. ***It will perform this check once per month on all watched library files and is quite dependant on music file meta data being present and accurate.***
- Add and remove library watch folders.
- File system watcher to monitor for changes in watched folders.
- Throttle upload bandwidth.
- Start with Windows, minimized to the system tray area.
- Reads music file tags, including cover art thumbnail.
- If not all data is found in the tags of the music file, it will use the MusicBrainz API to look it up (including the cover art thumbail) (Fetching the details is purely for UI purposes. It has now impact of uploading to YouTube and doesn't write the results to the music file).
- Show an upload log dialogue.
- Show an issues log dialogue.

&nbsp;
- **This application does not send any telemetry data of any kind to its source if the 'Send Diagnostic Data' checkbox is not set'**
- **Valid music file formats are the same as YouTube Music:  .flac, .m4a, .mp3, .oga, .wma**
- **Valid music playlist file formats are:  .m3u, .m3u8, .wpl, .pls, .zpl**
- **Maximum number of files you can upload to YouTube Music is 100, 000**
- **Maximum number of playlist items YouTube Music will allow is 5,000**
- **Maximum file size YouTube Music will allow is 300 MB**
- **Although you have the ability to delete from YouTube Music within the app, this application is strictly a one way synchronisation app.**
&nbsp;

&nbsp;


### Reason for Creation

I used to have Google Play music and liked uploading my own content via its automatic library uploader application; I have a large library of music and you could stream your own uploaded songs from Google Play music without paying for a subscription...

Google Play music is on its way out in December and its replacement, YouTube Music doesn't currently have a library watcher application. You can only drag and drop manually into the browser for a limited number of songs... So, I decided to create one.

I got a subscription in the end, so some might consider this pointless in the world of streaming anything you want these days... So I suppose the only real benefit is the ability to:

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
- Microsoft Visual Studio Installer Project
&nbsp;
&nbsp;

### Libraries

- [Brotli.Net (Decompress Google HTTP resonse body)](https://www.nuget.org/packages/Brotli.NET) 
- [xxHash - Fast file hash generator](https://www.nuget.org/packages/System.Data.HashFunction.xxHash/)
- [Dapper](https://github.com/StackExchange/Dapper) 
- [Metro Framework (UI Styling)](https://github.com/dennismagno/metroframework-modern-ui) 
- [Ookii Dialogues](http://www.ookii.org/software/dialogs)
- [MusicBrainz API - Zastai](https://github.com/Zastai/MetaBrainz.Common.Json)
- [MusicBrainz CoverArt - Zastai](https://github.com/Zastai/MetaBrainz.MusicBrainz.CoverArt)
- [MusicBrainz API implementation](https://github.com/avatar29A/MusicBrainz)
- [TagLibSharp - Read music file tags](https://www.nuget.org/packages/TagLibSharp/)
&nbsp;
&nbsp;

### Tools

- [Doxygen (Source code HTML documentation)](https://www.doxygen.nl/index.html)
&nbsp;
&nbsp;

### Special Thanks

- [sigma67](https://ytmusicapi.readthedocs.io/en/latest/) - Who created a Python YouTube Music API that I could reference. [sigma67: Github](https://github.com/sigma67/ytmusicapi).
- [wilsone8](https://www.codeproject.com/Articles/38959/A-Faster-Directory-Enumerator) - Who created a very fast Windows directory enumerator.
- [Dave Thomas](https://stackoverflow.com/users/984724/dave-thomas) - Who worked out how to get the SAPISID hash from the the YouTube Music authentication cookie on a post on [StackOverflow](https://stackoverflow.com/a/32065323/5726546).
- [0xDEADBEEF](https://stackoverflow.com/users/909365/0xdeadbeef) - Who made a simple class to bandwidth throttle a byte stream on a post on [StackOverflow](https://stackoverflow.com/questions/371032/bandwidth-throttling-in-c-sharp).
- [avatar29A](https://github.com/avatar29A/MusicBrainz) - For the MusicBrainz .Net API implementation.
- [DjSt3rios](https://github.com/DjSt3rios) - For some WebView2 insight.
- [tmk907](https://github.com/tmk907) - Who created a very good, easy to use, multi-type playlist reader. [tmk907: Github](https://github.com/tmk907/PlaylistsNET).

### Thanks for the Coffee

- EdgeGuy13
- Mew
- CowtownChina
- Someone
- Someone
- Stephen M
- Brian A
- @NourishedAIO
- nishantranacrm
- Oak

&nbsp;
&nbsp;

<a href="https://www.buymeacoffee.com/jamiebrindle" target="_blank"><img src="https://cdn.buymeacoffee.com/buttons/default-orange.png" alt="Buy Me A Coffee" height="41" width="174"></a>


