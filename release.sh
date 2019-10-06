#!/bin/bash

set -ex

SEVENZIP="/c/Program Files/7-Zip/7z"

EXCLUDES="-xr!source -xr!log.txt -xr!*.log -xr!*.suo -xr!*.pdb -xr!*.user -xr!bin -xr!obj -xr!packages -xr!.vs -xr!.git* -xr!.modtek -xr!_ignored -xr!*.zip -xr!*.sh"

"$SEVENZIP" a -tzip -mx9 ModTekTest.zip $EXCLUDES .
