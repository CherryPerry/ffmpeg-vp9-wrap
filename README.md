# VPX ENCODER
Соснольный враппер ffmpeg'a для кодирования webm.

####Скачать
[СКАЧАТЬ БЕСПЛАТНО БЕЗ СМС]

####Запуск
Перед запуском установи ffmpeg, добавь путь к самому vp9.exe (для удобства) и к ffmpeg.exe, ffprobe.exe (для работы приложения) в Path, настрой фонтконфиг. Можешь воспользоваться [установщиком] или [гайдом].
**ИЛИ**
Запусти скачанный vp9.exe из консоли, запущенной с правами Админстатора, с ключом -install.
```
vp9 -install
```
Программа:
- Качает архивчик с zeranoe ffmpeg под текущую архитектуру (x86 или x86_64)
- Распаковывает в папку установки (C:\Program Files\FFMPEG Compact)
- Прописывает в Path путь до папки установки (можно запускать ffmpeg, ffprobe, vp9 из любой папки)
- Настраивает fontconfig для хардсаба (задает системные переменные и создает fonts.conf)
- Копирует vp9.exe в папку установки

Не забудь поставить .net framework 4.5!

####Кодирование
Самый простой способ:
```
vp9 -file "1.mkv" -subs "1.ass" -ss 01:00.000 -to 01:30.000
```
2 pass, quality good, opus 80K, 960x540.

Для списка команд (вызов без аргументов):
```
vp9
```

####Замечания по командам
#####-t и -ti
Можно кодить параллельно несколько webm из одного исходника. Для этого создай файл с любым названием следующего содержания:
```
00:30.000 01:35.000
01:36.000 02:00.000
```
Далее запусти vp9.exe с следующими аргументами:
```
vp9 -file 1.mkv -t тайминги.txt
```
Для того, чтобы сделать конкретно какую-то сторчку из файла, добавь -ti № строки (или строк через запятую), считая от 0.
#####-alimit и -limit
limit для указания лимита (10240KB по умолчанию):
```
vp9 -file "1.mkv" -ss 01:00.000 -to 01:30.000 -limit 10240
```
alimit для подгона под лимит (с погрешностью -alimitD 240КБ) через перекодирование видео
```
vp9 -file "1.mkv" -ss 01:00.000 -to 01:30.000 -limit 10240 -alimit
```
Не любое видео можно подогнать, обычно не получаются ролики длиной меньше минуты.

#####-preview и -preview_s
Есть возможность добавить превью для уже полученного видео. Для этого выбери кадр (запомни его тайминг) из видео-файла и запусти:
```
vp9 -file webm_куда_добавить_превью.webm -preview_s исходник.mkv -preview 00:30.255
```
Для того, чтобы взять превью из webm, к которому оно добавляется, не указывай -preview_s:
```
vp9 -file webm_куда_добавить_превью.webm -preview 00:30.255
```

#####-youtube
Для скачивания видео c ютубчика для дальнейшего кодирования (лучшее качество mp4):
```
vp9 -youtube https://youtube...
```

#####-crop
Для удаления черных полос:
```
vp9 -file "1.mkv" -crop
```

####Сторонние библиотеки
- [YoutubeExtractor] MIT License
- [Json.NET] MIT license
- [Html Agility Pack] Microsoft Public License
- [SharpCompress] Microsoft Public License

[СКАЧАТЬ БЕСПЛАТНО БЕЗ СМС]:https://github.com/CherryPerry/ffmpeg-vp9-wrap/releases
[установщиком]:https://github.com/CherryPerry/zeranoe-ffmpeg-update-csharp
[гайдом]:https://github.com/pituz/webm-thread/wiki/installing-ffmpeg-on-windows
[YoutubeExtractor]:https://github.com/flagbug/YoutubeExtractor
[Json.NET]:http://www.newtonsoft.com/json
[Html Agility Pack]:https://htmlagilitypack.codeplex.com/
[SharpCompress]:https://sharpcompress.codeplex.com/
