#!/bin/bash

# LINUX == :heart:

OUTPUT=$(pwd)/documentation.html

if [[ $# -eq 0 ]] ; then
    echo 'Usage: <path to pandoc>'
    exit 0
fi

pathToPandoc=$1"/"
cd src

#add newline to the end of each file
nl='
'
for f in *.md; 
do echo "Processing $f file.."; 
echo "${nl}" >> $f; done

# generate DOC file
${pathToPandoc}pandoc *.md > ${OUTPUT}
