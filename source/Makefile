PANDOC = pandoc -s --webtex 
HTML  = home.html license.html message1.html message2.html message3.html \
message4.html people1.html people2.html video1.html video2.html article1.html \
article2.html article3.html article4.html info.html
MD   = license.md message.md message1.md  message2.md message3.md message4.md \
people.md people1.md  people2.md video.md video1.md video2.md article.md \
article1.md article2.md article3.md article4.md info.md reflink.md
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
	${RM} ../book/*.*
