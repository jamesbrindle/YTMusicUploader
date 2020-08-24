import json
from ytmusicapi import YTMusic
ytmusic = YTMusic('headers_auth.json')

search_results = ytmusic.get_library_upload_artists(500)

print(search_results)

text_file = open("c:\\temp\\test1.txt", "w")
n = text_file.write(json.dumps(search_results))
text_file.close()