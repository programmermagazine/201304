## 軟體短訊-Make 專案建置工具

Make 是 GNU 組織所釋出的老牌專案建置工具，通常會與 gcc 一同安裝，用來建置 c 語言的專案，
但是後來很多人也用 Make 來管理其他語言的專案，甚至有很多語言也發展出類似 make 的專案建置工具，
像是 Java 的 Maven 與 Ruby 的 Rake 等等。

Make 的預設建置對像為該資料夾下名稱為 Makefile 的檔案，因此您只要撰寫好 Makefile，然後執行指令
make 就可以開始建置整個專案了。

由於 Makefile 當中可以呼叫任何的命令列程式，因此並不受限於 C 語言編譯建置時使用，您可以把 
Make 當成是更好用的批次檔來用。

事實上、程式人雜誌從第二期以後就是用 make 建置整本電子書的，以下是程式人雜誌的專案建置檔。

```
PANDOC = pandoc -s --webtex 
HTML  = home.html license.html message1.html message2.html message3.html \
message4.html people1.html people2.html video1.html article1.html \
article2.html article3.html article4.html info.html
MD   = license.md message.md message1.md  message2.md  message3.md message4.md \
people.md people1.md  people2.md video.md video1.md article.md article1.md \
article2.md article3.md article4.md info.md reflink.md
PARG = -c ../css/pmag.css -B header.htm -A footer.htm 
PEPUB = --toc --epub-metadata=metadata.xml --epub-stylesheet=../css/pmag.css 
EARG = --margin-top=16 --margin-bottom=16 --margin-left=20 --margin-right=20 \
--pretty-print --base-font-size=9 --font-size-mapping="7, 8, 9, 12, 14, 16, 20, 24" 
RM = rm -f

.PHONY: all epubipad pdfipad pdfA4

all: $(HTML)

epubA4:
	$(PANDOC) $(PEPUB) --epub-cover-image=../img/coverA4.png toc.md $(MD) -o ../book/A4.epub

pdfA4: epubA4
	ebook-convert ../book/A4.epub ../book/A4.pdf $(EARG) --paper-size=a4
	
epubipad:
	$(PANDOC) $(PEPUB) --epub-cover-image=../img/cover.jpg toc.md $(MD) -o ../book/ipad.epub

pdfipad: epubipad
	ebook-convert ../book/ipad.epub ../book/ipad.pdf $(EARG) --output-profile=ipad3
	
html: $(HTML)

shtm: 
	$(PANDOC) --toc $(MD) -o ../book/pmag.html $(PARG)

%.html: %.md
	$(PANDOC) $< reflink.md -o ../htm/$@ $(PARG)
	
clean: 
	${RM} ../htm/*.*
```

在以上的專案建置檔中，我們使用了以下 pandoc 指令將 markdown 轉換為一頁一頁的 HTML，以建立網頁版的程式人雜誌。

```
%.html: %.md
    $(PANDOC) $< reflink.md -o ../htm/$@ $(FLAGS)
```

然後、我們也使用了以下指令呼叫 pandoc 將所有檔案合併轉換為 epub 電子書。

```
epubipad:
	$(PANDOC) $(PEPUB) --epub-cover-image=../img/cover.jpg toc.md $(MD) -o ../book/ipad.epub
```

最後、我們使用了 calibre 的 ebook-convert 將 epub 電子出轉換為 pdf 檔案，

```
pdfipad: epubipad
	ebook-convert ../book/ipad.epub ../book/ipad.pdf $(EARG) --output-profile=ipad3
```

以下是我們程式人雜誌 2013 年 4 月號的建置過程。

```
D:\Dropbox\Public\pmag\201304\source>make html
pandoc -s --webtex  home.md reflink.md -o ../htm/home.html -c ../css/pmag.css -B
 header.htm -A footer.htm
pandoc -s --webtex  license.md reflink.md -o ../htm/license.html -c ../css/pmag.
css -B header.htm -A footer.htm
pandoc -s --webtex  message1.md reflink.md -o ../htm/message1.html -c ../css/pma
g.css -B header.htm -A footer.htm
pandoc -s --webtex  message2.md reflink.md -o ../htm/message2.html -c ../css/pma
g.css -B header.htm -A footer.htm
pandoc -s --webtex  message3.md reflink.md -o ../htm/message3.html -c ../css/pma
g.css -B header.htm -A footer.htm
pandoc -s --webtex  message4.md reflink.md -o ../htm/message4.html -c ../css/pma
g.css -B header.htm -A footer.htm
pandoc -s --webtex  people1.md reflink.md -o ../htm/people1.html -c ../css/pmag.
css -B header.htm -A footer.htm
pandoc -s --webtex  people2.md reflink.md -o ../htm/people2.html -c ../css/pmag.
css -B header.htm -A footer.htm
pandoc -s --webtex  video1.md reflink.md -o ../htm/video1.html -c ../css/pmag.cs
s -B header.htm -A footer.htm
pandoc -s --webtex  video2.md reflink.md -o ../htm/video2.html -c ../css/pmag.cs
s -B header.htm -A footer.htm
pandoc -s --webtex  article1.md reflink.md -o ../htm/article1.html -c ../css/pma
g.css -B header.htm -A footer.htm
pandoc -s --webtex  article2.md reflink.md -o ../htm/article2.html -c ../css/pma
g.css -B header.htm -A footer.htm
pandoc -s --webtex  article3.md reflink.md -o ../htm/article3.html -c ../css/pma
g.css -B header.htm -A footer.htm
pandoc -s --webtex  article4.md reflink.md -o ../htm/article4.html -c ../css/pma
g.css -B header.htm -A footer.htm
pandoc -s --webtex  info.md reflink.md -o ../htm/info.html -c ../css/pmag.css -B
 header.htm -A footer.htm

pandoc -s --webtex  --toc --epub-metadata=metadata.xml --epub-stylesheet=../css/pmag.css
  --epub-cover-image=../img/cover.jpg toc.md license.md message.md message1.md  message2.md 
  message3.md message4.md people.md people1.md  people2.md video.md video1.md video2.md \
  article.md article1.md article2.md article3.md article4.md info.md reflink.md -o ../book/ipad.epub
ebook-convert ../book/ipad.epub ../book/ipad.pdf --margin-top=16 --margin-bottom=16 
--margin-left=20 --margin-right=20 --pretty-print --base-font-size=9 
--font-size-mapping="7, 8, 9, 12, 14, 16, 20, 24"  --output-profile=ipad3
1% 將輸入轉換為HTML格式...
InputFormatPlugin: EPUB Input running
on D:\Dropbox\Public\pmag\201304\book\ipad.epub
Parsing all content...
COLOR_VALUE: No match in Choice(HEX color, Named Color, Sequence(FUNCTION, Choice(unary +-, 
number, percentage), Sequence(comma, Choice(unary +-, number, percentage)), end FUNC ")"), 
Sequence(FUNCTION, Choice(unary +-, number, percentage), Sequence(comma, Choice(unary +-, 
number, percentage)), end FUNC ")")): ('HASH', u'#\ufeff\ufeff000080', 66, 18)
COLOR_VALUE: No match in Choice(HEX color, Named Color, Sequence(FUNCTION, Choice(unary +-, 
number, percentage), Sequence(comma, Choice(unary +-, number, percentage)), end FUNC ")"), 
Sequence(FUNCTION, Choice(unary +-, number, percentage), Sequence(comma, Choice(unary +-, 
number, percentage)), end FUNC ")"))
34% 正在對電子書籍進行轉換...
Merging user specified metadata...
Detecting structure...
Flattening CSS and remapping font sizes...
Source base font size is 12.00000pt
Removing fake margins...
Cleaning up manifest...
Trimming unused files from manifest...
Creating PDF Output...
67% 執行 PDF Output 外掛程式
71% Rendered cover.xhtml
75% Rendered title_page.xhtml
79% Rendered ch1.xhtml
83% Rendered ch2.xhtml
87% Rendered ch3.xhtml
91% Rendered ch4.xhtml
95% Rendered ch5.xhtml
100% Rendered ch6.xhtml
Rendered PDF in 4.316 seconds:
PDF output written to D:\Dropbox\Public\pmag\201304\book\ipad.pdf
將輸出儲存到   D:\Dropbox\Public\pmag\201304\book\ipad.pdf

```

透過這樣的方式，我們用很「程式人」的方法，編輯了一份雜誌。【本文由陳鍾誠撰寫】

